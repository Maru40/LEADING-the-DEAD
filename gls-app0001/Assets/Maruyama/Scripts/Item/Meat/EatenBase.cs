using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//食べる力
[System.Serializable]
public struct EatParametor
{
    public float power;

    public EatParametor(float power = 1.0f)
    {
        this.power = power;
    }
}

//食べられるベース
public abstract class EatenBase : MonoBehaviour
{

    public abstract void Eaten(float power = 1.0f);

}
