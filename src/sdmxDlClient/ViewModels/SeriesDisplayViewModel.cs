using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels;

public class SeriesDisplayViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IClient _client;
    private readonly ILoggerManager _loggerManager;

    public ViewModelActivator Activator { get; }

    [Reactive] public TimeSeriesDisplayViewModel? SelectedTimeSeriesDisplay { get; set; }
    public bool HasSeries { [ObservableAsProperty] get; }

    private readonly SourceCache<TimeSeriesDisplayViewModel , (Source, Flow, SeriesKey)> _timeSeriesCache
        = new( x => (x.Source, x.Flow, x.SeriesKey) );

    private ReadOnlyObservableCollection<TimeSeriesDisplayViewModel>? _timeSeries;
    public ReadOnlyObservableCollection<TimeSeriesDisplayViewModel>? TimeSeries => _timeSeries;

    public ReactiveCommand<string , (Source, Flow, SeriesKey)>? ParseKeyCommand { get; private set; }
    public ReactiveCommand<(Source, Flow, SeriesKey) , TimeSeriesDisplayViewModel>? FetchDataCommand { get; private set; }

    public SeriesDisplayViewModel( IClient client , ILoggerManager loggerManager )
    {
        _client = client;
        _loggerManager = loggerManager;
        Activator = new();

        InitializeCommands();
        OnActivated();
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            _timeSeriesCache.Connect()
                .Sort( SortExpressionComparer<TimeSeriesDisplayViewModel>.Ascending( ts => ts.Header ) )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Bind( out _timeSeries )
                .DisposeMany()
                .Subscribe()
                .DisposeWith( disposables );

            _timeSeriesCache.CountChanged
                .Select( cnt => cnt > 0 )
                .ToPropertyEx( this , x => x.HasSeries , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            FetchDataCommand!
                .Do( ts => _timeSeriesCache.AddOrUpdate( ts ) )
                .Delay( TimeSpan.FromMilliseconds( 20 ) )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Do( ts => SelectedTimeSeriesDisplay = ts )
                .Subscribe()
                .DisposeWith( disposables );

            FetchDataCommand!.ThrownExceptions.Subscribe( exc => _loggerManager.Error( exc ) );
        } );
    }

    private void InitializeCommands()
    {
        ParseKeyCommand = ReactiveCommand.CreateFromObservable( ( string key ) => Observable.Start( () =>
        {
            return (new Source() { Id = "" }, new Flow() { Name = "" , Ref = "" , StructureRef = "" }, new SeriesKey( "" ));
        } ) );

        FetchDataCommand = ReactiveCommand.CreateFromObservable( ( (Source, Flow, SeriesKey) t ) => Observable.Start( () =>
        {
            var (source, flow, seriesKey) = t;

            return _timeSeriesCache.Items
                .Find( ts => ts.Source.Equals( source ) && ts.Flow.Equals( flow ) && ts.SeriesKey.Equals( seriesKey ) )
                .Match( ts => ts ,
                    () =>
                    {
                        var data = _client.GetData( source , flow , seriesKey );
                        return new TimeSeriesDisplayViewModel( source , flow , seriesKey , data )
                        {
                            DisposeCommand = ReactiveCommand.Create( ( TimeSeriesDisplayViewModel ts ) => _timeSeriesCache.Remove( ts ) )
                        };
                    } );
        } ) );
    }
}