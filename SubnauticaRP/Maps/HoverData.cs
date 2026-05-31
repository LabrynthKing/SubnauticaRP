using System.Collections.Generic;
using JetBrains.Annotations;

namespace SubnauticaRP.Maps;

public class HoverData
{
    public string Name { get; set; } = string.Empty;
    [CanBeNull] public List<string> LargeImageText { get; set; }
    [CanBeNull] public List<string> SmallImageText { get; set; }
}

public class HoverRoot
{
    public string Version { get; set; }
    public List<HoverData> Data { get; set; } = new();
}