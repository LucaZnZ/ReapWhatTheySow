using System;
using TMPro;
using UnityEngine;

namespace ZnZUtil.Settings
{
    public abstract class SettingUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;

        public void SetLabel(string label)
        {
            this.label.text = label;
            name = this.label.text;
        }

        public string GetLabel() => label.text;

        public void Initialize(SettingHandle setting)
        {
            SetLabel(setting.name);
            Init(setting);
        }

        protected abstract void Init(SettingHandle setting);
    }

    public abstract class SettingUI<T> : SettingUI
    {
        private SettingHandle<T> setting;
        public virtual void SetValue(T value) => setting.SetValue(value);
        public T GetValue() => setting.GetValue();
        
        public abstract T ReadValueFromUI();

        protected override void Init(SettingHandle setting)
        {
            var set = (SettingHandle<T>) setting;
            this.setting = set;
            Init(set);
        }

        protected abstract void Init(SettingHandle<T> setting);
    }
}