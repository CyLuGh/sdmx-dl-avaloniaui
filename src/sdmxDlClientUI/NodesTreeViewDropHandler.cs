using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactions.DragAndDrop;
using ReactiveUI;
using sdmxDlClient.Models;
using sdmxDlClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClientUI;

public class NodesTreeViewDropHandler : DropHandlerBase
{
    private readonly NavigationViewModel _navigationViewModel;

    public NodesTreeViewDropHandler( NavigationViewModel navigationViewModel )
    {
        _navigationViewModel = navigationViewModel;
    }

    private bool Validate( HierarchicalCodeLabelViewModel hierarchicalCodeLabelViewModel , SeriesDisplayViewModel seriesDisplayViewModel , bool bExecute )
    {
        if ( _navigationViewModel.CurrentSource == null || _navigationViewModel.CurrentFlow == null )
            return false;

        if ( bExecute )
        {
            Observable.Return( (_navigationViewModel.CurrentSource, _navigationViewModel.CurrentFlow, new SeriesKey( hierarchicalCodeLabelViewModel.Code )) )
                .InvokeCommand( seriesDisplayViewModel , x => x.FetchDataCommand );
        }

        return true;
    }

    public override bool Validate( object? sender , DragEventArgs e , object? sourceContext , object? targetContext , object? state )
    {
        if ( sourceContext is HierarchicalCodeLabelViewModel hierarchicalCodeLabelViewModel
            && targetContext is SeriesDisplayViewModel seriesDisplayViewModel )
        {
            return Validate( hierarchicalCodeLabelViewModel , seriesDisplayViewModel , false );
        }

        return false;
    }

    public override bool Execute( object? sender , DragEventArgs e , object? sourceContext , object? targetContext , object? state )
    {
        if ( sourceContext is HierarchicalCodeLabelViewModel hierarchicalCodeLabelViewModel
            && targetContext is SeriesDisplayViewModel seriesDisplayViewModel )
        {
            return Validate( hierarchicalCodeLabelViewModel , seriesDisplayViewModel , true );
        }

        return false;
    }
}