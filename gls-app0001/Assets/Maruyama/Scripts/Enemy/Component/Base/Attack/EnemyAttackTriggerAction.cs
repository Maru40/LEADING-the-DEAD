using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using AttributeObject;

[RequireComponent(typeof(AudioManager))]
public class EnemyAttackTriggerAction : TriggerAction
{
    enum HitType 
    {
        Enter,
        Stay,
    }

    [SerializeField]
    private UnityEvent m_damageEvent = null;

    [SerializeField]
    StatusManagerBase m_statusManager = null;

    [SerializeField]
    AnimatorManagerBase m_animatorManager = null;

    [SerializeField]
    DamageData m_damageData = new DamageData();

    [SerializeField]
    HitType m_hitType = HitType.Enter;
    
    [SerializeField]
    AudioManager m_audioManager;

    [Header("ヒット時のパーティクル"), SerializeField]
    List<ParticleManager.ParticleID> m_hitParticleIDs = new List<ParticleManager.ParticleID>();
    //ParticleManager.ParticleID m_hitParticleID;
    
    Collider m_hitCollider;

    private void Awake()
    {
        //m_audio = GetComponent<AudioSource>();
        m_audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        m_hitCollider = GetComponent<Collider>();

        System.Action action = m_hitType switch {
            HitType.Enter => () => AddEnterAction(SendDamage),
            HitType.Stay => () => AddStayAction(SendDamage),
            _ => null
        };

        action?.Invoke();
        //AddEnterAction(SendDamage);
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
            CreateParticles();
            m_audioManager?.PlayOneShot();
            m_animatorManager?.HitStop(damageData);
            damage.TakeDamage(damageData);

            m_damageEvent?.Invoke();
        }
    }

    void CreateParticles()
    {
        foreach (var id in m_hitParticleIDs)
        {
            ParticleManager.Instance?.Play(id, transform.position);
        }
    }

    //アクセッサ・プロパティ--------------------------------------------------------------------------

    public DamageData damageData
    {
        get => m_damageData;
        set
        {
            Debug.Log(value.damageValue);
            if (value.obj == null)
            {
                var obj = m_damageData.obj;
                m_damageData = value;
                m_damageData.obj = obj;
                return;
            }

            m_damageData = value;
        }
    }
}
