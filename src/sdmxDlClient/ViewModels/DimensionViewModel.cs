using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using sdmxDlClient.Models;

namespace sdmxDlClient.ViewModels
{
    public class DimensionViewModel : ReactiveObject
    {
        public Dimension Dimension { get; }

        public string Label => Dimension.Name;
        public string Concept => Dimension.Id;
        public int Position => Dimension.Position;

        [Reactive] public int DesiredPosition { get; set; }

        public IDictionary<string , string> Codes { get; init; } = new Dictionary<string , string>();

        public DimensionViewModel( Dimension dimension )
        {
            Dimension = dimension;
        }
    }
}