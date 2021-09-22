using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AttributeObject
{
    /// <summary>
    /// �_���[�W�f�[�^�\����
    /// </summary>
    public readonly struct DamageData
    {
        /// <summary>
        /// �_���[�W��
        /// </summary>
        public readonly float damageValue;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="damageValue">�_���[�W��</param>
        public DamageData(float damageValue)
        {
            this.damageValue = damageValue;
        }
    }

    /// <summary>
    /// �_���[�W���󂯂���@�\������R���|�[�l���g
    /// </summary>
    public class TakeDamageObject : MonoBehaviour
    {
        /// <summary>
        /// �_���[�W���󂯂��ۂɌĂ΂��C�x���g
        /// </summary>
        [SerializeField]
        private UnityEvent<DamageData> m_takeDamageEvent;

        /// <summary>
        /// �_���[�W���󂯂郁�\�b�h
        /// </summary>
        /// <param name="damageData">�_���[�W�f�[�^</param>
        public void TakeDamage(in DamageData damageData)
        {
            m_takeDamageEvent.Invoke(damageData);
        }
    }
}