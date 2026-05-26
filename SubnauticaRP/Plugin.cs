using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility.ModMessages;
using SubnauticaRP.Utils;
using UnityEngine;

namespace SubnauticaRP;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
[BepInDependency("io.github.xhayper.RichPresenceAPI")]
public class Plugin : BaseUnityPlugin
{
    public static Discord Discord;
    private float _timer;

    public new static ManualLogSource Logger { get; private set; }
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    public static Config ModConfig { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;

        LanguageHandler.RegisterLocalizationFolder();

        ModMessageSystem.SendGlobal("FindMyUpdates",
            "https://raw.githubusercontent.com/LabrynthKing/SubnauticaRP/refs/heads/main/version.json");
        ModConfig = OptionsPanelHandler.RegisterModOptions<Config>();

        Discord = new Discord();
        Discord.Initialize();

        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        _timer += deltaTime;

        if (_timer >= ModConfig.RPCUpdateInterval)
        {
            _timer = 0f;
            Discord.UpdatePresence(false);
        }
    }

    public void OnDestroy()
    {
        Discord.Shutdown();
    }
}