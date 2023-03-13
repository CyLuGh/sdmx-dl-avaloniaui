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

    /// <summary>
    /// Get dimensions for a given flow in a source.
    /// </summary>
    Seq<Dimension> GetDimensions( Source? source , Flow? flow );

    Seq<CodeLabel> GetCodes( Source source , Flow flow , Dimension dimension );

    Seq<SeriesKey> GetKeys( Source? source , Flow? flow , Seq<Dimension> dimensions );

    Seq<SeriesKey> GetKeys( Source? source , Flow? flow , string key );

    Seq<DataSeries[]> GetData( Source source , Flow flow , SeriesKey key );

    Seq<DataSeries[]> GetData( string fullPath );
}