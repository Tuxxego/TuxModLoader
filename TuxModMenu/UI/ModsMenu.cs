using UnityEngine;
using System.Collections.Generic;

namespace TuxModLoader
{
    public class ModsMenu : MonoBehaviour
    {
        private bool isVisible = false;
        private List<Addon> modsList = new List<Addon>();
        private GUIStyle buttonStyle;

        void OnGUI()
        {
            if (!isVisible) return;

            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    fontSize = 16,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                };
            }

            GUILayout.BeginArea(new Rect(Screen.width / 2 - 200, 50, 400, 300), "Mods Menu", GUI.skin.window);
            GUILayout.Space(10);

            foreach (var mod in modsList)
            {
                GUILayout.Label(mod.AddonName);

                if (GUILayout.Button("Details", buttonStyle))
                {
                    ShowModDetails(mod);
                }

                bool isEnabled = PlayerPrefs.GetInt(mod.AddonName, 0) == 1;
                if (GUILayout.Toggle(isEnabled, "Enabled"))
                {
                    PlayerPrefs.SetInt(mod.AddonName, 1);
                }
                else
                {
                    PlayerPrefs.SetInt(mod.AddonName, 0);
                }

                GUILayout.Space(10);
            }

            if (GUILayout.Button("Close", buttonStyle))
            {
                HideModsMenu();
            }

            GUILayout.EndArea();
        }

        public void ShowModsMenu(List<Addon> mods)
        {
            modsList = mods;
            isVisible = true;
        }

        public void HideModsMenu()
        {
            isVisible = false;
        }

        private void ShowModDetails(Addon mod)
        {

            Debug.Log($"Showing details for mod: {mod.AddonName}");
        }
    }
}