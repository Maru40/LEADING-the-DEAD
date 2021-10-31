using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;

public class DamageParticleManager : MonoBehaviour
{
    [SerializeField]
    bool m_isParentChase = true;

    [SerializeField]
    GameObject m_createParticle = null; //生成するparticle
    GameObject m_particle = null;  //生成されたparticle

    [SerializeField]
    GameObject m_createPositionObject = null;  //生成する場所を表すオブジェクト

    [SerializeField]
    float m_time = 2.0f;  //particle表示時間

    Dictionary<DamageType, GameObject> m_createParticles = new Dictionary<DamageType, GameObject>();

    private void Awake()
    {
        NullCheck();
    }

    private void Start()
    {

    }

    private void Update()
    {
        ParentChase();
    }

    void ParentChase()
    {
        //追う状態で、particleがnullでなければ
        if(m_isParentChase == false || m_particle == null) { 
            return;
        }

        var setPosition = m_createPositionObject.transform.position;
        m_particle.transform.position = setPosition;
    }

    void CreateParticle()
    {
        var createPosition = m_createPositionObject.transform.position;

        var particle = Instantiate(m_createParticle, createPosition, Quaternion.identity);

        m_particle = particle;
    }

    //ダメージ開始-----------
    public void StartDamage()
    {
        StartDamage(m_time, m_createParticle);
    }
    public void StartDamage(float time)
    {
        StartDamage(time, m_createParticle);
    }
    public void StartDamage(GameObject particle)
    {
        StartDamage(m_time, particle);
    }
    public void StartDamage(DamageType type)
    {
        if (m_createParticles.ContainsKey(type))
        {
            StartDamage(m_time, m_createParticles[type]);
        }
    }
    public void StartDamage(float time, DamageType type)
    {
        if (m_createParticles.ContainsKey(type))
        {
            StartDamage(time, m_createParticles[type]);
        }
    }
    public void StartDamage(float time, GameObject particle)
    {
        m_time = time;
        SetCreateParticle(particle);
        CreateParticle();
    }

    //アクセッサ--------------------------------------------------------------

    public void SetCreateParticle(GameObject particle)
    {
        m_createParticle = particle;
    }

    public void SetCreateParticels(DamageType type ,GameObject particle)
    {
        m_createParticles[type] = particle;
    }

    //NullCheck----------------------------------------------------------------

    void NullCheck()
    { 
        if (m_createPositionObject == null) {
            m_createPositionObject = gameObject;
        }
    }
}
