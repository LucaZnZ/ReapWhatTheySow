using UnityEngine;
using UnityEngine.UI;

namespace ZnZUtil.Settings
{
    public class SettingBoolUI : SettingUI<bool>
    {
        [SerializeField] private Toggle toggle;
        public override bool ReadValueFromUI() => toggle.isOn;

        public override void SetValue(bool value)
        {
            base.SetValue(value);
            toggle.isOn = value;
        }

        protected override void Init(SettingHandle<bool> handle)
        {
            toggle.onValueChanged.AddListener(SetValue);
        }
    }
}