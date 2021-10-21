using System;
using UnityEngine;

public class PlayerMotionsTable
{
    public static readonly BaseLayerTable BaseLayer = new BaseLayerTable();
    public class BaseLayerTable
    {
        public readonly MoveTable Move = new MoveTable();
        public class MoveTable
        {
            public readonly AnimationState Walk = new AnimationState("Base Layer.Move.Walk","Base Layer");
            public readonly AnimationState Dash = new AnimationState("Base Layer.Move.Dash","Base Layer");
            
        }
        public readonly AnimationState Idle = new AnimationState("Base Layer.Idle","Base Layer");
        public readonly AnimationState Stun = new AnimationState("Base Layer.Stun","Base Layer");
        public readonly AnimationState Dead = new AnimationState("Base Layer.Dead","Base Layer");
        
    }
    public static readonly Upper_LayerTable Upper_Layer = new Upper_LayerTable();
    public class Upper_LayerTable
    {
        public readonly ThrowTable Throw = new ThrowTable();
        public class ThrowTable
        {
            public readonly AnimationState ThrowingStance = new AnimationState("Upper_Layer.Throw.ThrowingStance","Upper_Layer");
            public readonly AnimationState Throwing = new AnimationState("Upper_Layer.Throw.Throwing","Upper_Layer");
            
        }
        public readonly SwingTable Swing = new SwingTable();
        public class SwingTable
        {
            public readonly AnimationState Swing = new AnimationState("Upper_Layer.Swing.Swing","Upper_Layer");
            
        }
        public readonly AnimationState Idle = new AnimationState("Upper_Layer.Idle","Upper_Layer");
        public readonly AnimationState Shot = new AnimationState("Upper_Layer.Shot","Upper_Layer");
        
    }
    
}