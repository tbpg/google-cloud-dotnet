﻿// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Cloud.Tools.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Google.Cloud.Tools.ReleaseManager
{
    /// <summary>
    /// Description of a smoke test for an API, along with some fairly grungy code to execute it.
    /// </summary>
    public class SmokeTest
    {
        /// <summary>
        /// The method to test, e.g. AnalyzeSyntax. Only synchronous operations should be called,
        /// but long-running operations and list operations are handled automatically.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// The name of the client class, assumed to be in the same namespace as the package ID,
        /// e.g. "PublisherClient". This is optional if the package only contains a single client.
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// A map of named arguments to the method. Strings and numeric values can be represented
        /// directly as literals; object values are assumed to be protobuf messages, and are
        /// parsed as JSON. Note that every string value (even within an object) has the substring
        /// "${PROJECT_ID}" replaced by the project ID before invocation.
        /// </summary>
        public Dictionary<string, JToken> Arguments { get; set; }

        /// <summary>
        /// Executes a smoke test.
        /// </summary>
        /// <param name="assembly">The assembly of the client library being tests.</param>
        /// <param name="projectId">The project ID, to allow ${PROJECT_ID} to be replaced.</param>
        public void Execute(Assembly assembly, string projectId)
        {
            var client = FindClient(assembly);
            var method = FindMethod(client);
            var arguments = ConvertArguments(method, projectId);
            var clientInstance = client.GetMethod("Create", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);

            Console.WriteLine($"Running test for {client.Name}.{Method}");
            var result = method.Invoke(clientInstance, arguments);
            DisplayResult(result);
        }

        private Type FindClient(Assembly assembly)
        {
            if (Client is object)
            {
                // Assume the namespace is the same as the assembly name.
                string ns = assembly.GetName().Name;
                var typeName = $"{ns}.{Client}";
                var type = assembly.GetType(typeName);
                return type ?? throw new UserErrorException($"No such client type {typeName} in assembly");
            }

            // This is crude, but probably good enough.
            var clients = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Client"))
                // Check that FooClient has FooSettings
                .Where(t => assembly.GetType(t.FullName[..^6] + "Settings") is object)
                .ToList();
            return clients.Count switch
            {
                0 => throw new UserErrorException("Assembly contains no recognized API client classes"),
                1 => clients[0],
                _ => throw new UserErrorException($"Assembly contains multiple API clients: {string.Join(", ", clients.Select(c => c.Name))}")
            };
        }


        private MethodInfo FindMethod(Type client)
        {
            var methods = client.GetMethods().Where(method => method.Name == Method);
            var matchingMethods = methods.Where(method => MethodMatches(method, Arguments)).ToList();
            return matchingMethods.Count switch
            {
                0 => throw new UserErrorException($"No matching methods with name {Method} found"),
                1 => matchingMethods[0],
                _ => throw new UserErrorException($"Arguments for method {Method} are ambiguous; {matchingMethods.Count} overloads match")
            };

            bool MethodMatches(MethodInfo method, IReadOnlyDictionary<string, JToken> arguments)
            {
                var parameters = method.GetParameters();
                // If there are arguments that aren't parameters, it doesn't match.
                if (arguments.Keys.Except(parameters.Select(p => p.Name)).Any())
                {
                    return false;
                }
                // Check that each argument is supplied if it's required, and can be converted to the right type.
                foreach (var parameter in parameters)
                {
                    if (!arguments.TryGetValue(parameter.Name, out var argValue))
                    {
                        if (!parameter.IsOptional)
                        {
                            return false;
                        }
                    }
                    else if (!ArgumentMatches(parameter, argValue.Type))
                    {
                        return false;
                    }
                }
                return true;
            }

            bool ArgumentMatches(ParameterInfo parameter, JTokenType argument)
            {
                var parameterType = parameter.ParameterType;
                return argument switch
                {
                    JTokenType.String => parameterType == typeof(string),
                    JTokenType.Integer => parameterType == typeof(int),
                    JTokenType.Float => parameterType == typeof(float) || parameterType == typeof(double),
                    // TODO: Check whether it's a protobuf message?
                    JTokenType.Object => parameterType != typeof(string) && !parameterType.IsValueType,
                    JTokenType.Array => typeof(IEnumerable).IsAssignableFrom(parameterType),
                    _ => false
                };
            }
        }

        private object[] ConvertArguments(MethodInfo method, string projectId)
        {
            foreach (var value in Arguments.Values)
            {
                ReplaceProjectId(value, projectId);
            }
            var parameters = method.GetParameters();
            return parameters.Select(p => GetCorrespondingArgument(p)).ToArray();

            void ReplaceProjectId(JToken token, string projectId)
            {
                switch (token.Type)
                {
                    case JTokenType.String:
                        var value = (JValue) token;
                        value.Value = ((string) value.Value).Replace("${PROJECT_ID}", projectId);
                        break;
                    case JTokenType.Array:
                        var array = (JArray) token;
                        foreach (var element in array.Children())
                        {
                            ReplaceProjectId(element, projectId);
                        }
                        break;
                    case JTokenType.Object:
                        var obj = (JObject) token;
                        foreach (var property in obj.Properties())
                        {
                            ReplaceProjectId(property.Value, projectId);
                        }
                        break;
                }
            }

            object GetCorrespondingArgument(ParameterInfo parameter)
            {
                if (!Arguments.TryGetValue(parameter.Name, out var argValue))
                {
                    return parameter.DefaultValue;
                }
                var parameterType = parameter.ParameterType;
                dynamic protoParser = MaybeGetMessageParser(parameterType);
                return protoParser is object
                    ? protoParser.ParseJson(argValue.ToString())
                    : argValue.ToObject(parameterType);
            }

            object MaybeGetMessageParser(Type type)
            {
                // Note that we don't want to take a dependency on Google.Protobuf directly in this tool,
                // as it could cause versioning problems when we try to load the API.
                var parserProperty = type.GetProperty("Parser", BindingFlags.Static | BindingFlags.Public);
                if (parserProperty is null)
                {
                    return null;
                }
                var propertyType = parserProperty.PropertyType;
                Console.WriteLine("Got a parser property of type " + propertyType);
                return propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().FullName == "Google.Protobuf.MessageParser`1"
                    ? parserProperty.GetValue(null)
                    : null;
            }
        }

        private void DisplayResult(object result)
        {
            if (result is null)
            {
                Console.WriteLine($"Result: null");
                return;
            }
            var resultType = result.GetType();
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition().FullName == "Google.LongRunning.Operation`2")
            {
                dynamic operation = result;
                Console.WriteLine($"Result is long-running operation with name {operation.Name}");
                Console.WriteLine("Polling for completion. (This could take some time.)");
                operation = operation.PollUntilCompleted();
                Console.WriteLine($"Operation completed. Result: {operation.Result}");
                return;
            }
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition().FullName == "Google.Api.Gax.Grpc.GrpcPagedEnumerable`3")
            {
                Console.WriteLine($"Result is paged, with an item type of {resultType.GetGenericArguments()[2].Name}");
                Console.WriteLine("Fetching results.");
                var results = ((IEnumerable) result).Cast<object>().ToList();
                foreach (var item in results)
                {
                    Console.WriteLine(item);
                }
                return;
            }
            Console.WriteLine($"Result: {result}");
        }
    }
}
