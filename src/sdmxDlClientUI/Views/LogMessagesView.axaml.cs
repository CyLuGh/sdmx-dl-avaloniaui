using Avalonia.Controls;
using Avalonia.ReactiveUI;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using sdmxDlClient.ViewModels;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClientUI.Views
{
    public partial class LogMessagesView : ReactiveUserControl<LogMessagesViewModel>
    {
        public LogMessagesView()
        {
            InitializeComponent();

            this.WhenAnyValue( x => x.ViewModel )
                .Do( _ =>
                {
                    Console.WriteLine();
                } )
                .WhereNotNull()
                .Do( vm => PopulateFromViewModel( this , vm ) )
                .Subscribe();
        }

        private static void PopulateFromViewModel( LogMessagesView view , LogMessagesViewModel viewModel )
        {
            view.OneWayBind( viewModel ,
                vm => vm.Messages ,
                v => v.ListBoxMessage.Items );

            var b = new InfoBadge();
        }
    }
}