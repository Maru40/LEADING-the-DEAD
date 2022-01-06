using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagDamage : MonoBehaviour
{
    [SerializeField]
    private List<string> m_tags = new List<string>();

    [SerializeField]
    private GameObject m_particle = null;

    public void Damaged(AttributeObject.DamageData data)
    {
        foreach(var tag in m_tags){
            if (data.FindTag(tag))
            {
                DamageProcess();
                break;
            }
        }
    }

    protected virtual void DamageProcess()
    {
        CreateParticle();

        Destroy(gameObject, 0.1f);
    }

    protected virtual void CreateParticle()
    {
        if (m_particle) {
            Instantiate(m_particle, transform.position, Quaternion.identity);
        }
    }
}
