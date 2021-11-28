using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PopUpUI))]
public class PopUpUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var firstSelectObject = serializedObject.FindProperty("firstSelectObject");
        EditorGUILayout.PropertyField(firstSelectObject);

        GUILayout.Space(10);

        var directorType = serializedObject.FindProperty("m_directorType");
        EditorGUILayout.PropertyField(directorType);

        DisplayDirector((PopUpUI.DirectorType)directorType.enumValueIndex);

        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayDirector(PopUpUI.DirectorType directorType)
    {
        if(directorType == PopUpUI.DirectorType.None)
        {
            return;
        }

        if(directorType == PopUpUI.DirectorType.Timeline)
        {
            var director = serializedObject.FindProperty("m_director");
            EditorGUILayout.PropertyField(director);
            return;
        }

        if(directorType == PopUpUI.DirectorType.SimpleStartEndAnimator)
        {
            var m_animateDirector = serializedObject.FindProperty("m_animateDirector");
            EditorGUILayout.PropertyField(m_animateDirector);
            return;
        }
    }
}
