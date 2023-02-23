using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class DimensionsOrderingView : ReactiveUserControl<DimensionsOrderingViewModel>
    {
        public DimensionsOrderingView()
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

        private static void PopulateFromViewModel( DimensionsOrderingView view , DimensionsOrderingViewModel viewModel , CompositeDisposable disposables )
        {
            view.OneWayBind( viewModel ,
                vm => vm.Dimensions ,
                v => v.ItemsControlDimensions.Items )
                .DisposeWith( disposables );
        }
    }
}