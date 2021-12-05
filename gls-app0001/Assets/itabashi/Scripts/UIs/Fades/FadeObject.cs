using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FadeObject : MonoBehaviour
{
    public abstract void FadeStart();

    public abstract bool IsFinish();
}
