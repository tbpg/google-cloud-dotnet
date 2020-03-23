// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/cloud/securitycenter/settings/v1beta1/detector.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Cloud.SecurityCenter.Settings.V1Beta1 {

  /// <summary>Holder for reflection information generated from google/cloud/securitycenter/settings/v1beta1/detector.proto</summary>
  public static partial class DetectorReflection {

    #region Descriptor
    /// <summary>File descriptor for google/cloud/securitycenter/settings/v1beta1/detector.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DetectorReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cjtnb29nbGUvY2xvdWQvc2VjdXJpdHljZW50ZXIvc2V0dGluZ3MvdjFiZXRh",
            "MS9kZXRlY3Rvci5wcm90bxIsZ29vZ2xlLmNsb3VkLnNlY3VyaXR5Y2VudGVy",
            "LnNldHRpbmdzLnYxYmV0YTEaH2dvb2dsZS9hcGkvZmllbGRfYmVoYXZpb3Iu",
            "cHJvdG8aQ2dvb2dsZS9jbG91ZC9zZWN1cml0eWNlbnRlci9zZXR0aW5ncy92",
            "MWJldGExL2JpbGxpbmdfc2V0dGluZ3MucHJvdG8aHGdvb2dsZS9hcGkvYW5u",
            "b3RhdGlvbnMucHJvdG8irQEKCERldGVjdG9yEhUKCGRldGVjdG9yGAEgASgJ",
            "QgPgQQMSFgoJY29tcG9uZW50GAIgASgJQgPgQQMSVAoMYmlsbGluZ190aWVy",
            "GAMgASgOMjkuZ29vZ2xlLmNsb3VkLnNlY3VyaXR5Y2VudGVyLnNldHRpbmdz",
            "LnYxYmV0YTEuQmlsbGluZ1RpZXJCA+BBAxIcCg9kZXRlY3Rvcl9sYWJlbHMY",
            "BCADKAlCA+BBA0KuAgowY29tLmdvb2dsZS5jbG91ZC5zZWN1cml0eWNlbnRl",
            "ci5zZXR0aW5ncy52MWJldGExQg5EZXRlY3RvcnNQcm90b1ABWlRnb29nbGUu",
            "Z29sYW5nLm9yZy9nZW5wcm90by9nb29nbGVhcGlzL2Nsb3VkL3NlY3VyaXR5",
            "Y2VudGVyL3NldHRpbmdzL3YxYmV0YTE7c2V0dGluZ3P4AQGqAixHb29nbGUu",
            "Q2xvdWQuU2VjdXJpdHlDZW50ZXIuU2V0dGluZ3MuVjFCZXRhMcoCLEdvb2ds",
            "ZVxDbG91ZFxTZWN1cml0eUNlbnRlclxTZXR0aW5nc1xWMWJldGEx6gIwR29v",
            "Z2xlOjpDbG91ZDo6U2VjdXJpdHlDZW50ZXI6OlNldHRpbmdzOjpWMWJldGEx",
            "YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Api.FieldBehaviorReflection.Descriptor, global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingSettingsReflection.Descriptor, global::Google.Api.AnnotationsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Cloud.SecurityCenter.Settings.V1Beta1.Detector), global::Google.Cloud.SecurityCenter.Settings.V1Beta1.Detector.Parser, new[]{ "Detector_", "Component", "BillingTier", "DetectorLabels" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Detector is a set of detectors or scanners act as individual checks done
  /// within a component e.g. bad IP, bad domains, IAM anomaly, cryptomining, open
  /// firewall, etc. Detector is independent of Organization, meaning each detector
  /// must be defined for a given Security Center component under a specified
  /// billing tier. Organizations can configure the list of detectors based on
  /// their subscribed billing tier.
  ///
  /// Defines a detector, its billing tier and any applicable labels.
  /// </summary>
  public sealed partial class Detector : pb::IMessage<Detector> {
    private static readonly pb::MessageParser<Detector> _parser = new pb::MessageParser<Detector>(() => new Detector());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Detector> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Cloud.SecurityCenter.Settings.V1Beta1.DetectorReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Detector() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Detector(Detector other) : this() {
      detector_ = other.detector_;
      component_ = other.component_;
      billingTier_ = other.billingTier_;
      detectorLabels_ = other.detectorLabels_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Detector Clone() {
      return new Detector(this);
    }

    /// <summary>Field number for the "detector" field.</summary>
    public const int Detector_FieldNumber = 1;
    private string detector_ = "";
    /// <summary>
    /// Output only. Detector Identifier
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Detector_ {
      get { return detector_; }
      set {
        detector_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "component" field.</summary>
    public const int ComponentFieldNumber = 2;
    private string component_ = "";
    /// <summary>
    /// Output only. Component that supports detector type.  Multiple components may support the
    /// same detector.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Component {
      get { return component_; }
      set {
        component_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "billing_tier" field.</summary>
    public const int BillingTierFieldNumber = 3;
    private global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier billingTier_ = global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier.Unspecified;
    /// <summary>
    /// Output only. The billing tier may be different for a detector of the same name in
    /// another component.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier BillingTier {
      get { return billingTier_; }
      set {
        billingTier_ = value;
      }
    }

    /// <summary>Field number for the "detector_labels" field.</summary>
    public const int DetectorLabelsFieldNumber = 4;
    private static readonly pb::FieldCodec<string> _repeated_detectorLabels_codec
        = pb::FieldCodec.ForString(34);
    private readonly pbc::RepeatedField<string> detectorLabels_ = new pbc::RepeatedField<string>();
    /// <summary>
    /// Output only. Google curated detector labels. These are alphanumeric tags that are not
    /// necessarily human readable. Labels can be used to group detectors together
    /// in the future. An example might be tagging all detectors “PCI” that help
    /// with PCI compliance.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<string> DetectorLabels {
      get { return detectorLabels_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Detector);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Detector other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Detector_ != other.Detector_) return false;
      if (Component != other.Component) return false;
      if (BillingTier != other.BillingTier) return false;
      if(!detectorLabels_.Equals(other.detectorLabels_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Detector_.Length != 0) hash ^= Detector_.GetHashCode();
      if (Component.Length != 0) hash ^= Component.GetHashCode();
      if (BillingTier != global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier.Unspecified) hash ^= BillingTier.GetHashCode();
      hash ^= detectorLabels_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Detector_.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Detector_);
      }
      if (Component.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Component);
      }
      if (BillingTier != global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier.Unspecified) {
        output.WriteRawTag(24);
        output.WriteEnum((int) BillingTier);
      }
      detectorLabels_.WriteTo(output, _repeated_detectorLabels_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Detector_.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Detector_);
      }
      if (Component.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Component);
      }
      if (BillingTier != global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier.Unspecified) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) BillingTier);
      }
      size += detectorLabels_.CalculateSize(_repeated_detectorLabels_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Detector other) {
      if (other == null) {
        return;
      }
      if (other.Detector_.Length != 0) {
        Detector_ = other.Detector_;
      }
      if (other.Component.Length != 0) {
        Component = other.Component;
      }
      if (other.BillingTier != global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier.Unspecified) {
        BillingTier = other.BillingTier;
      }
      detectorLabels_.Add(other.detectorLabels_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Detector_ = input.ReadString();
            break;
          }
          case 18: {
            Component = input.ReadString();
            break;
          }
          case 24: {
            BillingTier = (global::Google.Cloud.SecurityCenter.Settings.V1Beta1.BillingTier) input.ReadEnum();
            break;
          }
          case 34: {
            detectorLabels_.AddEntriesFrom(input, _repeated_detectorLabels_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
