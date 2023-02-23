namespace sdmxDlClient.Models;

public record Source
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public string Aliases { get; init; } = string.Empty;
    public string Driver { get; init; } = string.Empty;
    public string Dialect { get; init; } = string.Empty;
    public string Endpoint { get; init; } = string.Empty;
    public string Properties { get; init; } = string.Empty;
    public string Website { get; init; } = string.Empty;
    public string Monitor { get; init; } = string.Empty;
}