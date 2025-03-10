using System.IO;
using UnityEngine;
using System;
namespace TuxModLoader
{
    public static class ResourceLoader
    {
        private static string addonsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons");
        public static Sprite LoadSprite(string path)
        {
            string[] addonDirectories = Directory.GetDirectories(addonsDirectory);
            foreach (string addonDirectory in addonDirectories)
            {
                string addonResourcesPath = Path.Combine(addonDirectory, "AddonResources");
                if (Directory.Exists(addonResourcesPath))
                {
                    string spritePath = Path.Combine(addonResourcesPath, path);
                    if (File.Exists(spritePath))
                    {
                        if (spritePath.EndsWith(".png") || spritePath.EndsWith(".jpg"))
                        {
                            return LoadSpriteFromFile(spritePath);
                        }
                    }
                }
            }
            Debug.LogWarning($"Sprite not found at {path} in any AddonResources folder.");
            return null;
        }
        private static Sprite LoadSpriteFromFile(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
