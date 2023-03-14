namespace sdmxDlClient.Models;

public record SeriesObs
{
    public required DateTimeOffset Period { get; init; }
    public required double Value { get; init; }
    public IDictionary<string , string>? Meta { get; init; }
}