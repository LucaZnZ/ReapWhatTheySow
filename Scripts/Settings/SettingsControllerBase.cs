using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace ZnZUtil.Settings
{
    public abstract class SettingsControllerBase : MonoBehaviour
    {
        [SerializeField] private SettingsWindow settingsWindow;

        private static readonly Dictionary<string, SettingPageHandle> pages = new();
        private static readonly Dictionary<string, SettingHandle> settings = new();

        // The Custom inspector will take care of displaying the events
        [SerializeField] [HideInInspector] private SerializableDictionary<string, UnityEvent<bool>> boolEvents = new();

        [SerializeField] [HideInInspector]
        private SerializableDictionary<string, UnityEvent<string>> stringEvents = new();

        [SerializeField] [HideInInspector]
        private SerializableDictionary<string, UnityEvent<float>> floatEvents = new();

        [SerializeField] [HideInInspector] private SerializableDictionary<string, UnityEvent<int>> intEvents = new();

#region AddingLogic

        /************************************************ BUILDING LOGIC **********************************************/

        private SettingPageHandle AddPageLogic(string name)
        {
            name = name.Trim();
            if (pages.ContainsKey(name))
                return pages[name];

            var page = new SettingPageHandle(name);
            pages.Add(name, page);
            return page;
        }

        private void AddSettingLogic(SettingPageHandle page, SettingHandle setting)
        {
            if (settings.ContainsKey(setting.name))
                return;

            settings.Add(setting.name, setting);
            page.AddSetting(setting);
            AddUnityeventLogic(setting);
        }

        private void AddUnityeventLogic<T>(T settingUI) where T : SettingHandle
        {
            switch (settingUI)
            {
                case SettingBoolHandle set:
                {
                    var e = new UnityEvent<bool>();
                    if (boolEvents.TryAdd(set.name, e))
                        set.onValueChanged += v => e.Invoke(v);
                    break;
                }
                case SettingStringHandle set:
                {
                    var e = new UnityEvent<string>();
                    if (stringEvents.TryAdd(set.name, e))
                        set.onValueChanged += v => e.Invoke(v);
                    break;
                }
                case SettingFloatHandle set:
                {
                    var e = new UnityEvent<float>();
                    if (floatEvents.TryAdd(set.name, e))
                        set.onValueChanged += v => e.Invoke(v);
                    break;
                }
                case SettingIntHandle set:
                {
                    var e = new UnityEvent<int>();
                    if (intEvents.TryAdd(set.name, e))
                        set.onValueChanged += v => e.Invoke(v);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(settingUI), settingUI, null);
            }
        }

#endregion

        /************************************************ NAVIGATION *************************************************/
        private void OnEnable() => settingsWindow.CreateAllUIElements(pages.Values.ToList());

        public void OpenPage(string name) => settingsWindow.OpenPage(name);

        public void OpenSettings() => settingsWindow.OpenSettings();

        public void CloseSettings() => settingsWindow.CloseSettings();

        /************************************************ INHERITANCE *************************************************/
        private void OnValidate()
        {
            Initialize();
        }


        /// <summary>
        /// Implement the settings here
        /// <seealso cref="AddPage"/>
        /// <seealso cref="AddSetting"/>
        /// </summary>
        protected abstract void Initialize();

        protected bool HasSetting(string name) => settings.ContainsKey(name);

        protected bool HasPage(string name) => pages.ContainsKey(name);

        /// <summary>
        /// Creates a new settings page and assigns the given settings to it
        /// </summary>
        /// <param name="name">Name of the settings page</param>
        /// <param name="settings">Settings to initally put on the page</param>
        /// <exception cref="ArgumentException">thrown if the page or a setting of that name already exists</exception>
        protected void AddPage(string name, List<SettingHandle> settings = null)
        {
            var page = AddPageLogic(name);
            settings?.ForEach(s => AddSettingLogic(page, s));
        }

        /// <summary>
        /// Creates a new UI Setting Object on the given page
        /// </summary>
        /// <param name="pageName">Name of the Setting page to add the setting on</param>
        /// <param name="setting">Setting to add to the page</param>
        /// <exception cref="ArgumentException"></exception>
        protected void AddSetting(string pageName, [NotNull] SettingHandle setting)
        {
            if (pages.TryGetValue(pageName, out var page))
                AddSettingLogic(page, setting);
            else
                throw new ArgumentException(
                    $"Settingpage {pageName} not found, could not add setting {setting.name} to it");
        }

        /// <summary>
        /// Receives the UI Settings Object of the given settting
        /// TODO return the value instead, so bool, string, int, float
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// thrown if the setting was not found, or the type doesn't match</exception>
        protected T GetSetting<T>(string name)
        {
            if (!settings.TryGetValue(name, out var setting))
                throw new ArgumentException($"Setting {name} not found");
            return ((SettingHandle<T>) setting).GetValue() ??
                   throw new ArgumentException($"Setting {name} is not of type {typeof(T)}");
        }


        /// <summary>
        /// Tries to set the given setting to the value
        /// </summary>
        /// <param name="name">Name of the setting</param>
        /// <param name="value">Desired value for the setting</param>
        /// <exception cref="ArgumentException">
        /// thrown if the settings doesn't exist,
        /// the valuetype doesn't match the one of the setting,
        /// or the valuetype is not supported</exception>
        protected void SetSetting<T>(string name, T value)
        {
            if (!settings.TryGetValue(name, out var setting))
                throw new ArgumentException($"Setting of name {name} not found.");
            try
            {
                switch (value)
                {
                    case bool val:
                        ((SettingBoolHandle) setting)?.SetValue(val);
                        break;
                    case string val:
                        ((SettingStringHandle) setting)?.SetValue(val);
                        break;
                    case float val:
                        ((SettingFloatHandle) setting)?.SetValue(val);
                        break;
                    case int val:
                        ((SettingIntHandle) setting)?.SetValue(val);
                        break;
                    default:
                        throw new ArgumentException($"Settings of type {typeof(T)} are not supported");
                }
            }
            catch (Exception)
            {
                throw new ArgumentException($"Setting {name} is not of type {typeof(T)}");
            }
        }
    }
}