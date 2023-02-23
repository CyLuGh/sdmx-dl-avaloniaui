using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClient.ViewModels
{
    public class DimensionViewModel : ReactiveObject
    {
        private readonly Dimension _dimension;

        public string Label => _dimension.Label;

        [Reactive] public int DesiredPosition { get; set; }

        public DimensionViewModel( Dimension dimension )
        {
            _dimension = dimension;
        }
    }
}