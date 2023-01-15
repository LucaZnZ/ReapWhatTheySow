using TMPro;
using UnityEngine;

namespace ZnZUtil.Settings
{
    public class SettingStringUI : SettingUI<string>
    {
        [SerializeField] private TMP_InputField text;
        public override string ReadValueFromUI() => text.text;

        public override void SetValue(string value)
        {
            base.SetValue(value);
            text.text = value;
        }

        protected override void Init(SettingHandle<string> handle)
        {
            var h = (SettingStringHandle) handle;
            text.onValueChanged.AddListener(SetValue);
            text.characterLimit = h.maxLength;
        }
    }
}