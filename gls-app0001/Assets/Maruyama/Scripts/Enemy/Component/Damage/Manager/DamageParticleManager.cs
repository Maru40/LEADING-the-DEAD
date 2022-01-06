using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;
using MaruUtility.UtilityDictionary;

public class DamageParticleManager : MonoBehaviour
{
    [SerializeField]
    private bool m_isParentChase = true;

    [SerializeField]
    private GameObject m_createParticle = null; //生成するparticle
    private GameObject m_particle = null;  //生成されたparticle

    [SerializeField]
    private GameObject m_createPositionObject = null;  //生成する場所を表すオブジェクト

    [SerializeField]
    private float m_time = 2.0f;  //particle表示時間

    [SerializeField]
    private Ex_Dictionary<DamageType, GameObject> m_createParticleDictionary = new Ex_Dictionary<DamageType, GameObject>();

    private void Awake()
    {
        m_createParticleDictionary.InsertInspectorData();
        NullCheck();
    }

    private void Start()
    {

    }

    private void Update()
    {
        ParentChase();
    }

    private void ParentChase()
    {
        //追う状態で、particleがnullでなければ
        if(m_isParentChase == false || m_particle == null) { 
            return;
        }

        var setPosition = m_createPositionObject.transform.position;
        m_particle.transform.position = setPosition;
    }

    private void CreateParticle()
    {
        var createPosition = m_createPositionObject.transform.position;

        var particle = Instantiate(m_createParticle, createPosition, Quaternion.identity);

        m_particle = particle;
    }

    //ダメージ開始---------------------------------------------------------------

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
    public void StartDamage(DamageData data)
    {
        StartDamage(data.type);
    }
    public void StartDamage(float time, DamageData data)
    {
        StartDamage(time, data.type);
    }
    public void StartDamage(DamageType type)
    {
        if (m_createParticleDictionary.ContainsKey(type))
        {
            StartDamage(m_time, m_createParticleDictionary[type]);
        }
    }
    public void StartDamage(float time, DamageType type)
    {
        if (m_createParticleDictionary.ContainsKey(type))
        {
            StartDamage(time, m_createParticleDictionary[type]);
        }
    }
    public void StartDamage(float time, GameObject particle)
    {
        m_time = time;
        SetCreateParticle(particle);
        CreateParticle();
    }

    //---------------------------------------------------------------------------

    //アクセッサ--------------------------------------------------------------

    public void SetCreateParticle(GameObject particle)
    {
        m_createParticle = particle;
    }

    public void AddCreateParticel(DamageType type ,GameObject particle)
    {
        m_createParticleDictionary[type] = particle;
    }

    //NullCheck----------------------------------------------------------------

    private void NullCheck()
    { 
        if (m_createPositionObject == null) {
            m_createPositionObject = gameObject;
        }
    }
}
