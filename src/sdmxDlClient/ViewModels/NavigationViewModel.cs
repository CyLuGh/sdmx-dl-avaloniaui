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
    private readonly ILoggerManager _loggerManager;
    private readonly SeriesDisplayViewModel _seriesDisplayViewModel;

    private readonly IObservable<int>? _positionChangedObservable;

    public ReactiveCommand<RxUnit , Seq<Source>>? GetSourcesCommand { get; private set; }
    public ReactiveCommand<Source? , Seq<Flow>>? GetFlowsCommand { get; private set; }
    public ReactiveCommand<(Source?, Flow?) , Option<DataStructure>>? GetDataStructureCommand { get; private set; }
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

    public bool IsRetrievingSources { [ObservableAsProperty] get; }
    public bool IsRetrievingFlows { [ObservableAsProperty] get; }
    public bool IsRetrievingDataStructure { [ObservableAsProperty] get; }

    public ViewModelActivator Activator { get; }

    public NavigationViewModel( IClient client , ILoggerManager loggerManager , SeriesDisplayViewModel seriesDisplayViewModel )
    {
        _client = client;
        _seriesDisplayViewModel = seriesDisplayViewModel;
        _loggerManager = loggerManager;
        Activator = new();

        RawDimensions = Seq<DimensionViewModel>.Empty;
        _positionChangedObservable = this.WhenAnyValue( x => x.RawDimensions )
           .Select( dims => dims.Select( d => d.WhenAnyValue( x => x.DesiredPosition ) ).Merge() )
           .Switch();

        InitializeCommands();
        OnActivated();

        Observable.Merge(
                GetSourcesCommand!.ThrownExceptions.Select( ex => ("Couldn't retrieve sources", ex) ) ,
                GetFlowsCommand!.ThrownExceptions.Select( ex => ("Couldn't retrieve flows", ex) ) ,
                GetDataStructureCommand!.ThrownExceptions.Select( ex => ("Couldn't retrieve data structure", ex) )
                ).Subscribe( t =>
                {
                    var (title, ex) = t;
                    _loggerManager.Error( title , ex );
                } );
    }

    private void OnActivated()
    {
        this.WhenActivated( disposables =>
        {
            GetSourcesCommand!.IsExecuting
                .ToPropertyEx( this , x => x.IsRetrievingSources , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetFlowsCommand!.IsExecuting
                .ToPropertyEx( this , x => x.IsRetrievingFlows , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetDataStructureCommand!.IsExecuting
                .CombineLatest( TransformDimensionsCommand!.IsExecuting ,
                    BuildHierarchyCommand!.IsExecuting ,
                    GetKeysCommand!.IsExecuting )
                .Select( t => new[] { t.First , t.Second , t.Third , t.Fourth }.Any( x => x ) )
                .Throttle( TimeSpan.FromMilliseconds( 10 ) )
                .DistinctUntilChanged()
                .Do( x =>
                {
                    System.Diagnostics.Debug.WriteLine( "IsRetrievingDataStructure: {0}" , x );
                } )
                .ToProperty( this , x => x.IsRetrievingDataStructure , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetSourcesCommand!.ToPropertyEx( this , x => x.Sources , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetFlowsCommand!
                .Merge( this.WhenAnyValue( x => x.CurrentSource ).Select( _ => Seq<Flow>.Empty ) )
                .ToPropertyEx( this , x => x.Flows , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetDataStructureCommand!
                .Select( s => s.Match( ds => ds.Dimensions.ToSeq() , () => Seq<Dimension>.Empty ) )
                .InvokeCommand( TransformDimensionsCommand )
                .DisposeWith( disposables );

            GetDataStructureCommand!
                .Select( s => s.Match( ds => ds.Dimensions.ToSeq() , () => Seq<Dimension>.Empty ) )
                .InvokeCommand( GetKeysCommand )
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
                .Throttle( TimeSpan.FromMilliseconds( 100 ) )
                .DistinctUntilChanged()
                .InvokeCommand( GetDataStructureCommand )
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

        GetDataStructureCommand = ReactiveCommand.CreateFromObservable( ( (Source?, Flow?) t ) => Observable.Start( () =>
        {
            var (source, flow) = t;
            return _client.GetStructure( source , flow );
        } ) );

        TransformDimensionsCommand = ReactiveCommand.CreateFromObservable( ( Seq<Dimension> dimensions ) => Observable.Start( () =>
            dimensions
                .Select( d => new DimensionViewModel( d )
                {
                    DesiredPosition = d.Position ,
                    Codes = d.CodeList.Codes
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

            if ( dimensions.IsEmpty /*|| keysOccurrences.IsEmpty*/ )
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