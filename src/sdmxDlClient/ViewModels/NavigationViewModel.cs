using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels;

public class NavigationViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IClient _client;
    private readonly DimensionsOrderingViewModel _dimensionsOrderingViewModel;

    public ReactiveCommand<RxUnit , Seq<Source>>? GetSourcesCommand { get; private set; }
    public ReactiveCommand<Source? , Seq<Flow>>? GetFlowsCommand { get; private set; }

    [Reactive] public bool IsActiveSearchSource { get; set; }
    [Reactive] public bool IsActiveSearchFlow { get; set; }

    [Reactive] public Source? CurrentSource { get; set; }
    [Reactive] public Flow? CurrentFlow { get; set; }

    public Seq<Source> Sources { [ObservableAsProperty] get; }
    public Seq<Flow> Flows { [ObservableAsProperty] get; }

    public IObservable<(Source?, Flow?)> SourceFlowObservable { get; private set; }

    public ViewModelActivator Activator { get; }

    public NavigationViewModel( IClient client , DimensionsOrderingViewModel dimensionsOrderingViewModel )
    {
        _client = client;
        _dimensionsOrderingViewModel = dimensionsOrderingViewModel;

        Activator = new ViewModelActivator();

        InitializeCommands();
        OnActivated();

        SourceFlowObservable = this.WhenAnyValue( x => x.CurrentSource , x => x.CurrentFlow );
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            GetSourcesCommand!.ToPropertyEx( this , x => x.Sources , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetFlowsCommand!.ToPropertyEx( this , x => x.Flows , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            Observable.Return( RxUnit.Default )
                .InvokeCommand( this , x => x.GetSourcesCommand )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentSource )
                .InvokeCommand( GetFlowsCommand! )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentSource )
                .Subscribe( s => _dimensionsOrderingViewModel.Source = s )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentFlow )
                .Subscribe( f => _dimensionsOrderingViewModel.Flow = f )
                .DisposeWith( disposables );

            //this.WhenAnyValue( x => x.CurrentSource , x => x.CurrentFlow )
            //    .Do( _ =>
            //    {
            //        Console.WriteLine( "test" );
            //    } ).Subscribe().DisposeWith( disposables );
        } );
    }

    private void InitializeCommands()
    {
        GetSourcesCommand = ReactiveCommand.CreateFromObservable( () => Observable.Start( () => _client.GetSources() ) );

        GetFlowsCommand = ReactiveCommand.CreateFromObservable( ( Source? src ) => Observable.Start( () => _client.GetFlows( src ) ) );
    }

    public async Task<IEnumerable<object>> PopulateSourcesAsync( string? searchText , CancellationToken _ )
    {
        return Sources.Where( s => string.IsNullOrWhiteSpace( searchText )
            || s.Name.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.Description.Contains( searchText , StringComparison.OrdinalIgnoreCase ) );
    }

    public async Task<IEnumerable<object>> PopulateFlowsAsync( string? searchText , CancellationToken _ )
    {
        return Flows.Where( s => string.IsNullOrWhiteSpace( searchText )
            || s.Ref.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.Label.Contains( searchText , StringComparison.OrdinalIgnoreCase ) );
    }
}