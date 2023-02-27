namespace sdmxDlClient.Models;

public record SeriesKey( string Series )
{
    public override string ToString()
        => Series;
}