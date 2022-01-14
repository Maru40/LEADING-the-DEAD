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
    private float m_moveSpeed = 5.0f;

    private TargetManager m_targetManager;
    private EnemyVelocityManager m_velocityManager;
    private EnemyRotationCtrl m_rotationController;
    private ThrongManager m_throngManager;

    private BarricadeDurability m_barricade = null;

    [Header("終了時にUpdateをoffにしたいビヘイビア"), SerializeField]
    private List<Behaviour> m_enableOffBehaviour = new List<Behaviour>();

    [Header("終了時に突っ込みながら出す音"), SerializeField]
    private AudioManager m_audioManager = null;
    [Header("音をランダムなタイミングで出す"), SerializeField]
    private RandomRange m_audioPlayRange = new RandomRange(0.0f, 0.5f);

    [Header("扉を開けた時にランダムに生成する位置"), SerializeField]
    private RandomRange m_positionRandomRange = new RandomRange(-1.0f, +1.0f);
    [Header(""), SerializeField]
    private RandomRange m_directRandomRange = new RandomRange(-30.0f, 30.0f);

    private AnimatorManager_ZombieNormal m_animatorManager = null;
    private WaitTimer m_waitTimer = null;

    private Vector3 m_moveDirect = Vector3.zero;

    private Stator_ZombieNormal m_stator;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_velocityManager = GetComponent<EnemyVelocityManager>();
        m_throngManager = GetComponent<ThrongManager>();

        m_animatorManager = GetComponent<AnimatorManager_ZombieNormal>();
        m_rotationController = GetComponent<EnemyRotationCtrl>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_stator = GetComponent<Stator_ZombieNormal>();

        m_barricade = FindObjectOfType<BarricadeDurability>();

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
        //m_stator.SetIsTransitionLock(true);

        m_targetManager.SetNowTarget(GetType(), null);
        ChangeEnableBehabiours(); //使わないBehaviourをfalseにする。

        CrossFadeAnimation(); //アニメーションの遷移

        SettingPosition();   //場所の移動
        SettingMoveDirect(); //移動方向の設定

        //欲しいコンポーネントのOn
        m_rotationController.enabled = true;
        m_velocityManager.enabled = true;
        m_throngManager.enabled = true;

        PlayShoutSE(); //声の再生
    }

    void ChangeEnableBehabiours(bool enable = false)
    {
        foreach(var behaviour in m_enableOffBehaviour)
        {
            behaviour.enabled = enable;
        }
    }

    private void CrossFadeAnimation()
    {
        m_animatorManager.CrossFadeIdleAnimation();
        m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.UpperLayerIndex);
        m_animatorManager.CrossFadeIdleAnimation(m_animatorManager.AllLayerIndex);
    }

    private void SettingPosition()
    {
        //扉に平行に合わせて、ランダムに生成。
        var movePosition = m_barricade.transform.position + m_barricade.transform.up;
        movePosition += m_barricade.transform.right * m_positionRandomRange.RandomValue;

        transform.position = movePosition;
    }

    private void SettingMoveDirect()
    {
        m_moveDirect = transform.forward;
        if (m_barricade)
        {  //バリケードの方向に向かって走り出す。
            //m_moveDirect = m_barricade.transform.position - transform.position;
            m_moveDirect = -m_barricade.transform.up;
            m_moveDirect = Quaternion.Euler(0.0f, m_directRandomRange.RandomValue, 0.0f) * m_moveDirect;
        }

        transform.forward = m_moveDirect;
        m_rotationController.SetDirect(m_moveDirect);
    }

    private void PlayShoutSE()
    {
        var time = m_audioPlayRange.RandomValue;
        m_waitTimer.AddWaitTimer(GetType(), time, () => m_audioManager?.PlayRandomClipOneShot());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enabled == false) {
            return;
        }
        
        //Reflection(collision);  //反射ベクトルに直す。
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
