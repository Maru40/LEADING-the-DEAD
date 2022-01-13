using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerAreaManager : MonoBehaviour
{
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(enabled == false) {
            return;
        }

        var evasion = other.GetComponent<I_DangerEvasion>();
        evasion?.Evasion(gameObject);
    }
}
