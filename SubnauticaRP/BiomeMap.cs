using System;
using System.Collections.Generic;
using System.Linq;
using DiscordRPC;

namespace SubnauticaRP;

// TODO: Maybe Add Custom Biome Support?
public static class BiomeMap
{
    // Check Caves First So Imma Do 2 Thingys
    private static readonly Dictionary<string, BiomeData> CaveBiomes = new()
    {
        ["safeshallows_cave"] = new BiomeData
        {
            Details = "Safe Shallows Caves", LargeImageKey = "safecaves",
            LargeImageText = "Woah, A Red Fis- *EXPLODES*", IsDeep = false
        },
        ["kelpforest_cave"] = new BiomeData
        {
            Details = "Kelp Forest Caves", LargeImageKey = "kelpforestcave",
            LargeImageText = "Eye Stalk U", IsDeep = false
        },
        ["bloodkelp_cave"] = new BiomeData
        {
            Details = "Blood Kelp Caves", LargeImageKey = "bloodkelpcave",
            LargeImageText = "BLOOD FOR THE BLOOD GOD", IsDeep = true
        },
        ["kooshzone_cave"] = new BiomeData
        {
            Details = "Bulb Zone Caves", LargeImageKey = "bulbzonecave",
            LargeImageText = "Don't Go In The Lava Vent Stoobid", IsDeep = false
        },
        ["mountains_cave"] = new BiomeData
        {
            Details = "Mountains Caves", LargeImageKey = "mountainscave",
            LargeImageText = "More Poisonous Tentacles!!?? Ah Shi-", IsDeep = false
        },
        ["mushroomforest_cave"] = new BiomeData
        {
            Details = "Mushroom Forest Caves", LargeImageKey = "mushroomforestcave",
            LargeImageText = "CUDDLEFISH EGGGGGGGGGGGG", IsDeep = false
        },
        ["grandreef_cave"] = new BiomeData
        {
            Details = "Grand Reef Caves", LargeImageKey = "grandreefcave",
            LargeImageText = "Is That URANIUM?? Call Nile Red", IsDeep = false
        },
        ["grassyplateaus_cave"] = new BiomeData
        {
            Details = "Grassy Plateaus Caves", LargeImageKey = "grassyplateauscave",
            LargeImageText = "Why Are There Poisonous Tentacles!?", IsDeep = false
        },
        ["seatreaderpath_cave"] = new BiomeData
        {
            Details = "Sea Treader's Tunnel Caves", LargeImageKey = "stpcave",
            LargeImageText = "Walking Simulator: Tunnel Edition", IsDeep = false
        },
        ["unwaterislands_islandcave"] = new BiomeData
        {
            Details = "Underwater Islands Caves", LargeImageKey = "undercave",
            LargeImageText = "I Go Under The Island", IsDeep = false
        },
        ["unwaterislands_cave"] = new BiomeData
        {
            Details = "Underwater Islands Caves", LargeImageKey = "undercave",
            LargeImageText = "I Go Under The Island", IsDeep = false
        },
        ["lostriver_bonesfield"] = new BiomeData
        {
            Details = "Lost River Bones Fields", LargeImageKey = "lrbonesfields",
            LargeImageText = "What If This Ancient Were To Return?", IsDeep = true
        },
        ["dunes_cave"] = new BiomeData
        {
            Details = "Dunes Caves", LargeImageKey = "dunescave",
            LargeImageText = "The Reapers Are Waiting.", IsDeep = false
        },
        ["sparsereef_deep"] = new BiomeData
        {
            Details = "Deep Sparse Reef", LargeImageKey = "deepsparse",
            LargeImageText = "Homing Thistles HELP", IsDeep = false
        },
        ["deepsparsereef"] = new BiomeData
        {
            Details = "Deep Sparse Reef", LargeImageKey = "deepsparse",
            LargeImageText = "Homing Thistles HELP", IsDeep = false
        },
        ["ilzcastle"] = new BiomeData
        {
            Details = "Lava Castle", LargeImageKey = "lavacastle",
            LargeImageText = "A Thermal Plant Here Would Be Nic- Oh...", IsDeep = true
        },
        ["prison_aquarium"] = new BiomeData
        {
            Details = "PCF - Aquarium", LargeImageKey = "pcfaq",
            LargeImageText = "HOLY SHI- ITS THE EMPEROR OF THE SEA", IsDeep = true
        },
        ["crashzone_mesa"] = new BiomeData
        {
            Details = "Crash Zone Mesas", LargeImageKey = "czmesa",
            LargeImageText = "All Around Me Are Reaper Faces...", IsDeep = false
        },
        ["deepgrandreef"] = new BiomeData
        {
            Details = "Deep Grand Reef", LargeImageKey = "deepgrandreef",
            LargeImageText = "Enemy EMP Inbound", IsDeep = true
        },
        // I forgor which of these two was in-game name so uhh who cares-
        ["lostriver_tree"] = new BiomeData
        {
            Details = "Cove Tree", LargeImageKey = "cove",
            LargeImageText = "So Comfy :)", IsDeep = true
        },
        ["lostriver_cove"] = new BiomeData
        {
            Details = "Cove Tree", LargeImageKey = "cove",
            LargeImageText = "So Comfy :)", IsDeep = true
        }
    };

    // For Normal Stuff
    private static readonly Dictionary<string, BiomeData> Biomes = new()
    {
        ["safeshallows"] = new BiomeData
        {
            Details = "Safe Shallows", LargeImageKey = "safeshallows",
            LargeImageText = "YAY I AM SAFE (for now)", IsDeep = false
        },
        ["kelpforest"] = new BiomeData
        {
            Details = "Kelp Forest", LargeImageKey = "kelpforest",
            LargeImageText = "Can Someone Tell Bro To Clean His Teeth...", IsDeep = false
        },
        ["bloodkelp"] = new BiomeData
        {
            Details = "Blood Kelp", LargeImageKey = "bloodkelp",
            LargeImageText = "Mr. Crabs? OH SHI ITS AN EMP-", IsDeep = false
        },
        ["kooshzone"] = new BiomeData
        {
            Details = "Bulb Zone", LargeImageKey = "bulbzone",
            LargeImageText = "Amp Up The Eels!", IsDeep = false
        },
        ["cragfield"] = new BiomeData
        {
            Details = "Crag Field", LargeImageKey = "cragfield",
            LargeImageText = "There's A Shark In My Bones", IsDeep = false
        },
        ["crashzone"] = new BiomeData
        {
            Details = "Crash Zone", LargeImageKey = "crashzone",
            LargeImageText = "Why's The Water So Murky- HOLY SHI-", IsDeep = false
        },
        ["void"] = new BiomeData
        {
            Details = "Void", LargeImageKey = "void",
            LargeImageText = "Warning! Entering Ecological Dead Zone...", IsDeep = true
        },
        ["dunes"] = new BiomeData
        {
            Details = "Dunes", LargeImageKey = "dunes",
            LargeImageText = "Detecting Multiple Leviathan Class Lifeforms Nearby..YOU'RE COOKED", IsDeep = false
        },
        ["grandreef"] = new BiomeData
        {
            Details = "Grand Reef", LargeImageKey = "grandreef",
            LargeImageText = "Wait, WHY'S THERE A GHOS- %%$$##$%Q$", IsDeep = false
        },
        ["grassyplateaus"] = new BiomeData
        {
            Details = "Grassy Plateaus", LargeImageKey = "grassyplateaus",
            LargeImageText = "Yay, I love reefies :)", IsDeep = false
        },
        ["mountains"] = new BiomeData
        {
            Details = "Mountains", LargeImageKey = "mountains",
            LargeImageText = "I-Is That An Island!??", IsDeep = false
        },
        ["mushroomforest"] = new BiomeData
        {
            Details = "Mushroom Forest", LargeImageKey = "mushroom",
            LargeImageText = "Dam Das A Lotta Trees..Or Mushrooms Ig", IsDeep = false
        },
        ["seatreaderpath"] = new BiomeData
        {
            Details = "Sea Treader's Path", LargeImageKey = "stp",
            LargeImageText = "Walking Simulator", IsDeep = false
        },
        ["sparsereef"] = new BiomeData
        {
            Details = "Sparse Reef", LargeImageKey = "sparsereef",
            LargeImageText = "I Hate Tiger Plants", IsDeep = false
        },
        ["underwaterislands"] = new BiomeData
        {
            Details = "Underwater Islands", LargeImageKey = "underisland",
            LargeImageText = "Are They Really Islands If They're Underwater?", IsDeep = false
        },
        ["floatingisland"] = new BiomeData
        {
            Details = "Floating Islands", LargeImageKey = "floating",
            LargeImageText = "It's An Island...That's Floating", IsDeep = false
        },
        ["lostriver"] = new BiomeData
        {
            Details = "Lost River", LargeImageKey = "lost",
            LargeImageText = "Mom...Dad...This Is Scary...", IsDeep = true
        },
        ["ilz"] = new BiomeData
        {
            Details = "Inactive Lava Zone", LargeImageKey = "ilz",
            LargeImageText = "There Are Dragons In This Game!?", IsDeep = true
        },
        ["jellyshroomcaves"] = new BiomeData
        {
            Details = "JellyShroom Caves", LargeImageKey = "jelly",
            LargeImageText = "Can I Eat The Jelly?", IsDeep = false
        },
        ["lavalakes"] = new BiomeData
        {
            Details = "Lava Lakes", LargeImageKey = "lavalakes",
            LargeImageText = "Kinda Hot In Here Don't You Think?", IsDeep = true
        },
        ["prison"] = new BiomeData
        {
            Details = "Primary Containment Facility", LargeImageKey = "pcf",
            LargeImageText = "This Is Where They Locked My Boy John Up", IsDeep = true
        }
    };

    public static void MapBiome(RichPresence presence, string biome, Func<string, bool, string> formatter)
    {
        foreach (var kv in CaveBiomes.Where(kv => biome.Contains(kv.Key)))
        {
            Apply(presence, kv.Value, formatter);
            return;
        }

        foreach (var kv in Biomes.Where(kv => biome.Contains(kv.Key)))
        {
            Apply(presence, kv.Value, formatter);
            return;
        }

        var formattedName = char.ToUpper(biome[0]) + biome[1..];
        presence.Details = formatter(formattedName, false);
        presence.Assets.LargeImageText = "I C@N H$@R TH$IR V0IC$E$$";
    }

    private static void Apply(RichPresence presence, BiomeData data, Func<string, bool, string> formatter)
    {
        presence.Details = formatter(data.Details, data.IsDeep);
        presence.Assets.LargeImageKey = data.LargeImageKey;
        presence.Assets.LargeImageText = data.LargeImageText;
    }
}