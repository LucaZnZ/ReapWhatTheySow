using System;
using UnityEngine;

namespace ZnZUtil
{
    public static class Logger
    {
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        public static void LogException(object message, Exception exception)
        {
            Debug.LogWarning(message + "Exception: \n" + exception.Message);
        }

        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
    }
}