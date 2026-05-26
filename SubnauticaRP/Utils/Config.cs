using Nautilus.Json;
using Nautilus.Options.Attributes;

// ReSharper disable InconsistentNaming

namespace SubnauticaRP.Utils;

[Menu("SRP (Subnautica Rich Presence)")]
public class Config : ConfigFile
{
    // No Text Input Yet Lol
    public string AppId = "1506535741109571705";

    [Toggle(LabelLanguageId = "EnableDeepDepths_Setting", Order = 3,
        TooltipLanguageId = "EnableDeepDepths_Setting_Tooltip")]
    public bool EnableDeepDepths = true;

    [Toggle(LabelLanguageId = "EnableHoverText_Setting", Order = 2,
        TooltipLanguageId = "EnableHoverText_Setting_Tooltip")]
    public bool EnableHoverText = true;

    [Toggle(LabelLanguageId = "EnableSRP_Setting", Order = 1, TooltipLanguageId = "EnableSRP_Setting_Tooltip")]
    public bool EnableSRP = true;

    [Slider(
        LabelLanguageId = "RPCUpdateInterval_Setting",
        Min = 1f,
        Max = 60f,
        DefaultValue = 15f,
        Format = "{0:F0}",
        Step = 1f,
        Order = 4,
        TooltipLanguageId = "RPCUpdateInterval_Setting_Tooltip"
    )]
    public float RPCUpdateInterval = 15f;
}