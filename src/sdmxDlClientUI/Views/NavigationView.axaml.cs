using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class NavigationView : ReactiveUserControl<NavigationViewModel>
    {
        public NavigationView()
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
        }

        private static void PopulateFromViewModel( NavigationView navigationView , NavigationViewModel viewModel , CompositeDisposable disposables )
        {
            navigationView.Bind( viewModel ,
                vm => vm.CurrentSource ,
                v => v.AutoCompleteBoxSource.SelectedItem )
                .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.CurrentSource ,
                v => v.ComboBoxSource.SelectedItem )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.Sources ,
                v => v.ComboBoxSource.Items )
                .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.IsActiveSearchSource ,
                v => v.ToggleButtonActiveSearchSource.IsChecked )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.IsActiveSearchSource ,
                v => v.ComboBoxSource.IsVisible ,
                b => !b )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.IsActiveSearchSource ,
                v => v.AutoCompleteBoxSource.IsVisible ,
                b => b )
                .DisposeWith( disposables );

            //navigationView.OneWayBind( viewModel ,
            //    vm => vm.Sources ,
            //    v => v.AutoCompleteBoxSource.Items )
            //    .DisposeWith( disposables );

            //navigationView.AutoCompleteBoxSource.IsTextCompletionEnabled = true;

            //var binding = new MultiBinding();
            //binding.Bindings.Add( new Binding( "Name" ) );
            //binding.Bindings.Add( new Binding( "Description" ) );
            //navigationView.AutoCompleteBoxSource.ValueMemberBinding = binding;

            //avigationView.AutoCompleteBoxSource.ItemFilter = NavigationViewModel.FindSource;

            navigationView.AutoCompleteBoxSource.AsyncPopulator = viewModel.PopulateSourcesAsync;

            //navigationView.AutoCompleteBoxSource.TextSelector = new AutoCompleteSelector<string?>()
            //see https://github.com/AvaloniaUI/Avalonia/blob/master/samples/ControlCatalog/Pages/AutoCompleteBoxPage.xaml.cs
        }
    }
}