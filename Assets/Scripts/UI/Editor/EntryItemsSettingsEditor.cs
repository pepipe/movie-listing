using UnityEditor;

namespace UI.Editor {
    [CustomEditor(typeof(EntryItemSettings))]
    public class EntryItemsSettingsEditor : UnityEditor.Editor {
        private SerializedProperty _split;

        private void OnEnable()
        {
            _split = serializedObject.FindProperty("splitEntryValue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("header"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("customEntryItemPrefab"));
            EditorGUILayout.PropertyField(_split);
            if (_split.boolValue)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("splitSetting"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
