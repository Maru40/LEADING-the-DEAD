using UnityEngine;

namespace Timelines.Playables
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

        public AnimationCurve3() :
            this(true, true, true)
        {
        }

        public AnimationCurve3(bool animateX, bool animateY, bool animateZ) :
            this(animateX, animateY, animateZ, Vector3.zero)
        {
        }

        public AnimationCurve3(bool animateX, bool animateY, bool animateZ, Vector3 initVector)
        {
            this.animateX = animateX;
            this.animateY = animateY;
            this.animateZ = animateZ;
            curveX = AnimationCurve.Constant(0, 1, initVector.x);
            curveY = AnimationCurve.Constant(0, 1, initVector.y);
            curveZ = AnimationCurve.Constant(0, 1, initVector.z);
        }

        public Vector3 GetValueToTime(float time, Vector3 defaultVector)
        {
            return new Vector3(
                animateX ? curveX.Evaluate(time) : defaultVector.x,
                animateY ? curveY.Evaluate(time) : defaultVector.y,
                animateZ ? curveZ.Evaluate(time) : defaultVector.z
                );
        }
    }
}