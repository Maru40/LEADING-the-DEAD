using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

/// <summary>
/// クリア時のゾンビの挙動まとめ
/// </summary>
public class ClearManager_Zombie : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = 5.0f;

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;
    EnemyRotationCtrl m_rotationController;

    [Header("終了時にUpdateをoffにしたいビヘイビア"), SerializeField]
    List<Behaviour> m_enableOffBehaviour = new List<Behaviour>();

    [Header("終了時に突っ込みながら出す音"), SerializeField]
    AudioManager m_audioManager = null;
    [Header("音をランダムなタイミングで出す"), SerializeField]
    RandomRange m_audioPlayRange = new RandomRange(0.0f, 0.5f);

    AnimatorManager_ZombieNormal m_animatorManager = null;
    WaitTimer m_waitTimer = null;

    Vector3 m_moveDirect = Vector3.zero;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();

        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
        m_rotationController = GetComponent<EnemyRotationCtrl>();
        m_waitTimer = GetComponent<WaitTimer>();

        enabled = false;
    }

    void Update()
    {
        m_velocityManager.velocity = m_moveDirect.normalized * m_moveSpeed;
        m_rotationController.SetDirect(m_moveDirect);

        ChangeEnableBehabiours();
    }

    public void ClearProcess()
    {
        enabled = true;

        m_targetManager.SetNowTarget(GetType(), null);
        ChangeEnableBehabiours();

        m_animatorManager.CrossFadeIdleAnimation();
        m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.UpperLayerIndex);
        m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.AllLayerIndex);

        m_moveDirect = transform.forward;
        m_rotationController.SetDirect(m_moveDirect);

        m_rotationController.enabled = true;
        m_velocityManager.enabled = true;

        var time = m_audioPlayRange.RandomValue;
        m_waitTimer.AddWaitTimer(GetType(), time, () => m_audioManager?.PlayOneShot());
        //m_audioManager?.PlayOneShot();
    }

    void ChangeEnableBehabiours(bool enable = false)
    {
        foreach(var behaviour in m_enableOffBehaviour)
        {
            behaviour.enabled = enable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enabled == false) {
            return;
        }
        
        Reflection(collision);  //反射ベクトルに直す。
    }

    //反射ベクトルに直す。
    private void Reflection(Collision collision)
    {
        m_moveDirect = CalcuVelocity.Reflection(m_moveDirect, collision);

        //float newDot = Mathf.Abs(Vector3.Dot(m_moveDirect, transform.forward));
        //Vector3 moveDirect = m_moveDirect + 2.0f * transform.forward * newDot;
        //m_moveDirect = moveDirect;
    }
}
