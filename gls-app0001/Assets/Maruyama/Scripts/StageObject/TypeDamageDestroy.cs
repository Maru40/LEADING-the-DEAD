using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;

public class TypeDamageDestroy : MonoBehaviour
{

    [SerializeField]
    private List<DamageType> m_takeDamageTypes = new List<DamageType>();
    public List<DamageType> GetTakeDamageTypes() => new List<DamageType>(m_takeDamageTypes);

    [SerializeField]
    private List<GameObject> m_particles = new List<GameObject>();

    public void Damaged(DamageData data)
    {
        foreach(var type in m_takeDamageTypes)
        {
            if(data.type == type)
            {
                Damage(data);
                break;
            }
        }
    }

    private void Damage(DamageData data)
    {
        CreateParticles();

        Destroy(this.gameObject);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        //Apple = null;
    }

    private void CreateParticles()
    {
        foreach (var particle in m_particles)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
