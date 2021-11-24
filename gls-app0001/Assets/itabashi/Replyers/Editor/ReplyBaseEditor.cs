using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Replyer.Editors
{
    [CustomEditor(typeof(ReplyerBase))]
    public class ReplyBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        protected void EditBaseGUI()
        {
            serializedObject.Update();

            var chance = serializedObject.FindProperty("m_chance");

            chance.floatValue = EditorGUILayout.Slider(new GUIContent("発生確率"), chance.floatValue, 0.0f, 100.0f);

            GUILayout.Space(20);

            serializedObject.ApplyModifiedProperties();
        }

        public void DefaultPropertyField(SerializedObject serializedObject,string propertyPath)
        {
            var property = serializedObject.FindProperty(propertyPath);
            EditorGUILayout.PropertyField(property);
        }
    }
}