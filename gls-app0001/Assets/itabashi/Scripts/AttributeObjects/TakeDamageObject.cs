using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AttributeObject
{
    /// <summary>
    /// ダメージデータ構造体
    /// </summary>
    public readonly struct DamageData
    {
        /// <summary>
        /// ダメージ量
        /// </summary>
        public readonly float damageValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="damageValue">ダメージ量</param>
        public DamageData(float damageValue)
        {
            this.damageValue = damageValue;
        }
    }

    /// <summary>
    /// ダメージを受けられる機能をつけるコンポーネント
    /// </summary>
    public class TakeDamageObject : MonoBehaviour
    {
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
            m_takeDamageEvent.Invoke(damageData);
        }
    }
}
