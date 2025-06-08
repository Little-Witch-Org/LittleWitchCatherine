#if UNITY_EDITOR
using _Scripts.InventorySystem.ByGuide.Scriptable;
using UnityEditor;
using UnityEngine;

namespace _Scripts.InventorySystem.ByGuide.ShapeEditor
{
    [CustomEditor(typeof(ItemDataSo), true)]
    public class ItemDataSoEditor : Editor
    {
        private SerializedProperty _shapeMaskSerializedProp;

        private void OnEnable()
        {
            _shapeMaskSerializedProp = serializedObject.FindProperty("shapeMaskSerialized");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ItemDataSo itemData = (ItemDataSo)target;

            // Рисуем стандартные свойства
            DrawPropertiesExcluding(serializedObject, "shapeMaskSerialized");

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Shape Configuration", EditorStyles.boldLabel);

            // Кнопки изменения размера
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+ Width")) itemData.width++;
            if (GUILayout.Button("- Width") && itemData.width > 1) itemData.width--;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+ Height")) itemData.height++;
            if (GUILayout.Button("- Height") && itemData.height > 1) itemData.height--;
            EditorGUILayout.EndHorizontal();

            itemData.InitializeShapeMask();

            // Рисуем сетку формы
            EditorGUILayout.Space(5);
            for (int y = 0; y < itemData.height; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < itemData.width; x++)
                {
                    int index = y * itemData.width + x;
                    SerializedProperty element = _shapeMaskSerializedProp.GetArrayElementAtIndex(index);
                
                    EditorGUI.BeginChangeCheck();
                    bool newValue = EditorGUILayout.Toggle(element.boolValue, GUILayout.Width(20));
                    if (EditorGUI.EndChangeCheck())
                    {
                        element.boolValue = newValue;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            if (serializedObject.ApplyModifiedProperties())
            {
                EditorUtility.SetDirty(target);
                AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif