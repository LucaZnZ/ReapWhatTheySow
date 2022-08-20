using UnityEngine;
using UnityEngine.UI;

namespace ZnZUtil.UI
{
    public class FirstButtonSelect : MonoBehaviour
    {
        [SerializeField] private Selectable firstButton;

        private void OnEnable()
        {
            firstButton.Select();
        }
    }

}