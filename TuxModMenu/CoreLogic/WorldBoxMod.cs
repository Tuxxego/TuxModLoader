using UnityEngine;

namespace TuxModLoader
{
    public class WorldBoxMod : MonoBehaviour
    {
        private ModSetup modSetup;
        private StartupPanel startupPanel;

        void Start()
        {
            startupPanel = FindObjectOfType<StartupPanel>();

            if (startupPanel == null)
            {
                Debug.Log("StartupPanel not found. Creating a new one.");
                GameObject panelObject = new GameObject("StartupPanel");
                startupPanel = panelObject.AddComponent<StartupPanel>();
            }

            modSetup = new ModSetup(startupPanel);

            modSetup.LoadMods();
        }
    }
}
