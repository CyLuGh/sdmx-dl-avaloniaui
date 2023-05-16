using LanguageExt;
using MoreLinq;
using sdmxDlClient;
using sdmxDlClient.Models;

namespace sdmxDlFaker;

public class ClientFaker : IClient
{
    public async Task StartServer( CancellationToken cancellationToken )
    {
        while ( !cancellationToken.IsCancellationRequested )
            await Task.Delay( 1000 , cancellationToken );

        cancellationToken.ThrowIfCancellationRequested();
    }

    public Task<Seq<Source>> GetSources()
        => Task.FromResult( Enumerable.Range( 1 , 4 )
            .Select( i => new Source { Id = $"Src {i}" , Names = new[] { ("en", $"Test {i}") }.ToDictionary() } )
            .ToSeq() );

    public Task<Seq<Flow>> GetFlows( Source? source )
        => Task.FromResult( source != null
            ? Enumerable.Range( 1 , 10 )
                .Select( i => new Flow { Ref = $"{i}" , StructureRef = $"{i}" , Name = $"{i} from {source.Id}" } )
                .ToSeq()
            : Seq<Flow>.Empty );

    public Option<DataStructure> GetStructure( Source? source , Flow? flow )
    {
        if ( source == null || flow == null ) return Option<DataStructure>.None;

        return new DataStructure
        {
            Name = flow.Name ,
            Ref = flow.Ref ,
            PrimaryMeasureId = "test" ,
            Dimensions = GetDimensions( source , flow ).ToArray()
        };
    }

    private static Seq<Dimension> GetDimensions( Source? source , Flow? flow )
        => source != null && flow != null
            ? Enumerable.Range( 1 , 5 )
                .Select( i => new Dimension
                {
                    Id = $"Dim {i}" ,
                    Name = $"Dim {i} Source {source.Id} Flow {flow.Name}" ,
                    Position = i ,
                    CodeList = new CodeList { Ref = $"{i}" , Codes = GetCodes( source , flow , $"Dim {i}" ) }
                } )
                .ToSeq()
            : Seq<Dimension>.Empty;

    private static IDictionary<string , string> GetCodes( Source source , Flow flow , string dimension )
         => Enumerable.Range( 0 , 5 )
            .Select( i => ($"{(char) ( 'A' + i )}", $"{i} {source} {flow} {dimension}") )
            .ToDictionary();

    public Seq<SeriesKey> GetKeys( Source? source , Flow? flow , Seq<Dimension> dimensions )
    {
        if ( source == null || flow == null || dimensions.IsEmpty )
            return Seq<SeriesKey>.Empty;

        var count = dimensions.Length;
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

    public Seq<Series> GetData( string fullPath )
    {
        if ( fullPath.Contains( "error" , StringComparison.OrdinalIgnoreCase ) )
            throw new ArgumentException( "Throw an error on purpose." );

        var elements = fullPath.Split( ' ' );
        if ( elements.Length < 3 )
            return Seq<Series>.Empty;

        return GetData( elements[0] , elements[1] , elements[2] );
    }

    public Seq<Series> GetData( string sourceId , string flowRef , string key )
        => Enumerable.Range( 0 , Random.Shared.Next( 1 , 5 ) )
            .Select( i => GenerateSeries( $"Test {i}" ) )
            .ToSeq();

    public Seq<Series> GetData( Source? source , Flow? flow , SeriesKey? key )
    {
        if ( source == null || flow == null || key == null )
            return Seq<Series>.Empty;

        return GetData( source.Id , flow.Ref , key.Series );
    }

    public Task<Seq<Series>> GetDataStream( string fullPath )
    {
        if ( fullPath.Contains( "error" , StringComparison.OrdinalIgnoreCase ) )
            throw new ArgumentException( "Throw an error on purpose." );

        var elements = fullPath.Split( ' ' );
        if ( elements.Length < 3 )
            return Task.FromResult( Seq<Series>.Empty );

        return GetDataStream( elements[0] , elements[1] , elements[2] );
    }

    public Task<Seq<Series>> GetDataStream( string sourceId , string flowRef , string key )
        => Task.FromResult( GetData( sourceId , flowRef , key ) );

    public Task<Seq<Series>> GetDataStream( Source? source , Flow? flow , SeriesKey? key )
    {
        if ( source == null || flow == null || key == null )
            return Task.FromResult( Seq<Series>.Empty );

        return GetDataStream( source.Id , flow.Ref , key.Series );
    }

    private static Series GenerateSeries( string name )
        => new()
        {
            Key = name ,
            Obs = Enumerable.Range( 0 , 60 )
                .Select( i => new SeriesObs { Period = new DateTimeOffset( new DateTime( 2015 , 1 , 1 ) , TimeSpan.Zero ).AddMonths( i ) , Value = Random.Shared.NextDouble() } )
                .ToSeq()
        };
}