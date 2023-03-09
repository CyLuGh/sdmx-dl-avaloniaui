global using RxUnit = System.Reactive.Unit;
global using RxCommandUnit = ReactiveUI.ReactiveCommand<System.Reactive.Unit , System.Reactive.Unit>;
using ReactiveUI;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using ReactiveUI.Fody.Helpers;

namespace sdmxDlClient.ViewModels;

public sealed class MainViewModel : ReactiveObject, IActivatableViewModel, IDisposable
{
    private readonly IClient _client;
    private readonly NavigationViewModel _navigationViewModel;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public ViewModelActivator Activator { get; }

    [Reactive] public bool IsLogsPaneOpen { get; set; }
    public bool IsServerRunning { [ObservableAsProperty] get; }

    private RxCommandUnit? StartServerCommand { get; set; }

    public MainViewModel( IClient client , NavigationViewModel navigationViewModel )
    {
        _client = client;
        _navigationViewModel = navigationViewModel;
        _cancellationTokenSource = new CancellationTokenSource();
        Activator = new();

        InitializeCommands();
        OnActivated();
    }

    private void InitializeCommands()
    {
        StartServerCommand = ReactiveCommand.CreateFromTask( async () =>
        {
            Console.WriteLine( "Test" );
            await _client.StartServer( _cancellationTokenSource.Token );
            Console.WriteLine( "Test 2" );
        } );
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            Observable.Return( RxUnit.Default )
                .Delay( TimeSpan.FromMilliseconds( 200 ) )
                .InvokeCommand( StartServerCommand )
                .DisposeWith( disposables );

            var serverDelay = TimeSpan.FromMilliseconds( 1500 );

            StartServerCommand!
                .IsExecuting
                .Where( x => x )
                .Select( _ => RxUnit.Default )
                .Throttle( serverDelay )
                .InvokeCommand( _navigationViewModel , x => x.GetSourcesCommand )
                .DisposeWith( disposables );

            StartServerCommand!.IsExecuting
                .Throttle( serverDelay )
                .ToPropertyEx( this , x => x.IsServerRunning , initialValue: false , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );
        } );
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        GC.SuppressFinalize( this );
    }
}