// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: demo.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Sdmxdl.Grpc {
  public static partial class SdmxWebManager
  {
    static readonly string __ServiceName = "sdmxdl.grpc.SdmxWebManager";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Grpc.Empty> __Marshaller_sdmxdl_grpc_Empty = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Grpc.Empty.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Format.Protobuf.Web.SdmxWebSource> __Marshaller_sdmxdl_format_protobuf_web_SdmxWebSource = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Format.Protobuf.Web.SdmxWebSource.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Grpc.SourceRequest> __Marshaller_sdmxdl_grpc_SourceRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Grpc.SourceRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Format.Protobuf.Web.MonitorReport> __Marshaller_sdmxdl_format_protobuf_web_MonitorReport = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Format.Protobuf.Web.MonitorReport.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Format.Protobuf.Dataflow> __Marshaller_sdmxdl_format_protobuf_Dataflow = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Format.Protobuf.Dataflow.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Grpc.FlowRequest> __Marshaller_sdmxdl_grpc_FlowRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Grpc.FlowRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Format.Protobuf.DataStructure> __Marshaller_sdmxdl_format_protobuf_DataStructure = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Format.Protobuf.DataStructure.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Grpc.KeyRequest> __Marshaller_sdmxdl_grpc_KeyRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Grpc.KeyRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Format.Protobuf.DataSet> __Marshaller_sdmxdl_format_protobuf_DataSet = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Format.Protobuf.DataSet.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::Sdmxdl.Format.Protobuf.Series> __Marshaller_sdmxdl_format_protobuf_Series = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Sdmxdl.Format.Protobuf.Series.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.Empty, global::Sdmxdl.Format.Protobuf.Web.SdmxWebSource> __Method_GetSources = new grpc::Method<global::Sdmxdl.Grpc.Empty, global::Sdmxdl.Format.Protobuf.Web.SdmxWebSource>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetSources",
        __Marshaller_sdmxdl_grpc_Empty,
        __Marshaller_sdmxdl_format_protobuf_web_SdmxWebSource);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.SourceRequest, global::Sdmxdl.Format.Protobuf.Web.MonitorReport> __Method_GetMonitorReport = new grpc::Method<global::Sdmxdl.Grpc.SourceRequest, global::Sdmxdl.Format.Protobuf.Web.MonitorReport>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetMonitorReport",
        __Marshaller_sdmxdl_grpc_SourceRequest,
        __Marshaller_sdmxdl_format_protobuf_web_MonitorReport);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.SourceRequest, global::Sdmxdl.Format.Protobuf.Dataflow> __Method_GetFlows = new grpc::Method<global::Sdmxdl.Grpc.SourceRequest, global::Sdmxdl.Format.Protobuf.Dataflow>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetFlows",
        __Marshaller_sdmxdl_grpc_SourceRequest,
        __Marshaller_sdmxdl_format_protobuf_Dataflow);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.FlowRequest, global::Sdmxdl.Format.Protobuf.Dataflow> __Method_GetFlow = new grpc::Method<global::Sdmxdl.Grpc.FlowRequest, global::Sdmxdl.Format.Protobuf.Dataflow>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetFlow",
        __Marshaller_sdmxdl_grpc_FlowRequest,
        __Marshaller_sdmxdl_format_protobuf_Dataflow);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.FlowRequest, global::Sdmxdl.Format.Protobuf.DataStructure> __Method_GetStructure = new grpc::Method<global::Sdmxdl.Grpc.FlowRequest, global::Sdmxdl.Format.Protobuf.DataStructure>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetStructure",
        __Marshaller_sdmxdl_grpc_FlowRequest,
        __Marshaller_sdmxdl_format_protobuf_DataStructure);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.KeyRequest, global::Sdmxdl.Format.Protobuf.DataSet> __Method_GetData = new grpc::Method<global::Sdmxdl.Grpc.KeyRequest, global::Sdmxdl.Format.Protobuf.DataSet>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetData",
        __Marshaller_sdmxdl_grpc_KeyRequest,
        __Marshaller_sdmxdl_format_protobuf_DataSet);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::Sdmxdl.Grpc.KeyRequest, global::Sdmxdl.Format.Protobuf.Series> __Method_GetDataStream = new grpc::Method<global::Sdmxdl.Grpc.KeyRequest, global::Sdmxdl.Format.Protobuf.Series>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetDataStream",
        __Marshaller_sdmxdl_grpc_KeyRequest,
        __Marshaller_sdmxdl_format_protobuf_Series);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Sdmxdl.Grpc.DemoReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for SdmxWebManager</summary>
    public partial class SdmxWebManagerClient : grpc::ClientBase<SdmxWebManagerClient>
    {
      /// <summary>Creates a new client for SdmxWebManager</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public SdmxWebManagerClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for SdmxWebManager that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public SdmxWebManagerClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected SdmxWebManagerClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected SdmxWebManagerClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Sdmxdl.Format.Protobuf.Web.SdmxWebSource> GetSources(global::Sdmxdl.Grpc.Empty request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetSources(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Sdmxdl.Format.Protobuf.Web.SdmxWebSource> GetSources(global::Sdmxdl.Grpc.Empty request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetSources, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.Web.MonitorReport GetMonitorReport(global::Sdmxdl.Grpc.SourceRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMonitorReport(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.Web.MonitorReport GetMonitorReport(global::Sdmxdl.Grpc.SourceRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetMonitorReport, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.Web.MonitorReport> GetMonitorReportAsync(global::Sdmxdl.Grpc.SourceRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetMonitorReportAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.Web.MonitorReport> GetMonitorReportAsync(global::Sdmxdl.Grpc.SourceRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetMonitorReport, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Sdmxdl.Format.Protobuf.Dataflow> GetFlows(global::Sdmxdl.Grpc.SourceRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetFlows(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Sdmxdl.Format.Protobuf.Dataflow> GetFlows(global::Sdmxdl.Grpc.SourceRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetFlows, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.Dataflow GetFlow(global::Sdmxdl.Grpc.FlowRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetFlow(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.Dataflow GetFlow(global::Sdmxdl.Grpc.FlowRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetFlow, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.Dataflow> GetFlowAsync(global::Sdmxdl.Grpc.FlowRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetFlowAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.Dataflow> GetFlowAsync(global::Sdmxdl.Grpc.FlowRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetFlow, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.DataStructure GetStructure(global::Sdmxdl.Grpc.FlowRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetStructure(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.DataStructure GetStructure(global::Sdmxdl.Grpc.FlowRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetStructure, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.DataStructure> GetStructureAsync(global::Sdmxdl.Grpc.FlowRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetStructureAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.DataStructure> GetStructureAsync(global::Sdmxdl.Grpc.FlowRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetStructure, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.DataSet GetData(global::Sdmxdl.Grpc.KeyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetData(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::Sdmxdl.Format.Protobuf.DataSet GetData(global::Sdmxdl.Grpc.KeyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetData, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.DataSet> GetDataAsync(global::Sdmxdl.Grpc.KeyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetDataAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::Sdmxdl.Format.Protobuf.DataSet> GetDataAsync(global::Sdmxdl.Grpc.KeyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetData, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Sdmxdl.Format.Protobuf.Series> GetDataStream(global::Sdmxdl.Grpc.KeyRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetDataStream(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::Sdmxdl.Format.Protobuf.Series> GetDataStream(global::Sdmxdl.Grpc.KeyRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GetDataStream, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override SdmxWebManagerClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new SdmxWebManagerClient(configuration);
      }
    }

  }
}
#endregion
