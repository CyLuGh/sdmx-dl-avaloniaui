using LanguageExt;
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
    public Seq<DataSeries[]> DataSeries { get; }
    public HashMap<string , string> GeneratedFields { get; }

    public string Header => $"{SeriesKey.Series}";

    [Reactive] public string PeriodFormatter { get; set; } = "yyyy-MM";
    [Reactive] public string ValueFormatter { get; set; } = "N2";

    public Seq<IDisplayData?> DisplaySeries { [ObservableAsProperty] get; }

    private ReactiveCommand<(string, string) , Seq<IDisplayData?>>? BuildDisplaySeriesCommand { get; set; }
    public ReactiveCommand<TimeSeriesDisplayViewModel,RxUnit>? DisposeCommand { get; init; }

    public TimeSeriesDisplayViewModel( Source source , Flow flow , SeriesKey seriesKey , Seq<DataSeries[]> dataSeries )
    {
        Source = source;
        Flow = flow;
        SeriesKey = seriesKey;
        DataSeries = dataSeries;

        GeneratedFields = dataSeries
            .Flatten()
            .Select( ds => ds.Series )
            .Distinct()
            .OrderBy( s => s )
            .Select( ( s , i ) => (Name: s, Field: $"Field_{i}") )
            .ToHashMap();

        InitializeCommands();

        BuildDisplaySeriesCommand!
            .ToPropertyEx( this , x => x.DisplaySeries , initialValue: Seq<IDisplayData?>.Empty , scheduler: RxApp.MainThreadScheduler );

        this.WhenAnyValue( x => x.PeriodFormatter , x => x.ValueFormatter )
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

            var data = DataSeries.Flatten()
                .GroupBy( x => x.ObsPeriod )
                .Select( g =>
                {
                    var displayData = (IDisplayData?) System.Activator.CreateInstance( generatedType , g.Key.ToString( periodFormatter ) );

                    foreach ( var s in g )
                    {
                        generatedType.InvokeMember( GeneratedFields[s.Series] , System.Reflection.BindingFlags.SetProperty , null , displayData , new object[] { ( s.ObsValue ?? double.NaN ).ToString( valueFormatter ) } );
                    }

                    return displayData;
                } )
                .ToSeq();

            return data;
        } ) );
    }
}