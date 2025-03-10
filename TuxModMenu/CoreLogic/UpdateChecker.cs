using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using SimpleJSON;
using System;

namespace TuxModManager
{
    private class UpdateChecker
{
    private readonly ModMenu modMenu;

    public string LatestVersion { get; private set; }

    public UpdateChecker(ModMenu modMenu)
    {
        this.modMenu = modMenu;
        StartCoroutine(CheckForUpdates());
    }

    private IEnumerator CheckForUpdates()
    {
        UnityWebRequest www = UnityWebRequest.Get(modMenu.modEntriesUrl);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching mod entries for update check: " + www.error);
            yield break;
        }

        string jsonData = www.downloadHandler.text;

        if (!string.IsNullOrEmpty(jsonData))
        {
            try
            {
                JSONNode webModEntriesNode = JSON.Parse(jsonData);

                if (webModEntriesNode != null)
                {
                    JSONArray modEntriesArray = webModEntriesNode["modEntries"].AsArray;

                    if (modEntriesArray != null)
                    {
                        foreach (JSONNode entryNode in modEntriesArray)
                        {
                            if (!entryNode.IsString && entryNode["name"] == modMenu.selectedMod.name)
                            {
                                LatestVersion = entryNode["version"];
                                modMenu.updateAvailableMods = !modMenu.selectedMod.version.Equals(LatestVersion);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception while parsing JSON for update check: " + ex.Message);
            }
        }
    }
}
    }

