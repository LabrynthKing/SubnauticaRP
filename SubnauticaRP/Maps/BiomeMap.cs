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
            Details = Language.main.Get("Safe_Shallows_Caves"), LargeImageKey = "safecaves", IsDeep = false
        },
        ["kelpforest_cave"] = new BiomeData
        {
            Details = Language.main.Get("Kelp_Forest_Caves"), LargeImageKey = "kelpforestcave", IsDeep = false
        },
        ["bloodkelp_cave"] = new BiomeData
        {
            Details = Language.main.Get("Blood_Kelp_Caves"), LargeImageKey = "bloodkelpcave", IsDeep = true
        },
        ["kooshzone_cave"] = new BiomeData
        {
            Details = Language.main.Get("Bulb_Zone_Caves"), LargeImageKey = "bulbzonecave", IsDeep = false
        },
        ["mountains_cave"] = new BiomeData
        {
            Details = Language.main.Get("Mountains_Caves"), LargeImageKey = "mountainscave", IsDeep = false
        },
        ["mushroomforest_cave"] = new BiomeData
        {
            Details = Language.main.Get("Mushroom_Forest_Caves"), LargeImageKey = "mushroomforestcave", IsDeep = false
        },
        ["grandreef_cave"] = new BiomeData
        {
            Details = Language.main.Get("Grand_Reef_Caves"), LargeImageKey = "grandreefcave", IsDeep = false
        },
        ["grassyplateaus_cave"] = new BiomeData
        {
            Details = Language.main.Get("Grassy_Plateaus_Caves"), LargeImageKey = "grassyplateauscave", IsDeep = false
        },
        ["seatreaderpath_cave"] = new BiomeData
        {
            Details = Language.main.Get("Sea_Treader_Tunnel_Caves"), LargeImageKey = "stpcave", IsDeep = false
        },
        ["unwaterislands_islandcave"] = new BiomeData
        {
            Details = Language.main.Get("Underwater_Islands_Caves"), LargeImageKey = "undercave", IsDeep = false
        },
        ["unwaterislands_cave"] = new BiomeData
        {
            Details = Language.main.Get("Underwater_Islands_Caves"), LargeImageKey = "undercave", IsDeep = false
        },
        ["lostriver_bonesfield"] = new BiomeData
        {
            Details = Language.main.Get("Lost_River_Bones_Fields"), LargeImageKey = "lrbonesfields", IsDeep = true
        },
        ["dunes_cave"] = new BiomeData
        {
            Details = Language.main.Get("Dunes_Caves"), LargeImageKey = "dunescave", IsDeep = false
        },
        ["sparsereef_deep"] = new BiomeData
        {
            Details = Language.main.Get("Deep_Sparse_Reef"), LargeImageKey = "deepsparse", IsDeep = false
        },
        ["deepsparsereef"] = new BiomeData
        {
            Details = Language.main.Get("Deep_Sparse_Reef"), LargeImageKey = "deepsparse", IsDeep = false
        },
        ["ilzcastle"] = new BiomeData
        {
            Details = Language.main.Get("Lava_Castle"), LargeImageKey = "lavacastle", IsDeep = true
        },
        ["prison_aquarium"] = new BiomeData
        {
            Details = Language.main.Get("PCF_Aquarium"), LargeImageKey = "pcfaq", IsDeep = true
        },
        ["crashzone_mesa"] = new BiomeData
        {
            Details = Language.main.Get("Crash_Zone_Mesas"), LargeImageKey = "czmesa", IsDeep = false
        },
        ["deepgrandreef"] = new BiomeData
        {
            Details = Language.main.Get("Deep_Grand_Reef"), LargeImageKey = "deepgrandreef", IsDeep = true
        },
        // I forgor which of these two was in-game name so uhh who cares-
        ["lostriver_tree"] = new BiomeData
        {
            Details = Language.main.Get("Cove_Tree"), LargeImageKey = "cove", IsDeep = true
        },
        ["lostriver_cove"] = new BiomeData
        {
            Details = Language.main.Get("Cove_Tree"), LargeImageKey = "cove", IsDeep = true
        }
    };

    // For Normal Stuff
    private static readonly Dictionary<string, BiomeData> Biomes = new()
    {
        ["safeshallows"] = new BiomeData
        {
            Details = Language.main.Get("Safe_Shallows"), LargeImageKey = "safeshallows", IsDeep = false
        },
        ["kelpforest"] = new BiomeData
        {
            Details = Language.main.Get("Kelp_Forest"), LargeImageKey = "kelpforest", IsDeep = false
        },
        ["bloodkelp"] = new BiomeData
        {
            Details = Language.main.Get("Blood_Kelp"), LargeImageKey = "bloodkelp", IsDeep = false
        },
        ["kooshzone"] = new BiomeData
        {
            Details = Language.main.Get("Bulb_Zone"), LargeImageKey = "bulbzone", IsDeep = false
        },
        ["cragfield"] = new BiomeData
        {
            Details = Language.main.Get("Crag_Field"), LargeImageKey = "cragfield", IsDeep = false
        },
        ["crashzone"] = new BiomeData
        {
            Details = Language.main.Get("Crash_Zone"), LargeImageKey = "crashzone", IsDeep = false
        },
        ["void"] = new BiomeData
        {
            Details = Language.main.Get("Void"), LargeImageKey = "void", IsDeep = true
        },
        ["dunes"] = new BiomeData
        {
            Details = Language.main.Get("Dunes"), LargeImageKey = "dunes", IsDeep = false
        },
        ["grandreef"] = new BiomeData
        {
            Details = Language.main.Get("Grand_Reef"), LargeImageKey = "grandreef", IsDeep = false
        },
        ["grassyplateaus"] = new BiomeData
        {
            Details = Language.main.Get("Grassy_Plateaus"), LargeImageKey = "grassyplateaus", IsDeep = false
        },
        ["mountains"] = new BiomeData
        {
            Details = Language.main.Get("Mountains"), LargeImageKey = "mountains", IsDeep = false
        },
        ["mushroomforest"] = new BiomeData
        {
            Details = Language.main.Get("Mushroom_Forest"), LargeImageKey = "mushroom", IsDeep = false
        },
        ["seatreaderpath"] = new BiomeData
        {
            Details = Language.main.Get("Sea_Treader_Path"), LargeImageKey = "stp", IsDeep = false
        },
        ["sparsereef"] = new BiomeData
        {
            Details = Language.main.Get("Sparse_Reef"), LargeImageKey = "sparsereef", IsDeep = false
        },
        ["underwaterislands"] = new BiomeData
        {
            Details = Language.main.Get("Underwater_Islands"), LargeImageKey = "underisland", IsDeep = false
        },
        ["floatingisland"] = new BiomeData
        {
            Details = Language.main.Get("Floating_Islands"), LargeImageKey = "floating", IsDeep = false
        },
        ["lostriver"] = new BiomeData
        {
            Details = Language.main.Get("Lost_River"), LargeImageKey = "lost", IsDeep = true
        },
        ["ilz"] = new BiomeData
        {
            Details = Language.main.Get("Inactive_Lava_Zone"), LargeImageKey = "ilz", IsDeep = true
        },
        ["jellyshroomcaves"] = new BiomeData
        {
            Details = Language.main.Get("JellyShroom_Caves"), LargeImageKey = "jelly", IsDeep = false
        },
        ["lavalakes"] = new BiomeData
        {
            Details = Language.main.Get("Lava_Lakes"), LargeImageKey = "lavalakes", IsDeep = true
        },
        ["prison"] = new BiomeData
        {
            Details = Language.main.Get("Primary_Containment_Facility"), LargeImageKey = "pcf", IsDeep = true
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