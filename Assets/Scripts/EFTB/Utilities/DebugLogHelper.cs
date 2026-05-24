using System;
using UnityEngine;

namespace Assets.Scripts.EFTB.Utilities
{
    public class DebugLogHelper
    {
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
        }
    }
}
