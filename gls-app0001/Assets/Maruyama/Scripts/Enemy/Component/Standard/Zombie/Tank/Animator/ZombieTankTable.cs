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
        public readonly AnimationState Drumming = new AnimationState("Base Layer.Drumming","Base Layer");
        public readonly AnimationState Shout = new AnimationState("Base Layer.Shout","Base Layer");
        
    }
    public static readonly UpperLayerTable UpperLayer = new UpperLayerTable();
    public class UpperLayerTable
    {
        public readonly AnimationState TackleCharge = new AnimationState("Upper Layer.TackleCharge","Upper Layer");
        public readonly AnimationState Idle = new AnimationState("Upper Layer.Idle","Upper Layer");
        
    }
    
}