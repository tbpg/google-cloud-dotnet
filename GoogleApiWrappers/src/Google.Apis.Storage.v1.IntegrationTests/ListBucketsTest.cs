﻿// Copyright 2015 Google Inc. All Rights Reserved.
// Licensed under the Apache License Version 2.0.
using Google.Apis.Storage.v1.ClientWrapper;
using Google.Apis.Storage.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Google.Apis.Storage.v1.IntegrationTests
{
    // Preconditions:
    // - Logged in locally with gcloud auth
    // - Environment variable TEST_PROJECT identifies an existing project
    // - Buckets exist called "integrationtests-0", "integrationtests-1" etc.
    // - One bucket called "integrationtests-extra"
    public class ListBucketsTest
    {
        // TODO:
        // - Automate creating the buckets etc

        private static readonly CloudConfiguration s_config = CloudConfiguration.Instance;

        private static readonly string s_extraBucket = s_config.TempBucketPrefix + "extra";
        private static readonly string[] s_allKnownBuckets = Enumerable
                .Range(0, 10)
                .Select(x => s_config.TempBucketPrefix + x)
                .Concat(new[] { s_extraBucket })
                .ToArray();

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "integ")]
        [InlineData(4, null)]
        public async Task AllBuckets(int? pageSize, string prefix)
        {
            var options = new ListBucketsOptions { PageSize = pageSize, Prefix = prefix };
            await AssertBuckets(options, s_allKnownBuckets);
        }

        [Fact]
        public async Task Prefix()
        {
            var options = new ListBucketsOptions { Prefix = s_config.TempBucketPrefix + "e" };
            await AssertBuckets(options, s_extraBucket);
        }

        [Fact]
        public async Task CancellationTokenRespected()
        {
            var cts = new CancellationTokenSource();
            var enumerable = s_config.Client.ListBucketsAsync(s_config.Project, null);
            var enumerator = enumerable.GetEnumerator();
            Assert.True(await enumerator.MoveNext(cts.Token));
            cts.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await enumerator.MoveNext(cts.Token));
        }

        // Fetches buckets using the given options in each possible way, validating that the expected bucket names are returned.
        private async Task AssertBuckets(ListBucketsOptions options, params string[] expectedBucketNames)
        {
            IEnumerable<Bucket> actual = s_config.Client.ListBuckets(s_config.Project, options);
            AssertBucketNames(actual, expectedBucketNames);
            actual = await s_config.Client.ListAllBucketsAsync(s_config.Project, options, CancellationToken.None);
            AssertBucketNames(actual, expectedBucketNames);
            actual = await s_config.Client.ListBucketsAsync(s_config.Project, options).ToList(CancellationToken.None);
            AssertBucketNames(actual, expectedBucketNames);
        }

        private void AssertBucketNames(IEnumerable<Bucket> actualBuckets, string[] expectedNames)
        {
            // Intersection with known buckets to avoid non-test buckets causing issues.
            var actualNames = actualBuckets.Select(b => b.Name).Intersect(s_allKnownBuckets).OrderBy(x => x).ToList();
            Assert.Equal(expectedNames.OrderBy(x => x), actualNames);
        }
    }
}
