using System;
using UnityEngine;

public class SelectGroupUIsTable
{
    public static readonly BaseLayerTable BaseLayer = new BaseLayerTable();
    public class BaseLayerTable
    {
        public readonly AnimationState Fadein = new AnimationState("Base Layer.Fadein","Base Layer");
        public readonly AnimationState NewState = new AnimationState("Base Layer.NewState","Base Layer");
        public readonly AnimationState Fadeout = new AnimationState("Base Layer.Fadeout","Base Layer");
        
    }
    
}