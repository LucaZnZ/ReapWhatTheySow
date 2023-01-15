using System.Globalization;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZnZUtil.Settings
{
    public class SettingFloatUI : SettingUI<float>
    {
        [SerializeField] private Slider slider;
        [SerializeField] [CanBeNull] private TMP_Text minText, maxText;
        public override float ReadValueFromUI() => slider.value;

        public override void SetValue(float value)
        {
            base.SetValue(value);
            slider.value = value;
        }

        protected override void Init(SettingHandle<float> handle)
        {
            var h = (SettingFloatHandle) handle;
            slider.onValueChanged.AddListener(SetValue);
            SetBoundaries(h.min, h.max);
        }

        private void SetBoundaries(float min, float max)
        {
            slider.minValue = min;
            slider.maxValue = max;

            if (minText != null)
                minText.text = min.ToString(CultureInfo.InvariantCulture);
            if (maxText != null)
                maxText.text = max.ToString(CultureInfo.InvariantCulture);
        }
    }
}