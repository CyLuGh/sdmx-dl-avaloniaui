using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels;

public class NavigationViewModel : ReactiveObject, IActivatableViewModel
{
    private readonly IClient _client;
    private readonly SeriesDisplayViewModel _seriesDisplayViewModel;

    public ReactiveCommand<RxUnit , Seq<Source>>? GetSourcesCommand { get; private set; }
    public ReactiveCommand<Source? , Seq<Flow>>? GetFlowsCommand { get; private set; }
    public ReactiveCommand<(Source?, Flow?) , Seq<Dimension>>? GetDimensionsCommand { get; private set; }
    public ReactiveCommand<Seq<Dimension> , Seq<DimensionViewModel>>? TransformDimensionsCommand { get; private set; }
    public ReactiveCommand<Seq<Dimension> , Seq<SeriesKey>>? GetKeysCommand { get; private set; }
    public ReactiveCommand<RxUnit , Option<(Source, Flow, SeriesKey)>>? ParseLookUpCommand { get; private set; }
    public ReactiveCommand<HierarchicalCodeLabelViewModel , (Source, Flow, SeriesKey)>? ViewHierarchyElementCommand { get; private set; }

    public ReactiveCommand<(Seq<DimensionViewModel>, Seq<LanguageExt.HashSet<string>>) , Seq<HierarchicalCodeLabelViewModel>>? BuildHierarchyCommand { get; private set; }
    public ReactiveCommand<DimensionViewModel , RxUnit>? ForwardPositionCommand { get; private set; }
    public ReactiveCommand<DimensionViewModel , RxUnit>? BackwardPositionCommand { get; private set; }

    [Reactive] public bool IsActiveSearchSource { get; set; }
    [Reactive] public bool IsActiveSearchFlow { get; set; }

    [Reactive] public DimensionViewModel? SelectedDimension { get; set; }
    [Reactive] public HierarchicalCodeLabelViewModel? SelectedHierarchicalElement { get; set; }

    [Reactive] public Source? CurrentSource { get; set; }
    [Reactive] public Flow? CurrentFlow { get; set; }
    [Reactive] public string KeyLookup { get; set; } = string.Empty;

    public Seq<Source> Sources { [ObservableAsProperty] get; }
    public Seq<Flow> Flows { [ObservableAsProperty] get; }

    /* Work around SourceCache having weird results in UI */
    [Reactive] private Seq<DimensionViewModel> RawDimensions { get; set; }
    public Seq<DimensionViewModel> Dimensions { [ObservableAsProperty] get; }
    public Seq<LanguageExt.HashSet<string>> KeysOccurrences { [ObservableAsProperty] get; }
    public Seq<HierarchicalCodeLabelViewModel> Hierarchy { [ObservableAsProperty] get; }

    private readonly IObservable<int>? _positionChangedObservable;

    public ViewModelActivator Activator { get; }

    public NavigationViewModel( IClient client , SeriesDisplayViewModel seriesDisplayViewModel )
    {
        _client = client;
        _seriesDisplayViewModel = seriesDisplayViewModel;
        Activator = new();

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

            GetDimensionsCommand!.InvokeCommand( TransformDimensionsCommand )
                .DisposeWith( disposables );

            GetDimensionsCommand!.InvokeCommand( GetKeysCommand )
                .DisposeWith( disposables );

            TransformDimensionsCommand!
                .Subscribe( dims => RawDimensions = dims )
                .DisposeWith( disposables );

            GetKeysCommand!
                .ObserveOn( RxApp.TaskpoolScheduler )
                .Select( keys =>
                {
                    if ( keys.IsEmpty )
                        return Seq<LanguageExt.HashSet<string>>.Empty;

                    var splits = keys.AsParallel()
                        .Select( k => k.Series.Split( '.' ) )
                        .ToSeq();
                    var count = splits.First().Length;

                    return Enumerable.Range( 0 , count )
                        .Select( i => LanguageExt.HashSet.createRange( splits.Select( s => s[i] ) ) )
                        .ToSeq();
                } )
                .ToPropertyEx( this , x => x.KeysOccurrences , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            BuildHierarchyCommand!
                .ToPropertyEx( this , x => x.Hierarchy , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentSource )
                .InvokeCommand( GetFlowsCommand )
                .DisposeWith( disposables );

            this.WhenAnyValue( x => x.CurrentSource , x => x.CurrentFlow )
                .Throttle( TimeSpan.FromMilliseconds( 500 ) )
                .DistinctUntilChanged()
                .InvokeCommand( GetDimensionsCommand )
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

            this.WhenAnyValue( x => x.Dimensions , x => x.KeysOccurrences )
                .Throttle( TimeSpan.FromMilliseconds( 200 ) )
                .DistinctUntilChanged()
                .InvokeCommand( BuildHierarchyCommand )
                .DisposeWith( disposables );

            ParseLookUpCommand!
                .Select( t => t.Match( x => Observable.Return( x ) , () => Observable.Empty<(Source, Flow, SeriesKey)>() ) )
                .Switch()
                .ObserveOn( RxApp.MainThreadScheduler )
                .Do( _ => KeyLookup = string.Empty )
                .Merge( ViewHierarchyElementCommand! )
                .InvokeCommand( _seriesDisplayViewModel , x => x.FetchDataCommand )
                .DisposeWith( disposables );
        } );
    }

    private void InitializeCommands()
    {
        GetSourcesCommand = ReactiveCommand.CreateFromTask( () => _client.GetSources() );

        GetFlowsCommand = ReactiveCommand.CreateFromTask( ( Source? src ) => _client.GetFlows( src ) );

        GetDimensionsCommand = ReactiveCommand.CreateFromObservable( ( (Source?, Flow?) t ) => Observable.Start( () =>
        {
            var (source, flow) = t;
            return _client.GetDimensions( source , flow );
        } ) );

        TransformDimensionsCommand = ReactiveCommand.CreateFromObservable( ( Seq<Dimension> dimensions ) => Observable.Start( () =>
            dimensions
                .Select( ( d , i ) => (d, i) )
                .AsParallel()
                .Select( t =>
                {
                    var (d, idx) = t;
                    var codes = _client.GetCodes( CurrentSource! , CurrentFlow! , d );
                    return new DimensionViewModel( d )
                    {
                        DesiredPosition = idx + 1 ,
                        Values = codes
                    };
                } )
                .ToSeq() ) );

        GetKeysCommand = ReactiveCommand.CreateFromObservable( ( Seq<Dimension> dimensions ) => Observable.Start( () =>
            _client.GetKeys( CurrentSource , CurrentFlow , dimensions ) ) );

        ParseLookUpCommand = ReactiveCommand.CreateFromObservable( () => Observable.Start( () =>
        {
            if ( string.IsNullOrWhiteSpace( KeyLookup ) )
                return Option<(Source, Flow, SeriesKey)>.None;

            return (new Source() { Id = "" }, new Flow() { Name = "" , Ref = "" , StructureRef = "" }, new SeriesKey( KeyLookup ));
        } ) );

        BuildHierarchyCommand = ReactiveCommand.CreateFromObservable( ( (Seq<DimensionViewModel>, Seq<LanguageExt.HashSet<string>>) t ) => Observable.Start( () =>
        {
            var (dimensions, keysOccurrences) = t;

            if ( dimensions.IsEmpty || keysOccurrences.IsEmpty )
                return Seq<HierarchicalCodeLabelViewModel>.Empty;

            return HierarchyBuilder.Build( dimensions , keysOccurrences );
        } ) );

        var canViewHierarchyElement =
            this.WhenAnyValue( x => x.CurrentSource , x => x.CurrentFlow , x => x.SelectedHierarchicalElement )
            .Select( t => t.Item1 != null && t.Item2 != null && t.Item3 != null )
            .ObserveOn( RxApp.MainThreadScheduler );
        ViewHierarchyElementCommand = ReactiveCommand.Create( ( HierarchicalCodeLabelViewModel item )
            => (CurrentSource!, CurrentFlow!, new SeriesKey( item.Code )) , canViewHierarchyElement );

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
            || s.Id.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.Aliases.Any( x => x.Contains( searchText , StringComparison.OrdinalIgnoreCase ) )
            || s.Names.Values.Any( x => x.Contains( searchText , StringComparison.OrdinalIgnoreCase ) ) );
    }

    public async Task<IEnumerable<object>> PopulateFlowsAsync( string? searchText , CancellationToken _ )
    {
        return Flows.Where( s => string.IsNullOrWhiteSpace( searchText )
            || s.Ref.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.StructureRef.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.Name.Contains( searchText , StringComparison.OrdinalIgnoreCase )
            || s.Description.Contains( searchText , StringComparison.OrdinalIgnoreCase ) );
    }
}