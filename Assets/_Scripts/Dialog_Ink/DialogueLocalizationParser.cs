#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;


namespace _Scripts.Dialog_Ink
{
    
    /// <summary>
    /// Parses tables and ink folders.
    /// Generates localized dialogue files
    /// </summary>
    public class DialogueLocalizationParser : MonoBehaviour
    {
        [Tooltip("Папки с Ink-файлами. Пример: Assets/Resources/IncDialogue/Mother")]
        public List<string> dialogueFolderPaths = new();

        [Tooltip("Соответствующие им таблицы локализации")]
        public List<string> localizationTableNames = new();

        [ContextMenu("Generate All Localized Ink Files")]
        public void GenerateAllLocalizedInkFiles()
        {
            if (dialogueFolderPaths.Count != localizationTableNames.Count)
            {
                Debug.LogError("Количество папок и таблиц должно совпадать.");
                return;
            }

            for (int i = 0; i < dialogueFolderPaths.Count; i++)
            {
                string folderPath = dialogueFolderPaths[i];
                string tableName = localizationTableNames[i];

                var english = LoadTableSync(tableName, "en");
                var russian = LoadTableSync(tableName, "ru");
                var tags = LoadTableSync(tableName, "tags");

                InkFileGenerator.GenerateLocalizedInkFiles(folderPath, english, russian, tags);
            }

            AssetDatabase.Refresh();
        }

        private Dictionary<string, string> LoadTableSync(string tableName, string localeCode)
        {
            var dict = new Dictionary<string, string>();

            Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
            if (locale == null)
            {
                Debug.LogError($"Locale '{localeCode}' not found.");
                return dict;
            }

            var tableOp = LocalizationSettings.StringDatabase.GetTableAsync(tableName, locale);
            tableOp.WaitForCompletion();

            if (!tableOp.IsValid() || tableOp.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load table '{tableName}' for locale '{localeCode}'");
                return dict;
            }

            var table = tableOp.Result;
            foreach (var entry in table.Values)
            {
                if (!string.IsNullOrEmpty(entry.LocalizedValue))
                    dict[entry.Key] = entry.LocalizedValue;
            }

            return dict;
        }
    }
}
#endif
