using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Replyer.Editors
{
    [CustomEditor(typeof(ReplyRotater))]
    public class ReplyRotaterEditor : ReplyBaseEditor
    {
        GUIContent m_previewTitle = new GUIContent("Preview");

        private bool m_isPlaying = false;

        private PreviewRenderUtility m_renderer = null;

        private float m_countTime = 0.0f;

        private float m_beforeStartUpTime = 0.0f;

        public override void OnInspectorGUI()
        {
            EditBaseGUI();

            serializedObject.Update();

            DefaultPropertyField(serializedObject, "m_targetTransform");

            GUILayout.Space(20);

            GUILayout.Label(new GUIContent("時間設定"), EditorStyles.boldLabel);

            DefaultPropertyField(serializedObject, "m_duration");
            DefaultPropertyField(serializedObject, "m_timeScaleType");

            GUILayout.Space(20);

            GUILayout.Label(new GUIContent("回転設定"), EditorStyles.boldLabel);

            DefaultPropertyField(serializedObject, "m_worldSpaceType");

            GUILayout.Space(5);

            var curveZeroValue = serializedObject.FindProperty("m_curveZeroValue");
            EditorGUILayout.PropertyField(curveZeroValue);

            var curveOneValue = serializedObject.FindProperty("m_curveOneValue");
            EditorGUILayout.PropertyField(curveOneValue);

            curveOneValue.floatValue = Mathf.Max(curveZeroValue.floatValue, curveOneValue.floatValue);

            GUILayout.Space(5);

            RotateCurveField("m_rotateX", "m_rotateCurveX", Color.green);
            RotateCurveField("m_rotateY", "m_rotateCurveY", Color.red);
            RotateCurveField("m_rotateZ", "m_rotateCurveZ", Color.yellow);

            serializedObject.ApplyModifiedProperties();
        }

        private void RotateCurveField(string rotateEulerPath, string rotateCurvePath, Color color)
        {
            var rotateEuler = serializedObject.FindProperty(rotateEulerPath);
            EditorGUILayout.PropertyField(rotateEuler);

            if(rotateEuler.boolValue)
            {
                var animationCurve = serializedObject.FindProperty(rotateCurvePath);
                animationCurve.animationCurveValue = EditorGUILayout.CurveField(animationCurve.displayName, animationCurve.animationCurveValue, color, new Rect(0, 0, 0, 0));
                //DefaultPropertyField(serializedObject, rotateCurvePath);
            }
        }

        private float GetCurveValue(string rotatePath, string rotateCurvePath, float time)
        {
            var rotate = serializedObject.FindProperty(rotatePath);

            if(!rotate.boolValue)
            {
                return 0.0f;
            }

            var rotateCurve = serializedObject.FindProperty(rotateCurvePath);

            var animationCurve = rotateCurve.animationCurveValue;

            return animationCurve.Evaluate(time);
        }

        public override bool HasPreviewGUI()
        {
            return true;
        }

        public override GUIContent GetPreviewTitle()
        {
            return m_previewTitle;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            if (m_renderer == null)
            {
                m_renderer = new PreviewRenderUtility();
                m_renderer.camera.farClipPlane = 100;
                m_renderer.camera.transform.position = new Vector3(-20, 13.5f, -20);
                m_renderer.camera.transform.rotation = Quaternion.Euler(25, 45, 0);
                m_renderer.camera.clearFlags = CameraClearFlags.SolidColor;
            }

            m_renderer.BeginPreview(r, background);

            var replyRotater = target as ReplyRotater;

            var mesh = replyRotater.GetComponent<MeshFilter>().sharedMesh;
            var mat = replyRotater.GetComponent<MeshRenderer>().sharedMaterial;

            if (m_isPlaying)
            {
                m_countTime += (float)EditorApplication.timeSinceStartup - m_beforeStartUpTime;
                m_isPlaying = m_countTime < replyRotater.duration;

                if (!m_isPlaying)
                {
                    m_countTime = replyRotater.duration;
                }
            }
            else
            {
                m_countTime = 0.0f;
            }

            Vector3 euler = replyRotater.GetRotateToTime(m_countTime / replyRotater.duration);

            m_renderer.DrawMesh(mesh, new Vector3(), Quaternion.identity * Quaternion.Euler(euler), mat, 0);

            m_renderer.camera.Render();
            var texture = m_renderer.EndPreview();

            GUI.DrawTexture(r, texture);

            m_beforeStartUpTime = (float)EditorApplication.timeSinceStartup;
        }

        public override void OnPreviewSettings()
        {
            base.OnPreviewSettings();

            var playButton = EditorGUIUtility.IconContent("preAudioPlayOn");
            var pauseButton = EditorGUIUtility.IconContent("preAudioPlayOff");

            EditorGUI.BeginChangeCheck();
            m_isPlaying = GUILayout.Toggle(m_isPlaying, m_isPlaying ? playButton : pauseButton, "preButton");
            if (EditorGUI.EndChangeCheck())
            {
                m_beforeStartUpTime = (float)EditorApplication.timeSinceStartup;
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return m_isPlaying;
        }
    }
}
