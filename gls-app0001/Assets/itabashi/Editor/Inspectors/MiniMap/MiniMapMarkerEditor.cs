using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MiniMap
{
    [CustomEditor(typeof(MiniMapMarker))]
    public class MiniMapMarkerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var markerData = serializedObject.FindProperty("m_markerData");
            var isForwardToUp = serializedObject.FindProperty("m_isForwardToUp");

            EditorGUILayout.PropertyField(markerData, new GUIContent("マーカー情報"));
            EditorGUILayout.PropertyField(isForwardToUp, new GUIContent("forwardと同期する"));

            var drawOutOfRange = serializedObject.FindProperty("m_drawOutOfRange");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(drawOutOfRange, new GUIContent("範囲外でも表示する"));

            if(drawOutOfRange.boolValue)
            {
                var outOfRangeMarkerData = serializedObject.FindProperty("m_outOfRangeMarkerData");
                var isOutwardToUp = serializedObject.FindProperty("m_isOutwardToUp");

                EditorGUILayout.PropertyField(outOfRangeMarkerData, new GUIContent("範囲外マーカー情報"));
                EditorGUILayout.PropertyField(isOutwardToUp, new GUIContent("外向きに向ける"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}