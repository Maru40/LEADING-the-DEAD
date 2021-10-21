using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimationState
{
    readonly public string stateFullPath;

    readonly public string layerName;

    public AnimationState(string stateFullPath,string layerName)
    {
        this.stateFullPath = stateFullPath;
        this.layerName = layerName;
    }

    public T GetBehaviour<T>(Animator animator) where T : StateMachineBehaviour
    {
        var behaviours = animator.GetBehaviours(Animator.StringToHash(stateFullPath), animator.GetLayerIndex(layerName));

        foreach (var behaviour in behaviours)
        {
            T t = behaviour as T;
            if (t)
            {
                return t;
            }
        }

        return null;
    }
}
