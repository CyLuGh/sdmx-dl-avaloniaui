using System.Text.RegularExpressions;

namespace sdmxDlClient.Models;

public record Flow
{
    private static readonly Regex _regex = new( @"(?<=\:)(.*?)(?=\()" );

    public required string Ref { get; init; }
    public required string StructureRef { get; init; }
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;

    public string InputRef => _regex.Match( Ref ).Value;

    public override string ToString()
        => Name;
}