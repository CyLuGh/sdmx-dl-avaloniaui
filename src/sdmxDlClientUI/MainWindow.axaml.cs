using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI
{
    public partial class MainWindow : ReactiveWindow<MainViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WhenActivated( disposables =>
            {
                this.WhenAnyValue( x => x.ViewModel )
                    .BindTo( this , x => x.DataContext )
                    .DisposeWith( disposables );

                this.WhenAnyValue( x => x.ViewModel )
                    .WhereNotNull()
                    .Do( vm => PopulateFromViewModel( this , vm , disposables ) )
                    .Subscribe()
                    .DisposeWith( disposables );
            } );

            ViewModel = ViewModelLocator.Main;
        }

        private static void PopulateFromViewModel( MainWindow view , MainViewModel viewModel , CompositeDisposable disposables )
        {
            view.OneWayBind( viewModel ,
                vm => vm.IsServerRunning ,
                v => v.BorderRunning.IsVisible )
                .DisposeWith( disposables );

            view.OneWayBind( viewModel ,
                vm => vm.IsServerRunning ,
                v => v.BorderStopped.IsVisible ,
                b => !b )
                .DisposeWith( disposables );
        }
    }
}