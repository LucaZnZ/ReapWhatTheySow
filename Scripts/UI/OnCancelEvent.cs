using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ZnZUtil.UI
{
    [RequireComponent(typeof(UnityEngine.UI.Selectable))]
    public class OnCancelEvent : MonoBehaviour, ICancelHandler
    {
        public UnityEvent OnCancel;

        void ICancelHandler.OnCancel(BaseEventData eventData)
        {
            // Debug.Log("cancel on " + name);
            Debug.Log(eventData.used);
            OnCancel?.Invoke();
        }
    }

}