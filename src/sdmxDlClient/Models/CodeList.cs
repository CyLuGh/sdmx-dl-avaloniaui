namespace sdmxDlClient.Models;

public record CodeList
{
    public required string Ref { get; init; }
    public IDictionary<string , string> Codes { get; init; } = new Dictionary<string , string>();
}