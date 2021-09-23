using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ターゲットの範囲外にリスポーンする処理
/// </summary>
public class EnemyRespawnManager : EnemyRespawnBase
{
    //[SerializeField]
    //GameObject m_target = null;  //現在使っていない。

    [SerializeField]
    EnemyGenerator m_generator = null;

    StatorBase m_stator;

    void Start()
    {
        //StartTargetNullCheck();
        StartGeneratorNullCheck();

        m_stator = GetComponent<StatorBase>();
    }

    public void Respawn()
    {
        //if(m_target == null || m_generator == null) {
        if (m_generator == null) { 
            //StartTargetNullCheck();
            StartGeneratorNullCheck();
        }

        var respawnPosition = CalcuRespawnRandomPosition();
        transform.position = respawnPosition;

        m_stator.Reset();  //ステートのリセット
    }

    /// <summary>
    /// リスポーンする場所の計算
    /// </summary>
    /// <returns>リスポーンする場所</returns>
    Vector3 CalcuRespawnRandomPosition()
    {
        return m_generator.CalcuRandomPosition();
    }


    //StartNullCheck----------------------------------------------------

    void StartTargetNullCheck()
    {
        //if(m_target == null) {
        //    m_target = GameObject.Find("Player");
        //}
    }

    void StartGeneratorNullCheck()
    {
        if(m_generator != null) { //null出ないなら処理をしない
            return;
        }

        var generators = GameObject.FindObjectsOfType<EnemyGenerator>();

        foreach (var generator in generators)
        {
            if(generator.IsEqualCreateObject(gameObject))
            {
                m_generator = generator;
                return;
            }
        }
    }
}
