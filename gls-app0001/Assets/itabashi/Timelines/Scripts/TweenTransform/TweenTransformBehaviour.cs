using UnityEngine;
using UnityEngine.Playables;

namespace Timelines.Playables.TweenTransform
{
    [System.Serializable]
    public class TweenTransformBehaviour : PlayableBehaviour
    {
        [System.Serializable]
        public class AnimationCurve3
        {
            public bool animateX = true;
            public bool animateY = true;
            public bool animateZ = true;

            public AnimationCurve curveX = AnimationCurve.Linear(0, 0, 1, 1);
            public AnimationCurve curveY = AnimationCurve.Linear(0, 0, 1, 1);
            public AnimationCurve curveZ = AnimationCurve.Linear(0, 0, 1, 1);

            public Vector3 GetValueToTime(float time,Vector3 defaultVector)
            {
                return new Vector3(
                    animateX ? curveX.Evaluate(time) : defaultVector.x,
                    animateY ? curveY.Evaluate(time) : defaultVector.y,
                    animateZ ? curveZ.Evaluate(time) : defaultVector.z
                    );
            }
        }

        public bool animatePosition = false;
        public AnimationCurve3 positionCurve;

        public bool animateRotation = false;
        public AnimationCurve3 rotationCurve;

        public bool animateScale = false;
        public AnimationCurve3 scaleCurve;
    }
}