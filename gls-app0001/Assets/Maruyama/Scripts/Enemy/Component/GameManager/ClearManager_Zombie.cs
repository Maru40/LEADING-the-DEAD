using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クリア時のゾンビの挙動まとめ
/// </summary>
public class ClearManager_Zombie : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = 5.0f;

    TargetManager m_targetManager;
    EnemyVelocityMgr m_velocityManager;

    [Header("終了時にUpdateをoffにしたいビヘイビア"), SerializeField]
    List<Behaviour> m_enableOffBehaviour = new List<Behaviour>();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();

        enabled = false;
    }

    void Update()
    {
        m_velocityManager.velocity = transform.forward * m_moveSpeed;

        ChangeEnableBehabiours();
    }

    public void ClearProcess()
    {
        enabled = true;

        m_targetManager.SetNowTarget(GetType(), null);
        ChangeEnableBehabiours();
    }

    void ChangeEnableBehabiours(bool enable = false)
    {
        foreach(var behaviour in m_enableOffBehaviour)
        {
            behaviour.enabled = enable;
        }
    }
}
