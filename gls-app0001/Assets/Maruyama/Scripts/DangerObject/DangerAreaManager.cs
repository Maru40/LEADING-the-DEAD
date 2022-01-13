using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerAreaManager : MonoBehaviour
{
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(enabled == false) {
            return;
        }

        var evasion = other.GetComponent<I_DangerEvasion>();
        evasion?.Evasion(gameObject);
    }
}
