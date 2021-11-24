using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Replyer
{
    public class ReplyRotater : ReplyerBase
    {
        private enum TimeScaleType
        {
            Scaled,
            UnScaled
        }

        private enum WorldSpaceType
        {
            World,
            Local
        }

        private const float ROTATE_DEFAULT_MAX = 360.0f;

        [SerializeField]
        private Transform m_targetTransform;

        [SerializeField, Min(0.02f)]
        private float m_duration = 1.0f;
        public float duration => m_duration;

        [SerializeField]
        private TimeScaleType m_timeScaleType = TimeScaleType.Scaled;

        [SerializeField]
        private WorldSpaceType m_worldSpaceType = WorldSpaceType.World;

        [SerializeField]
        private float m_curveZeroValue = 0.0f;
        public float curveZeroValue => m_curveZeroValue;
        [SerializeField]
        private float m_curveOneValue = ROTATE_DEFAULT_MAX;
        public float curveOneValue => m_curveOneValue;

        [SerializeField]
        private bool m_rotateX = false;
        public bool rotateX => m_rotateX;
        [SerializeField]
        private AnimationCurve m_rotateCurveX = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve rotateCurveX => m_rotateCurveX;

        [SerializeField]
        private bool m_rotateY = false;
        public bool rotateY => m_rotateY;
        [SerializeField]
        private AnimationCurve m_rotateCurveY = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve rotateCurveY => m_rotateCurveY;

        [SerializeField]
        private bool m_rotateZ = false;
        public bool rotateZ => m_rotateZ;
        [SerializeField]
        private AnimationCurve m_rotateCurveZ = AnimationCurve.Linear(0, 0, 1, 1);
        public AnimationCurve rotateCurveZ => m_rotateCurveZ;



        public override void OnReply()
        {
            StartCoroutine(Rotation());
        }

        private IEnumerator Rotation()
        {
            float countTime = 0.0f;

            while(countTime < m_duration)
            {
                countTime += m_timeScaleType switch
                {
                    TimeScaleType.Scaled => Time.deltaTime,
                    TimeScaleType.UnScaled => Time.unscaledDeltaTime,
                    _ => Time.deltaTime
                };

                float normalizedTime = countTime / m_duration;

                Vector3 eulerAngles = GetRotateToTime(countTime / m_duration);

                if(m_worldSpaceType == WorldSpaceType.World)
                {
                    m_targetTransform.rotation = Quaternion.Euler(eulerAngles);
                }

                if(m_worldSpaceType == WorldSpaceType.Local)
                {
                    m_targetTransform.localEulerAngles = eulerAngles;
                }

                yield return null;
            }

            m_targetTransform.localEulerAngles = GetRotateToTime(1.0f);
        }

        public Vector3 GetRotateToTime(float time)
        {
            return new Vector3(
                m_rotateX ? GetCurveToValue(m_rotateCurveX.Evaluate(time)) : 0.0f,
                m_rotateY ? GetCurveToValue(m_rotateCurveY.Evaluate(time)) : 0.0f,
                m_rotateZ ? GetCurveToValue(m_rotateCurveZ.Evaluate(time)) : 0.0f
                );
        }

        private float GetCurveToValue(float curveValue)
        {
            return curveValue * (m_curveOneValue - m_curveZeroValue) + m_curveZeroValue;
        }

    }
}