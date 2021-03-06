using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Es.InkPainter;

public class BloodBagManager : MonoBehaviour
{
    private const float PositionAdjustDistance = 0.1f;

    public enum StateEnum
    {
        Put,    //置く
        Throw,  //投げる
    }

    [Header("血だまりパーティクル"), SerializeField]
    private List<ParticleManager.ParticleID> m_particleIDs = new List<ParticleManager.ParticleID>();

    [Header("自分が消滅したときに出る血だまり"), SerializeField]
    private GameObject m_breakBlood = null;

    [Header("血だまりブラシ設定"), SerializeField]
    private Brush m_brush = null;

    [Header("障害物の対象"), SerializeField]
    private List<string> m_layerStrings = new List<string>();

    private StateEnum m_state = StateEnum.Put;

    [Header("割れた時の音"), SerializeField]
    private AudioClip m_breakSoundClip = null;

    [Header("血だまり"), SerializeField]
    private GameObject m_pudBlood = null;

    private NumBloodManager m_numBloodManager = null;

    private void Awake()
    {
        m_numBloodManager = FindObjectOfType<NumBloodManager>();
    }

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
        if (m_numBloodManager == null) {
            m_numBloodManager = FindObjectOfType<NumBloodManager>();
        }

        CreateParticle();

        //var bloodPuddleGround = CreateBloodPuddleGround();
        if (IsCreateBloodPuddle(other)) {
            CreateBloodPuddleContacts(other);  //当たった場所に血だまりを増やす。
        }
        else
        {
            CreateBloodPuddleGround();
        }

        //CreateBloodInk(other, bloodPuddle);       //当たった場所の血だまり生成
        //CreateBloodInk(hitGround, bloodPuddle);   //地面に血だまり

        //割れた音を出す。
        Manager.GameAudioManager.Instance.SEPlayOneShot(m_breakSoundClip);
        m_numBloodManager.RemoveBloodBag(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// 血だまりを作るかどうか
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool IsCreateBloodPuddle(Collision other)
    {
        int layerInt = 1 << other.gameObject.layer;

        foreach (var str in m_layerStrings)  //Layerなら
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
    private RaycastHit CalcuRaycastHitGround()
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
    private void CreateBloodPuddleContacts(Collision other)
    {
        foreach (var contact in other.contacts)
        {
            //そのままの位置だとオブジェクトに埋まるから、法線方向に少し位置をずらす。
            //const float PositionAdjustDistance = 0.05f;
            var puddle = Instantiate(m_breakBlood, contact.point, Quaternion.identity);
            var direct = contact.normal;
            puddle.transform.forward = direct;
            puddle.transform.position = contact.point + direct.normalized * PositionAdjustDistance;
            puddle.transform.parent = other.transform;

            puddle.GetComponentInChildren<VerticesAlphaManager>().ChangeAlpha();
        }
    }

    /// <summary>
    /// 地面に血だまりを作る
    /// </summary>
    /// <returns>作った血だまり</returns>
    private BloodPuddleManager CreateBloodPuddleGround()
    {
        //後でCreateBloodPuddleをまとめる。
        var hitGround = CalcuRaycastHitGround();
        var createPosition = hitGround.point + hitGround.normal * PositionAdjustDistance;
        var breakBlood = Instantiate(m_breakBlood, createPosition, Quaternion.identity);
        //breakBlood.transform.parent = hitGround.collider.gameObject.transform;
        breakBlood.transform.forward = hitGround.normal;
        var bloodPuddle = breakBlood.GetComponent<BloodPuddleManager>();
        return bloodPuddle;
    }

    /// <summary>
    /// 血の表示
    /// </summary>
    private void CreateBloodInk(Collision other, BloodPuddleManager bloodPuddle)
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
    private void CreateBloodInk(RaycastHit hit, BloodPuddleManager bloodPuddle)
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
    private void CreateParticle()
    {
        foreach(var id in m_particleIDs)
        {
            ParticleManager.Instance.Play(id, transform.position);
        }
    }

    /// <summary>
    /// 拾う
    /// </summary>
    public void PickUp()
    {
        m_pudBlood?.SetActive(false);
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
