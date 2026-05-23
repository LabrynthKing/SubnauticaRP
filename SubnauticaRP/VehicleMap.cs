using System;
using System.Collections.Generic;
using System.Linq;
using DiscordRPC;

namespace SubnauticaRP;

public class VehicleMap
{
    // Funni Typo So I Am Leaving It In
    // TODO: Add More Vehicles Support (Gotta Ask Authors Lol)
    private static readonly Dictionary<string, VehicleData> Vahicles = new()
    {
        ["exosuit"] = new VehicleData
        {
            State = "Prawn Suit",
            SmallImageKey = "exosuit",
            SmallImageText = "SpiderMan With A Drill INCOMING!"
        },
        ["seamoth"] = new VehicleData
        {
            State = "Seamoth",
            SmallImageKey = "seamoth",
            SmallImageText = "Reaper's Lunch"
        },
        ["archon"] = new VehicleData
        {
            State = "Archon",
            SmallImageKey = "archon",
            SmallImageText = "Rich People Be Like:-"
        },
        ["echelon"] = new VehicleData
        {
            State = "Echelon",
            SmallImageKey = "echelon",
            SmallImageText = "I Am SPEED!!"
        }
    };

    public static void MapVehicle(RichPresence presence, string vehicle, bool addedSmallImage,
        Func<string, string> formatter)
    {
        if (string.IsNullOrEmpty(vehicle))
        {
            presence.State = formatter("Unknown Vehicle");
            if (!addedSmallImage)
            {
                presence.Assets.SmallImageKey = "unknown";
                presence.Assets.SmallImageText = "What Is Bro Driving??";
            }

            return;
        }

        var data = Vahicles.FirstOrDefault(v => vehicle.Contains(v.Key));

        if (data.Key != null)
        {
            Apply(presence, data.Value, addedSmallImage, formatter);
            return;
        }

        var formattedName = char.ToUpper(vehicle[0]) + vehicle[1..];
        presence.State = formatter(formattedName);
        if (!addedSmallImage)
        {
            presence.Assets.SmallImageKey = "unknown";
            presence.Assets.SmallImageText = formattedName;
        }
    }

    private static void Apply(RichPresence presence, VehicleData data, bool addedSmallImage,
        Func<string, string> formatter)
    {
        presence.State = formatter(data.State);
        if (!addedSmallImage)
        {
            presence.Assets.SmallImageKey = data.SmallImageKey;
            presence.Assets.SmallImageText = data.SmallImageText;
        }
    }
}