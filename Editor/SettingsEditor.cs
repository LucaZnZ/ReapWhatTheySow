using UnityEditor;
using UnityEngine;
using ZnZUtil.Settings;

namespace ZnZUtiliy.Editor
{
    [CustomEditor(typeof(SettingsControllerBase), true)]
    public class SettingsEditor : UnityEditor.Editor
    {
        private SerializedProperty intEventsProperty,
            boolEventsProperty,
            stringEventsProperty,
            floatEventsProperty,
            testEvent;

        // private bool eventFoldout = true;

        private void OnEnable()
        {
            boolEventsProperty = serializedObject.FindProperty("boolEvents");
            stringEventsProperty = serializedObject.FindProperty("stringEvents");
            floatEventsProperty = serializedObject.FindProperty("floatEvents");
            intEventsProperty = serializedObject.FindProperty("intEvents");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ProcessEventDictionary(boolEventsProperty);
            ProcessEventDictionary(floatEventsProperty);
            ProcessEventDictionary(intEventsProperty);
            ProcessEventDictionary(stringEventsProperty);

            serializedObject.ApplyModifiedProperties();
        }

        private static void ProcessEventDictionary(SerializedProperty property)
        {
            var keys = property.FindPropertyRelative("keys").GetEnumerator();
            var values = property.FindPropertyRelative("values").GetEnumerator();
            while (keys.MoveNext() && values.MoveNext())
            {
                var prop = (SerializedProperty) values.Current;
                // prop.LogAllValues();
                EditorGUILayout.PropertyField(prop,
                    new GUIContent(BuildEventName(keys.Current)));
            }
        }

        private static string BuildEventName(object obj)
            => $"On{((SerializedProperty) obj)?.stringValue.Replace(" ", "")}Changed";
    }
}