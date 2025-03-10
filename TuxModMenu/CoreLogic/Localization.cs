using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
// A large portion of this file is derived from NCMS (made by Nikon)
namespace TuxModLoader
{
    public static class Localization
    {
        private static Dictionary<string, string> GetLocalizedText()
        {
            FieldInfo field = typeof(LocalizedTextManager).GetField("localizedText", BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null || LocalizedTextManager.instance == null)
            {
                Debug.LogError("Failed to access LocalizedTextManager.localizedText");
                return null;
            }
            return field.GetValue(LocalizedTextManager.instance) as Dictionary<string, string>;
        }
        public static void AddOrUpdate(string key, string value)
        {
            Dictionary<string, string> localizedText = GetLocalizedText();
            if (localizedText == null) return;
            if (localizedText.ContainsKey(key))
                localizedText[key] = value;
            else
                localizedText.Add(key, value);
        }
    }
}
