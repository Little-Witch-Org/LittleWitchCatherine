using UnityEngine;

namespace _Scripts.Service.Log
{
    public class DialogDebug : MonoBehaviour
    {
        public static DialogDebug Instance;

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

        public bool isEnabled = true; // Включить/выключить логи

        public void Log(string message)
        {
            if (isEnabled)
            {
                Debug.Log($"[DialogueSystem] {message}");
            }
        }

        public void LogWarning(string message)
        {
            if (isEnabled)
            {
                Debug.LogWarning($"[DialogueSystem] {message}");
            }
        }

        public void LogError(string message)
        {
            // Ошибки можно оставить всегда включенными
            Debug.LogError($"[DialogueSystem] {message}");
        }
    }
}