using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TuxModLoader
{
    public class SpriteLoader
    {
        private Dictionary<string, Sprite> loadedSprites = new Dictionary<string, Sprite>();

        public void LoadSprites(string modDirectory)
        {
            string resourcePath = Path.Combine(modDirectory, "AddonResources");

            if (!Directory.Exists(resourcePath))
            {
                Debug.LogWarning($"No AddonResources folder found in mod: {modDirectory}");
                return;
            }

            string[] imageFiles = Directory.GetFiles(resourcePath, "*.*", SearchOption.AllDirectories);
            foreach (var file in imageFiles)
            {
                if (file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    LoadSprite(file);
                }
            }
        }

        private void LoadSprite(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);

            if (texture.LoadImage(fileData))
            {
                texture.filterMode = FilterMode.Point;
                string spriteName = Path.GetFileNameWithoutExtension(filePath);
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );

                loadedSprites[spriteName] = sprite;
                Debug.Log($"Loaded sprite: {spriteName} from {filePath}");
            }
            else
            {
                Debug.LogError($"Failed to load image: {filePath}");
            }
        }

        public Sprite GetSprite(string name)
        {
            return loadedSprites.TryGetValue(name, out Sprite sprite) ? sprite : null;
        }
    }
}