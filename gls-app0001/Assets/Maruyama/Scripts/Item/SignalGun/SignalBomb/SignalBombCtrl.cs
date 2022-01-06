using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

[Serializable]
public struct SignalBombParametor
{
    public float moveSpeed;
    public Vector3 moveVec;  //移動する向き
    public float explosionTime;  //何秒後に爆発するか

    public SignalBombParametor(float moveSpeed, float explosionTime)
        :this(moveSpeed, Vector3.up, explosionTime)
    { }

    public SignalBombParametor(float moveSpeed, Vector3 moveVec, float explosionTime)
    {
        this.moveSpeed = moveSpeed;
        this.moveVec = moveVec;
        this.explosionTime = explosionTime;
    }
}


[RequireComponent(typeof(WaitTimer))]
public class SignalBombCtrl : MonoBehaviour
{
    [SerializeField]
    private GameObject m_explosionBomb = null;  //爆発する場合のparticle。

    [SerializeField]
    private SignalBombParametor m_param = new SignalBombParametor(2.0f, Vector3.up, 3.0f);

    //コンポーネント

    private WaitTimer m_waitTimer;

    private void Awake()
    {
        m_waitTimer = GetComponent<WaitTimer>();
    }

    private void Start()
    {
        m_waitTimer.AddWaitTimer(GetType(), m_param.explosionTime, Explosion);
    }

    private void Update()
    {
        MoveProcess();
    }

    private void MoveProcess()
    {
        var moveVec = m_param.moveVec.normalized * m_param.moveSpeed * Time.deltaTime;

        transform.position += moveVec;
    }

    /// <summary>
    /// 爆発
    /// </summary>
    void Explosion()
    {
        //爆発particleの生成
        Instantiate(m_explosionBomb, transform.position, Quaternion.identity);

        Destroy(this.gameObject, 0.1f);
    }
}
