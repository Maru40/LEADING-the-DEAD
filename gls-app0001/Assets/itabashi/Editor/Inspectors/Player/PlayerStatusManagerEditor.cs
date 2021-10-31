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
        /// <summary>
        /// 状態異常関係の折り畳み
        /// </summary>
        private bool m_abnormalStateFoldout = true;

        private bool m_stunFoldout = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var gameStateManager = serializedObject.FindProperty("m_gameStateManager");
            EditorGUILayout.PropertyField(gameStateManager);

            HpGUILayout();

            StaminaGUILayout();

            AbnormalGUILayout();

            EditorGUILayout.Space();

            var animatorManager = serializedObject.FindProperty("m_animatorManager");
            EditorGUILayout.PropertyField(animatorManager);

            var stunStar = serializedObject.FindProperty("m_stunStar");
            EditorGUILayout.PropertyField(stunStar);

            var deadStart = serializedObject.FindProperty("m_deadStartEvent");
            EditorGUILayout.PropertyField(deadStart, new GUIContent("死亡開始イベント"));

            var deadEnd = serializedObject.FindProperty("m_deadEndEvent");
            EditorGUILayout.PropertyField(deadEnd, new GUIContent("死亡終了イベント"));

            serializedObject.ApplyModifiedProperties();
        }

        private void HpGUILayout()
        {
            m_hpFoldout = EditorGUILayout.Foldout(m_hpFoldout, "HP");

            if (!m_hpFoldout)
            {
                return;
            }

            var hp = serializedObject.FindProperty("m_hp");
            var maxHp = serializedObject.FindProperty("m_maxHp");
            var hpGauge = serializedObject.FindProperty("m_hpGauge");

            var hpValue = hp.FindPropertyRelative("value");

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(hpGauge, new GUIContent("HPゲージ"));
            EditorGUILayout.PropertyField(maxHp, new GUIContent("体力の最大値"));

            hpValue.floatValue = Mathf.Min(hpValue.floatValue, maxHp.floatValue);

            EditorGUILayout.Slider(hpValue, 0.0f, maxHp.floatValue, new GUIContent("現在の体力"));

            EditorGUI.indentLevel--;
        }

        private void StaminaGUILayout()
        {
            m_stamitaFoldout = EditorGUILayout.Foldout(m_stamitaFoldout, "スタミナ");

            if (!m_stamitaFoldout)
            {
                return;
            }

            var stamina = serializedObject.FindProperty("m_stamina");
            var maxStamina = serializedObject.FindProperty("m_maxStamina");
            var staminaGauge = serializedObject.FindProperty("m_staminaGauge");
            var staminaRecoveryPerSeconds = serializedObject.FindProperty("m_staminaRecoveryPerSeconds");
            var updateStaminaEvent = serializedObject.FindProperty("m_updateStaminaEvent");

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(staminaGauge, new GUIContent("スタミナゲージ"));
            EditorGUILayout.PropertyField(maxStamina, new GUIContent("スタミナの最大値"));

            stamina.floatValue = Mathf.Min(stamina.floatValue, maxStamina.floatValue);

            EditorGUILayout.Slider(stamina, 0.0f, maxStamina.floatValue, new GUIContent("現在のスタミナ"));

            EditorGUILayout.PropertyField(staminaRecoveryPerSeconds, new GUIContent("回復スタミナ量/秒"));

            EditorGUI.indentLevel--;
        }

        private void AbnormalGUILayout()
        {
            m_abnormalStateFoldout = EditorGUILayout.Foldout(m_abnormalStateFoldout, "状態");

            if(!m_abnormalStateFoldout)
            {
                return;
            }

            EditorGUI.indentLevel++;

            var isInvincible = serializedObject.FindProperty("m_isInvincible");

            EditorGUILayout.PropertyField(isInvincible);

            StunGUILayout();

            EditorGUI.indentLevel--;
        }

        private void StunGUILayout()
        {
            EditorGUILayout.BeginHorizontal();
            {
                m_stunFoldout = EditorGUILayout.Foldout(m_stunFoldout, "スタン");
            }
            EditorGUILayout.EndHorizontal();

            if(!m_stunFoldout)
            {
                return;
            }

            EditorGUI.indentLevel++;

            var isStun = serializedObject.FindProperty("m_isStun");

            EditorGUILayout.PropertyField(isStun);

            var stunSecound = serializedObject.FindProperty("m_stunSecond");

            EditorGUILayout.PropertyField(stunSecound, new GUIContent("スタン時間(秒)"));

            stunSecound.floatValue = Mathf.Max(stunSecound.floatValue, 0.0f);

            EditorGUI.indentLevel--;

        }
    }
}
