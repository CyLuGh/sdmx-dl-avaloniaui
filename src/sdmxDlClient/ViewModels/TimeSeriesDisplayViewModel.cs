using LanguageExt;
using MoreLinq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;
using System.Reactive.Linq;

namespace sdmxDlClient.ViewModels;

public class TimeSeriesDisplayViewModel : ReactiveObject
{
    public Source Source { get; }
    public Flow Flow { get; }
    public SeriesKey SeriesKey { get; }
    public Seq<Series> DataSeries { get; }
    public HashMap<string , string> GeneratedFields { get; }

    public string Header => $"{SeriesKey.Series}";

    [Reactive] public string PeriodFormatter { get; set; } = "yyyy-MM";
    public string? ValueFormatter { [ObservableAsProperty] get; }
    [Reactive] public int DecimalCount { get; set; } = 2;

    public Seq<IDisplayData?> DisplaySeries { [ObservableAsProperty] get; }

    private ReactiveCommand<(string, string) , Seq<IDisplayData?>>? BuildDisplaySeriesCommand { get; set; }
    public ReactiveCommand<TimeSeriesDisplayViewModel , RxUnit>? DisposeCommand { get; init; }

    public TimeSeriesDisplayViewModel( Source source , Flow flow , SeriesKey seriesKey , Seq<Series> dataSeries )
    {
        Source = source;
        Flow = flow;
        SeriesKey = seriesKey;
        DataSeries = dataSeries;

        GeneratedFields = dataSeries
            .Select( ds => ds.Key )
            .Distinct()
            .OrderBy( s => s )
            .Select( ( s , i ) => (Name: s, Field: $"Field_{i}") )
            .ToHashMap();

        InitializeCommands();

        BuildDisplaySeriesCommand!
            .ToPropertyEx( this , x => x.DisplaySeries , initialValue: Seq<IDisplayData?>.Empty , scheduler: RxApp.MainThreadScheduler );

        this.WhenAnyValue( x => x.DecimalCount )
            .Throttle( TimeSpan.FromMilliseconds( 50 ) )
            .DistinctUntilChanged()
            .Select( c => $"N{c}" )
            .ToPropertyEx( this , x => x.ValueFormatter , initialValue: "N2" , scheduler: RxApp.MainThreadScheduler );

        this.WhenAnyValue( x => x.PeriodFormatter , x => x.ValueFormatter! )
            .InvokeCommand( BuildDisplaySeriesCommand );
    }

    private void InitializeCommands()
    {
        BuildDisplaySeriesCommand = ReactiveCommand.CreateFromObservable( ( (string, string) t ) => Observable.Start( () =>
        {
            var (periodFormatter, valueFormatter) = t;

            var generatedType = ReflectionTypeGenerator.GenerateTypedObject(
                GeneratedFields.Select( t => (t.Value, typeof( string )) ) ,
                constructorParameters: new[] { ("Period", typeof( string )) } ,
                interfaces: new[] { typeof( IDisplayData ) } );

            var data = DataSeries.Select( d => d.Obs.Select( x => (d.Key, Obs: x) ) )
                .Flatten()
                .GroupBy( t => t.Obs.Period )
                .Select( g =>
                {
                    var displayData = (IDisplayData?) System.Activator.CreateInstance( generatedType , g.Key.ToString( periodFormatter ) );

                    foreach ( var (key, obs) in g )
                    {
                        generatedType.InvokeMember( GeneratedFields[key] , System.Reflection.BindingFlags.SetProperty , null , displayData , new object[] { obs.Value.ToString( valueFormatter ) } );
                    }

                    return displayData;
                } )
                .ToSeq();

            return data;
        } ) );
    }
}