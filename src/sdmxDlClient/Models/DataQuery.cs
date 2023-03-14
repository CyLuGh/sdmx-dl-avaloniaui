namespace sdmxDlClient.Models;

public enum DataDetail
{
    FULL = 0,
    DATA_ONLY = 1,
    SERIES_KEY_ONLY = 2,
    NO_DATA = 3
}

public record DataQuery( string Key , DataDetail Detail );