using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels;

public class NavigationViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IClient _client;

    public ReactiveCommand<RxUnit , Seq<Source>>? GetSourcesCommand { get; private set; }
    public ReactiveCommand<Source? , Seq<Flow>>? GetFlowsCommand { get; private set; }
    public ReactiveCommand<(Source?, Flow?) , Seq<Dimension>>? GetDimensionsCommand { get; private set; }

    public ReactiveCommand<DimensionViewModel , RxUnit>? ForwardPositionCommand { get; private set; }
    public ReactiveCommand<DimensionViewModel , RxUnit>? BackwardPositionCommand { get; private set; }

    [Reactive] public bool IsActiveSearchSource { get; set; }
    [Reactive] public bool IsActiveSearchFlow { get; set; }

    [Reactive] public DimensionViewModel? SelectedDimension { get; set; }

    [Reactive] public Source? CurrentSource { get; set; }
    [Reactive] public Flow? CurrentFlow { get; set; }

    public Seq<Source> Sources { [ObservableAsProperty] get; }
    public Seq<Flow> Flows { [ObservableAsProperty] get; }

    /* Work around SourceCache having weird results in UI */
    [Reactive] private Seq<DimensionViewModel> RawDimensions { get; set; }
    public Seq<DimensionViewModel> Dimensions { [ObservableAsProperty] get; }

    private readonly IObservable<int>? _positionChangedObservable;

    public ViewModelActivator Activator { get; }

    public NavigationViewModel( IClient client )
    {
        _client = client;

        Activator = new ViewModelActivator();

        RawDimensions = Seq<DimensionViewModel>.Empty;
        _positionChangedObservable = this.WhenAnyValue( x => x.RawDimensions )
           .Select( dims => dims.Select( d => d.WhenAnyValue( x => x.DesiredPosition ) ).Merge() )
           .Switch();

        InitializeCommands();
        OnActivated();
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            GetSourcesCommand!.ToPropertyEx( this , x => x.Sources , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetFlowsCommand!.ToPropertyEx( this , x => x.Flows , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetDimensionsCommand!.Select( dims => dims.Select( ( d , i ) => new DimensionViewModel( d ) { DesiredPosition = i + 1 } ).ToSeq() )
                .Subscribe( dims => RawDimensions = dims )

                .DisposeWith( disposables );

            Observable.Return( RxUnit.Default )
                .InvokeCommand( this , x => x.GetSourcesCommand )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentSource )
                .InvokeCommand( GetFlowsCommand! )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentSource , x => x.CurrentFlow )
                .Throttle( TimeSpan.FromMilliseconds( 500 ) )
                .DistinctUntilChanged()
                .InvokeCommand( GetDimensionsCommand! )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.RawDimensions )
                .CombineLatest( _positionChangedObservable! )
                .Select( t => t.First.OrderBy( x => x.DesiredPosition ).ToSeq() )
                .ToPropertyEx( this , x => x.Dimensions , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            /* Reselect dimension when changing desired positions */
            this.WhenAnyValue( x => x.SelectedDimension ).WhereNotNull().DistinctUntilChanged()
                .CombineLatest( this.WhenAnyValue( x => x.Dimensions ) )
                .Throttle( TimeSpan.FromMilliseconds( 100 ) )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Subscribe( t =>
                {
                    var (selected, dimensions) = t;
                    if ( dimensions.Contains( selected ) )
                        SelectedDimension = selected;
                } )
                .DisposeWith( disposables );
        } );
    }

    private void InitializeCommands()
    {
        GetSourcesCommand = ReactiveCommand.CreateFromObservable( () => Observable.Start( () => _client.GetSources() ) );

        GetFlowsCommand = ReactiveCommand.CreateFromObservable( ( Source? src ) => Observable.Start( () => _client.GetFlows( src ) ) );

        GetDimensionsCommand = ReactiveCommand.CreateFromObservable( ( (Source?, Flow?) t ) => Observable.Start( () =>
        {
            var (source, flow) = t;
            return _client.GetDimensions( source , flow );
        } ) );

        var canForward = this.WhenAnyValue( x => x.SelectedDimension )
                .CombineLatest( _positionChangedObservable! )
                .Select( x => x.First )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Select( x => x?.DesiredPosition > 1 );

        ForwardPositionCommand = ReactiveCommand.Create( ( DimensionViewModel dim ) =>
        {
            Dimensions!.First( x => x.DesiredPosition == dim.DesiredPosition - 1 ).DesiredPosition++;
            dim.DesiredPosition--;
        } , canForward );

        var canBackward = this.WhenAnyValue( x => x.SelectedDimension )
                .CombineLatest( _positionChangedObservable! )
                .Select( x => x.First )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Select( x => x != null && x.DesiredPosition < Dimensions.Count );

        BackwardPositionCommand = ReactiveCommand.Create( ( DimensionViewModel dim ) =>
        {
            Dimensions!.First( x => x.DesiredPosition == dim.DesiredPosition + 1 ).DesiredPosition--;
            dim.DesiredPosition++;
        } , canBackward );
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