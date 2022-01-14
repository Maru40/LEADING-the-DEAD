using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

/// <summary>
/// 音を出すオブジェクトの管理
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioManager))]
public class SoundObjectManager : MonoBehaviour
{
    [System.Serializable]
    private struct Parametor
    {
        [Header("音の範囲")]
        public float range;
        [Header("音の時間")]
        public float time;
    }

    [SerializeField]
    private Parametor m_param = new Parametor();

    [SerializeField]
    private FoundObject m_foundObject = null;

    private SphereCollider m_collider;
    private AudioManager m_audioManager;

    private GameTimer m_timer = new GameTimer();

    private void Awake()
    {
        m_collider = GetComponent<SphereCollider>();
        m_collider.isTrigger = true;
        //m_collider.enabled = false;
        m_audioManager = GetComponent<AudioManager>();

        NullCheck();
    }

    private void Start()
    {
        m_collider.radius = m_param.range;
    }

    private void Update()
    {
        m_timer.UpdateTimer();
    }

    /// <summary>
    /// 音の再生
    /// </summary>
    public void PlayOneShot()
    {
        m_audioManager.PlayRandomClipOneShot();

        m_collider.enabled = true;  //音検知用のコライダーをOn
        m_timer.ResetTimer(m_param.time, () => m_collider.enabled = false); //指定時間後に音検知用のコライダーをoff
    }

    private void OnTriggerEnter(Collider other)
    {
        if(enabled == false) {
            return;
        }

        var ear = other.GetComponent<EarBase>();
        if(ear)
        {
            ear.Listen(m_foundObject);
        }
    }

    private void NullCheck()
    {
        if (m_foundObject == null)
        {
            m_foundObject = GetComponentInParent<FoundObject>();
        }
    }

    //アクセッサ----------------------------------------------------------------------------------

    public float Range
    {
        get => m_param.range;
        set
        {
            m_param.range = value;
            m_collider.radius = value;
        }
    }

}
