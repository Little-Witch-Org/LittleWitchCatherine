using UnityEditor;
using UnityEngine;

public class ToggleActiveShortcut
{
    [MenuItem("GameObject/Toggle Active F1", false, -100)] // Alt + F1
    private static void ToggleActive()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Undo.RecordObject(obj, "Toggle Active");
            obj.SetActive(!obj.activeSelf);
        }
    }

    // Делаем кнопку активной только если выбран объект
    [MenuItem("GameObject/Toggle Active &a", true)]
    private static bool ValidateToggleActive()
    {
        return Selection.activeGameObject != null;
    }
}
