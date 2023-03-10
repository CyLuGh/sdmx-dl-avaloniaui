using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using sdmxDlClient.Models;
using System;
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
        _messagesCache.AddOrUpdate( new LogMessage( MessageKind.Info , message ) );
    }

    public void Warn( string message )
    {
        _messagesCache.AddOrUpdate( new LogMessage( MessageKind.Warn , message ) );
    }

    public void Error( string message )
    {
        _messagesCache.AddOrUpdate( new LogMessage( MessageKind.Error , message ) );
    }

    public void Error( Exception exception )
    {
        _messagesCache.AddOrUpdate( new LogMessage( MessageKind.Error , exception.Message ) );
    }
}