using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

//todo not good to use more then one singletons on object -> do refactor (using interface or logger manager (check seek history))
//todo add dynamic script name in beginning of string log - like "Manager:...."
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

        public bool isEnabled = true;

        private string GetCallerInfo()
        {
            // Пропускаем 2 фрейма: этот метод и метод Log/LogWarning и т.д.
            var frame = new StackTrace(2, true).GetFrame(0);
            var method = frame.GetMethod();
            var className = method.ReflectedType.Name;
            return $"[DialogueSystem] [{className}]";
        }

        public void Log(string message)
        {
            // warnings always on
            Debug.Log($"{GetCallerInfo()} {message}");

        }

        public void LogWarning(string message)
        {
            if (isEnabled)
            {
                Debug.LogWarning($"{GetCallerInfo()} {message}");
            }
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