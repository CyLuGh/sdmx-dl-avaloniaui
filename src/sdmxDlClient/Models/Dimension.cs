namespace sdmxDlClient.Models;

public record Dimension
{
    public required string Id { get; init; } = string.Empty;
    public required string Name { get; init; } = string.Empty;
    public required CodeList CodeList { get; init; }
    public required int Position { get; init; }
}