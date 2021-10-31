using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AttributeObject
{
    public enum DamageType
    {
        None,
        Fire,
    }

    /// <summary>
    /// ダメージデータ構造体
    /// </summary>
    [System.Serializable]
    public struct DamageData
    {
        /// <summary>
        /// ダメージを与えるオブジェクト
        /// </summary>
        public GameObject obj;
        /// <summary>
        /// ダメージ量
        /// </summary>
        public float damageValue;
        /// <summary>
        /// この攻撃でスタンするか
        /// </summary>
        public bool isStunAttack;
        /// <summary>
        /// ヒットストップ時間
        /// </summary>
        public float hitStopTime;

        public DamageType type;
        public List<string> damageTags;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="damageValue">ダメージ量</param>
        public DamageData(float damageValue, bool isStunAttack = false, float hitStopTime = 0.0f, GameObject obj = null)
        {
            this.obj = obj;
            this.damageValue = damageValue;
            this.isStunAttack = isStunAttack;
            this.hitStopTime = hitStopTime;

            this.type = DamageType.None;
            damageTags = new List<string>();
        }

        public bool FindTag(string tag)
        {
            foreach (var damageTag in damageTags)
            {
                if (damageTag == tag)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// ダメージを受けられる機能をつけるコンポーネント
    /// </summary>
    public class TakeDamageObject : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        /// <summary>
        /// ヒットストップするか
        /// </summary>
        [SerializeField]
        private bool m_isHitStopable = true;

        /// <summary>
        /// ダメージを受けた際に呼ばれるイベント
        /// </summary>
        [SerializeField]
        private UnityEvent<DamageData> m_takeDamageEvent;

        /// <summary>
        /// ダメージを受けるメソッド
        /// </summary>
        /// <param name="damageData">ダメージデータ</param>
        public void TakeDamage(in DamageData damageData)
        {
            if(m_animator && m_isHitStopable)
            {
                StartCoroutine(HitStop(damageData.hitStopTime));
            }

            m_takeDamageEvent.Invoke(damageData);
        }

        IEnumerator HitStop(float hitStopTime)
        {
            float animatorSpeed = m_animator.speed;

            m_animator.speed = 0.0f;

            float countHitStopTime = 0.0f;

            while (countHitStopTime < hitStopTime)
            {
                countHitStopTime += Time.deltaTime;

                yield return null;
            }

            m_animator.speed = animatorSpeed;
        }
    }
}
