// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: demo.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Sdmxdl.Grpc {

  /// <summary>Holder for reflection information generated from demo.proto</summary>
  public static partial class DemoReflection {

    #region Descriptor
    /// <summary>File descriptor for demo.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DemoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpkZW1vLnByb3RvEgtzZG14ZGwuZ3JwYxokc2RteGRsL2Zvcm1hdC9wcm90",
            "b2J1Zi9tb25pdG9yLnByb3RvGidzZG14ZGwvZm9ybWF0L3Byb3RvYnVmL3Jl",
            "cG9zaXRvcnkucHJvdG8aI3NkbXhkbC9mb3JtYXQvcHJvdG9idWYvc291cmNl",
            "LnByb3RvGhtnb29nbGUvcHJvdG9idWYvZW1wdHkucHJvdG8iHwoNU291cmNl",
            "UmVxdWVzdBIOCgZzb3VyY2UYASABKAkiKwoLRmxvd1JlcXVlc3QSDgoGc291",
            "cmNlGAEgASgJEgwKBGZsb3cYAiABKAkiNwoKS2V5UmVxdWVzdBIOCgZzb3Vy",
            "Y2UYASABKAkSDAoEZmxvdxgCIAEoCRILCgNrZXkYAyABKAkyswQKDlNkbXhX",
            "ZWJNYW5hZ2VyElEKCkdldFNvdXJjZXMSFi5nb29nbGUucHJvdG9idWYuRW1w",
            "dHkaKS5zZG14ZGwuZm9ybWF0LnByb3RvYnVmLndlYi5TZG14V2ViU291cmNl",
            "MAESWQoQR2V0TW9uaXRvclJlcG9ydBIaLnNkbXhkbC5ncnBjLlNvdXJjZVJl",
            "cXVlc3QaKS5zZG14ZGwuZm9ybWF0LnByb3RvYnVmLndlYi5Nb25pdG9yUmVw",
            "b3J0EkoKCEdldEZsb3dzEhouc2RteGRsLmdycGMuU291cmNlUmVxdWVzdBog",
            "LnNkbXhkbC5mb3JtYXQucHJvdG9idWYuRGF0YWZsb3cwARJFCgdHZXRGbG93",
            "Ehguc2RteGRsLmdycGMuRmxvd1JlcXVlc3QaIC5zZG14ZGwuZm9ybWF0LnBy",
            "b3RvYnVmLkRhdGFmbG93Ek8KDEdldFN0cnVjdHVyZRIYLnNkbXhkbC5ncnBj",
            "LkZsb3dSZXF1ZXN0GiUuc2RteGRsLmZvcm1hdC5wcm90b2J1Zi5EYXRhU3Ry",
            "dWN0dXJlEkMKB0dldERhdGESFy5zZG14ZGwuZ3JwYy5LZXlSZXF1ZXN0Gh8u",
            "c2RteGRsLmZvcm1hdC5wcm90b2J1Zi5EYXRhU2V0EkoKDUdldERhdGFTdHJl",
            "YW0SFy5zZG14ZGwuZ3JwYy5LZXlSZXF1ZXN0Gh4uc2RteGRsLmZvcm1hdC5w",
            "cm90b2J1Zi5TZXJpZXMwAUICUAFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Sdmxdl.Format.Protobuf.Web.MonitorReflection.Descriptor, global::Sdmxdl.Format.Protobuf.RepositoryReflection.Descriptor, global::Sdmxdl.Format.Protobuf.Web.SourceReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Sdmxdl.Grpc.SourceRequest), global::Sdmxdl.Grpc.SourceRequest.Parser, new[]{ "Source" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Sdmxdl.Grpc.FlowRequest), global::Sdmxdl.Grpc.FlowRequest.Parser, new[]{ "Source", "Flow" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Sdmxdl.Grpc.KeyRequest), global::Sdmxdl.Grpc.KeyRequest.Parser, new[]{ "Source", "Flow", "Key" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class SourceRequest : pb::IMessage<SourceRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<SourceRequest> _parser = new pb::MessageParser<SourceRequest>(() => new SourceRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<SourceRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Sdmxdl.Grpc.DemoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SourceRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SourceRequest(SourceRequest other) : this() {
      source_ = other.source_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public SourceRequest Clone() {
      return new SourceRequest(this);
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 1;
    private string source_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Source {
      get { return source_; }
      set {
        source_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as SourceRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(SourceRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Source != other.Source) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Source.Length != 0) hash ^= Source.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Source.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Source);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Source.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Source);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Source.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Source);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(SourceRequest other) {
      if (other == null) {
        return;
      }
      if (other.Source.Length != 0) {
        Source = other.Source;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Source = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Source = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class FlowRequest : pb::IMessage<FlowRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<FlowRequest> _parser = new pb::MessageParser<FlowRequest>(() => new FlowRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<FlowRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Sdmxdl.Grpc.DemoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FlowRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FlowRequest(FlowRequest other) : this() {
      source_ = other.source_;
      flow_ = other.flow_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public FlowRequest Clone() {
      return new FlowRequest(this);
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 1;
    private string source_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Source {
      get { return source_; }
      set {
        source_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "flow" field.</summary>
    public const int FlowFieldNumber = 2;
    private string flow_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Flow {
      get { return flow_; }
      set {
        flow_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as FlowRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(FlowRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Source != other.Source) return false;
      if (Flow != other.Flow) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Source.Length != 0) hash ^= Source.GetHashCode();
      if (Flow.Length != 0) hash ^= Flow.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Source.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Source);
      }
      if (Flow.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Flow);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Source.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Source);
      }
      if (Flow.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Flow);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Source.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Source);
      }
      if (Flow.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Flow);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(FlowRequest other) {
      if (other == null) {
        return;
      }
      if (other.Source.Length != 0) {
        Source = other.Source;
      }
      if (other.Flow.Length != 0) {
        Flow = other.Flow;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Source = input.ReadString();
            break;
          }
          case 18: {
            Flow = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Source = input.ReadString();
            break;
          }
          case 18: {
            Flow = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class KeyRequest : pb::IMessage<KeyRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<KeyRequest> _parser = new pb::MessageParser<KeyRequest>(() => new KeyRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<KeyRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Sdmxdl.Grpc.DemoReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public KeyRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public KeyRequest(KeyRequest other) : this() {
      source_ = other.source_;
      flow_ = other.flow_;
      key_ = other.key_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public KeyRequest Clone() {
      return new KeyRequest(this);
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 1;
    private string source_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Source {
      get { return source_; }
      set {
        source_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "flow" field.</summary>
    public const int FlowFieldNumber = 2;
    private string flow_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Flow {
      get { return flow_; }
      set {
        flow_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "key" field.</summary>
    public const int KeyFieldNumber = 3;
    private string key_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Key {
      get { return key_; }
      set {
        key_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as KeyRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(KeyRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Source != other.Source) return false;
      if (Flow != other.Flow) return false;
      if (Key != other.Key) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Source.Length != 0) hash ^= Source.GetHashCode();
      if (Flow.Length != 0) hash ^= Flow.GetHashCode();
      if (Key.Length != 0) hash ^= Key.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Source.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Source);
      }
      if (Flow.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Flow);
      }
      if (Key.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Key);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Source.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Source);
      }
      if (Flow.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Flow);
      }
      if (Key.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(Key);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (Source.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Source);
      }
      if (Flow.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Flow);
      }
      if (Key.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Key);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(KeyRequest other) {
      if (other == null) {
        return;
      }
      if (other.Source.Length != 0) {
        Source = other.Source;
      }
      if (other.Flow.Length != 0) {
        Flow = other.Flow;
      }
      if (other.Key.Length != 0) {
        Key = other.Key;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Source = input.ReadString();
            break;
          }
          case 18: {
            Flow = input.ReadString();
            break;
          }
          case 26: {
            Key = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Source = input.ReadString();
            break;
          }
          case 18: {
            Flow = input.ReadString();
            break;
          }
          case 26: {
            Key = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code