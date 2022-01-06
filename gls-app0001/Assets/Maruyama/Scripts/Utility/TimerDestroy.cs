using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDestroy : MonoBehaviour
{
    [SerializeField]
    private float m_time = 1.0f;

    private void Start()
    {
        Destroy(gameObject, m_time);
    }
}
