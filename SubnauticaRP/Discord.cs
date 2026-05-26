using System;
using DiscordRPC;
using RichPresenceAPI;
using SubnauticaRP.Maps;
using static SubnauticaRP.Maps.HoverMap;

namespace SubnauticaRP;

public class Discord
{
    private readonly Timestamps _sessionTime = new() { Start = DateTime.UtcNow };
    private DiscordRpcClient _client;
    private bool _hasPresence;

    public void Initialize()
    {
        var appId = string.IsNullOrWhiteSpace(Plugin.ModConfig.AppId)
            ? "1506535741109571705"
            : Plugin.ModConfig.AppId.Trim();

        Plugin.Logger.LogInfo($"Connecting To AppID: {appId}");

        try
        {
            _client = Utility.CreateDiscordRpcClient(appId);
            _client.SkipIdenticalPresence = false;

            _client.OnReady += (_, e) =>
            {
                Plugin.Logger.LogInfo($"Bound Successfully To Discord Profile: {e.User.Username}");
            };

            _client.OnError += (_, e) => { Plugin.Logger.LogError($"API Exception: {e.Message}"); };

            _client.OnConnectionFailed += (_, e) =>
            {
                Plugin.Logger.LogError($"Connection Drop On Pipe Channel: {e.FailedPipe}");
            };

            _client.Initialize();

            Plugin.Logger.LogInfo("Pipeline Initialization Fired");
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e.Message);
        }
    }

    public void UpdatePresence(bool runMainMenu)
    {
        if (_client is null) return;

        if (!Plugin.ModConfig.EnableSRP)
        {
            if (_hasPresence)
            {
                _client.ClearPresence();
                _hasPresence = false;
            }

            return;
        }

        if (runMainMenu || Player.main is null || DayNightCycle.main is null)
        {
            _client.SetPresence(new RichPresence
            {
                Details = Language.main.Get("MainMenu_Details"),
                State = Language.main.Get("MainMenu_State"),
                Timestamps = _sessionTime
            });
            _hasPresence = true;

            return;
        }

        var presence = new RichPresence
        {
            Timestamps = _sessionTime,
            Assets = new Assets()
        };

        var addedSmallImage = AddBiomeInfo(presence);

        if (Player.main.GetVehicle() is not null)
        {
            AddVehicleInfo(presence, addedSmallImage);
        }
        else if (Player.main.GetCurrentSub() is not null)
        {
            // Observatory Is A Biome...For Some Reason??
            if (Player.main.GetCurrentSub().isBase && Player.main.GetBiomeString().ToLower() != "observatory")
            {
                presence.Details = Language.main.Get("Base_Details");
                presence.State = Language.main.Get("Base_State").Replace("{depth}", Player.main.cachedDepth.ToString());
                if (!addedSmallImage)
                {
                    presence.Assets.SmallImageKey = "room";
                    presence.Assets.SmallImageText = GetRandomS("base");
                }
            }
            else if (Player.main.GetCurrentSub().isCyclops)
            {
                if (!addedSmallImage)
                {
                    presence.Assets.SmallImageKey = "cyclops";
                    presence.Assets.SmallImageText = GetRandomS("cyclops");
                }

                presence.State = VehicleState(Language.main.Get("Cyclops_Vehicle"));
            }
        }
        else if (!Player.main.isUnderwater.value && (Player.main.motorMode == Player.MotorMode.Walk ||
                                                     Player.main.motorMode ==
                                                     Player.MotorMode
                                                         .Run)) // I Won't Check Mech Cuz I Think It Should Be Caught Already
        {
            presence.State = Language.main.Get("Land_State");
            if (!addedSmallImage)
            {
                presence.Assets.SmallImageKey = "fins";
                presence.Assets.SmallImageText = GetRandomS("land");
            }
        }
        else
        {
            if (Player.main.motorMode == Player.MotorMode.Seaglide)
            {
                presence.State = Language.main.Get("Seaglide_State")
                    .Replace("{depth}", Player.main.cachedDepth.ToString());
                if (!addedSmallImage)
                {
                    presence.Assets.SmallImageKey = "seaglide";
                    presence.Assets.SmallImageText = GetRandomS("seaglide");
                }
            }
            else
            {
                presence.State = Language.main.Get("Swim_State").Replace("{depth}", Player.main.cachedDepth.ToString());
                if (!addedSmallImage)
                {
                    presence.Assets.SmallImageKey = "fins";
                    presence.Assets.SmallImageText = GetRandomS("swim");
                }
            }
        }

        if (!Plugin.ModConfig.EnableHoverText)
        {
            presence.Assets.LargeImageText = null;
            presence.Assets.SmallImageText = null;
        }

        _client.SetPresence(presence);
        _hasPresence = true;
    }

    private static bool AddBiomeInfo(RichPresence presence)
    {
        var biomeString = Player.main.GetBiomeString().ToLower().Trim();

        switch (biomeString)
        {
            case "observatory": // if (ObservatoryAmbientSound.IsPlayerInObservatory()) { return "observatory"; }
            {
                presence.Details = Language.main.Get("Observatory_Details");
                presence.Assets.LargeImageKey = "observatory";
                presence.Assets.LargeImageText = GetRandomL("observe");
                presence.Assets.SmallImageKey = "eyes";
                presence.Assets.SmallImageText = GetRandomS("observe");
                return true;
            }
            case "generatorroom"
                : // if (GeneratorRoomAmbientSound.main && GeneratorRoomAmbientSound.main.isPlayerInside) { return "generatorRoom"; }
            {
                presence.Details = Language.main.Get("Aurora_Drive_Room_Details");
                presence.Assets.LargeImageKey = "aurora";
                presence.Assets.LargeImageText = GetRandomL("genroom");
                return false;
            }
            case "crashedship"
                : // if (CrashedShipAmbientSound.main && CrashedShipAmbientSound.main.isPlayerInside) { return "crashedShip"; }
            {
                presence.Details = Language.main.Get("Aurora_Explore_Details");
                presence.Assets.LargeImageKey = "aurora";
                presence.Assets.LargeImageText = GetRandomL("aurora");
                return false;
            }
            case "lifepod":
            {
                presence.Details = Language.main.Get("Lifepod_Details");
                presence.Assets.LargeImageKey = "lifepod";
                presence.Assets.LargeImageText = GetRandomL("lifepod");
                presence.Assets.SmallImageKey = "room";
                presence.Assets.SmallImageText = GetRandomS("lifepod");
                return true;
            }
            case "precursor":
            {
                presence.Details = Language.main.Get("Precursor_Details");
                presence.Assets.LargeImageKey = "precursor";
                presence.Assets.LargeImageText = GetRandomL("precursor");
                return false;
            }
            case "<unknown>":
            case "unassigned":
            case "":
            {
                presence.Details = Language.main.Get("UnknownBiome_Details");
                presence.Assets.LargeImageKey = "unknown";
                presence.Assets.LargeImageText = GetRandomL("unk");
                return false;
            }
        }

        BiomeMap.MapBiome(presence, biomeString, BiomeDetails);
        return false;
    }

    private static void AddVehicleInfo(RichPresence presence, bool addedSmallImage)
    {
        var vehicle = Player.main.GetVehicle().GetType().Name.ToLower().Trim();

        VehicleMap.MapVehicle(presence, vehicle, addedSmallImage, VehicleState);
    }

    // This Only Here Cuz I Am Too Lazy
    private static string VehicleState(string vehicle)
    {
        return Player.main.isPiloting
            ? Language.main.Get("Vehicle_Piloting_State").Replace("{vehicle}", vehicle)
                .Replace("{depth}", Player.main.cachedDepth.ToString())
            : Language.main.Get("Vehicle_Chilling_State").Replace("{vehicle}", vehicle)
                .Replace("{depth}", Player.main.cachedDepth.ToString());
    }

    private static string BiomeDetails(string biomeString, bool deep = false)
    {
        if (deep && Plugin.ModConfig.EnableDeepDepths)
            return Language.main.Get("Biome_Deep_Details").Replace("{biome}", biomeString);

        return Language.main.Get("Biome_Details").Replace("{biome}", biomeString);
    }

    public void Shutdown()
    {
        Plugin.Logger.LogInfo("Disposing Discord Connection");
        _client?.Dispose();
        _client = null;
    }
}