using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using BepInEx;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace SubnauticaRP.Maps;

public static class HoverMap
{
    private const string CurrentVersion = "1.1.5";

    private static readonly string FolderPath = Path.Combine(Paths.ConfigPath, "SubnauticaRP");
    private static readonly Random Random = new();

    private static readonly JsonSerializerSettings Settings = new()
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore
    };

    private static HoverRoot _loadedRoot;
    private static string _currentLoadedFile;
    private static string _activeLanguage = "English";

    static HoverMap()
    {
        Initialize();
    }

    private static void Initialize()
    {
        try
        {
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);

            _activeLanguage = Language.main.GetCurrentLanguage() ?? "English";

            var fileName = _activeLanguage.Equals("English", StringComparison.OrdinalIgnoreCase)
                ? "hoverTexts.json"
                : $"hoverTexts_{_activeLanguage}.json";

            _currentLoadedFile = Path.Combine(FolderPath, fileName);

            if (!File.Exists(_currentLoadedFile))
            {
                Plugin.Logger.LogWarning($"{fileName} Not Found! Using Defaults...");
                _loadedRoot = GetDefaultRoot();
                Save();
                return;
            }

            var json = File.ReadAllText(_currentLoadedFile);
            _loadedRoot = JsonConvert.DeserializeObject<HoverRoot>(json, Settings);

            if (_loadedRoot is not null && _loadedRoot.Version != CurrentVersion)
            {
                Plugin.Logger.LogInfo(
                    $"Hover Text File Version Mismatch ({_loadedRoot.Version} -> {CurrentVersion}). Merging Updates...");

                // Only Merge If English
                if (_activeLanguage == "English") MergeDefaultData();

                _loadedRoot.Version = CurrentVersion;
                Save();
            }
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Failed To Load Hover Texts: {e.Message}, Using Default...");
            _loadedRoot = GetDefaultRoot();
        }
    }

    [SuppressMessage("ReSharper", "ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator")]
    private static void MergeDefaultData()
    {
        var oldDefaults = GetOldDefaultRoot();
        var newDefaults = GetDefaultRoot();

        foreach (var newProfile in newDefaults.Data)
        {
            var userProfile =
                _loadedRoot.Data.Find(p => p.Name.Equals(newProfile.Name, StringComparison.OrdinalIgnoreCase));
            var oldProfile =
                oldDefaults.Data.Find(p => p.Name.Equals(newProfile.Name, StringComparison.OrdinalIgnoreCase));

            if (userProfile == null)
            {
                // Add New Category As Whole
                _loadedRoot.Data.Add(newProfile);
                continue;
            }

            userProfile.LargeImageText = ProcessListMerge(
                userProfile.LargeImageText,
                oldProfile?.LargeImageText ?? new List<string>(),
                newProfile.LargeImageText ?? new List<string>()
            );

            userProfile.SmallImageText = ProcessListMerge(
                userProfile.SmallImageText,
                oldProfile?.SmallImageText ?? new List<string>(),
                newProfile.SmallImageText ?? new List<string>()
            );
        }
    }

    [CanBeNull]
    private static List<string> ProcessListMerge([CanBeNull] List<string> userList, List<string> oldDefaultList,
        List<string> newDefaultList)
    {
        // The User Didn't Want This One
        if (userList is null) return null;

        var userSet = new HashSet<string>(userList);
        var oldSet = new HashSet<string>(oldDefaultList);
        var newSet = new HashSet<string>(newDefaultList);

        var resultList = new List<string>();

        // Ok, Ok So For Each Entry In The User's File
        foreach (var text in userSet)
            // I Check If It's An Entry That I Have Already Done Or Added, If Yes Then Stay There
            if (newSet.Contains(text))
                resultList.Add(text);
            // I Check If It's A User Specific Entry Which Ain't In Old Stuff 
            else if (!oldSet.Contains(text)) resultList.Add(text);

        // If It's Missing In The New Stuff But Was In The Old Stuff Then It's Removed By Me So Who Cares

        // Inject Any New Stuff I Did
        foreach (var text in newSet)
            if (!oldSet.Contains(text) && !userSet.Contains(text))
                resultList.Add(text);

        return resultList;
    }

    private static void Save()
    {
        try
        {
            var json = JsonConvert.SerializeObject(_loadedRoot, Settings);
            File.WriteAllText(_currentLoadedFile, json);
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Failed To Write hoverTexts.json: {e.Message}");
        }
    }

    private static void CheckActiveLanguage()
    {
        var currentLang = Language.main.GetCurrentLanguage() ?? "English";

        if (!currentLang.Equals(_activeLanguage,
                StringComparison.OrdinalIgnoreCase)) Initialize(); // Hot Reload This Gun
    }

    [CanBeNull]
    public static string GetRandomL(string name)
    {
        CheckActiveLanguage();

        var profile = _loadedRoot?.Data?.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (profile?.LargeImageText == null || profile.LargeImageText.Count == 0) return null;

        return profile.LargeImageText[Random.Next(profile.LargeImageText.Count)];
    }

    [CanBeNull]
    public static string GetRandomS(string name)
    {
        CheckActiveLanguage();

        var profile = _loadedRoot?.Data?.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (profile?.SmallImageText == null || profile.SmallImageText.Count == 0) return null;

        return profile.SmallImageText[Random.Next(profile.SmallImageText.Count)];
    }

    // These Two Should Never Be Null Lel
    private static HoverRoot GetDefaultRoot()
    {
        return JsonConvert.DeserializeObject<HoverRoot>(HoverConsts.NewDefault, Settings)!;
    }

    private static HoverRoot GetOldDefaultRoot()
    {
        return JsonConvert.DeserializeObject<HoverRoot>(HoverConsts.OldDefault, Settings)!;
    }
}