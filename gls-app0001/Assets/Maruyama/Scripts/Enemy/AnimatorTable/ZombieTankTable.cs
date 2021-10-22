using System;
using UnityEngine;

public class ZombieTankTable
{
    public static readonly BaseLayerTable BaseLayer = new BaseLayerTable();
    public class BaseLayerTable
    {
        public readonly AnimationState TackleAttack = new AnimationState("Base Layer.TackleAttack","Base Layer");
        public readonly AnimationState Z_Idle = new AnimationState("Base Layer.Z_Idle","Base Layer");
        public readonly AnimationState Z_Walk1 = new AnimationState("Base Layer.Z_Walk1","Base Layer");
        public readonly AnimationState NormalAttack = new AnimationState("Base Layer.NormalAttack","Base Layer");
        public readonly AnimationState TackleLast = new AnimationState("Base Layer.TackleLast","Base Layer");
        
    }
    
}