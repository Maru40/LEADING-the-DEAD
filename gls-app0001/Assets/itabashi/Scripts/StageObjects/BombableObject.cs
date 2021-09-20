using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombableObject : MonoBehaviour
{
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    private AudioClip m_explosionSound;

    /// <summary>
    /// �����G�t�F�N�g
    /// </summary>
    [SerializeField]
    private ParticleSystem m_explosionEfectPrefab;

    private AudioSource m_audioSource;

    private void Start()
    {
        if(m_explosionSound)
        {
            m_audioSource = GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// �ĂԂƔ������ď��ł���
    /// </summary>
    public void Explosion()
    {
        if(m_audioSource && m_explosionSound)
        {
            m_audioSource.PlayOneShot(m_explosionSound);
        }

        if(m_explosionEfectPrefab)
        {
            var particleSystem = Instantiate(m_explosionEfectPrefab);

            particleSystem.transform.position = transform.position;

            particleSystem.Play();
        }

        Debug.Log($"{gameObject.name}���������܂���");

        Destroy(gameObject);
    }
}
