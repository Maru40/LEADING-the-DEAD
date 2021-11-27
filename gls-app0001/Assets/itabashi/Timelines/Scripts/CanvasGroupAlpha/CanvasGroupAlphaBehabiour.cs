using UnityEngine;
using UnityEngine.Playables;

namespace Timelines.Playables.CanvasGroupAlpha
{
    [System.Serializable]
    public class CanvasGroupAlphaBehabiour : PlayableBehaviour
    {
        public AnimationCurve alphaCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}