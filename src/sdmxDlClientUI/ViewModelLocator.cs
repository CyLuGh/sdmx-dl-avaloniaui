﻿using ReactiveUI;
using sdmxDlClient;
using sdmxDlClient.ViewModels;
using sdmxDlClientUI.Views;
using sdmxDlFaker;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClientUI;

public static class ViewModelLocator
{
    static ViewModelLocator()
    {
        var container = Locator.CurrentMutable;

        container.Register( () => new ClientFaker() , typeof( IClient ) );
        container.Register( () => new DimensionsOrderingView() , typeof( IViewFor<DimensionsOrderingViewModel> ) );
        container.Register( () => new DimensionView() , typeof( IViewFor<DimensionViewModel> ) );

        SplatRegistrations.Register<MainViewModel>();
        SplatRegistrations.Register<NavigationViewModel>();
        SplatRegistrations.Register<DimensionsOrderingViewModel>();

        SplatRegistrations.SetupIOC();
    }

    public static MainViewModel Main => Locator.Current.GetService<MainViewModel>()!;
    public static NavigationViewModel Navigation => Locator.Current.GetService<NavigationViewModel>()!;
    public static DimensionsOrderingViewModel DimensionsOrdering => Locator.Current.GetService<DimensionsOrderingViewModel>()!;
}