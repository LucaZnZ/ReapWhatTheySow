using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ZnZUtil.Settings
{
    public class SettingUIButton : MonoBehaviour
    {
        [SerializeField] [CanBeNull] private TMP_Text label;
        [SerializeField] private Button button;

        public void SetLabel(string label)
        {
            name = label;
            if (this.label != null)
                this.label.text = label;
        }

        public void AddOnClick(UnityAction action) => button.onClick.AddListener(action);
    }
}