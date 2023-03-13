using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.ReactiveUI;
using LanguageExt;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using sdmxDlClient.Models;
using sdmxDlClient.ViewModels;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class TimeSerieDisplayView : ReactiveUserControl<TimeSeriesDisplayViewModel>
    {
        public static Func<double , string> YFormatter { get; set; } = val => val.ToString( "N2" );
        public static Func<double , string> XFormatter { get; set; } = val => DateTime.FromOADate( val ).ToString( "yyyy-MM" );

        public TimeSerieDisplayView()
        {
            InitializeComponent();

            ComboBoxDateFormat.Items = new[] { "yyyy" , "yyyy-MM" , "yyyy-MM-dd" };

            this.WhenAnyValue( x => x.ViewModel )
                .WhereNotNull()
                .Do( viewModel =>
                {
                    this.Bind( viewModel ,
                        vm => vm.DecimalCount ,
                        v => v.NumericUpDownDecimals.Value ,
                        i => (decimal?) i ,
                        d => d != null ? (int) d : 0 );

                    this.Bind( viewModel ,
                        vm => vm.PeriodFormatter ,
                        v => v.ComboBoxDateFormat.SelectedItem );

                    this.OneWayBind( viewModel , vm => vm.DisplaySeries , v => v.DataGrid.Items );
                    BuildGridColumns( viewModel );

                    Observable.Start( () => BuildSeries( viewModel.DataSeries ) )
                        .ObserveOn( RxApp.MainThreadScheduler )
                        .Subscribe( series => CartesianChart.Series = series );

                    viewModel.WhenAnyValue( x => x.PeriodFormatter )
                        .ObserveOn( RxApp.TaskpoolScheduler )
                        .Select( fmt => BuildXAxes( fmt ) )
                        .ObserveOn( RxApp.MainThreadScheduler )
                        .Subscribe( axes => CartesianChart.XAxes = axes );

                    viewModel.WhenAnyValue( x => x.ValueFormatter )
                        .ObserveOn( RxApp.TaskpoolScheduler )
                        .Select( fmt => BuildYAxes( fmt! ) )
                        .ObserveOn( RxApp.MainThreadScheduler )
                        .Subscribe( axes => CartesianChart.YAxes = axes );
                } )
                .Subscribe();
        }

        private void BuildGridColumns( TimeSeriesDisplayViewModel vm )
        {
            DataGrid.Columns.Clear();
            DataGrid.Columns.Add( new DataGridTextColumn { Header = "Period" , Binding = new Binding( "Period" ) } );
            foreach ( var (display, field) in vm.GeneratedFields )
            {
                DataGrid.Columns.Add( new DataGridTextColumn
                {
                    Header = display ,
                    Binding = new Binding( field , BindingMode.OneWay ) ,
                    CanUserReorder = true ,
                    CanUserResize = true
                } );
            }
        }

        private static Seq<LineSeries<ObservablePoint>> BuildSeries( Seq<DataSeries[]> series )
        {
            return series.Flatten()
                .GroupBy( s => s.Series )
                .Select( g =>
                {
                    var ls = new LineSeries<ObservablePoint>
                    {
                        LineSmoothness = .2 ,
                        Fill = null ,
                        Name = g.Key ,
                        Values = g
                            .OrderBy( x => x.ObsPeriod )
                            .Select( x => new ObservablePoint( x.ObsPeriod.Ticks , x.ObsValue ) ) ,
                        TooltipLabelFormatter = ( chartPoint ) => $"{chartPoint.Context.Series}"
                    };

                    return ls;
                } )
                .ToSeq()
                .Strict();
        }

        private static Axis[] BuildXAxes( string format ) => new[]
        {
            new Axis {
                Labeler = value => new DateTime((long)value).ToString(format),
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        private static Axis[] BuildYAxes( string format ) => new[]
        {
            new Axis {
                Labeler = value => value.ToString(format)
            }
        };
    }
}