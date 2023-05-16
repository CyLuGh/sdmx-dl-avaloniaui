using System.Globalization;

namespace sdmxDlClient.Models;

public record Source
{
    public required string Id { get; init; }
    public IDictionary<string , string> Names { get; init; } = new Dictionary<string , string>();
    public string Driver { get; init; } = string.Empty;
    public string Dialect { get; init; } = string.Empty;
    public string Endpoint { get; init; } = string.Empty;
    public IDictionary<string , string> Properties { get; init; } = new Dictionary<string , string>();
    public string[] Aliases { get; init; } = Array.Empty<string>();
    public string Website { get; init; } = string.Empty;
    public string Monitor { get; init; } = string.Empty;
    public string MonitorWebsite { get; init; } = string.Empty;

    public string Name
    {
        get
        {
            var language = CultureInfo.CurrentCulture.Name[..2];

            if ( Names.TryGetValue( language , out var languageName ) )
                return languageName;

            if ( Names.TryGetValue( "en" , out var internationalName ) )
                return internationalName;

            if ( Names.Values.Any() )
                return Names.Values.First();

            return string.Empty;
        }
    }

    public override string ToString()
        => Id;
}