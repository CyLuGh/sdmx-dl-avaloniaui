using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClient.ViewModels
{
    public class DimensionsOrderingViewModel : ReactiveObject, IActivatableViewModel
    {
        private readonly IClient _client;

        public Seq<DimensionViewModel> Dimensions { [ObservableAsProperty] get; }

        public ReactiveCommand<(Source?, Flow?) , Seq<Dimension>>? GetDimensionsCommand { get; private set; }

        [Reactive] public Source? Source { get; set; }
        [Reactive] public Flow? Flow { get; set; }

        public ViewModelActivator Activator { get; }

        public DimensionsOrderingViewModel( IClient client )
        {
            _client = client;
            Activator = new ViewModelActivator();

            InitializeCommands();
            OnActivated();
        }

        private void OnActivated()
        {
            this.WhenAnyValue( x => x.Source , x => x.Flow )
                    .Throttle( TimeSpan.FromMilliseconds( 500 ) )
                    .DistinctUntilChanged()
                    .Do( _ =>
                    {
                        Console.WriteLine( "test" );
                    } )
                    .InvokeCommand( GetDimensionsCommand! );

            this.WhenActivated( disposables =>
            {
                GetDimensionsCommand!.Select( dims => dims.Select( ( d , i ) => new DimensionViewModel( d ) { DesiredPosition = i + 1 } ).ToSeq() )
                    .ToPropertyEx( this , x => x.Dimensions , scheduler: RxApp.MainThreadScheduler )
                    .DisposeWith( disposables );
            } );
        }

        private void InitializeCommands()
        {
            GetDimensionsCommand = ReactiveCommand.CreateFromObservable( ( (Source?, Flow?) t ) => Observable.Start( () =>
            {
                var (source, flow) = t;
                return _client.GetDimensions( source , flow );
            } ) );
        }
    }
}