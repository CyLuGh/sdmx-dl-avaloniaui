using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System.Reactive.Disposables;

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
            } );

            ViewModel = ViewModelLocator.Main;
        }
    }
}