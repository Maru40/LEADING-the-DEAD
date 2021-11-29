using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Es.InkPainter;

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

    [Header("血だまりブラシ設定"), SerializeField]
    Brush m_brush = null;

    [Header("障害物の対象"), SerializeField]
    List<string> m_layerStrings = new List<string>();

    StateEnum m_state = StateEnum.Put;

    private void Start()
    {
        if(m_layerStrings.Count == 0) {
            m_layerStrings.Add("L_Obstacle");
            m_layerStrings.Add("L_Ground");
        }
    }

    public void ChangeState(StateEnum state)
    {
        m_state = state;
    }

    /// <summary>
    /// 血だまりになる処理
    /// </summary>
    void BreakBlood(Collision other)
    {
        CreateParticle();

        //var bloodPuddleGround = CreateBloodPuddleGround();
        if (IsCreateBloodPuddle(other)) {
            Debug.Log("■　生成");
            CreateBloodPuddleContacts(other);  //当たった場所に血だまりを増やす。
        }

        //CreateBloodInk(other, bloodPuddle);       //当たった場所の血だまり生成
        //CreateBloodInk(hitGround, bloodPuddle);   //地面に血だまり

        Destroy(gameObject);
    }

    bool IsCreateBloodPuddle(Collision other)
    {
        int layerInt = 1 << other.gameObject.layer;

        foreach (var str in m_layerStrings)
        {
            if(layerInt == LayerMask.GetMask(str)) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 地面に例を飛ばして、血だまりを作る。
    /// </summary>
    /// <returns></returns>
    RaycastHit CalcuRaycastHitGround()
    {
        RaycastHit hit;

        var layerMask = LayerMask.GetMask("L_Ground");
        Physics.Raycast(transform.position, Vector3.down, out hit, layerMask);

        return hit;
    }

    /// <summary>
    /// 接した場所の血だまりをくっつける
    /// </summary>
    /// <param name="other">当たったコライダー</param>
    void CreateBloodPuddleContacts(Collision other)
    {
        foreach (var contact in other.contacts)
        {
            //そのままの位置だとオブジェクトに埋まるから、法線方向に少し位置をずらす。
            const float positionAdjustDistance = 0.05f;

            var puddle = Instantiate(m_breakBlood, contact.point, Quaternion.identity);
            puddle.transform.forward = contact.normal;
            puddle.transform.position += contact.normal.normalized * positionAdjustDistance;
            puddle.transform.parent = other.transform;

            puddle.GetComponentInChildren<VerticesAlphaManager>().ChangeAlpha();
        }
    }

    BloodPuddleManager CreateBloodPuddleGround()
    {
        var hitGround = CalcuRaycastHitGround();
        var breakBlood = Instantiate(m_breakBlood, hitGround.point, Quaternion.identity);
        var bloodPuddle = breakBlood.GetComponent<BloodPuddleManager>();
        return bloodPuddle;
    }

    /// <summary>
    /// 血の表示
    /// </summary>
    void CreateBloodInk(Collision other, BloodPuddleManager bloodPuddle)
    {
        foreach (var contact in other.contacts)
        {
            var ink = contact.otherCollider.GetComponent<InkCanvas>();
            if (ink)
            {
                ink.Paint(m_brush, contact.point);
                bloodPuddle.AddInk(ink, m_brush, contact.point);
            }
        }
    }

    /// <summary>
    /// 血の表示
    /// </summary>
    /// <param name="other"></param>
    void CreateBloodInk(RaycastHit hit, BloodPuddleManager bloodPuddle)
    {
        var ink = hit.collider.gameObject.GetComponent<InkCanvas>();
        if (ink)
        {
            ink.Paint(m_brush, hit.point);
            bloodPuddle.AddInk(ink, m_brush, hit.point);
        }
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
        if (player) {
            return;
        }

        System.Action<Collision> action = m_state switch
        {
            StateEnum.Put => null,
            StateEnum.Throw => (Collision collsition) => BreakBlood(collsition),
            _ => null
        };

        action?.Invoke(collision);
    }
}
