using ReactiveMarbles.ObservableEvents;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace sdmxDlClientWPF.Views
{
    /// <summary>
    /// Interaction logic for NavigationView.xaml
    /// </summary>
    public partial class NavigationView
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
            PopulateSources( navigationView , viewModel , disposables );
            PopulateFlows( navigationView , viewModel , disposables );
            PopulateDimensions( navigationView , viewModel , disposables );

            navigationView.Bind( viewModel ,
                vm => vm.KeyLookup ,
                v => v.TextBoxLookUp.Text )
                .DisposeWith( disposables );

            navigationView.TextBoxLookUp
                .Events()
                .KeyDown
                .Subscribe( evt =>
                {
                    if ( evt.Key == Key.Return || evt.Key == Key.Enter )
                    {
                        Observable.Return( Unit.Default )
                            .InvokeCommand( viewModel , x => x.ParseLookUpCommand );
                    }
                } )
                .DisposeWith( disposables );
        }

        private static void PopulateDimensions( NavigationView navigationView , NavigationViewModel viewModel , CompositeDisposable disposables )
        {
            navigationView.OneWayBind( viewModel ,
                vm => vm.Hierarchy ,
                v => v.TreeViewHierarchy.ItemsSource )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.Dimensions ,
                v => v.ListBoxDimensions.ItemsSource )
                .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.SelectedDimension ,
                v => v.ListBoxDimensions.SelectedItem )
                .DisposeWith( disposables );

            navigationView.BindCommand( viewModel ,
                vm => vm.ForwardPositionCommand ,
                v => v.ButtonForward ,
                viewModel.WhenAnyValue( x => x.SelectedDimension ) )
                .DisposeWith( disposables );

            navigationView.BindCommand( viewModel ,
                vm => vm.BackwardPositionCommand ,
                v => v.ButtonBackward ,
                viewModel.WhenAnyValue( x => x.SelectedDimension ) )
                .DisposeWith( disposables );
        }

        private static void PopulateFlows( NavigationView navigationView , NavigationViewModel viewModel , CompositeDisposable disposables )
        {
            //navigationView.Bind( viewModel ,
            //    vm => vm.CurrentFlow ,
            //    v => v.AutoCompleteBoxFlow.SelectedItem )
            //    .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.CurrentFlow ,
                v => v.ComboBoxFlow.SelectedItem )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                    vm => vm.Flows ,
                    v => v.ComboBoxFlow.ItemsSource )
                    .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.IsActiveSearchFlow ,
                v => v.ToggleButtonActiveSearchFlow.IsChecked )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.IsActiveSearchFlow ,
                v => v.ComboBoxFlow.Visibility ,
                b => b ? Visibility.Collapsed : Visibility.Visible )
                .DisposeWith( disposables );

            //navigationView.OneWayBind( viewModel ,
            //    vm => vm.IsActiveSearchFlow ,
            //    v => v.AutoCompleteBoxFlow.IsVisible ,
            //    b => b )
            //    .DisposeWith( disposables );

            //see https://github.com/AvaloniaUI/Avalonia/blob/master/samples/ControlCatalog/Pages/AutoCompleteBoxPage.xaml.cs
            //navigationView.AutoCompleteBoxFlow.AsyncPopulator = viewModel.PopulateFlowsAsync;
        }

        private static void PopulateSources( NavigationView navigationView , NavigationViewModel viewModel , CompositeDisposable disposables )
        {
            //navigationView.Bind( viewModel ,
            //    vm => vm.CurrentSource ,
            //    v => v.AutoCompleteBoxSource.SelectedItem )
            //    .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.CurrentSource ,
                v => v.ComboBoxSource.SelectedItem )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.Sources ,
                v => v.ComboBoxSource.ItemsSource )
                .DisposeWith( disposables );

            navigationView.Bind( viewModel ,
                vm => vm.IsActiveSearchSource ,
                v => v.ToggleButtonActiveSearchSource.IsChecked )
                .DisposeWith( disposables );

            navigationView.OneWayBind( viewModel ,
                vm => vm.IsActiveSearchSource ,
                v => v.ComboBoxSource.Visibility ,
                b => b ? Visibility.Collapsed : Visibility.Visible )
                .DisposeWith( disposables );

            //navigationView.OneWayBind( viewModel ,
            //    vm => vm.IsActiveSearchSource ,
            //    v => v.AutoCompleteBoxSource.IsVisible ,
            //    b => b )
            //    .DisposeWith( disposables );

            ////see https://github.com/AvaloniaUI/Avalonia/blob/master/samples/ControlCatalog/Pages/AutoCompleteBoxPage.xaml.cs
            //navigationView.AutoCompleteBoxSource.AsyncPopulator = viewModel.PopulateSourcesAsync;
        }
    }
}