using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拾われる機能を持ったコンポーネント
/// </summary>
public class PickedUpObject : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_pickedUpSound;

    private AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject PickUp()
    {
        if(m_pickedUpSound)
        {
            m_audioSource?.PlayOneShot(m_pickedUpSound);
        }

        return gameObject;
    }
}
