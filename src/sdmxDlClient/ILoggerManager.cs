namespace sdmxDlClient;

public interface ILoggerManager
{
    void Info( string message );

    void Info( string title , string message );

    void Warn( string message );

    void Warn( string title , string message );

    void Error( string message );

    void Error( string title , string message );

    void Error( Exception exception );

    void Error( string title , Exception exception );
}