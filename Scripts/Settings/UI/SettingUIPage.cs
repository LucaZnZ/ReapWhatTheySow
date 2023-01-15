using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace ZnZUtil.Settings
{
    public class SettingUIPage : MonoBehaviour
    {
        [SerializeField] [CanBeNull] private TMP_Text label;
        public GameObject fullPage;
        public Transform body;

        public void SetLabel(string label)
        {
            name = label;
            if (this.label != null)
                this.label.text = label;
        }

        public void Open() => fullPage.SetActive(true);
        public void Close() => fullPage.SetActive(false);
    }
}