using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZnZUtil.Settings
{
    public class SettingIntUI : SettingUI<int>
    {
        [SerializeField] private Slider slider;
        [SerializeField] [CanBeNull] private TMP_Text minText, maxText;
        public override int ReadValueFromUI() => (int) slider.value;

        public override void SetValue(int value)
        {
            base.SetValue(value);
            slider.value = value;
        }

        protected override void Init(SettingHandle<int> handle)
        {
            var h = (SettingIntHandle) handle;
            slider.wholeNumbers = true;
            slider.onValueChanged.AddListener(v => SetValue((int) v));
            SetBoundaries(h.min, h.max);
        }

        private void SetBoundaries(int min, int max)
        {
            slider.minValue = min;
            slider.maxValue = max;

            if (minText != null)
                minText.text = min.ToString();
            if (maxText != null)
                maxText.text = max.ToString();
        }
    }
}