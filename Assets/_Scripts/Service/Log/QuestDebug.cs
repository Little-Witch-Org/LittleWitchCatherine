using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Service.Log
{
    public class QuestDebug:MonoBehaviour
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

        public  bool isEnabled = true; // Включить/выключить логи
    
        public  void Log(string message)
        {
            if (isEnabled)
            {
                Debug.Log($"[QuestSystem] {message}");
            }
        }
    
        public  void LogWarning(string message)
        {
            if (isEnabled)
            {
                Debug.LogWarning($"[QuestSystem] {message}");
            }
        }
    
        public  void LogError(string message)
        {
            // Ошибки можно оставить всегда включенными
            Debug.LogError($"[QuestSystem] {message}");
        }
    }
}