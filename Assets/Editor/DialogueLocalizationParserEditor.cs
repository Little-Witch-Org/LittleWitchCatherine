#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(_Scripts.Dialog_Ink.DialogueLocalizationParser))]
public class DialogueLocalizationParserEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var parser = (_Scripts.Dialog_Ink.DialogueLocalizationParser)target;

        GUILayout.Space(10);
        if (GUILayout.Button("🔁 Generate All Localized Ink Files"))
        {
            parser.GenerateAllLocalizedInkFiles();
        }
    }
}
#endif