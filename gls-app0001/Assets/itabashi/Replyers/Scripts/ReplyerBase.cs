using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Replyer
{
    [RequireComponent(typeof(ReplyerManager))]

    public abstract class ReplyerBase : MonoBehaviour
    {
        [SerializeField, Range(0.0f, 100.0f)]
        private float m_chance = 100.0f;


        public bool CanReply()
        {
            float rate = Random.Range(0.0f, 100.0f);

            if(rate == 0.0f)
            {
                return false;
            }

            return rate <= m_chance;
        }

        public abstract void OnReply();
    }
}