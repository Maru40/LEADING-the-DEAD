using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RadioStateManager))]
public class RadioStateManagerEditor : Editor
{
    private int m_arraySize;

    private bool m_groundTagsFoldout = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        m_groundTagsFoldout = EditorGUILayout.Foldout(m_groundTagsFoldout, "接地タグ");

        var groundTags = serializedObject.FindProperty("m_groundTags");

        m_arraySize = groundTags.arraySize;

        m_arraySize = EditorGUILayout.IntField("Size", m_arraySize);

        m_arraySize = Mathf.Max(m_arraySize, 0);

        groundTags.arraySize = m_arraySize;

        EditorGUILayout.EndHorizontal();

        if (m_groundTagsFoldout)
        {
            for (int i = 0; i < m_arraySize; i++)
            {
                var tag = groundTags.GetArrayElementAtIndex(i);
                tag.stringValue = EditorGUILayout.TagField($"    タグ {i}", tag.stringValue);
            }
        }

        var stopTimerSecond = serializedObject.FindProperty("m_stopTimerSecond");

        EditorGUILayout.PropertyField(stopTimerSecond, new GUIContent("止まるまでの時間"));

        serializedObject.ApplyModifiedProperties();
    }
}
