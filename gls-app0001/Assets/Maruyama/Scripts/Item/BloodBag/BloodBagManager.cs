using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBagManager : MonoBehaviour
{
    public enum StateEnum
    {
        Put,    //置く
        Throw,  //投げる
    }

    [Header("血だまりパーティクル"), SerializeField]
    List<ParticleManager.ParticleID> m_particleIDs = new List<ParticleManager.ParticleID>();

    [Header("自分が消滅したときに出る血だまり"), SerializeField]
    GameObject m_breakBlood = null;

    StateEnum m_state = StateEnum.Put;

    public void ChangeState(StateEnum state)
    {
        m_state = state;
    }

    /// <summary>
    /// 血だまりになる処理
    /// </summary>
    void BreakBlood()
    {
        CreateParticle();

        var position = CalcuBreakBloodPosition();
        Instantiate(m_breakBlood, position, Quaternion.identity);

        Destroy(gameObject);
    }

    Vector3 CalcuBreakBloodPosition()
    {
        RaycastHit hit;

        var layerMask = LayerMask.GetMask("L_Obstacle");
        Physics.Raycast(transform.position, Vector3.down, out hit, layerMask);

        return hit.point;
    }

    /// <summary>
    /// パーティクルの生成
    /// </summary>
    void CreateParticle()
    {
        foreach(var id in m_particleIDs)
        {
            ParticleManager.Instance.Play(id, transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<Player.PlayerStatusManager>();
        if (player)
        {
            return;
        }

        System.Action action = m_state switch
        {
            StateEnum.Put => null,
            StateEnum.Throw => BreakBlood,
            _ => null
        };

        action?.Invoke();
    }
}
