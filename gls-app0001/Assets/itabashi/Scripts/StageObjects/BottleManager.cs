using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_crashInstantiateObject;

    [SerializeField]
    private bool m_isCrashable = false;

    public bool isCrashable { set => m_isCrashable = value; get => m_isCrashable; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!m_isCrashable)
        {
            return;
        }

        Debug.Log("壊れた");

        if (m_crashInstantiateObject)
        {
            Instantiate(m_crashInstantiateObject, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);

    }
}
