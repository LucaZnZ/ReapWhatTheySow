using UnityEngine;
using UnityEngine.SceneManagement;

namespace ZnZUtil
{
    public abstract class SingletonBase<T> : MonoBehaviour where T : class
    {
        protected enum SceneMode
        {
            Unload,
            DontDestroyOnLoad
        }

        protected static T instance;
        public static T getInstance => instance;

        protected virtual SceneMode sceneMode => SceneMode.Unload; // QUEST make it abstract ??

        public virtual void Awake()
        {
            if (instance == null)
                instance = this as T;
            else
            {
                Debug.LogWarning("Destroyed Singleton Object: " + name);
                Destroy(this);
            }

            if (sceneMode == SceneMode.Unload) SceneManager.sceneUnloaded += (_) => { instance = null; };
            else if (sceneMode == SceneMode.DontDestroyOnLoad) DontDestroyOnLoad(this);
        }
    }
}