using LanguageExt;
using sdmxDlClient.Models;

namespace sdmxDlClient;

public interface IClient
{
    Task StartServer( CancellationToken cancellationToken );

    /// <summary>
    /// Get all available sources.
    /// </summary>
    Task<Seq<Source>> GetSources();

    /// <summary>
    /// Get all flows available for a given source.
    /// </summary>
    Task<Seq<Flow>> GetFlows( Source? source );

    Option<DataStructure> GetStructure( Source? source , Flow? flow );

    Seq<SeriesKey> GetKeys( Source? source , Flow? flow , Seq<Dimension> dimensions );

    Seq<SeriesKey> GetKeys( Source? source , Flow? flow , string key );

    Task<Seq<Series>> GetDataStream( string fullPath );

    Task<Seq<Series>> GetDataStream( string sourceId , string flowRef , string key );

    Task<Seq<Series>> GetDataStream( Source? source , Flow? flow , SeriesKey? key );

    Seq<Series> GetData( string fullPath );

    Seq<Series> GetData( string sourceId , string flowRef , string key );

    Seq<Series> GetData( Source? source , Flow? flow , SeriesKey? key );
}