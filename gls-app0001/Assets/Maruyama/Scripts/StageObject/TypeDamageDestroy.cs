using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AttributeObject;

[RequireComponent(typeof(AudioManager))]
public class TypeDamageDestroy : MonoBehaviour
{
    [SerializeField, Header("ダメージタイプ一覧")]
    private List<DamageType> m_takeDamageTypes = new List<DamageType>();
    public List<DamageType> GetTakeDamageTypes() => new List<DamageType>(m_takeDamageTypes);

    [SerializeField, Header("生成するパーティクル一覧")]
    private List<GameObject> m_particles = new List<GameObject>();

    private AudioManager m_audioManager = null;

    private void Awake()
    {
        m_audioManager = GetComponent<AudioManager>();
    }

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
        m_audioManager.PlayRandomClipOneShot(true);
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
