using System;
using DiscordRPC;
using RichPresenceAPI;

namespace SubnauticaRP;

public class Discord
{
    private readonly Timestamps _sessionTime = new() { Start = DateTime.UtcNow };
    private DiscordRpcClient _client;
    private bool _hasPresence;

    private float _timeSinceRefresh;

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

    public void UpdatePresence(float deltaTime)
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

        if (Player.main is null || DayNightCycle.main is null)
        {
            MenuPresence();

            return;
        }

        var presence = new RichPresence
        {
            Timestamps = _sessionTime,
            Assets = new Assets()
        };

        AddBiomeInfo(presence);

        if (Player.main.GetVehicle() is not null)
        {
            AddVehicleInfo(presence);
        }
        else if (Player.main.GetCurrentSub() is not null)
        {
            // Observatory Is A Biome...For Some Reason??
            if (Player.main.GetCurrentSub().isBase && Player.main.GetBiomeString().ToLower() != "observatory")
            {
                presence.Details = "In A Base";
                presence.State = $"Chilling At {Player.main.cachedDepth}m";
                presence.Assets.SmallImageKey = "room";
                presence.Assets.SmallImageText = "Ghost Leviathans Watch Me Sleep";
            }
            else if (Player.main.GetCurrentSub().isCyclops)
            {
                presence.Assets.SmallImageKey = "cyclops";
                presence.Assets.SmallImageText = "One Eye";
                presence.State = VehicleState("Cyclops");
            }
        }
        else if (!Player.main.isUnderwater.value && (Player.main.motorMode == Player.MotorMode.Walk ||
                                                     Player.main.motorMode ==
                                                     Player.MotorMode
                                                         .Run)) // I Won't Check Mech Cuz I Think It Should Be Caught Already
        {
            presence.State = "Walking Across The Mountains";
            presence.Assets.SmallImageKey = "fins";
            presence.Assets.SmallImageText = "Finally! LAND! Wait What Do You Mean There's Killer Cra- AHHHHH";
        }
        else
        {
            if (Player.main.motorMode == Player.MotorMode.Seaglide)
            {
                presence.State = $"Seagliding Across The Sea At {Player.main.cachedDepth}m";
                presence.Assets.SmallImageKey = "seaglide";
                presence.Assets.SmallImageText = "Gotta Go Fast!";
            }
            else
            {
                presence.State = $"Swimming Across The Sea At {Player.main.cachedDepth}m";
                presence.Assets.SmallImageKey = "fins";
                presence.Assets.SmallImageText = "OXYGEN";
            }
        }

        if (Plugin.ModConfig.EnableForceRefresh)
        {
            _timeSinceRefresh += deltaTime;
            if (_timeSinceRefresh > Plugin.ModConfig.ForceRefreshTimer)
            {
                Plugin.Logger.LogInfo("Forcing Discord Refresh...");
                _timeSinceRefresh = 0f;

                presence.State = "\u200B";
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

    // TODO: Add All Biomes
    private static void AddBiomeInfo(RichPresence presence)
    {
        var biomeString = Player.main.GetBiomeString().ToLower();

        switch (biomeString)
        {
            case "observatory": // if (ObservatoryAmbientSound.IsPlayerInObservatory()) { return "observatory"; }
            {
                presence.Details = "Observing The World";
                presence.Assets.LargeImageKey = "observatory";
                presence.Assets.LargeImageText = "ALW@Y$ W@TCHING";
                presence.Assets.SmallImageKey = "eyes";
                presence.Assets.SmallImageText = "WOAH LOOK THAT BIG LEVIATHAN, IT HAS 3 EYES";
                break;
            }
            case "generatorroom"
                : // if (GeneratorRoomAmbientSound.main && GeneratorRoomAmbientSound.main.isPlayerInside) { return "generatorRoom"; }
            {
                presence.Details = "Fixing The Aurora";
                presence.Assets.LargeImageKey = "aurora";
                presence.Assets.LargeImageText = "AHHH RADIATION HELPPP";
                break;
            }
            case "crashedship"
                : // if (CrashedShipAmbientSound.main && CrashedShipAmbientSound.main.isPlayerInside) { return "crashedShip"; }
            {
                presence.Details = "Exploring The Aurora";
                presence.Assets.LargeImageKey = "aurora";
                presence.Assets.LargeImageText = "Where Did The Captain Go??";
                break;
            }
            case not null when biomeString.Contains("safeshallows"):
            {
                presence.Details = BiomeDetails("Safe Shallows");
                presence.Assets.LargeImageKey = "safeshallows";
                presence.Assets.LargeImageText = "YAY I AM SAFE (for now)";
                break;
            }
            case not null when biomeString.Contains("bloodkelp"):
            {
                presence.Details = BiomeDetails("Blood Kelp");
                presence.Assets.LargeImageKey = "bloodkelp";
                presence.Assets.LargeImageText = "Mr. Crabs? OH SHI ITS AN EMP-";
                break;
            }
            case "<unknown>":
            case "unassigned":
            case "":
            {
                presence.Details = BiomeDetails("Exploring An Unknown Biome...");
                presence.Assets.LargeImageKey = "unknown";
                presence.Assets.LargeImageText = "S@VE MEEE$$$";
                break;
            }
            default:
            {
                presence.Details = BiomeDetails(char.ToUpper(biomeString[0]) + biomeString[1..]);
                presence.Assets.LargeImageText = "I C@N H$@R TH$IR V0IC$E$$";
                break;
            }
        }
    }

    // TODO: Add More Vehicles Support
    private static void AddVehicleInfo(RichPresence presence)
    {
        var vehicle = Player.main.GetVehicle().GetType().Name.ToLower();

        switch (vehicle)
        {
            case "exosuit":
            {
                presence.State = VehicleState("Prawn Suit");
                presence.Assets.SmallImageKey = "exosuit";
                presence.Assets.SmallImageText = "SpiderMan With A Drill";
                break;
            }
            case "seamoth":
            {
                presence.State = VehicleState("Seamoth");
                presence.Assets.SmallImageKey = "seamoth";
                presence.Assets.SmallImageText = "Reaper's Lunch";
                break;
            }
            case "blossom":
            {
                presence.State = VehicleState("Blossom");
                presence.Assets.SmallImageKey = "blossom";
                presence.Assets.SmallImageText = "Hypnotizing Fishes";
                break;
            }
            case "archon":
            {
                presence.State = VehicleState("Archon");
                presence.Assets.SmallImageKey = "archon";
                presence.Assets.SmallImageText = "Rich People Be Like:-";
                break;
            }
            case "beluga":
            {
                presence.State = VehicleState("Beluga");
                presence.Assets.SmallImageKey = "beluga";
                presence.Assets.SmallImageText = "CaseOh Of Submarines";
                break;
            }
            case "echelon":
            {
                presence.State = VehicleState("Echelon");
                presence.Assets.SmallImageKey = "echelon";
                presence.Assets.SmallImageText = "I Am SPEED";
                break;
            }
            case "hydra":
            {
                presence.State = VehicleState("Hydra");
                presence.Assets.SmallImageKey = "hydra";
                presence.Assets.SmallImageText = "The VOID Is CALLING";
                break;
            }
            default:
            {
                presence.State = VehicleState($"{vehicle}");
                presence.Assets.SmallImageKey = "unknown";
                presence.Assets.SmallImageText = $"{vehicle}";
                break;
            }
        }
    }

    public void MenuPresence()
    {
        _client?.SetPresence(new RichPresence
        {
            Details = "In Main Menu",
            State = "Thinking About Life Choices...",
            Assets = new Assets
            {
                LargeImageText = "Press Play Already!"
            },
            Timestamps = _sessionTime
        });
    }

    // This Only Here Cuz I Am Too Lazy
    private static string VehicleState(string vehicle)
    {
        return Player.main.isPiloting
            ? $"Piloting {vehicle} At {Player.main.cachedDepth}m"
            : $"Chilling In {vehicle} At {Player.main.cachedDepth}m";
    }

    private static string BiomeDetails(string biomeString, bool deep = false)
    {
        return deep ? $"Exploring {biomeString} Of The Deep Depths" : $"Exploring {biomeString}";
    }

    public void Shutdown()
    {
        Plugin.Logger.LogInfo("Disposing Discord Connection");
        _client?.Dispose();
        _client = null;
    }
}