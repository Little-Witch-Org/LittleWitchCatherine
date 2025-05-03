using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class BatchRenameTool : EditorWindow
{
    [MenuItem("Tools/Batch Rename Files with Folder Prefix")]
    public static void ShowWindow()
    {
        GetWindow<BatchRenameTool>("Batch Rename");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Rename Files (Unity-safe)", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Rename Files in Selected Folder"))
        {
            RenameFilesInSelectedFolder();
        }
    }

    private static void RenameFilesInSelectedFolder()
    {
        if (Selection.activeObject == null)
        {
            Debug.LogError("No folder selected! Select a folder in Project window first.");
            return;
        }

        string selectedFolderPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        
        if (!AssetDatabase.IsValidFolder(selectedFolderPath))
        {
            Debug.LogError("Selected object is not a folder!"); 
            return;
        }

        string[] subDirectories = AssetDatabase.GetSubFolders(selectedFolderPath);
        
        foreach (string dir in subDirectories)
        {
            RenameFilesInDirectory(dir);
        }
        
        AssetDatabase.Refresh();
        Debug.Log("Renaming complete!");
    }

    private static void RenameFilesInDirectory(string dirPath)
    {
        string dirName = Path.GetFileName(dirPath);
        Match match = Regex.Match(dirName, @"^(\d+)");
        
        if (!match.Success)
        {
            Debug.LogWarning($"Skipping folder (no number prefix): {dirName}");
            return;
        }

        string folderNumber = match.Groups[1].Value;
        string[] assetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { dirPath });

        foreach (string guid in assetGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string extension = Path.GetExtension(assetPath);

            if (fileName.StartsWith(folderNumber + "."))
            {
                Debug.Log($"Skipping (already renamed): {fileName}");
                continue;
            }

            string[] parts = fileName.Split(new[] { '.' }, 2);
            if (parts.Length != 2)
            {
                Debug.LogWarning($"Skipping (unexpected format): {fileName}");
                continue;
            }

            string newName = $"{folderNumber}.{parts[0]}.{parts[1]}{extension}";
            string newPath = Path.Combine(dirPath, newName);

            if (AssetDatabase.LoadAssetAtPath<Texture2D>(newPath) != null)
            {
                Debug.LogWarning($"File already exists, skipping: {newPath}");
                continue;
            }

            string error = AssetDatabase.MoveAsset(assetPath, newPath);
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Failed to rename {assetPath}: {error}");
            }
            else
            {
                Debug.Log($"Renamed: {fileName} → {newName}");
            }
        }
    }
}