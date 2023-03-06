using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels;

public class SeriesDisplayViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IClient _client;
    public ViewModelActivator Activator { get; }

    private readonly SourceCache<TimeSerieDisplayViewModel , (Source, Flow, SeriesKey)> _timeSeriesCache
        = new( x => (x.Source, x.Flow, x.SeriesKey) );

    private ReadOnlyObservableCollection<TimeSerieDisplayViewModel>? _timeSeries;
    public ReadOnlyObservableCollection<TimeSerieDisplayViewModel>? TimeSeries => _timeSeries;

    public ReactiveCommand<string , (Source, Flow, SeriesKey)>? ParseKeyCommand { get; private set; }
    public ReactiveCommand<(Source, Flow, SeriesKey) , TimeSerieDisplayViewModel>? FetchDataCommand { get; private set; }

    public SeriesDisplayViewModel( IClient client )
    {
        _client = client;
        Activator = new();

        InitializeCommands();
        OnActivated();
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            _timeSeriesCache.Connect()
                .ObserveOn( RxApp.MainThreadScheduler )
                .Bind( out _timeSeries )
                .DisposeMany()
                .Subscribe()
                .DisposeWith( disposables );

            FetchDataCommand!
                .Do( ts => _timeSeriesCache.AddOrUpdate( ts ) )
                .Subscribe()
                .DisposeWith( disposables );
        } );
    }

    private void InitializeCommands()
    {
        ParseKeyCommand = ReactiveCommand.CreateFromObservable( ( string key ) => Observable.Start( () =>
        {
            return (new Source() { Description = "" , Name = "" }, new Flow() { Label = "" , Ref = "" }, new SeriesKey( "" ));
        } ) );

        FetchDataCommand = ReactiveCommand.CreateFromObservable( ( (Source, Flow, SeriesKey) t ) => Observable.Start( () =>
        {
            var (source, flow, seriesKey) = t;
            return new TimeSerieDisplayViewModel( source , flow , seriesKey );
        } ) );
    }
}