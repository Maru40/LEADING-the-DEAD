using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantPlantHead : MonoBehaviour
{
    [SerializeField]
    private AttributeObject.DamageData m_eatenDamageData;

    [SerializeField]
    private Collider m_headTrigger;

    private bool m_isEatable = false;

    private float m_sumEatenWeight = 0.0f;

    public float sumEatenWeight => m_sumEatenWeight;

    public bool isEatable
    {
        set { m_isEatable = value; m_headTrigger.enabled = value; }
        get => m_isEatable;
    }

    public void SumWeightClear()
    {
        m_sumEatenWeight = 0;
    }

    private void Awake()
    {
        isEatable = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!m_isEatable)
        {
            return;
        }

        var eatenObject = other.GetComponent<EatenObject>();

        if (eatenObject)
        {
            m_sumEatenWeight += eatenObject.eatWeight;
            eatenObject.Eaten(m_eatenDamageData);
        }
    }
}
