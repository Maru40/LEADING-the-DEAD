using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumControlZombieManager : MonoBehaviour
{
    [System.Serializable]
    public struct Parametor
    {
        public float numClear;  //クリアに必要な人数
        public GameObject createPrefab;  //生成するプレハブ
        public GameObject createPositionObject; //生成したい場所を指す空のオブジェクト
        public Vector3 CreatePosition => createPositionObject.transform.position;
    }

    [SerializeField]
    private AllEnemyGeneratorManager m_allGeneratorManager = null;

    [SerializeField]
    private Parametor m_param = new Parametor();

    private GameObject m_effect = null;

    private void Awake()
    {
        if(m_allGeneratorManager == null)
        {
            m_allGeneratorManager = FindObjectOfType<AllEnemyGeneratorManager>();
        }
    }

    private void Start()
    {
        CreateEffect();
    }

    private void Update()
    {
        if(m_allGeneratorManager.GetNumFindPlayerZombie() > m_param.numClear)
        {
            m_effect.SetActive(true);
        }
        else
        {
            m_effect.SetActive(false);
        }
    }

    private void CreateEffect()
    {
        var effect = Instantiate(m_param.createPrefab, m_param.CreatePosition, Quaternion.identity, transform);

        m_effect = effect;
        m_effect.SetActive(false);
    }
}
