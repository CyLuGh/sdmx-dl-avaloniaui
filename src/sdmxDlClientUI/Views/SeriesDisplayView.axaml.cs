using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class SeriesDisplayView : ReactiveUserControl<SeriesDisplayViewModel>
    {
        public SeriesDisplayView()
        {
            InitializeComponent();

            this.WhenActivated( disposables =>
            {
                this.WhenAnyValue( x => x.ViewModel )
                    .WhereNotNull()
                    .Do( vm => PopulateFromViewModel( this , vm , disposables ) )
                    .Subscribe()
                    .DisposeWith( disposables );
            } );
        }

        private static void PopulateFromViewModel( SeriesDisplayView view , SeriesDisplayViewModel viewModel , CompositeDisposable disposables )
        {
            view.OneWayBind( viewModel ,
                vm => vm.TimeSeries ,
                v => v.TabControlTimeSeries.ItemsSource )
                .DisposeWith( disposables );

            view.Bind( viewModel ,
                vm => vm.SelectedTimeSeriesDisplay ,
                v => v.TabControlTimeSeries.SelectedItem )
                .DisposeWith( disposables );

            view.OneWayBind( viewModel ,
                vm => vm.HasSeries ,
                v => v.TextBlockInvite.IsVisible ,
                b => !b )
                .DisposeWith( disposables );
        }
    }
}