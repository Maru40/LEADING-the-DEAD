using System;
using UnityEngine;

public class SlideUITable
{
    public static readonly BaseLayerTable BaseLayer = new BaseLayerTable();
    public class BaseLayerTable
    {
        public readonly LeftScrollTable LeftScroll = new LeftScrollTable();
        public class LeftScrollTable
        {
            public readonly AnimationState LeftScrollOut = new AnimationState("Base Layer.LeftScroll.LeftScrollOut","Base Layer");
            public readonly AnimationState RightScrollIn = new AnimationState("Base Layer.LeftScroll.RightScrollIn","Base Layer");
            
        }
        public readonly RightScrollTable RightScroll = new RightScrollTable();
        public class RightScrollTable
        {
            public readonly AnimationState RihgtScrollOut = new AnimationState("Base Layer.RightScroll.RihgtScrollOut","Base Layer");
            public readonly AnimationState LeftScrollIn = new AnimationState("Base Layer.RightScroll.LeftScrollIn","Base Layer");
            
        }
        public readonly AnimationState Idle = new AnimationState("Base Layer.Idle","Base Layer");
        
    }
    
}