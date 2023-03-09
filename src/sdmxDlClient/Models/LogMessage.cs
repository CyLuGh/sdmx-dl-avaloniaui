namespace sdmxDlClient.Models;

public enum MessageKind
{
    Info,
    Warn,
    Error
}

public readonly struct LogMessage
{
    public MessageKind Kind { get; }
    public string Message { get; }
    public string Title { get; }
    public DateTimeOffset TimeStamp { get; } = DateTimeOffset.Now;
    internal Guid InternalId { get; } = Guid.NewGuid();

    public LogMessage( string message )
    {
        Message = message;
        Title = string.Empty;
    }

    public LogMessage( MessageKind kind , string message )
    {
        Kind = kind;
        Message = message;
        Title = string.Empty;
    }

    public LogMessage( string message , string title ) : this( message )
    {
        Title = title;
    }

    public LogMessage( MessageKind kind , string message , string title ) : this( kind , message )
    {
        Title = title;
    }
}