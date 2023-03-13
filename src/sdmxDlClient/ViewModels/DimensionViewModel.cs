using LanguageExt;
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
        public Dimension Dimension { get; }

        public string Label => Dimension.Name;
        public string Concept => Dimension.Id;
        public int Position => Dimension.Position;

        [Reactive] public int DesiredPosition { get; set; }

        public Seq<CodeLabel> Values { get; init; }

        public DimensionViewModel( Dimension dimension )
        {
            Dimension = dimension;
        }
    }
}