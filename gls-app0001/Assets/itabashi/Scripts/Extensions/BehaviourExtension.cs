using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BehaviourExtension
{
    public static bool IsValid(this Behaviour behaviour)
    {
        return behaviour && behaviour.enabled;
    }
}
