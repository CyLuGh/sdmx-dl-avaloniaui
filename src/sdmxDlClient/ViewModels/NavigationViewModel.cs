using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels;

public class NavigationViewModel : ReactiveObject, IActivatableViewModel
{
    public ReactiveCommand<RxUnit , Seq<Source>>? GetSourcesCommand { get; private set; }

    [Reactive] public bool IsActiveSearchSource { get; set; }

    [Reactive] public Source? CurrentSource { get; set; }
    public Seq<Source> Sources { [ObservableAsProperty] get; }

    public ViewModelActivator Activator { get; }

    public NavigationViewModel()
    {
        Activator = new ViewModelActivator();

        InitializeCommands( this );
        OnActivated( this );

        this.WhenAnyValue( x => x.CurrentSource )
            .Subscribe( cs =>
            {
                Console.WriteLine( cs );
            } );
    }

    private static void OnActivated( NavigationViewModel @this )
    {
        @this.WhenActivated( disposables =>
        {
            @this.GetSourcesCommand!.ToPropertyEx( @this , x => x.Sources , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            Observable.Return( RxUnit.Default )
                .InvokeCommand( @this , x => x.GetSourcesCommand );
        } );
    }

    private static void InitializeCommands( NavigationViewModel @this )
    {
        @this.GetSourcesCommand = ReactiveCommand.CreateFromObservable( () => Observable.Start( () => @this.GetSources() ) );
    }

    private Seq<Source> GetSources()
    {
        // TODO: real implementation
        return Seq.create( new Source { Name = "Test" , Description = "First test" } , new Source { Name = "Test 2" , Description = "Second test" } );
    }

    public async Task<IEnumerable<object>> PopulateSourcesAsync( string? searchText , CancellationToken _ )
    {
        return Sources.Where( s => string.IsNullOrWhiteSpace( searchText )
            || s.Name.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.Description.Contains( searchText , StringComparison.OrdinalIgnoreCase ) );
    }
}