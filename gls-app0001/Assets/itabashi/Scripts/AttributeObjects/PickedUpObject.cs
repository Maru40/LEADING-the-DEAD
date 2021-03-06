using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 拾われる機能を持ったコンポーネント
/// </summary>
public class PickedUpObject : MonoBehaviour
{
    public enum PickedUpType
    {
        Touch,
        Decision
    }

    [SerializeField]
    private string m_pickedUpObjectName = "PickedUpObject";

    [SerializeField]
    private PickedUpType m_pickedUpType = PickedUpType.Touch;

    public PickedUpType pickedUpType => m_pickedUpType;

    [SerializeField]
    private AudioClip m_pickedUpSound;

    private AudioSource m_audioSource;

    public string pickedUpObjectName => m_pickedUpObjectName;

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
