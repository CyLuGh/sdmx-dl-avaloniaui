﻿using ReactiveUI;
using sdmxDlClient;
using sdmxDlClient.ViewModels;
using sdmxDlClientUI.Views;
using sdmxDlFaker;
using Splat;

namespace sdmxDlClientUI;

public static class ViewModelLocator
{
    static ViewModelLocator()
    {
        var container = Locator.CurrentMutable;

        container.RegisterLazySingleton( () => new ClientFaker() , typeof( IClient ) );

        container.Register( () => new HierarchicalCodeLabelView() , typeof( IViewFor<HierarchicalCodeLabelViewModel> ) );
        container.Register( () => new SeriesDisplayView() , typeof( IViewFor<SeriesDisplayViewModel> ) );

        SplatRegistrations.RegisterLazySingleton<MainViewModel>();
        SplatRegistrations.RegisterLazySingleton<NavigationViewModel>();
        SplatRegistrations.RegisterLazySingleton<SeriesDisplayViewModel>();

        SplatRegistrations.SetupIOC();
    }

    public static MainViewModel Main => Locator.Current.GetService<MainViewModel>()!;
    public static NavigationViewModel Navigation => Locator.Current.GetService<NavigationViewModel>()!;
    public static SeriesDisplayViewModel SeriesDisplay => Locator.Current.GetService<SeriesDisplayViewModel>()!;
}