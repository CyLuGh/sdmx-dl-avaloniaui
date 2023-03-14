using ReactiveUI;
using sdmxDlClient;
using sdmxDlClient.ViewModels;
using sdmxDlClientUI.Views;
using sdmxDlConsumer;
using sdmxDlFaker;
using Splat;

namespace sdmxDlClientUI;

public static class ViewModelLocator
{
    static ViewModelLocator()
    {
        var container = Locator.CurrentMutable;

        container.RegisterLazySingleton( () => new ClientFaker() , typeof( IClient ) );
        //container.RegisterLazySingleton( () => new Consumer() , typeof( IClient ) );

        container.Register( () => new HierarchicalCodeLabelView() , typeof( IViewFor<HierarchicalCodeLabelViewModel> ) );
        container.Register( () => new SeriesDisplayView() , typeof( IViewFor<SeriesDisplayViewModel> ) );
        container.Register( () => new TimeSerieDisplayView() , typeof( IViewFor<TimeSeriesDisplayViewModel> ) );

        SplatRegistrations.RegisterLazySingleton<MainViewModel>();
        SplatRegistrations.RegisterLazySingleton<NavigationViewModel>();
        SplatRegistrations.RegisterLazySingleton<SeriesDisplayViewModel>();

        SplatRegistrations.RegisterLazySingleton<NodesTreeViewDropHandler>();
        SplatRegistrations.RegisterConstant<ILoggerManager>( new LogMessagesViewModel() );

        SplatRegistrations.SetupIOC();
    }

    public static MainViewModel Main => Locator.Current.GetService<MainViewModel>()!;
    public static NavigationViewModel Navigation => Locator.Current.GetService<NavigationViewModel>()!;
    public static SeriesDisplayViewModel SeriesDisplay => Locator.Current.GetService<SeriesDisplayViewModel>()!;

    public static NodesTreeViewDropHandler DropHandler => Locator.Current.GetService<NodesTreeViewDropHandler>()!;
    public static LogMessagesViewModel Logs => (LogMessagesViewModel) Locator.Current.GetService<ILoggerManager>()!;
}