using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmellManaer : MonoBehaviour
{
    TargetManager m_targetManager;
    I_Smell m_smell;

    private void Awake()
    {
        m_targetManager = GetComponent<TargetManager>();
        m_smell = GetComponent<I_Smell>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var foundObject = other.GetComponent<FoundObject>();
        if(foundObject)
        {
            if(foundObject.GetFoundData().type == FoundObject.FoundType.Smell)
            {
                m_smell?.SmellFind(foundObject);
            }
        }
    }
}
