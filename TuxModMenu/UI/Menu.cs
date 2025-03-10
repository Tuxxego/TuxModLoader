using UnityEngine;
using System.Collections.Generic;
using System;

namespace TuxModLoader
{
    public class Menu : MonoBehaviour
    {
        private bool isVisible = false;
        private float slideSpeed = 8f;
        private float targetY = -100f;
        private float currentY = -100f;

        private GUIStyle headerStyle;
        private GUIStyle subStyle;
        private GUIStyle buttonStyle;
        private GUIStyle panelStyle;

        private Texture2D gradientTexture;

   //     private SettingsWindow ballsPanel2;

        private void Start()
        {
            gradientTexture = CreateGradientTexture(Screen.width, 100, new Color(0.1f, 0.1f, 0.3f), new Color(0.3f, 0.3f, 0.5f));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                isVisible = !isVisible;
                targetY = isVisible ? 0f : -100f;
            }

            currentY = Mathf.Lerp(currentY, targetY, Time.deltaTime * slideSpeed);
        }

        private void OnGUI()
        {
            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 28,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white },
                    alignment = TextAnchor.MiddleCenter
                };
            }
            if (subStyle == null)
            {
                subStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 10,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white },
                    alignment = TextAnchor.MiddleCenter
                };
            }

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 18,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    fixedWidth = 120,
                    fixedHeight = 40
                };

                buttonStyle.normal.textColor = Color.cyan;
                buttonStyle.hover.textColor = Color.white;

                buttonStyle.normal.background = MakeTex(2, 2, new Color(0f, 0.5f, 0.5f, 0.6f));
                buttonStyle.hover.background = MakeTex(2, 2, new Color(0.2f, 0.7f, 0.8f, 0.8f));
            }

        

            if (panelStyle == null)
            {
                panelStyle = new GUIStyle(GUI.skin.box)
                {
                    normal = { background = gradientTexture },
                    padding = new RectOffset(15, 15, 15, 15)
                };
            }

            GUI.Box(new Rect(0, currentY, Screen.width, 100), "", panelStyle);

            GUI.Label(new Rect(Screen.width / 2 - 250, currentY + 20, 500, 50), "TuxModLoader", headerStyle);
            GUI.Label(new Rect(Screen.width / 2 - 250, currentY + 55, 500, 50), "A simple and easy modloader for WorldBox.", subStyle);

            float buttonSpacing = Screen.width / 5;
            float buttonY = currentY + 60;

            if (GUI.Button(new Rect(buttonSpacing * 1 - 60, buttonY, 120, 40), "MODS", buttonStyle))
            {

                ModsWindow modsWindow = FindObjectOfType<ModsWindow>();

                if (modsWindow == null)
                {
                    Debug.Log("Creating a new ModsWindow instance...");

                    GameObject modsObject = new GameObject("ModsWindow");
                    modsWindow = modsObject.AddComponent<ModsWindow>();
                }

                modsWindow.ShowModsWindow();
            }

            if (GUI.Button(new Rect(buttonSpacing * 2 - 60, buttonY, 120, 40), "SETTINGS", buttonStyle))
            {
          //      GameObject panelObject = new GameObject("ballsPanel2");
           //     ballsPanel2 = panelObject.AddComponent<SettingsWindow>();
            //    ballsPanel2.ShowSettingsWindow();
            }

            if (GUI.Button(new Rect(buttonSpacing * 3 - 60, buttonY, 120, 40), "DISCORD", buttonStyle))
            {
                Debug.Log("Discord button clicked!");
            }

            if (GUI.Button(new Rect(buttonSpacing * 4 - 60, buttonY, 120, 40), "UPDATE", buttonStyle))
            {
                Debug.Log("Update button clicked!");
            }
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private Texture2D CreateGradientTexture(int width, int height, Color startColor, Color endColor)
        {
            Texture2D texture = new Texture2D(width, height);
            for (int y = 0; y < height; y++)
            {
                Color color = Color.Lerp(startColor, endColor, (float)y / height);
                for (int x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }
            texture.Apply();
            return texture;
        }
    }
}
