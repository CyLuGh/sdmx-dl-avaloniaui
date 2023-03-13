namespace sdmxDlClient.Models;

public enum AttributeRelationship
{
    UNKNOWN = 0,
    DATAFLOW = 1,
    GROUP = 2,
    SERIES = 3,
    OBSERVATION = 4
}

public record Attribute
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public CodeList? CodeList { get; init; }
    public AttributeRelationship Relationship { get; init; }
}