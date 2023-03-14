using CliWrap;
using CliWrap.Buffered;
using Google.Protobuf.Collections;
using Grpc.Net.Client;
using LanguageExt;
using Mapster;
using Sdmxdl.Grpc;
using sdmxDlClient;
using sdmxDlClient.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace sdmxDlConsumer;

public class Consumer : IClient
{
    private readonly SemaphoreSlim _semaphore = new SemaphoreSlim( 1 );
    private SdmxWebManager.SdmxWebManagerClient? _client;

    private SdmxWebManager.SdmxWebManagerClient Client
    {
        get
        {
            _semaphore.Wait();

            if ( _client != null )
            {
                _semaphore.Release();
                return _client;
            }

            var channel = GrpcChannel.ForAddress( "http://localhost:4567" );
            _client = new( channel );

            _semaphore.Release();
            return _client;
        }
    }

    static Consumer()
    {
        // See https://github.com/MapsterMapper/Mapster/issues/316
        TypeAdapterConfig.GlobalSettings.Default
            .UseDestinationValue( member => member.SetterModifier == AccessModifier.None &&
                                            member.Type.IsGenericType &&
                                            member.Type.GetGenericTypeDefinition() == typeof( RepeatedField<> ) );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Web.SdmxWebSource , Source>
            .NewConfig()
            .Map( dest => dest.Id , src => src.Id )
            .Map( dest => dest.Names , src => src.Names )
            .Map( dest => dest.Driver , src => src.Driver )
            .Map( dest => dest.Dialect , src => src.HasDialect ? src.Dialect : string.Empty )
            .Map( dest => dest.Endpoint , src => src.Endpoint )
            .Map( dest => dest.Properties , src => src.Properties )
            .Map( dest => dest.Aliases , src => src.Aliases.ToArray() )
            .Map( dest => dest.Website , src => src.HasWebsite ? src.Website : string.Empty )
            .Map( dest => dest.Monitor , src => src.HasMonitor ? src.Monitor : string.Empty )
            .Map( dest => dest.MonitorWebsite , src => src.HasMonitorWebsite ? src.MonitorWebsite : string.Empty );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Dataflow , Flow>
            .NewConfig()
            .Map( dest => dest.Ref , src => src.Ref )
            .Map( dest => dest.StructureRef , src => src.StructureRef )
            .Map( dest => dest.Name , src => src.Name )
            .Map( dest => dest.Description , src => src.HasDescription ? src.Description : string.Empty );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Codelist , CodeList>
            .NewConfig()
            .Map( dest => dest.Ref , src => src.Ref )
            .Map( dest => dest.Codes , src => src.Codes );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Dimension , Dimension>
            .NewConfig()
            .Map( dest => dest.Id , src => src.Id )
            .Map( dest => dest.Name , src => src.Name )
            .Map( dest => dest.Position , src => src.Position )
            .Map( dest => dest.CodeList , src => src.Codelist );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Attribute , sdmxDlClient.Models.Attribute>
            .NewConfig()
            .Map( dest => dest.Id , src => src.Id )
            .Map( dest => dest.Name , src => src.Name )
            .Map( dest => dest.Relationship , src => src.Relationship )
            .Map( dest => dest.CodeList , src => src.Codelist );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Series , Series>
            .NewConfig()
            .Map( dest => dest.Obs , src => src.Obs.ToSeq() )
            .Map( dest => dest.Key , src => src.Key )
            .Map( dest => dest.Meta , src => src.Meta );

        TypeAdapterConfig<Sdmxdl.Format.Protobuf.Obs , SeriesObs>
            .NewConfig()
            .Map( dest => dest.Value , src => src.Value )
            .Map( dest => dest.Period , src => DateTimeOffset.Parse( src.Period ) )
            .Map( dest => dest.Meta , src => src.Meta );
    }

    public async Task StartServer( CancellationToken cancellationToken )
    {
        var cmd = await Cli.Wrap( "java" )
            .WithArguments( new[]
            {
                "-jar",
                @"lib\sdmx-dl-grpc-3.0.0-beta.10-bin.jar"
            } )
            .ExecuteBufferedAsync( cancellationToken );

        if ( !string.IsNullOrEmpty( cmd.StandardError ) )
            throw new SdmxDlServerException( $"SDMXDL server has encountered an exception:{System.Environment.NewLine}{cmd.StandardError}" );
    }

    public Option<DataStructure> GetStructure( Source? source , Flow? flow )
    {
        if ( source == null || flow == null ) return Option<DataStructure>.None;

        var structure = Client.GetStructure( new FlowRequest { Source = source.Id , Flow = flow.Ref } );
        return structure.Adapt<DataStructure>();
    }

    public async Task<Seq<Flow>> GetFlows( Source? source )
    {
        if ( source == null ) return Seq<Flow>.Empty;

        var cancelSource = new CancellationTokenSource();
        var flows = new Queue<Flow>();
        var response = Client.GetFlows( new SourceRequest { Source = source.Id } );
        while ( await response.ResponseStream.MoveNext( cancelSource.Token ) )
        {
            flows.Enqueue( response.ResponseStream.Current.Adapt<Flow>() );
        }
        return flows.ToSeq();
    }

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , Seq<Dimension> dimensions )
    {
        return Seq<SeriesKey>.Empty;
    }

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , string key )
    {
        return Seq<SeriesKey>.Empty;
    }

    public async Task<Seq<Source>> GetSources()
    {
        var cancelSource = new CancellationTokenSource();
        var sources = new Queue<Source>();
        var response = Client.GetSources( new Google.Protobuf.WellKnownTypes.Empty() );
        while ( await response.ResponseStream.MoveNext( cancelSource.Token ) )
        {
            sources.Enqueue( response.ResponseStream.Current.Adapt<Source>() );
        }
        return sources.ToSeq();
    }

    public Seq<Series> GetData( string fullPath )
    {
        var elements = fullPath.Split( ' ' );
        if ( elements.Length < 3 )
            return Seq<Series>.Empty;

        return GetData( elements[0] , elements[1] , elements[2] );
    }

    public Seq<Series> GetData( Source? source , Flow? flow , SeriesKey? key )
    {
        if ( source == null || flow == null || key == null )
            return Seq<Series>.Empty;

        return GetData( source.Id , flow.Ref , key.Series );
    }

    public Seq<Series> GetData( string sourceId , string flowRef , string key )
    {
        var dataSet = Client.GetData( new KeyRequest { Source = sourceId , Flow = flowRef , Key = key } )
            .Adapt<DataSet>();

        return dataSet.Series;
    }

    public Task<Seq<Series>> GetDataStream( string fullPath )
    {
        var elements = fullPath.Split( ' ' );
        if ( elements.Length < 3 )
            return Task.FromResult( Seq<Series>.Empty );

        return GetDataStream( elements[0] , elements[1] , elements[2] );
    }

    public Task<Seq<Series>> GetDataStream( Source? source , Flow? flow , SeriesKey? key )
    {
        if ( source == null || flow == null || key == null )
            return Task.FromResult( Seq<Series>.Empty );

        return GetDataStream( source.Id , flow.Ref , key.Series );
    }

    public async Task<Seq<Series>> GetDataStream( string sourceId , string flowRef , string key )
    {
        var cancelSource = new CancellationTokenSource();
        var series = new Queue<Series>();
        var response = Client.GetDataStream( new KeyRequest { Source = sourceId , Flow = flowRef , Key = key } );
        while ( await response.ResponseStream.MoveNext( cancelSource.Token ) )
        {
            series.Enqueue( response.ResponseStream.Current.Adapt<Series>() );
        }
        return series.ToSeq();
    }
}