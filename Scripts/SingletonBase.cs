using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour where T : class
{
    protected static T instance;
    public static T getInstance => instance;
    public virtual void Awake()
    {
        if (instance == null)
            instance = this as T;
        else
        {
            Debug.LogWarning("Destroyed Singleton Object: " + name);
            Destroy(this);
        }
    }
}
