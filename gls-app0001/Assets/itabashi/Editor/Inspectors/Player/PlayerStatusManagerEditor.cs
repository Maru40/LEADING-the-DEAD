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
        /// HP関係の折り畳み
        /// </summary>
        private bool m_hpFoldout = true;
        /// <summary>
        /// スタミナ関係の折り畳み
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

                EditorGUILayout.PropertyField(hpGauge, new GUIContent("    HPゲージ"));
                EditorGUILayout.PropertyField(maxHp, new GUIContent("    体力の最大値"));

                hp.floatValue = Mathf.Min(hp.floatValue, maxHp.floatValue);

                EditorGUILayout.Slider(hp, 0.0f, maxHp.floatValue,new GUIContent("    現在の体力"));
            }

            m_stamitaFoldout = EditorGUILayout.Foldout(m_stamitaFoldout, "スタミナ");

            if (m_stamitaFoldout)
            {
                var stamina = serializedObject.FindProperty("m_stamina");
                var maxStamina = serializedObject.FindProperty("m_maxStamina");
                var staminaGauge = serializedObject.FindProperty("m_staminaGauge");
                var staminaRecoveryPerSeconds = serializedObject.FindProperty("m_staminaRecoveryPerSeconds");
                var updateStaminaEvent = serializedObject.FindProperty("m_updateStaminaEvent");

                EditorGUILayout.PropertyField(staminaGauge, new GUIContent("    スタミナゲージ"));
                EditorGUILayout.PropertyField(maxStamina, new GUIContent("    スタミナの最大値"));

                stamina.floatValue = Mathf.Min(stamina.floatValue, maxStamina.floatValue);

                EditorGUILayout.Slider(stamina, 0.0f, maxStamina.floatValue, new GUIContent("    現在のスタミナ"));

                EditorGUILayout.PropertyField(staminaRecoveryPerSeconds, new GUIContent("    回復スタミナ量/秒"));

                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(updateStaminaEvent, new GUIContent("スタミナ更新イベント"));
            }

            EditorGUILayout.Space();

            var deadEvent = serializedObject.FindProperty("m_deadEvent");
            EditorGUILayout.PropertyField(deadEvent, new GUIContent("死亡イベント"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
