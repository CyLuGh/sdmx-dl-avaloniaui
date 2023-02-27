using ReactiveUI;
using System.Reactive.Disposables;

namespace sdmxDlClientWPF.Views
{
    /// <summary>
    /// Interaction logic for HierarchicalCodeLabelView.xaml
    /// </summary>
    public partial class HierarchicalCodeLabelView
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