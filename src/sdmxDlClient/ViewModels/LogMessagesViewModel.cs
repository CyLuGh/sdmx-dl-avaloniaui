using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using sdmxDlClient.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace sdmxDlClient.ViewModels;

public class LogMessagesViewModel : ReactiveObject, ILoggerManager
{
    private readonly SourceCache<LogMessage , Guid> _messagesCache = new( x => x.InternalId );

    private ReadOnlyObservableCollection<LogMessage>? _messages;
    public ReadOnlyObservableCollection<LogMessage>? Messages => _messages;

    public LogMessagesViewModel()
    {
        _messagesCache
            .Connect()
            .Sort( SortExpressionComparer<LogMessage>.Descending( x => x.TimeStamp ) )
            .ObserveOn( RxApp.MainThreadScheduler )
            .Bind( out _messages )
            .DisposeMany()
            .Subscribe();
    }

    public void Info( string message )
    {
        throw new NotImplementedException();
    }

    public void Warn( string message )
    {
        throw new NotImplementedException();
    }

    public void Error( string message )
    {
        throw new NotImplementedException();
    }

    public void Error( Exception exception )
    {
        _messagesCache.AddOrUpdate( new LogMessage( exception.Message ) );
    }
}