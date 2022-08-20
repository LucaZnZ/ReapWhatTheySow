using System;

namespace ZnZUtil
{
    public abstract class Singleton<T> where T : class
    {
        // protected static T instance;
        private static readonly Lazy<T> instance = new Lazy<T>();
        public static T GetInstance() => instance.Value;
    }
}