using LanguageExt;

namespace sdmxDlClient.Models;

public record DataSet
{
    public required string Ref { get; init; }
    public Seq<Series> Series { get; init; }
    public required DataQuery Query { get; init; }
}