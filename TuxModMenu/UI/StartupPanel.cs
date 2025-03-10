﻿using UnityEngine;

namespace TuxModLoader
{
    public class StartupPanel : MonoBehaviour
    {
        private int successfulMods;
        private int failedMods;
        private string tuxModLoaderVersion = "1.0.0";

        private GUIStyle headerStyle;
        private GUIStyle countStyle;
        private GUIStyle panelStyle;
        private GUIStyle buttonStyle;

        private float slideSpeed = 5f;
        private float targetY = 50f;
        private float currentY = -250f;
        private bool isPanelVisible = false;

        private Texture2D gradientTexture;

        void OnGUI()
        {

            if (headerStyle == null)
            {
                headerStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 24,
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white }
                };
            }

            if (countStyle == null)
            {
                countStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 18,
                    normal = { textColor = Color.green }
                };
            }

            if (panelStyle == null)
            {
                panelStyle = new GUIStyle(GUI.skin.box)
                {
                    border = new RectOffset(10, 10, 10, 10),
                    padding = new RectOffset(15, 15, 15, 15)
                };
            }

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { background = MakeTex(1, 1, new Color(0.2f, 0.6f, 0.2f)) },
                    hover = { background = MakeTex(1, 1, new Color(0.3f, 0.7f, 0.3f)) },
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                    border = new RectOffset(4, 4, 4, 4)
                };
            }

            if (isPanelVisible)
            {
                currentY = Mathf.Lerp(currentY, targetY, Time.deltaTime * slideSpeed);
                if (Mathf.Abs(currentY - targetY) < 1f)
                {
                    currentY = targetY;
                }
            }

            if (gradientTexture == null)
            {
                gradientTexture = CreateGradientTexture(700, 500, new Color(0.1f, 0.1f, 0.3f), new Color(0.3f, 0.3f, 0.5f));
            }

            if (isPanelVisible)
            {

                GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, currentY, 400, 250), panelStyle);

                GUILayout.Label("Welcome to TuxModLoader", headerStyle);

                GUILayout.Space(20);
                GUILayout.Label($"Mods Loaded: {successfulMods}", countStyle);
                GUILayout.Label($"Failed Mods: {failedMods}", countStyle);
                GUILayout.Space(10);
                GUILayout.Label($"TuxModLoader Version: {tuxModLoaderVersion}", countStyle);

                GUILayout.Space(20);
                GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));

                GUILayout.Space(20);
                if (GUILayout.Button("Close Panel", buttonStyle))
                {
                    HideStartupPanel();
                }

                GUI.backgroundColor = new Color(0.05f, 0.05f, 0.1f);
                GUI.skin.window.normal.background = gradientTexture;

                GUILayout.EndArea();
            }
        }

        public void ShowStartupPanel(int successful, int failed)
        {
            successfulMods = successful;
            failedMods = failed;

            isPanelVisible = true;

            Debug.Log("ShowStartupPanel called.");
        }

        public void HideStartupPanel()
        {
            isPanelVisible = false;
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++) pix[i] = col;
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
                for (int x = 0; x < width; x++)
                {
                    Color lerpedColor = Color.Lerp(startColor, endColor, (float)y / height);
                    texture.SetPixel(x, y, lerpedColor);
                }
            }
            texture.Apply();
            return texture;
        }
    }
}