using System;
using UnityEngine;

public class SelectStageLabelTable
{
    public static readonly BaseLayerTable BaseLayer = new BaseLayerTable();
    public class BaseLayerTable
    {
        public readonly AnimationState Idle = new AnimationState("Base Layer.Idle","Base Layer");
        public readonly AnimationState FadeOut = new AnimationState("Base Layer.FadeOut","Base Layer");
        public readonly AnimationState FadeIn = new AnimationState("Base Layer.FadeIn","Base Layer");
        
    }
    
}