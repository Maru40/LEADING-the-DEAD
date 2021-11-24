using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Replyer
{
    public class ReplyerManager : MonoBehaviour
    {
        private List<ReplyerBase> m_replyerBases = new List<ReplyerBase>();

        private void Awake()
        {
            m_replyerBases = new List<ReplyerBase>(GetComponents<ReplyerBase>());
        }

        void Start()
        {
        }

        public void OnRelpy()
        {
            if(enabled)
            {
                foreach(var replyerBase in m_replyerBases)
                {
                    if(replyerBase.CanReply())
                    {
                        replyerBase.OnReply();
                    }
                }
            }
        }
    }
}