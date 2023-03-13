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
    internal SourceCache<LogMessage , Guid> MessagesCache { get; } = new( x => x.InternalId );

    private readonly ReadOnlyObservableCollection<LogMessage>? _messages;
    public ReadOnlyObservableCollection<LogMessage>? Messages => _messages;

    public LogMessagesViewModel()
    {
        MessagesCache
            .Connect()
            .Sort( SortExpressionComparer<LogMessage>.Descending( x => x.TimeStamp ) )
            .ObserveOn( RxApp.MainThreadScheduler )
            .Bind( out _messages )
            .DisposeMany()
            .Subscribe();
    }

    public void Info( string message )
    {
        MessagesCache.AddOrUpdate( new LogMessage( MessageKind.Info , message ) );
    }

    public void Warn( string message )
    {
        MessagesCache.AddOrUpdate( new LogMessage( MessageKind.Warn , message ) );
    }

    public void Error( string message )
    {
        MessagesCache.AddOrUpdate( new LogMessage( MessageKind.Error , message ) );
    }

    public void Error( Exception exception )
    {
        MessagesCache.AddOrUpdate( new LogMessage( MessageKind.Error , exception.Message ) );
    }
}