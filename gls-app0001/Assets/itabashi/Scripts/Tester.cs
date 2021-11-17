using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
abstract class Base
{

}

[System.Serializable]
class A : Base
{
    public int a;
}

public class Tester : MonoBehaviour
{
    [SerializeReference]
    private Base a;
    private void Awake()
    {

    }
}
