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
        public readonly AnimationState Stunned = new AnimationState("Base Layer.Stunned","Base Layer");
        public readonly AnimationState Anger = new AnimationState("Base Layer.Anger","Base Layer");
        public readonly AnimationState Land = new AnimationState("Base Layer.Land","Base Layer");
        public readonly AnimationState Death = new AnimationState("Base Layer.Death","Base Layer");
        public readonly AnimationState KnockBack = new AnimationState("Base Layer.KnockBack","Base Layer");
        public readonly AnimationState Eat = new AnimationState("Base Layer.Eat","Base Layer");
        
    }
    public static readonly UpperLayerTable UpperLayer = new UpperLayerTable();
    public class UpperLayerTable
    {
        public readonly AnimationState Idle = new AnimationState("Upper Layer.Idle","Upper Layer");
        public readonly AnimationState NormalAttack = new AnimationState("Upper Layer.NormalAttack","Upper Layer");
        public readonly AnimationState KnockBack = new AnimationState("Upper Layer.KnockBack","Upper Layer");
        public readonly AnimationState Stunned = new AnimationState("Upper Layer.Stunned","Upper Layer");
        public readonly AnimationState PreliminaryNormalAttack = new AnimationState("Upper Layer.PreliminaryNormalAttack","Upper Layer");
        public readonly AnimationState Eat = new AnimationState("Upper Layer.Eat","Upper Layer");
        public readonly AnimationState DashAttackWalk = new AnimationState("Upper Layer.DashAttackWalk","Upper Layer");
        public readonly AnimationState DashAttack = new AnimationState("Upper Layer.DashAttack","Upper Layer");
        public readonly AnimationState WallAttack = new AnimationState("Upper Layer.WallAttack","Upper Layer");
        public readonly AnimationState PutWallAttack = new AnimationState("Upper Layer.PutWallAttack","Upper Layer");
        
    }
    public static readonly LowerLayerTable LowerLayer = new LowerLayerTable();
    public class LowerLayerTable
    {
        public readonly AnimationState Idle = new AnimationState("Lower Layer.Idle","Lower Layer");
        
    }
    
}