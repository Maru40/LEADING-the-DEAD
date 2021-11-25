using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Timelines.Playables.TweenTransform
{
    [CustomPropertyDrawer(typeof(TweenTransformBehaviour.AnimationCurve3))]
    public class AnimationCurve3Drawer : PropertyDrawer
    {
        private GUILayoutOption m_labelWidth = GUILayout.Width(25);

        private (string name, Color color)[] m_fields = { ("X", Color.green), ("Y", Color.red), ("Z", Color.yellow) };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            foreach(var field in m_fields)
            {
                DrawHorizontalCurve(property, field.name, field.color);
            }
        }

        private void DrawHorizontalCurve(SerializedProperty property, string field, Color color)
        {
            var animate = property.FindPropertyRelative("animate" + field);
            var curve = property.FindPropertyRelative("curve" + field);

            using(new EditorGUILayout.HorizontalScope())
            {
                animate.boolValue = EditorGUILayout.Toggle(animate.boolValue);
                EditorGUILayout.LabelField(field, m_labelWidth);
                curve.animationCurveValue = EditorGUILayout.CurveField(curve.animationCurveValue, color, Rect.zero);
            }
        }
    }
}