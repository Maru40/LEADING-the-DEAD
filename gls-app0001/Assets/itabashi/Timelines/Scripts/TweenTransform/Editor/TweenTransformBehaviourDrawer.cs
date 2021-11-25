using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Timelines.Playables.TweenTransform
{
    [CustomPropertyDrawer(typeof(TweenTransformBehaviour))]
    public class TweenTransformBehaviourDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawAnimateCurve(property, "animatePosition", "positionCurve");
            DrawAnimateCurve(property, "animateRotation", "rotationCurve");
            DrawAnimateCurve(property, "animateScale", "scaleCurve");
        }

        private void DrawAnimateCurve(SerializedProperty property,string animatePath,string curvePath)
        {
            var animate = property.FindPropertyRelative(animatePath);
            EditorGUILayout.PropertyField(animate);

            if(animate.boolValue)
            {
                var curve = property.FindPropertyRelative(curvePath);
                EditorGUILayout.PropertyField(curve);
            }
        }
    }
}