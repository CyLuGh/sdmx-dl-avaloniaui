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
    internal Guid InternalId { get; }

    public LogMessage( string message )
    {
        InternalId = Guid.NewGuid();
        Message = message;
        Title = string.Empty;
    }

    public LogMessage( MessageKind kind , string message )
    {
        InternalId = Guid.NewGuid();
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