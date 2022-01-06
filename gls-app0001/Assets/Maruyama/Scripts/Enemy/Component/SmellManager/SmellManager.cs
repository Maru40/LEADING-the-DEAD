using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class SmellManager : MonoBehaviour
{
    [Header("正体に気づく距離"), SerializeField]
    private float m_nearRange = 0.5f;

    [Header("近くで待機する時間"),SerializeField]
    private float m_nearWaitTime = 1.0f;

    private TargetManager m_targetManager;
    private I_Smell m_smell;
    private WaitTimer m_waitTimer;
    private EnemyVelocityManager m_velocityManager;
    private I_Eat m_eat;
    private Stator_ZombieNormal m_stator;
    private AttackNodeManagerBase m_attackManager;
    private WallAttack_ZombieNormal m_wallAttack;

    [Header("攻撃するタグ"), SerializeField]
    private List<string> m_attackTags = new List<string>();

    [Header("TriggerStayのインターバルタイム"), SerializeField]
    private float m_stayTriggerIntervalTime = 0.1f;
    private GameTimer m_timer = new GameTimer();

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_smell = GetComponent<I_Smell>();
        m_waitTimer = GetComponent<WaitTimer>();
        m_velocityManager = GetComponent<EnemyVelocityManager>();
        m_eat = GetComponent<I_Eat>();
        m_stator = GetComponent<Stator_ZombieNormal>();
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
    private bool IsUpdate()
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

    private bool IsAttack()
    {
        if (!m_targetManager.HasTarget()) {
            return false;
        }

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
    private void MeatCheck()
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
    private void BloodPuddleCheck(BloodPuddleManager blood)
    {
        if (IsAttack()) { //攻撃するなら
            //m_attackManager.AttackStart();
            m_wallAttack.AttackStart();
            
            return;
        }

        if (IsTargetNear(m_nearRange))  //正体に気づく距離まで来たら。
        {
            enabled = false; //自分自身をoff
            m_velocityManager.StartDeseleration(); //速度のリセット

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
        if (enabled == false) {
            return;
        }

        CheckSmellFind(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (enabled == false) {
            return;
        }

        if (m_timer.IsTimeUp)
        {
            CheckSmellFind(other);
            m_timer.ResetTimer(m_stayTriggerIntervalTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(enabled == false || IsAttack() || !IsUpdate()) {
            return;
        }

        if (m_stator?.GetNowStateType() == ZombieNormalState.Attack) {
            return;
        }

        if (Obstacle.IsObstacle(collision.gameObject))
        {
            m_targetManager.AddExcludeNowTarget();  //ターゲット対象から外す。
        }
    }

    private void CheckSmellFind(Collider other)
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
