using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdmxDlClient.Models;

public record DataStructure
{
    public required string Ref { get; init; }
    public Dimension[] Dimensions { get; init; } = Array.Empty<Dimension>();
    public Attribute[] Attributes { get; init; } = Array.Empty<Attribute>();
    public string TimeDimensionId { get; init; } = string.Empty;
    public required string PrimaryMeasureId { get; init; }
    public required string Name { get; init; }
}