using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        var foundObject = other.gameObject.GetComponentInParentAndChildren<FoundObject>();

        if(!foundObject)
        {
            return;
        }

        Debug.Log($"{foundObject.gameObject.name}");
    }
}
