using System;
using UnityEngine;
using UnityEngine.UI;
// A portion of this file is derived from NCMS (made by Nikon)
namespace TuxModLoader.Builders
{
    public class TabBuilder
    {
        private string buttonID;
        private string tabID;
        private string name;
        private string description;
        private int xPos;
        private Sprite icon;
        public TabBuilder SetButtonID(string id)
        {
            buttonID = id;
            return this;
        }
        public TabBuilder SetTabID(string id)
        {
            tabID = id;
            return this;
        }
        public TabBuilder SetName(string tabName)
        {
            name = tabName;
            return this;
        }
        public TabBuilder SetDescription(string tabDescription)
        {
            description = tabDescription;
            return this;
        }
        public TabBuilder SetPosition(int positionX)
        {
            xPos = positionX;
            return this;
        }
        public TabBuilder SetIcon(string resourcePath)
        {
            icon = ResourceLoader.LoadSprite(resourcePath);
            return this;
        }
        public void Build()
        {
            GameObject otherTabButton = FindEvenInactive("Button_Other");
            if (otherTabButton == null)
            {
                Debug.LogError("Error: Could not find 'Button_Other' to clone for tab creation.");
                return;
            }
            Localization.AddOrUpdate(buttonID, name);
            Localization.AddOrUpdate($"{buttonID} Description", description);
            Localization.AddOrUpdate("Tuxxego_mod_creator", "Made By Tuxxego");
            Localization.AddOrUpdate(tabID, name);
            GameObject newTabButton = GameObject.Instantiate(otherTabButton);
            newTabButton.transform.SetParent(otherTabButton.transform.parent);
            newTabButton.name = buttonID;
            Button buttonComponent = newTabButton.GetComponent<Button>();
            TipButton tipButton = newTabButton.GetComponent<TipButton>();
            tipButton.textOnClick = buttonID;
            tipButton.textOnClickDescription = $"{buttonID} Description";
            tipButton.text_description_2 = "Tuxxego_mod_creator";
            newTabButton.transform.localPosition = new Vector3(xPos, 49.57f);
            newTabButton.transform.localScale = Vector3.one;
            if (icon != null)
            {
                newTabButton.transform.Find("Icon").GetComponent<Image>().sprite = icon;
            }
            GameObject otherTab = FindEvenInactive("Tab_Other");
            if (otherTab == null)
            {
                Debug.LogError("Error: Could not find 'Tab_Other' to clone for tab creation.");
                return;
            }
            foreach (Transform child in otherTab.transform)
            {
                child.gameObject.SetActive(false);
            }
            GameObject newTab = GameObject.Instantiate(otherTab);
            foreach (Transform child in newTab.transform)
            {
                if (child.gameObject.name == "tabBackButton" || child.gameObject.name == "-space")
                {
                    child.gameObject.SetActive(true);
                    continue;
                }
                GameObject.Destroy(child.gameObject);
            }
            foreach (Transform child in otherTab.transform)
            {
                child.gameObject.SetActive(true);
            }
            newTab.transform.SetParent(otherTab.transform.parent);
            newTab.name = tabID;
            PowersTab powersTabComponent = newTab.GetComponent<PowersTab>();
            powersTabComponent.powerButton = buttonComponent;
            powersTabComponent.powerButtons.Clear();
            powersTabComponent.powerButton.onClick = new Button.ButtonClickedEvent();
            powersTabComponent.powerButton.onClick.AddListener(() => ShowTab(tabID));
            newTab.SetActive(true);
            powersTabComponent.powerButton.gameObject.SetActive(true);
        }
        private static void ShowTab(string tabID)
        {
            GameObject additionalTab = FindEvenInactive(tabID);
            if (additionalTab != null)
            {
                PowersTab powersTabComponent = additionalTab.GetComponent<PowersTab>();
                powersTabComponent.showTab(powersTabComponent.powerButton);
            }
        }
        // FindEvenInactive is from NCMS (made by Nikon)
        public static GameObject FindEvenInactive(string Name)
        {
            GameObject[] objectsOfTypeAll = Resources.FindObjectsOfTypeAll<GameObject>();
            for (int index = 0; index < objectsOfTypeAll.Length; ++index)
            {
                if (objectsOfTypeAll[index].gameObject.gameObject.name == Name)
                    return objectsOfTypeAll[index];
            }
            return (GameObject)null;
        }
    }
}
