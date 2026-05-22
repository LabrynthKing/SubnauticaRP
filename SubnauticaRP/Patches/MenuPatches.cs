using HarmonyLib;

namespace SubnauticaRP.Patches;

// TBH This Was Made Mainly B/C I Wanted To See How Harmony Works
[HarmonyPatch(typeof(MainMenuController))]
public static class MenuPatches
{
    [HarmonyPatch(nameof(MainMenuController.Start))]
    [HarmonyPostfix]
    public static void PostfixMenuStart()
    {
        if (Plugin.Discord is not null) Plugin.Discord.MenuPresence();
    }
}