using DynamicData;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClient.ViewModels;

public class SeriesDisplayViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IClient _client;
    public ViewModelActivator Activator { get; }

    //private readonly SourceCache<TimeSerieDisplayViewModel , (Source, Flow, SeriesKey)> _timeSeriesCache
    //    = new( x => (x.Source, x.Flow, x.SeriesKey) );

    //private ReadOnlyObservableCollection<TimeSerieDisplayViewModel>? _timeSeries;
    //public ReadOnlyObservableCollection<TimeSerieDisplayViewModel>? TimeSeries => _timeSeries;

    public ReactiveCommand<string , (Source, Flow, SeriesKey)>? ParseKeyCommand { get; private set; }
    public ReactiveCommand<(Source, Flow, SeriesKey) , TimeSerieDisplayViewModel>? FetchDataCommand { get; private set; }

    //public Seq<TimeSerieDisplayViewModel> TimeSeries { [ObservableAsProperty] get; }
    public Seq<string> TimeSeries { [ObservableAsProperty] get; }

    public int TestCount { [ObservableAsProperty] get; }

    [Reactive] public int TestCount2 { get; set; }

    public SeriesDisplayViewModel( IClient client )
    {
        _client = client;
        Activator = new();

        InitializeCommands();

        //_timeSeriesCache.Connect()
        //        .ObserveOn( RxApp.MainThreadScheduler )
        //        .Bind( out _timeSeries )
        //        .DisposeMany()
        //        .Subscribe();

        OnActivated();

        this.Activator.Activated
            .Subscribe( _ =>
            {
                Console.WriteLine( "Activated" );
            } );

        this.Activator.Deactivated
            .Subscribe( _ =>
            {
                Console.WriteLine( "Deactivated" );
            } );
    }

    private void OnActivated()
    {
        FetchDataCommand!
               .Do( tss =>
               {
                   Console.WriteLine();
               } )
               .Select( ts => TimeSeries.Add( ts.Header ) )
               .Do( tss =>
               {
                   Console.WriteLine( tss.Length );
               } )
               .ToPropertyEx( this , x => x.TimeSeries , Seq<string>.Empty , scheduler: RxApp.MainThreadScheduler );

        this.WhenAnyValue( x => x.TimeSeries )
            .ObserveOn( RxApp.MainThreadScheduler )
            .Do( tss =>
            {
                Console.WriteLine( tss.Length );
                TestCount2 = tss.Length == 0 ? 42 : tss.Length;
            } )
            .Select( tss => tss.Length == 0 ? 42 : tss.Length )
            .ToPropertyEx( this , x => x.TestCount , scheduler: RxApp.MainThreadScheduler );
        //.Subscribe( tss =>
        //{
        //    Console.WriteLine( tss.Length );
        //}
        //);

        //this.WhenActivated( disposables =>
        //{
        //    this.WhenAnyValue( x => x.TimeSeries )
        //    .Subscribe( tss =>
        //    {
        //        Console.WriteLine( tss.Length );
        //    }
        //    ).DisposeWith( disposables );
        //} );
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
            //_timeSeriesCache.AddOrUpdate( new TimeSerieDisplayViewModel( source , flow , seriesKey ) );
            return new TimeSerieDisplayViewModel( source , flow , seriesKey );
        } ) );
    }
}