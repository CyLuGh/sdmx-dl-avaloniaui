using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using LanguageExt;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using sdmxDlClient.Models;
using sdmxDlClient.ViewModels;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class TimeSerieDisplayView : ReactiveUserControl<TimeSeriesDisplayViewModel>
    {
        public TimeSerieDisplayView()
        {
            InitializeComponent();

            this.WhenAnyValue( x => x.ViewModel )
                    .WhereNotNull()
                    .Do( vm =>
                    {
                        this.OneWayBind( vm , vm => vm.DisplaySeries , v => v.DataGrid.Items );

                        DataGrid.Columns.Clear();
                        DataGrid.Columns.Add( new DataGridTextColumn { Header = "Period" , Binding = new Binding( "Period" ) } );
                        foreach ( var (display, field) in vm.GeneratedFields )
                        {
                            DataGrid.Columns.Add( new DataGridTextColumn { Header = display , Binding = new Binding( field ) } );
                        }

                        CartesianChart.Series = BuildSeries( vm.DataSeries );
                    } )
                    .Subscribe();
        }

        private Seq<LineSeries<ObservablePoint>> BuildSeries( Seq<DataSeries[]> series )
        {
            return series.Flatten()
                .GroupBy( s => s.Series )
                .Select( g =>
                {
                    var ls = new LineSeries<ObservablePoint>
                    {
                        LineSmoothness = .2 ,
                        //Fill = null ,
                        Name = g.Key ,
                        Values = g
                            .OrderBy( x => x.ObsPeriod )
                            .Select( x => new ObservablePoint( x.ObsPeriod.ToOADate() , x.ObsValue ) )
                    };

                    return ls;
                } )
                .ToSeq();
        }
    }
}