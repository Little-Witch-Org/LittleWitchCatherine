using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace _Scripts.Service.Log
{
    public class QuestDebug : MonoBehaviour
    {
        public static QuestDebug Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool isEnabled = true;

        private string GetCallerInfo()
        {
            // Пропускаем 2 фрейма: этот метод и метод Log/LogWarning и т.д.
            var frame = new StackTrace(2, true).GetFrame(0);
            var method = frame.GetMethod();
            var className = method.ReflectedType.Name;
            return $"[QuestSystem] [{className}]";
        }

        public void Log(string message)
        {
            if (isEnabled)
            {
                Debug.Log($"{GetCallerInfo()} {message}");
            }
        }

        public void LogWarning(string message)
        {
            // warnings always on
            Debug.LogWarning($"{GetCallerInfo()} {message}");

        }

        public void LogError(string message)
        {
            // errors always on
            Debug.LogError($"{GetCallerInfo()} {message}");
        }

        public void LogFormat(string format, params object[] args)
        {
            if (isEnabled)
            {
                Debug.LogFormat($"{GetCallerInfo()} {format}", args);
            }
        }
    }
}