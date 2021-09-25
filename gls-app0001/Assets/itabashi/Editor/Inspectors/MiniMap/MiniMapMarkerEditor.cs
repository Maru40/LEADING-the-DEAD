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

            EditorGUILayout.PropertyField(markerData, new GUIContent("�}�[�J�[���"));
            EditorGUILayout.PropertyField(isForwardToUp, new GUIContent("forward�Ɠ�������"));

            var drawOutOfRange = serializedObject.FindProperty("m_drawOutOfRange");

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(drawOutOfRange, new GUIContent("�͈͊O�ł��\������"));

            if(drawOutOfRange.boolValue)
            {
                var outOfRangeMarkerData = serializedObject.FindProperty("m_outOfRangeMarkerData");
                var isOutwardToUp = serializedObject.FindProperty("m_isOutwardToUp");

                EditorGUILayout.PropertyField(outOfRangeMarkerData, new GUIContent("�͈͊O�}�[�J�[���"));
                EditorGUILayout.PropertyField(isOutwardToUp, new GUIContent("�O�����Ɍ�����"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}