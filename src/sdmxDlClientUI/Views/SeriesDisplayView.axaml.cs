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

            //this.WhenAnyValue( x => x.ViewModel )
            //        .BindTo( this , x => x.DataContext );

            this.WhenActivated( disposables =>
            {
                //this.WhenAnyValue( x => x.ViewModel )
                //    .BindTo( this , x => x.DataContext )
                //    .DisposeWith( disposables );

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
                v => v.TabControlTimeSeries.Items )
                .DisposeWith( disposables );
        }
    }
}