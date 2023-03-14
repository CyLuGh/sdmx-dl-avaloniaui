using LanguageExt;

namespace sdmxDlClient.Models;

public record Series
{
    public required string Key { get; init; }
    public IDictionary<string , string>? Meta { get; init; }
    public Seq<SeriesObs> Obs { get; set; }
}