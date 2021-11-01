using System;
using UnityEngine;

public class ZombieNormalTable
{
    public static readonly BaseLayerTable BaseLayer = new BaseLayerTable();
    public class BaseLayerTable
    {
        public readonly WalkTable Walk = new WalkTable();
        public class WalkTable
        {
            public readonly AnimationState Z_Walk = new AnimationState("Base Layer.Walk.Z_Walk","Base Layer");
            public readonly AnimationState Z_Run = new AnimationState("Base Layer.Walk.Z_Run","Base Layer");
            
        }
        public readonly AnimationState Idle = new AnimationState("Base Layer.Idle","Base Layer");
        public readonly AnimationState NormalAttack = new AnimationState("Base Layer.NormalAttack","Base Layer");
        public readonly AnimationState Stunned = new AnimationState("Base Layer.Stunned","Base Layer");
        public readonly AnimationState Anger = new AnimationState("Base Layer.Anger","Base Layer");
        public readonly AnimationState Land = new AnimationState("Base Layer.Land","Base Layer");
        public readonly AnimationState Death = new AnimationState("Base Layer.Death","Base Layer");
        
    }
    
}