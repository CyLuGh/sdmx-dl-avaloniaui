using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System.Reactive.Disposables;

namespace sdmxDlClientUI.Views
{
    public partial class HierarchicalCodeLabelView : ReactiveUserControl<HierarchicalCodeLabelViewModel>
    {
        public HierarchicalCodeLabelView()
        {
            InitializeComponent();

            this.WhenActivated( disposables =>
            {
                this.WhenAnyValue( x => x.ViewModel )
                    .BindTo( this , x => x.DataContext )
                    .DisposeWith( disposables );
            } );
        }
    }
}