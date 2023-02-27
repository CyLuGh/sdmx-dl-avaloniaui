using ReactiveUI;
using sdmxDlClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClient.ViewModels;

public class TimeSerieDisplayViewModel : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }

    public Source Source { get; }
    public Flow Flow { get; }
    public SeriesKey SeriesKey { get; }

    public TimeSerieDisplayViewModel( Source source , Flow flow , SeriesKey seriesKey )
    {
        Activator = new();
        Source = source;
        Flow = flow;
        SeriesKey = seriesKey;
    }
}