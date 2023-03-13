using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using sdmxDlClient.Models;
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

            ButtonClosePanel.Command = ReactiveCommand.Create( () =>
            {
                if ( ViewModel != null )
                    ViewModel.IsLogsPaneOpen = false;
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

            view.Bind( viewModel ,
                vm => vm.IsLogsPaneOpen ,
                v => v.SplitViewLogs.IsPaneOpen )
                .DisposeWith( disposables );

            view.Bind( viewModel ,
                vm => vm.IsLogsPaneOpen ,
                v => v.ToggleButtonLogsPane.IsChecked )
                .DisposeWith( disposables );

            view.Bind( viewModel ,
                vm => vm.IsShowingMessage ,
                v => v.InfoBar.IsOpen )
                .DisposeWith( disposables );

            view.OneWayBind( viewModel ,
                vm => vm.LogMessage ,
                v => v.InfoBar.Message ,
                lm => lm.Message )
                .DisposeWith( disposables );

            view.OneWayBind( viewModel ,
                vm => vm.LogMessage ,
                v => v.InfoBar.Title ,
                lm => lm.Title )
                .DisposeWith( disposables );

            static InfoBarSeverity ConvertToSeverity( MessageKind kind )
                => kind switch
                {
                    MessageKind.Error => InfoBarSeverity.Error,
                    MessageKind.Warn => InfoBarSeverity.Warning,
                    MessageKind.Info => InfoBarSeverity.Informational,
                    _ => InfoBarSeverity.Informational
                };

            view.OneWayBind( viewModel ,
                vm => vm.LogMessage ,
                v => v.InfoBar.Severity ,
                lm => ConvertToSeverity( lm.Kind ) )
                .DisposeWith( disposables );
        }
    }
}