using Avalonia.Controls;
using Avalonia.ReactiveUI;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class LogMessagesView : ReactiveUserControl<LogMessagesViewModel>
    {
        public LogMessagesView()
        {
            InitializeComponent();

            this.WhenAnyValue( x => x.ViewModel )
                .WhereNotNull()
                .Do( vm => PopulateFromViewModel( this , vm ) )
                .Subscribe();
        }

        private static void PopulateFromViewModel( LogMessagesView view , LogMessagesViewModel viewModel )
        {
            view.OneWayBind( viewModel ,
                vm => vm.Messages ,
                v => v.ListBoxMessage.ItemsSource );
        }
    }
}