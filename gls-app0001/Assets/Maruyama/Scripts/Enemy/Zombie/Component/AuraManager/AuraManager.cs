using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager : MonoBehaviour
{
    [System.Serializable]
    public struct AuraParametor
    {
        public Vector3 offsetPosition;
        public GameObject auraPrefab;
    }

    [System.Serializable]
    public struct Parametor 
    {
        public List<AuraParametor> auraParametors;
    }

    [SerializeField]
    private Parametor m_param = new Parametor();
    public Parametor parametor 
    {
        get => m_param;
        set => m_param = value;
    }

    List<GameObject> m_auras = new List<GameObject>();

    private void Start()
    {
        CreateAura();
    }

    /// <summary>
    /// オーラ生成
    /// </summary>
    private void CreateAura()
    {
        foreach (var param in m_param.auraParametors)
        {
            var cretaePosition = transform.position + param.offsetPosition;
            var aura = Instantiate(param.auraPrefab, cretaePosition, Quaternion.identity, transform);

            m_auras.Add(aura);
        }
    }

    /// <summary>
    /// オーラ消去
    /// </summary>
    private void DestroysAura()
    {
        foreach(var aura in m_auras)
        {
            Destroy(aura);
        }
    }
}
