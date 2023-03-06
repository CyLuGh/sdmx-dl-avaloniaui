using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
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
                .Do( vm => PopulateFromViewModel( this , vm ) )
                .Subscribe();
        }

        private static void PopulateFromViewModel( TimeSerieDisplayView view , TimeSeriesDisplayViewModel viewModel )
        {
            view.OneWayBind( viewModel ,
                vm => vm.DisplaySeries ,
                v => v.DataGrid.Items );

            viewModel.BuildGridInteraction.RegisterHandler( ctx =>
            {
                view.DataGrid.Columns.Clear();
                view.DataGrid.Columns.Add( new DataGridTextColumn { Header = "Period" , Binding = new Binding( "Period" ) } );

                foreach ( var (display, field) in ctx.Input )
                {
                    view.DataGrid.Columns.Add( new DataGridTextColumn { Header = display , Binding = new Binding( field ) } );
                }

                ctx.SetOutput( RxUnit.Default );
            } );
        }
    }
}