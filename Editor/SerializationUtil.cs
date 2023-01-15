using System;
using UnityEditor;
using UnityEngine;

namespace ZnZUtiliy.Editor
{
    public static class SerializationUtil
    {
        /// <summary>
        /// Shows all the properties in the serialized object with name and type
        /// </summary>
        /// <param name="so"></param>
        /// <param name="includeChildren"></param>
        public static void LogProperties(this SerializedObject so, bool includeChildren = true)
        {
            so.Update();
            SerializedProperty propertyLogger = so.GetIterator();
            string message = $"Properties of {so.context?.name}:";
            while (propertyLogger.Next(includeChildren))
            {
                message += $"\nname = {propertyLogger.name}\n\t type = {propertyLogger.type}";
            }

            Debug.Log(message);
        }

        /// <summary>
        /// Logs all fields of a property to the unity debug window
        /// </summary>
        /// <param name="serializedProperty"></param>
        public static void LogAllValues(this SerializedProperty serializedProperty)
        {
            try
            {
                Debug.Log($"Property: name = {serializedProperty.name}\n" +
                          $"type = {serializedProperty.type}\n" +
                          $"depth = {serializedProperty.depth}\n" +
                          $"hasChildren = {serializedProperty.hasChildren}\n" +
                          $"hasVisibleChildren = {serializedProperty.hasVisibleChildren}\n" +
                          $"isArray = {serializedProperty.isArray}\n" +
                          $"arraySize = {serializedProperty.arraySize}\n" +
                          $"editable = {serializedProperty.editable}\n" +
                          $"enumNames = {serializedProperty.enumNames}\n" +
                          $"enumValueIndex = {serializedProperty.enumValueIndex}\n" +
                          $"hasMultipleDifferentValues = {serializedProperty.hasMultipleDifferentValues}\n" +
                          $"isAnimated = {serializedProperty.isAnimated}\n" +
                          $"isExpanded = {serializedProperty.isExpanded}\n" +
                          $"isInstantiatedPrefab = {serializedProperty.isInstantiatedPrefab}\n" +
                          $"prefabOverride = {serializedProperty.prefabOverride}\n" +
                          $"propertyPath = {serializedProperty.propertyPath}\n" +
                          $"propertyType = {serializedProperty.propertyType}\n" +
                          $"serializedObject = {serializedProperty.serializedObject}\n" +
                          $"tooltip = {serializedProperty.tooltip}\n\n" +
                          $"animationCurveValue = {serializedProperty.animationCurveValue}\n" +
                          $"boolValue = {serializedProperty.boolValue}\n" +
                          $"boundsValue = {serializedProperty.boundsValue}\n" +
                          $"colorValue = {serializedProperty.colorValue}\n" +
                          $"floatValue = {serializedProperty.floatValue}\n" +
                          $"intValue = {serializedProperty.intValue}\n" +
                          $"objectReferenceInstanceIDValue = {serializedProperty.objectReferenceInstanceIDValue}\n" +
                          $"objectReferenceValue = {serializedProperty.objectReferenceValue}\n" +
                          $"quaternionValue = {serializedProperty.quaternionValue}\n" +
                          $"rectValue = {serializedProperty.rectValue}\n" +
                          $"stringValue = {serializedProperty.stringValue}\n" +
                          $"vector2Value = {serializedProperty.vector2Value}\n" +
                          $"vector3Value = {serializedProperty.vector3Value}\n");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void ProcessLabelPropertyDictionary(this SerializedProperty property)
        {
            var keys = property.FindPropertyRelative("keys").Copy().GetEnumerator();
            var values = property.FindPropertyRelative("values").Copy().GetEnumerator();
            while (keys.MoveNext() && values.MoveNext())
            {
                EditorGUILayout.PropertyField((SerializedProperty) values.Current,
                    new GUIContent(((SerializedProperty) keys.Current)?.stringValue));
            }
        }
    }
}