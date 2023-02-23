namespace sdmxDlClient.Models;

public record Dimension
{
    public string Concept { get; init; } = string.Empty;
    public string Label { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public bool Coded { get; init; }
    public int? Position { get; init; }
}