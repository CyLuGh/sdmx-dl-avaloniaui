using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClient.ViewModels;

public class TimeSeriesDisplayViewModel : ReactiveObject
{
    public Source Source { get; }
    public Flow Flow { get; }
    public SeriesKey SeriesKey { get; }

    public string Header => $"{SeriesKey.Series}";

    [Reactive] public string PeriodFormatter { get; set; } = "yyyy-MM";
    [Reactive] public string ValueFormatter { get; set; } = "N2";
    [Reactive] public Seq<(string SeriesName, string Field)> GeneratedFields { get; set; } = Seq<(string, string)>.Empty;
    public Seq<DataSeries[]> DataSeries { [ObservableAsProperty] get; }
    public Seq<IDisplayData?> DisplaySeries { [ObservableAsProperty] get; }

    private ReactiveCommand<(Source, Flow, SeriesKey) , Seq<DataSeries[]>>? RetrieveSeriesDataCommand { get; set; }
    public Interaction<Seq<(string, string)> , RxUnit> BuildGridInteraction { get; } = new( RxApp.MainThreadScheduler );

    public TimeSeriesDisplayViewModel( IClient client , Source source , Flow flow , SeriesKey seriesKey )
    {
        Source = source;
        Flow = flow;
        SeriesKey = seriesKey;

        BuildGridInteraction.RegisterHandler( ctx => ctx.SetOutput( RxUnit.Default ) );
        InitializeCommands( client );

        RetrieveSeriesDataCommand!
            .ToPropertyEx( this , x => x.DataSeries , initialValue: Seq<DataSeries[]>.Empty , scheduler: RxApp.MainThreadScheduler );

        this.WhenAnyValue( x => x.GeneratedFields )
            .Subscribe( async gfs =>
            {
                await BuildGridInteraction.Handle( gfs );
            } );

        this.WhenAnyValue( x => x.DataSeries ,
            x => x.PeriodFormatter ,
            x => x.ValueFormatter )
            .Select( t =>
            {
                var (series, periodFormatter, valueFormatter) = t;

                Func<string , string> escaper = s => s.Replace( ' ' , '_' );
                GeneratedFields = series.Flatten()
                        .Select( ds => ds.Series )
                        .Distinct()
                        .Select( s => (s, escaper( s )) )
                        .ToSeq();

                var generatedType = ReflectionTypeGenerator.GenerateTypedObject(
                    GeneratedFields.Select( t => (t.Field, typeof( string )) ) ,
                    constructorParameters: new[] { ("Period", typeof( string )) } ,
                    interfaces: new[] { typeof( IDisplayData ) } );

                var data = series.Flatten()
                    .GroupBy( x => x.ObsPeriod )
                    .Select( g =>
                    {
                        var displayData = (IDisplayData) Activator.CreateInstance( generatedType , g.Key.ToString( periodFormatter ) );

                        foreach ( var s in g )
                        {
                            generatedType.InvokeMember( escaper( s.Series ) , System.Reflection.BindingFlags.SetProperty , null , displayData , new object[] { ( s.ObsValue ?? double.NaN ).ToString( valueFormatter ) } );
                        }

                        return displayData;
                    } )
                    .ToSeq();

                return data;
            } )
            .ToPropertyEx( this , x => x.DisplaySeries , initialValue: Seq<IDisplayData?>.Empty , scheduler: RxApp.MainThreadScheduler );

        Observable.Return( (source, flow, seriesKey) )
            .InvokeCommand( RetrieveSeriesDataCommand );
    }

    private void InitializeCommands( IClient client )
    {
        RetrieveSeriesDataCommand = ReactiveCommand.CreateFromObservable( ( (Source, Flow, SeriesKey) t ) => Observable.Start( () =>
        {
            var (source, flow, seriesKey) = t;
            return client.GetData( source , flow , seriesKey );
        } ) );
    }
}