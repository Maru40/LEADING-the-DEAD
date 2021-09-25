using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Player
{
    [CustomEditor(typeof(PlayerStatusManager))]
    public class PlayerStatusManagerEditor : Editor
    {
        /// <summary>
        /// HP�֌W�̐܂���
        /// </summary>
        private bool m_hpFoldout = true;
        /// <summary>
        /// �X�^�~�i�֌W�̐܂���
        /// </summary>
        private bool m_stamitaFoldout = true;
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            m_hpFoldout = EditorGUILayout.Foldout(m_hpFoldout, "HP");

            var playerStatusManager = (PlayerStatusManager)target;

            if(m_hpFoldout)
            {
                var hp = serializedObject.FindProperty("m_hp");
                var maxHp = serializedObject.FindProperty("m_maxHp");
                var hpGauge = serializedObject.FindProperty("m_hpGauge");

                EditorGUILayout.PropertyField(hpGauge, new GUIContent("    HP�Q�[�W"));
                EditorGUILayout.PropertyField(maxHp, new GUIContent("    �̗͂̍ő�l"));

                hp.floatValue = Mathf.Min(hp.floatValue, maxHp.floatValue);

                EditorGUILayout.Slider(hp, 0.0f, maxHp.floatValue,new GUIContent("    ���݂̗̑�"));
            }

            m_stamitaFoldout = EditorGUILayout.Foldout(m_stamitaFoldout, "�X�^�~�i");

            if (m_stamitaFoldout)
            {
                var stamina = serializedObject.FindProperty("m_stamina");
                var maxStamina = serializedObject.FindProperty("m_maxStamina");
                var staminaGauge = serializedObject.FindProperty("m_staminaGauge");
                var staminaRecoveryPerSeconds = serializedObject.FindProperty("m_staminaRecoveryPerSeconds");
                var updateStaminaEvent = serializedObject.FindProperty("m_updateStaminaEvent");

                EditorGUILayout.PropertyField(staminaGauge, new GUIContent("    �X�^�~�i�Q�[�W"));
                EditorGUILayout.PropertyField(maxStamina, new GUIContent("    �X�^�~�i�̍ő�l"));

                stamina.floatValue = Mathf.Min(stamina.floatValue, maxStamina.floatValue);

                EditorGUILayout.Slider(stamina, 0.0f, maxStamina.floatValue, new GUIContent("    ���݂̃X�^�~�i"));

                EditorGUILayout.PropertyField(staminaRecoveryPerSeconds, new GUIContent("    �񕜃X�^�~�i��/�b"));

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(updateStaminaEvent, new GUIContent("�X�^�~�i�X�V�C�x���g"));
            }

            EditorGUILayout.Space();

            var deadEvent = serializedObject.FindProperty("m_deadEvent");
            EditorGUILayout.PropertyField(deadEvent, new GUIContent("���S�C�x���g"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}