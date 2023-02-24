using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class DimensionView : ReactiveUserControl<DimensionViewModel>
    {
        public DimensionView()
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

        private void PopulateFromViewModel( DimensionView dimensionView , DimensionViewModel viewModel , CompositeDisposable disposables )
        {
            dimensionView.OneWayBind( viewModel ,
                vm => vm.DesiredPosition ,
                v => v.TextBlockDesiredPosition.Text )
                .DisposeWith( disposables );

            dimensionView.OneWayBind( viewModel ,
                vm => vm.Label ,
                v => v.TextBlockLabel.Text )
                .DisposeWith( disposables );
        }
    }
}