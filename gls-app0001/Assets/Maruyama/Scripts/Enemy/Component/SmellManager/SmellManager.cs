using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellManager : MonoBehaviour
{
    [Header("正体に気づく距離"), SerializeField]
    float m_nearRange = 0.5f;

    [Header("近くで待機する時間"),SerializeField]
    float m_nearWaitTime = 1.0f;

    TargetManager m_targetManager;
    I_Smell m_smell;
    WaitTimer m_waitTimer;
    EnemyVelocityMgr m_velocityManager;
    I_Eat m_eat;
    StatorBase m_stator;
    AttackNodeManagerBase m_attackManager;
    WallAttack_ZombieNormal m_wallAttack;

    [Header("攻撃するタグ"), SerializeField]
    List<string> m_attackTags = new List<string>();

    [Header("TriggerStayのインターバルタイム"), SerializeField]
    float m_stayTriggerIntervalTime = 0.1f;
    GameTimer m_timer = new GameTimer();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_smell = GetComponent<I_Smell>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_velocityManager = GetComponent<EnemyVelocityMgr>();
        m_eat = GetComponent<I_Eat>();
        m_stator = GetComponent<StatorBase>();
        m_attackManager = GetComponent<AttackNodeManagerBase>();
        m_wallAttack = GetComponent<WallAttack_ZombieNormal>();

        if(m_attackTags.Count == 0) { 
            m_attackTags.Add("T_Wall"); 
        }
    }

    private void Start()
    {
        m_timer.ResetTimer(m_stayTriggerIntervalTime);
    }

    private void Update()
    {
        m_timer.UpdateTimer();

        if (!m_targetManager.HasTarget()) {
            return;
        }

        if (IsUpdate())
        {
            var meat = m_targetManager.GetNowTarget().GetComponent<MeatManager>();
            if (meat)
            {
                MeatCheck();
                return;
            }

            var bloodPuddle = m_targetManager.GetNowTarget().GetComponent<BloodPuddleManager>();
            if (bloodPuddle)
            {
                BloodPuddleCheck(bloodPuddle);
                return;
            }
        }
    }

    /// <summary>
    /// アップデート処理が必要かどうか
    /// </summary>
    /// <returns></returns>
    bool IsUpdate()
    {
        if (!m_targetManager.HasTarget()) { //ターゲットが無かったら処理をしない。
            return false; 
        }

        var type = m_targetManager.GetNowTargetType();
        //タイプが匂いタイプなら。
        return type == FoundObject.FoundType.Smell ? true : false;
    }

    public bool IsTargetNear(float nearRange)
    {
        var positionCheck = m_targetManager.GetToNowTargetVector();
        if (positionCheck == null) {
            return false;
        }
        var toTargetVec = (Vector3)positionCheck;
        toTargetVec.y = transform.position.y;

        //正体に気づく距離まで来たら。
        return toTargetVec.magnitude < nearRange ? true : false;  
    }

    bool IsAttack()
    {
        //T_Wallでないなら
        var parent = m_targetManager.GetNowTarget().transform.parent;
        if(parent == null) {
            return false;
        }

        foreach(var tag in m_attackTags) {
            //タグが一緒で、攻撃範囲なら
            if(parent.tag == tag && m_wallAttack.IsAttackStartRange())
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 肉のチェック
    /// </summary>
    void MeatCheck()
    {
        if(IsTargetNear(m_nearRange))
        {
            //食べるモーション再生
            m_eat.Eat();
        }
    }

    /// <summary>
    /// 血の液体チェック
    /// </summary>
    void BloodPuddleCheck(BloodPuddleManager blood)
    {
        if (IsAttack()) { //攻撃するなら
            //m_attackManager.AttackStart();
            m_wallAttack.AttackStart();
            
            return;
        }

        if (IsTargetNear(m_nearRange))  //正体に気づく距離まで来たら。
        {
            //Debug.Log("△PulldeCheck");

            enabled = false; //自分自身をoff
            m_velocityManager.StartDeseleration(); //速度のリセット
            //m_velocityManager.enabled = false; //速度を計算しないようにする。

            m_waitTimer.AddWaitTimer(GetType(), m_nearWaitTime, () => {
                m_targetManager.AddExcludeNowTarget();  //ターゲット対象から外す。
                enabled = true;
                m_velocityManager.enabled = true;
                m_velocityManager.ResetAll();
                m_velocityManager.SetIsDeseleration(false);
            });
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckSmellFind(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_timer.IsTimeUp)
        {
            CheckSmellFind(other);
            m_timer.ResetTimer(m_stayTriggerIntervalTime);
        }
    }

    void CheckSmellFind(Collider other)
    {
        var foundObject = other.GetComponent<FoundObject>();
        if (foundObject)
        {
            //対象が匂いタイプだったら
            if (foundObject.GetFoundData().type == FoundObject.FoundType.Smell)
            {
                m_smell?.SmellFind(foundObject);
            }
        }
    }

    //アクセッサ-----------------------------------------------------------------

    public float NearRange => m_nearRange;
}
