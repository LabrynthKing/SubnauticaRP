using Nautilus.Json;
using Nautilus.Options.Attributes;

// ReSharper disable InconsistentNaming

namespace SubnauticaRP;

[Menu("SRP (Subnautica Rich Presence)")]
public class Config : ConfigFile
{
    // No Text Input Yet Lol
    public string AppId = "1506535741109571705";

    [Toggle("Enable Force Refresh", Order = 3)]
    public bool EnableForceRefresh;

    [Toggle("Enable Hover Text", Order = 2)]
    public bool EnableHoverText;

    [Toggle("Enable SRP", Order = 1)] public bool EnableSRP;

    [Slider("Force Refresh Timer", 20, 300, DefaultValue = 60, Format = "{0:F2}", Order = 5)]
    public int ForceRefreshTimer;

    [Slider("RPC Update Interval", 0.1f, 60.0f, DefaultValue = 5.0f, Format = "{0:F2}", Order = 4)]
    public float RPCUpdateInterval;
}