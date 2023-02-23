using LanguageExt;
using sdmxDlClient;
using sdmxDlClient.Models;

namespace sdmxDlFaker;

public class ClientFaker : IClient
{
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
                .Select( i => new Dimension { Concept = $"Dim {i}" , Label = $"Dim {i} Source {source.Name} Flow {flow.Label}" } )
                .ToSeq()
            : Seq<Dimension>.Empty;
}