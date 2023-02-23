using System.Text.RegularExpressions;

namespace sdmxDlClient.Models;

public record Flow
{
    private static readonly Regex _regex = new( @"(?<=\:)(.*?)(?=\()" );

    public required string Ref { get; init; }
    public required string Label { get; init; }

    public string InputRef => _regex.Match( Ref ).Value;

    public override string ToString()
        => Label;
}