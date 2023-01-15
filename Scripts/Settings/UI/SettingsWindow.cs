using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZnZUtil.Settings
{
    public class SettingsWindow : MonoBehaviour
    {
        [SerializeField] private Transform full, window, menuHolder;
        [Header("Prefabs")] [SerializeField] private SettingBoolUI togglePrefab;
        [SerializeField] private SettingStringUI stringPrefab, textPrefab;
        [SerializeField] private SettingFloatUI floatSliderPrefab;
        [SerializeField] private SettingIntUI intSliderPrefab;
        [Space] [SerializeField] private SettingUIPage pagePrefab;
        [SerializeField] private SettingUIButton buttonPrefab;

        private readonly Dictionary<string, SettingUIPage> pagesUI = new();


#region CreatingUI

        /************************************************ BUILDING UI *************************************************/
        public void CreateAllUIElements(List<SettingPageHandle> pages)
        {
            foreach (var page in pages)
            {
                // create the page
                var uiPage = CreateUIPage(page.name);
                // create all settings on the page
                page.settings.ForEach(s => CreateUISetting(s, uiPage.body));
                // create menu button and link it to the page
                GetOrCreateUIMenuButton(page.name)
                    .AddOnClick(() => OpenPage(uiPage));
            }
        }

        private SettingUIPage CreateUIPage(string name)
        {
            var page = Instantiate(pagePrefab, window).GetComponent<SettingUIPage>();
            page.SetLabel(name);
            pagesUI.Add(name, page);
            return page;
        }

        private void CreateUISetting(SettingHandle settingHandle, Transform parent)
        {
            var setting = Instantiate(GetSettingPrefab(settingHandle), parent).GetComponent<SettingUI>();
            setting.Initialize(settingHandle);
        }

        private GameObject GetSettingPrefab<T>(T handle) where T : SettingHandle
        {
            try
            {
                return handle switch
                {
                    SettingBoolHandle => togglePrefab.gameObject,
                    SettingTextHandle => textPrefab.gameObject,
                    SettingStringHandle => stringPrefab.gameObject,
                    SettingFloatHandle => floatSliderPrefab.gameObject,
                    SettingIntHandle => intSliderPrefab.gameObject,
                    _ => throw new ArgumentException($"Unknown Setting Type {handle}")
                };
            }
            catch (Exception)
            {
                Debug.LogError($"No Prefab to Instantiate for Setting of type {handle}");
                // TODO use defaults?
                throw;
            }
        }

        private SettingUIButton GetOrCreateUIMenuButton(string label)
        {
            foreach (Transform button in menuHolder.transform)
                if (button.name == label)
                    return button.GetComponent<SettingUIButton>();
            return CreateUIMenuButton(label);
        }

        private SettingUIButton CreateUIMenuButton(string label)
        {
            var button = Instantiate(buttonPrefab, menuHolder).GetComponent<SettingUIButton>();
            button.SetLabel(label);
            return button;
        }

#endregion

        /************************************************ NAVIGATION **************************************************/
        /// <summary>
        /// Opens the settings page of the given name, if it exists
        /// </summary>
        /// <param name="name">Pagename</param>
        public void OpenPage(string name) => OpenPage(GetPage(name));

        private SettingUIPage GetPage(string name)
        {
            if (!pagesUI.TryGetValue(name, out var page))
                throw new ArgumentException($"Settingpage {name} not found");
            return page;
        }

        private void OpenPage(SettingUIPage page)
        {
            OpenSettings();
            foreach (var p in pagesUI.Values) p.Close();
            page.Open();
        }

        public void OpenSettings() => full.gameObject.SetActive(true);

        public void CloseSettings() => full.gameObject.SetActive(false);
    }
}