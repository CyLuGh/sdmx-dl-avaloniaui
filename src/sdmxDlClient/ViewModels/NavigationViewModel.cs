using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
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

    internal SourceCache<DimensionViewModel , string> DimensionsCache { get; }
            = new( s => s.Concept );

    private ReadOnlyObservableCollection<DimensionViewModel>? _dimensions;
    public ReadOnlyObservableCollection<DimensionViewModel>? Dimensions => _dimensions;

    public ViewModelActivator Activator { get; }

    public NavigationViewModel( IClient client )
    {
        _client = client;

        Activator = new ViewModelActivator();

        InitializeCommands();
        OnActivated();
    }

    private void OnActivated()
    {
        DimensionsCache.Connect()
                .AutoRefresh( x => x.DesiredPosition )
                .Sort( SortExpressionComparer<DimensionViewModel>.Ascending( x => x.DesiredPosition ) )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Bind( out _dimensions )
                .DisposeMany()
                .Subscribe();

        this.WhenActivated( disposables =>
        {
            GetSourcesCommand!.ToPropertyEx( this , x => x.Sources , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetFlowsCommand!.ToPropertyEx( this , x => x.Flows , scheduler: RxApp.MainThreadScheduler )
                .DisposeWith( disposables );

            GetDimensionsCommand!.Select( dims => dims.Select( ( d , i ) => new DimensionViewModel( d ) { DesiredPosition = i + 1 } ).ToSeq() )
                .Subscribe( dims => DimensionsCache.Edit( e =>
                {
                    e.Clear();
                    e.AddOrUpdate( dims );
                } ) )
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
                .CombineLatest( this.DimensionsCache.Connect()
                    .AutoRefresh( x => x.DesiredPosition )
                    .DisposeMany() )
                .Select( x => x.First )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Select( x => x?.DesiredPosition > 1 );

        ForwardPositionCommand = ReactiveCommand.Create( ( DimensionViewModel dim ) =>
        {
            Dimensions!.First( x => x.DesiredPosition == dim.DesiredPosition - 1 ).DesiredPosition++;
            dim.DesiredPosition--;
        } , canForward );

        var canBackward = this.WhenAnyValue( x => x.SelectedDimension )
                .CombineLatest( this.DimensionsCache.Connect()
                    .AutoRefresh( x => x.DesiredPosition )
                    .DisposeMany() )
                .Select( x => x.First )
                .ObserveOn( RxApp.MainThreadScheduler )
                .Select( x => x != null && x.DesiredPosition < Dimensions?.Count );

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