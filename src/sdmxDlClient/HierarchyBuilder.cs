using LanguageExt;
using sdmxDlClient.ViewModels;

namespace sdmxDlClient;

public static class HierarchyBuilder
{
    public static Seq<HierarchicalCodeLabelViewModel> Build( Seq<DimensionViewModel> dimensions , Seq<LanguageExt.HashSet<string>> keysOccurrences )
    {
        if ( dimensions.IsEmpty )
            return Seq<HierarchicalCodeLabelViewModel>.Empty;

        var key = string.Join( "." , Enumerable.Range( 0 , dimensions.Count() )
                .Select( _ => string.Empty ) );

        return Build( key , 1 , dimensions , keysOccurrences );
    }

    public static Seq<HierarchicalCodeLabelViewModel> Build( string key , int desiredPosition , Seq<DimensionViewModel> dimensions , Seq<LanguageExt.HashSet<string>> keysOccurrences )
    {
        if ( desiredPosition > dimensions.Count )
            return Seq<HierarchicalCodeLabelViewModel>.Empty;

        var dim = dimensions[desiredPosition - 1];
        return dim
            .Codes
            .Where( o => keysOccurrences.Length == 0
                || keysOccurrences[dim.Position - 1].Length == 0
                || keysOccurrences[dim.Position - 1].Contains( o.Key ) )
            .Select( o =>
            {
                var splits = key.Split( '.' );
                if ( dim.Position - 1 < splits.Length )
                    splits[dim.Position - 1] = o.Key;
                return new HierarchicalCodeLabelViewModel( dimensions , keysOccurrences , lazyLoad: desiredPosition != dimensions.Count )
                {
                    Label = o.Value ,
                    Code = string.Join( "." , splits ) ,
                    Position = desiredPosition
                };
            } )
            .OrderBy( o => o.Label )
            .ToSeq();
    }
}