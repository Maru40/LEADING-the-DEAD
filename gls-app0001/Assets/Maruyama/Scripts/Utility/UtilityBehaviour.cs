using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public struct ChangeBehaviourEnablePair
    {
        public Behaviour behaviour;
        public bool isEnable;

        public ChangeBehaviourEnablePair(Behaviour behaviour, bool isEnable)
        {
            this.behaviour = behaviour;
            this.isEnable = isEnable;
        }
    }

    public class UtilityBehaviour
    {
        public static void ChangeBehaviourEnable(List<Behaviour> behaviours, bool isEnable)
        {
            foreach(var behaviour in behaviours)
            {
                behaviour.enabled = isEnable;
            }
        }

        public static void ChangeBehaviourEnable(List<ChangeBehaviourEnablePair> parametors)
        {
            foreach (var param in parametors)
            {
                param.behaviour.enabled = param.isEnable;
            }
        }
    }
}



