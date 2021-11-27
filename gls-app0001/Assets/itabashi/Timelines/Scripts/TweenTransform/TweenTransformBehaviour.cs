using UnityEngine;
using UnityEngine.Playables;

namespace Timelines.Playables.TweenTransform
{
    [System.Serializable]
    public class TweenTransformBehaviour : PlayableBehaviour
    {
        public bool animatePosition = false;
        public AnimationCurve3 positionCurve = new AnimationCurve3(true, true, true, Vector3.zero);

        public bool animateRotation = false;
        public AnimationCurve3 rotationCurve = new AnimationCurve3(true, true, true, Vector3.zero);

        public bool animateScale = false;
        public AnimationCurve3 scaleCurve = new AnimationCurve3(true, true, true, Vector3.one);
    }
}