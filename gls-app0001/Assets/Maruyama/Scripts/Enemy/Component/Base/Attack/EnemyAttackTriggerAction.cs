using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;

[RequireComponent(typeof(AudioManager))]
public class EnemyAttackTriggerAction : TriggerAction
{
    [SerializeField]
    StatusManagerBase m_statusManager = null;

    [SerializeField]
    AnimatorManagerBase m_animatorManager = null;

    [SerializeField]
    DamageData m_damageData = new DamageData();

    [SerializeField]
    AudioManager m_audioManager;

    [Header("ヒット時のパーティクル"),SerializeField]
    ParticleManager.ParticleID m_hitParticleID;
    
    Collider m_hitCollider;

    private void Awake()
    {
        //m_audio = GetComponent<AudioSource>();
        m_audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        m_hitCollider = GetComponent<Collider>();

        AddEnterAction(SendDamage);
    }

    /// <summary>
    /// 攻撃開始
    /// </summary>
    /// <param name="hitTime">ヒット時間</param>
    public void AttackStart()
    {
        m_hitCollider.enabled = true;
    }

    public void AttackEnd()
    {
        m_hitCollider.enabled = false;
    }

    /// <summary>
    /// 相手にダメージを与える。
    /// </summary>
    private void SendDamage(Collider other)
    {
        if (other.gameObject == this.gameObject) {
            return;
        }

        var damage = other.GetComponent<TakeDamageObject>();
        if (damage != null)
        {
            var damageData = m_damageData;
            if (m_statusManager != null)  //StatusManagerが存在したらバフを掛ける。
            {
                var power = damageData.damageValue * m_statusManager.GetBuffParametor().angerParam.attackPower;
                damageData.damageValue = power;
            }

            //m_audio.PlayOneShot(m_audio.clip);  //音再生
            ParticleManager.Instance.Play(m_hitParticleID ,transform.position);
            m_audioManager?.PlayOneShot();
            m_animatorManager?.HitStop(damageData);
            damage.TakeDamage(damageData);
        }
    }
}
