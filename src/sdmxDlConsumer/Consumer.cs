using CliWrap;
using CliWrap.EventStream;
using LanguageExt;
using sdmxDlClient;
using sdmxDlClient.Models;
using System.Text;

namespace sdmxDlConsumer;

public class Consumer : IClient
{
    public async Task StartServer( CancellationToken cancellationToken )
    {
        var cmd = await Cli.Wrap( "java" )
            .WithArguments( new[]
            {
                "--version"
            } )
            .ExecuteAsync( cancellationToken );
    }

    public Seq<CodeLabel> GetCodes( Source source , Flow flow , Dimension dimension )
    {
        return Seq<CodeLabel>.Empty;
    }

    public Seq<Dimension> GetDimensions( Source? source , Flow? flow )
    {
        return Seq<Dimension>.Empty;
    }

    public Seq<Flow> GetFlows( Source? source )
    {
        return Seq<Flow>.Empty;
    }

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , Seq<Dimension> dimensions )
    {
        return Seq<SeriesKey>.Empty;
    }

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , string key )
    {
        return Seq<SeriesKey>.Empty;
    }

    public Seq<Source> GetSources()
    {
        return Seq<Source>.Empty;
    }

    public Seq<DataSeries[]> GetData( Source source , Flow flow , SeriesKey key )
        => GetData( $"{source.Name} {flow.Label} {key.Series}" );

    public Seq<DataSeries[]> GetData( string fullPath )
    {
        return Seq<DataSeries[]>.Empty;
    }
}