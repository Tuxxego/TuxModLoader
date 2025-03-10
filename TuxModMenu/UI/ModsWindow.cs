using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System;
namespace TuxModLoader
{
    public class ModsWindow : MonoBehaviour
    {
        private bool isVisible = false;
        private List<Addon> addons = new List<Addon>();
        private Vector2 scrollPosition = Vector2.zero;
        private GUIStyle windowStyle;
        private GUIStyle headerStyle;
        private GUIStyle labelStyle;
        private GUIStyle buttonStyle;
        private GUIStyle boxStyle;
        private string addonsPath;
        private Texture2D gradientTexture;
        private void Start()
        {
            gradientTexture = CreateGradientTexture(Screen.width, 100, new Color(0.1f, 0.1f, 0.3f), new Color(0.3f, 0.3f, 0.5f));
        }
        void Awake()
        {
            addonsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Addons");
            LoadAddons();
        }
        void OnGUI()
        {
            if (!isVisible) return;
            if (windowStyle == null)
            {
                windowStyle = new GUIStyle(GUI.skin.window)
                {
                    fontSize = 22,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { background = gradientTexture, textColor = Color.white }
                };
            }
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 24,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white }
                };
            }
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 16,
                    normal = { textColor = Color.white },
                    wordWrap = true
                };
            }
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                };
            }
            if (boxStyle == null)
            {
                boxStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 14,
                    normal = { textColor = Color.white }
                };
            }
            Rect windowRect = new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400);
            GUI.Window(0, windowRect, DrawModsWindow, "", windowStyle);
        }
        void DrawModsWindow(int windowID)
        {
            if (!Directory.Exists(addonsPath))
            {
                Debug.LogWarning($"Addons directory does not exist at: {addonsPath}");
                return;
            }
            GUILayout.Label("Installed Addons", headerStyle);
            GUILayout.Space(10);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(580), GUILayout.Height(300));
            foreach (var addon in addons)
            {
                GUILayout.BeginVertical(boxStyle);
                GUILayout.BeginHorizontal();
                if (addon.Icon != null)
                {
                    GUILayout.Label(addon.Icon, GUILayout.Width(64), GUILayout.Height(64));
                }
                GUILayout.BeginVertical();
                GUILayout.Label(addon.Name, headerStyle);
                GUILayout.Label("By: " + addon.Author, labelStyle);
                GUILayout.Label(addon.Description, labelStyle, GUILayout.Width(400));
                GUILayout.EndVertical();
                bool isEnabled = PlayerPrefs.GetInt(addon.Name, 0) == 1;
                buttonStyle.normal.textColor = isEnabled ? Color.green : Color.red;
                string buttonText = isEnabled ? "Enabled" : "Disabled";
                if (GUILayout.Button(buttonText, buttonStyle, GUILayout.Width(100), GUILayout.Height(40)))
                {
                    isEnabled = !isEnabled;
                    PlayerPrefs.SetInt(addon.Name, isEnabled ? 1 : 0);
                    PlayerPrefs.Save();
                    Debug.Log($"Mod '{addon.Name}' toggled {(isEnabled ? "enabled" : "disabled")}");
                }
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                GUILayout.Space(5);
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Close", buttonStyle))
            {
                isVisible = false;
            }
        }
        public void ShowModsWindow()
        {
            isVisible = true;
        }
        private void LoadAddons()
        {
            addons.Clear();
            if (!Directory.Exists(addonsPath))
            {
                Debug.LogWarning($"Addons directory does not exist at: {addonsPath}");
                return;
            }
            Debug.Log($"Checking Addons directory: {addonsPath}");
            foreach (string directory in Directory.GetDirectories(addonsPath))
            {
                string infoPath = Path.Combine(directory, "info.json");
                if (!File.Exists(infoPath))
                {
                    Debug.LogWarning($"No info.json found in directory: {directory}");
                    continue;
                }
                try
                {
                    string json = File.ReadAllText(infoPath);
                    Addon addon = JsonConvert.DeserializeObject<Addon>(json);
                    if (addon != null)
                    {
                        string iconPath = Path.Combine(directory, "icon.png");
                        if (File.Exists(iconPath))
                        {
                            Texture2D icon = LoadTexture(iconPath);
                            addon.Icon = icon;
                            Debug.Log($"Icon found and loaded for addon: {addon.Name}");
                        }
                        else
                        {
                            Debug.LogWarning($"No icon.png found for addon: {addon.Name}");
                        }
                        addons.Add(addon);
                        Debug.Log($"Addon loaded: {addon.Name}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to deserialize info.json in: {directory}");
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error reading or deserializing info.json in {directory}: {ex.Message}");
                }
            }
        }
        private Texture2D CreateGradientTexture(int width, int height, Color startColor, Color endColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color lerpedColor = Color.Lerp(startColor, endColor, (float)y / height);
                    texture.SetPixel(x, y, lerpedColor);
                }
            }
            texture.Apply();
            return texture;
        }
        private Texture2D LoadTexture(string path)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return texture;
        }
    }
}
