using System;
using System.Collections.Generic;
using System.Linq;
using DiscordRPC;
using static SubnauticaRP.Maps.HoverMap;

namespace SubnauticaRP.Maps;

// TODO: Maybe Add Custom Biome Support?
public static class BiomeMap
{
    // Check Caves First So Imma Do 2 Thingys
    private static readonly Dictionary<string, BiomeData> CaveBiomes = new()
    {
        ["safeshallows_cave"] = new BiomeData
        {
            Details = "Safe Shallows Caves", LargeImageKey = "safecaves", IsDeep = false
        },
        ["kelpforest_cave"] = new BiomeData
        {
            Details = "Kelp Forest Caves", LargeImageKey = "kelpforestcave", IsDeep = false
        },
        ["bloodkelp_cave"] = new BiomeData
        {
            Details = "Blood Kelp Caves", LargeImageKey = "bloodkelpcave", IsDeep = true
        },
        ["kooshzone_cave"] = new BiomeData
        {
            Details = "Bulb Zone Caves", LargeImageKey = "bulbzonecave", IsDeep = false
        },
        ["mountains_cave"] = new BiomeData
        {
            Details = "Mountains Caves", LargeImageKey = "mountainscave", IsDeep = false
        },
        ["mushroomforest_cave"] = new BiomeData
        {
            Details = "Mushroom Forest Caves", LargeImageKey = "mushroomforestcave", IsDeep = false
        },
        ["grandreef_cave"] = new BiomeData
        {
            Details = "Grand Reef Caves", LargeImageKey = "grandreefcave", IsDeep = false
        },
        ["grassyplateaus_cave"] = new BiomeData
        {
            Details = "Grassy Plateaus Caves", LargeImageKey = "grassyplateauscave", IsDeep = false
        },
        ["seatreaderpath_cave"] = new BiomeData
        {
            Details = "Sea Treader's Tunnel Caves", LargeImageKey = "stpcave", IsDeep = false
        },
        ["unwaterislands_islandcave"] = new BiomeData
        {
            Details = "Underwater Islands Caves", LargeImageKey = "undercave", IsDeep = false
        },
        ["unwaterislands_cave"] = new BiomeData
        {
            Details = "Underwater Islands Caves", LargeImageKey = "undercave", IsDeep = false
        },
        ["lostriver_bonesfield"] = new BiomeData
        {
            Details = "Lost River Bones Fields", LargeImageKey = "lrbonesfields", IsDeep = true
        },
        ["dunes_cave"] = new BiomeData
        {
            Details = "Dunes Caves", LargeImageKey = "dunescave", IsDeep = false
        },
        ["sparsereef_deep"] = new BiomeData
        {
            Details = "Deep Sparse Reef", LargeImageKey = "deepsparse", IsDeep = false
        },
        ["deepsparsereef"] = new BiomeData
        {
            Details = "Deep Sparse Reef", LargeImageKey = "deepsparse", IsDeep = false
        },
        ["ilzcastle"] = new BiomeData
        {
            Details = "Lava Castle", LargeImageKey = "lavacastle", IsDeep = true
        },
        ["prison_aquarium"] = new BiomeData
        {
            Details = "PCF - Aquarium", LargeImageKey = "pcfaq", IsDeep = true
        },
        ["crashzone_mesa"] = new BiomeData
        {
            Details = "Crash Zone Mesas", LargeImageKey = "czmesa", IsDeep = false
        },
        ["deepgrandreef"] = new BiomeData
        {
            Details = "Deep Grand Reef", LargeImageKey = "deepgrandreef", IsDeep = true
        },
        // I forgor which of these two was in-game name so uhh who cares-
        ["lostriver_tree"] = new BiomeData
        {
            Details = "Cove Tree", LargeImageKey = "cove", IsDeep = true
        },
        ["lostriver_cove"] = new BiomeData
        {
            Details = "Cove Tree", LargeImageKey = "cove", IsDeep = true
        }
    };

    // For Normal Stuff
    private static readonly Dictionary<string, BiomeData> Biomes = new()
    {
        ["safeshallows"] = new BiomeData
        {
            Details = "Safe Shallows", LargeImageKey = "safeshallows", IsDeep = false
        },
        ["kelpforest"] = new BiomeData
        {
            Details = "Kelp Forest", LargeImageKey = "kelpforest", IsDeep = false
        },
        ["bloodkelp"] = new BiomeData
        {
            Details = "Blood Kelp", LargeImageKey = "bloodkelp", IsDeep = false
        },
        ["kooshzone"] = new BiomeData
        {
            Details = "Bulb Zone", LargeImageKey = "bulbzone", IsDeep = false
        },
        ["cragfield"] = new BiomeData
        {
            Details = "Crag Field", LargeImageKey = "cragfield", IsDeep = false
        },
        ["crashzone"] = new BiomeData
        {
            Details = "Crash Zone", LargeImageKey = "crashzone", IsDeep = false
        },
        ["void"] = new BiomeData
        {
            Details = "Void", LargeImageKey = "void", IsDeep = true
        },
        ["dunes"] = new BiomeData
        {
            Details = "Dunes", LargeImageKey = "dunes", IsDeep = false
        },
        ["grandreef"] = new BiomeData
        {
            Details = "Grand Reef", LargeImageKey = "grandreef", IsDeep = false
        },
        ["grassyplateaus"] = new BiomeData
        {
            Details = "Grassy Plateaus", LargeImageKey = "grassyplateaus", IsDeep = false
        },
        ["mountains"] = new BiomeData
        {
            Details = "Mountains", LargeImageKey = "mountains", IsDeep = false
        },
        ["mushroomforest"] = new BiomeData
        {
            Details = "Mushroom Forest", LargeImageKey = "mushroom", IsDeep = false
        },
        ["seatreaderpath"] = new BiomeData
        {
            Details = "Sea Treader's Path", LargeImageKey = "stp", IsDeep = false
        },
        ["sparsereef"] = new BiomeData
        {
            Details = "Sparse Reef", LargeImageKey = "sparsereef", IsDeep = false
        },
        ["underwaterislands"] = new BiomeData
        {
            Details = "Underwater Islands", LargeImageKey = "underisland", IsDeep = false
        },
        ["floatingisland"] = new BiomeData
        {
            Details = "Floating Islands", LargeImageKey = "floating", IsDeep = false
        },
        ["lostriver"] = new BiomeData
        {
            Details = "Lost River", LargeImageKey = "lost", IsDeep = true
        },
        ["ilz"] = new BiomeData
        {
            Details = "Inactive Lava Zone", LargeImageKey = "ilz", IsDeep = true
        },
        ["jellyshroomcaves"] = new BiomeData
        {
            Details = "JellyShroom Caves", LargeImageKey = "jelly", IsDeep = false
        },
        ["lavalakes"] = new BiomeData
        {
            Details = "Lava Lakes", LargeImageKey = "lavalakes", IsDeep = true
        },
        ["prison"] = new BiomeData
        {
            Details = "Primary Containment Facility", LargeImageKey = "pcf", IsDeep = true
        }
    };

    public static void MapBiome(RichPresence presence, string biome, Func<string, bool, string> formatter)
    {
        foreach (var kv in CaveBiomes.Where(kv => biome.Contains(kv.Key)))
        {
            Apply(presence, kv.Key, kv.Value, formatter);
            return;
        }

        foreach (var kv in Biomes.Where(kv => biome.Contains(kv.Key)))
        {
            Apply(presence, kv.Key, kv.Value, formatter);
            return;
        }

        var formattedName = char.ToUpper(biome[0]) + biome[1..];
        presence.Details = formatter(formattedName, false);
        presence.Assets.LargeImageText = GetRandomL("unkbiome");
    }

    private static void Apply(RichPresence presence, string key, BiomeData data, Func<string, bool, string> formatter)
    {
        presence.Details = formatter(data.Details, data.IsDeep);
        presence.Assets.LargeImageKey = data.LargeImageKey;
        presence.Assets.LargeImageText = GetRandomL(key);
    }
}