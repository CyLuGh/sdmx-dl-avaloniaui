using DynamicData;
using LanguageExt;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace sdmxDlClient.ViewModels;

public class HierarchicalCodeLabelViewModel : ReactiveObject, IActivatableViewModel
{
    public static HierarchicalCodeLabelViewModel Dummy { get; }
            = new( Seq<DimensionViewModel>.Empty , Seq<LanguageExt.HashSet<string>>.Empty ) { Code = "Dummy" , Label = "Dummy" };

    public ViewModelActivator Activator { get; }

    [Reactive] public string Code { get; init; } = string.Empty;
    [Reactive] public string Label { get; init; } = string.Empty;
    [Reactive] public bool IsExpanded { get; set; }
    public int Position { get; set; }
    public ObservableCollection<HierarchicalCodeLabelViewModel> Children { get; }

    public bool HasDummyChild => Children.Count == 1 && Children[0].Equals( Dummy );

    public Seq<DimensionViewModel> Dimensions { get; }
    public Seq<LanguageExt.HashSet<string>> KeysOccurrences { get; }

    public HierarchicalCodeLabelViewModel( Seq<DimensionViewModel> dimensions , Seq<LanguageExt.HashSet<string>> keysOccurrences , bool lazyLoad = true )
    {
        Activator = new ViewModelActivator();
        Children = new ObservableCollection<HierarchicalCodeLabelViewModel>();

        if ( lazyLoad )
            Children.Add( Dummy );

        this.WhenActivated( disposables =>
        {
            this.WhenAnyValue( x => x.IsExpanded )
                .Where( x => x )
                .Do( _ =>
                {
                    if ( HasDummyChild )
                    {
                        Children.Clear();
                        LoadChildren();
                    }
                } )
                .Subscribe()
                .DisposeWith( disposables );
        } );
        Dimensions = dimensions;
        KeysOccurrences = keysOccurrences;
    }

    private void LoadChildren()
    {
        Children.AddRange( HierarchyBuilder.Build( Code , Position + 1 , Dimensions , KeysOccurrences ) );
    }
}