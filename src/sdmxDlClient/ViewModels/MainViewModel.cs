global using RxUnit = System.Reactive.Unit;
global using RxCommandUnit = ReactiveUI.ReactiveCommand<System.Reactive.Unit , System.Reactive.Unit>;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace sdmxDlClient.ViewModels;

public class MainViewModel : IActivatableViewModel
{
    private readonly IClient _client;
    public ViewModelActivator Activator { get; }

    private RxCommandUnit? TestJavaCommand { get; set; }

    public MainViewModel( IClient client )
    {
        _client = client;
        Activator = new();

        InitializeCommands();
        OnActivated();
    }

    private void InitializeCommands()
    {
        TestJavaCommand = ReactiveCommand.CreateFromObservable( () => Observable.Start( () => { } ) );
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            Observable.Return( RxUnit.Default )
                .InvokeCommand( TestJavaCommand )
                .DisposeWith( disposables );
        } );
    }
}