using DynamicData;
using LanguageExt;
using MoreLinq;
using sdmxDlClient;
using sdmxDlClient.Models;
using System.Linq;

namespace sdmxDlFaker;

public class ClientFaker : IClient
{
    public async Task StartServer( CancellationToken cancellationToken )
    {
        while ( !cancellationToken.IsCancellationRequested )
            await Task.Delay( 1000 , cancellationToken );

        cancellationToken.ThrowIfCancellationRequested();
    }

    public Seq<Source> GetSources()
        => Enumerable.Range( 1 , 4 )
            .Select( i => new Source { Name = $"Src {i}" , Description = $"Test {i}" } )
            .ToSeq();

    public Seq<Flow> GetFlows( Source? source )
        => source != null
            ? Enumerable.Range( 1 , 10 )
                .Select( i => new Flow { Ref = $"{i}" , Label = $"{i} from {source.Name}" } )
                .ToSeq()
            : Seq<Flow>.Empty;

    public Seq<Dimension> GetDimensions( Source? source , Flow? flow )
        => source != null && flow != null
            ? Enumerable.Range( 1 , 5 )
                .Select( i => new Dimension { Concept = $"Dim {i}" , Label = $"Dim {i} Source {source.Name} Flow {flow.Label}" , Position = i } )
                .ToSeq()
            : Seq<Dimension>.Empty;

    public Seq<CodeLabel> GetCodes( Source source , Flow flow , Dimension dimension )
        => Enumerable.Range( 0 , 5 )
            .Select( i => new CodeLabel( $"{(char) ( 'A' + i )}" , $"{i} {source} {flow} {dimension}" ) )
            .ToSeq();

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , Seq<Dimension> dimensions )
    {
        if ( source == null || flow == null || dimensions.IsEmpty )
            return Seq<SeriesKey>.Empty;

        var count = dimensions.Count( x => x.Position.HasValue );
        var key = string.Join( "." , Enumerable.Range( 0 , count )
            .Select( _ => string.Empty ) );

        return GetKeys( source , flow , key );
    }

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , string key )
    {
        if ( source == null || flow == null )
            return Seq<SeriesKey>.Empty;

        var count = key.Split( '.' ).Length;
        var codes = Seq.create( "A" , "B" , "C" , "D" , "E" );
        var elements = Enumerable.Range( 0 , count )
            .Select( _ => codes )
            .ToSeq();

        var combinations = elements.Skip( 1 ).Aggregate( elements[0] ,
            ( a , b ) => a.Cartesian( b , ( x , y ) => string.Join( "." , x , y ) ).ToSeq().Strict() );

        return combinations.Select( t => new SeriesKey( t ) );
    }

    public Seq<DataSeries[]> GetData( Source source , Flow flow , SeriesKey key )
        => GetData( $"{source.Name} {flow.Label} {key.Series}" );

    public Seq<DataSeries[]> GetData( string fullPath )
    {
        if ( string.IsNullOrWhiteSpace( fullPath ) )
            return Seq<DataSeries[]>.Empty;

        return Enumerable.Range( 0 , Random.Shared.Next( 5 ) )
            .Select( i => GenerateSeries( $"Test {i}" ) )
            .ToSeq();
    }

    private DataSeries[] GenerateSeries( string name )
        => Enumerable.Range( 0 , 60 )
            .Select( i => new DataSeries( name , "" , new DateTime( 2015 , 1 , 1 ).AddMonths( i ) , Random.Shared.NextDouble() ) )
        .ToArray();
}