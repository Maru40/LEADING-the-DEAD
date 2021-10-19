using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGenerator : GeneratorBase
{
    [SerializeField]
    GameObject m_createObject = null;

    [SerializeField]
    List<GameObject> m_positionObjects = new List<GameObject>();

    private void Start()
    {
        CreateObjects();
    }
    
    void CreateObjects()
    {
        foreach(var obj in m_positionObjects)
        {
            var newObject = Instantiate(m_createObject, obj.transform.position, Quaternion.identity);
            newObject.gameObject.SetActive(true);
            newObject.transform.SetParent(this.transform);
        }
    }
}
