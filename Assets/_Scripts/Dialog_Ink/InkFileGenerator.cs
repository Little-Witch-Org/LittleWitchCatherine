#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _Scripts.Dialog_Ink
{
    public static class InkFileGenerator
    {
        public static void GenerateLocalizedInkFiles(string folderPath, Dictionary<string, string> enDict, Dictionary<string, string> ruDict, Dictionary<string, string> tagsDict)
        {
            if (!Directory.Exists(folderPath))
            {
                Debug.LogError($"Folder not found: {folderPath}");
                return;
            }

            string[] inkFiles = Directory.GetFiles(folderPath, "*.ink", SearchOption.TopDirectoryOnly);
            string ruDir = Path.Combine(folderPath, "ru");
            string enDir = Path.Combine(folderPath, "en");

            Directory.CreateDirectory(ruDir);
            Directory.CreateDirectory(enDir);

            foreach (var filePath in inkFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);

                string[] lines = File.ReadAllLines(filePath);

                List<string> engLines = new();
                List<string> ruLines = new();

                foreach (string line in lines)
                {
                    engLines.Add(ProcessLine(line, enDict, tagsDict, "en"));
                    ruLines.Add(ProcessLine(line, ruDict, tagsDict, "ru"));
                }

                string ruName = fileName + "_ru.ink";
                string enName = fileName + "_en.ink";

                File.WriteAllLines(Path.Combine(ruDir, ruName), ruLines);
                File.WriteAllLines(Path.Combine(enDir, enName), engLines);
            }

            Debug.Log($"Generated localized Ink files in:\n{ruDir}\n{enDir}");
        }

        private static string ProcessLine(string line, Dictionary<string, string> langDict, Dictionary<string, string> tagsDict, string lang)
        {
            string result = line;

            // Заменяем INCLUDE на INCLUDE _ru/_eng
            if (line.TrimStart().StartsWith("INCLUDE "))
            {
                result = Regex.Replace(line, @"INCLUDE\s+(\w+)", $"INCLUDE $1_{lang}");
                return result;
            }

            // Заменяем все ID вида dia_...
            result = Regex.Replace(result, @"dia_\w+", match =>
            {
                string id = match.Value;
                string text = langDict.TryGetValue(id, out var t) ? t : id;

                // Экранируем цвет внутри текста
                text = Regex.Replace(text, @"<color=#", "<color=\\#");

                string tag = tagsDict.TryGetValue(id, out var g) ? g : "";
                return $"{text} {tag}".Trim();
            });

            return result;
        }
    }
}
#endif
