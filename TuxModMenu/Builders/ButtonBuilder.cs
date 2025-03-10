using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace TuxModLoader.Builders
{
    public class ButtonBuilder
    {
        private string buttonID;
        private string localizedName;
        private string localizedDescription;
        private Sprite icon;
        private Vector2 position;
        private ButtonType type;
        private string tabName;
        private UnityAction onClickAction;
        private static Dictionary<string, PowerButton> customButtons = new Dictionary<string, PowerButton>();
        // ButtonType is from NCMS (made by Nikon)
        public enum ButtonType
        {
            Click,
            GodPower,
            Toggle,
        }
        public ButtonBuilder(string id)
        {
            buttonID = id;
        }
        public ButtonBuilder SetName(string name)
        {
            localizedName = name;
            return this;
        }
        public ButtonBuilder SetDescription(string description)
        {
            localizedDescription = description;
            return this;
        }
        public ButtonBuilder SetIcon(string path)
        {
            icon = ResourceLoader.LoadSprite(path);
            if (icon == null)
            {
                Debug.LogWarning($"Icon not found at path: {path}");
            }
            return this;
        }
        public ButtonBuilder SetPosition(Vector2 pos)
        {
            position = pos;
            return this;
        }
        public ButtonBuilder SetType(string buttonTypeString)
        {
            if (Enum.TryParse(buttonTypeString, true, out ButtonType parsedButtonType))
            {
                type = parsedButtonType;
            }
            else
            {
                Debug.LogError($"Invalid ButtonType string: {buttonTypeString}");
            }
            return this;
        }
        public ButtonBuilder SetTab(string tab)
        {
            tabName = tab;
            return this;
        }
        public ButtonBuilder SetOnClick(UnityAction action)
        {
            onClickAction = action;
            return this;
        }
        public PowerButton Build()
        {
            Localization.AddOrUpdate(buttonID, localizedName);
            Localization.AddOrUpdate($"{buttonID} Description", localizedDescription);
            GameObject tabObject = FindEvenInactive(tabName);
            if (tabObject == null)
            {
                Debug.LogError($"Tab '{tabName}' not found! Make sure it exists.");
                return null;
            }
            PowersTab parentTab = tabObject.GetComponent<PowersTab>();
            GameObject worldLawsTemplate = FindEvenInactive("WorldLaws");
            worldLawsTemplate.SetActive(false);
            GameObject buttonObject = UnityEngine.Object.Instantiate(worldLawsTemplate, parentTab.transform);
            buttonObject.transform.localPosition = position;
            buttonObject.transform.localScale = Vector3.one;
            buttonObject.name = buttonID;
            buttonObject.SetActive(true);
            Button buttonComponent = buttonObject.GetComponent<Button>();
            if (buttonComponent == null)
            {
                buttonComponent = buttonObject.AddComponent<Button>();
            }
            buttonComponent.onClick.AddListener(onClickAction);
            Image image = buttonObject.GetComponent<Image>();
            if (image == null)
            {
                image = buttonObject.AddComponent<Image>();
            }
            if (icon != null)
            {
                image.sprite = icon;
            }
            PowerButton powerButton = buttonObject.GetComponent<PowerButton>();
            if (powerButton == null)
            {
                powerButton = buttonObject.AddComponent<PowerButton>();
            }
            powerButton.type = (type == ButtonType.Click) ? PowerButtonType.Library : PowerButtonType.Active;
            customButtons[buttonID] = powerButton;
            return powerButton;
        }
        // FindEvenInactive is from NCMS (made by Nikon)
        public static GameObject FindEvenInactive(string name)
        {
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == name)
                {
                    return obj;
                }
            }
            return null;
        }
    }
}
