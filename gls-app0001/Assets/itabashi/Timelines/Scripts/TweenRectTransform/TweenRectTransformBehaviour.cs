using UnityEngine;
using UnityEngine.Playables;

namespace Timelines.Playables.TweenRectTransform
{
    [System.Serializable]
    public class TweenRectTransformBehaviour : PlayableBehaviour
    {
        public bool animatePosition = false;
        public bool animateRotation = false;
        public bool animateScale = false;

        public AnimationCurve3 positionCurve = new AnimationCurve3(true, true, false);
        public AnimationCurve3 rotationCurve = new AnimationCurve3();
        public AnimationCurve3 scaleCurve = new AnimationCurve3(true, true, false);
    }
}