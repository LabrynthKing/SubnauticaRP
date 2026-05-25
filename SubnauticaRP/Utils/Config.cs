using Nautilus.Json;
using Nautilus.Options.Attributes;

// ReSharper disable InconsistentNaming

namespace SubnauticaRP.Utils;

[Menu("SRP (Subnautica Rich Presence)")]
public class Config : ConfigFile
{
    // No Text Input Yet Lol
    public string AppId = "1506535741109571705";

    [Toggle("Enable 'Deep Depths' Text", Order = 3,
        Tooltip = "Enables Or Disables The 'Of The Deep Depths' Text In Deep Biomes")]
    public bool EnableDeepDepths;

    [Toggle("Enable Hover Text", Order = 2,
        Tooltip = "Enables Or Disables The Image Hover Texts (In Case You Don't Like Them)")]
    public bool EnableHoverText;

    [Toggle("Enable SRP", Order = 1, Tooltip = "Enables Or Disables The Rich Presence")]
    public bool EnableSRP;

    [Slider("RPC Update Interval", 1f, 60f, DefaultValue = 15f, Format = "{0:F2}", Order = 4,
        Tooltip =
            "Sets The Update Interval Of The Rich Presence (RECOMMENDED 15 SECONDS BECAUSE OF DISCORD RATE LIMITS)")]
    public float RPCUpdateInterval;
}