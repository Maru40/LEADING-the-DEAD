using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{

    public class PlayerAnimatorManager : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private PlayerStatusManager m_statusManager;

        private StateMachineBehaviourTable<TimeEventStateMachineBehaviour> m_behaviourTable;

        public StateMachineBehaviourTable<TimeEventStateMachineBehaviour> behaviourTable
        {
            get
            {
                if(m_behaviourTable == null)
                {
                    m_behaviourTable = new StateMachineBehaviourTable<TimeEventStateMachineBehaviour>(m_animator);
                }

                return m_behaviourTable;
            }
        }

        private FloatReactiveProperty m_moveSpeed = new FloatReactiveProperty(0);

        public float moveSpeed { set => m_moveSpeed.Value = value; get => m_moveSpeed.Value; }

        private BoolReactiveProperty m_isUseActionMoving = new BoolReactiveProperty(false);

        public bool isUseActionMoving { set => m_isUseActionMoving.Value = value; get => m_isUseActionMoving.Value; }

        public System.IObservable<bool> OnIsUseActionMovingChanged => m_isUseActionMoving;

        private void Awake()
        {
            if (m_behaviourTable == null)
            {
                m_behaviourTable = new StateMachineBehaviourTable<TimeEventStateMachineBehaviour>(m_animator);
            }
            
            SettingBaseLayer();

            SettingUpperLayer();

            SettingParamator();
        }

        private void SettingBaseLayer()
        {
            int layerIndex = m_animator.GetLayerIndex("Base Layer");

            m_statusManager.OnIsStanChanged.Where(isStun => isStun)
                .Subscribe(_ => GoState("Stun", layerIndex))
                .AddTo(this);

            m_statusManager.OnIsStanChanged.Where(isStun => !isStun)
                .Subscribe(_ => GoState("Idle", layerIndex))
                .AddTo(this);
        }

        private void SettingUpperLayer()
        {
            int layerIndex = m_animator.GetLayerIndex("Upper_Layer");

            m_statusManager.OnIsStanChanged.Where(isStun => isStun)
                .Subscribe(_ => GoState("Idle", layerIndex))
                .AddTo(this);
        }

        private void SettingParamator()
        {
            m_moveSpeed.Subscribe(moveInput => m_animator.SetFloat("moveInput", moveInput)).AddTo(this);
        }

        public void GoState(string stateName, string layerName, float transitionTime = 0.25f)
        {
            GoState(stateName, m_animator.GetLayerIndex(layerName), transitionTime);
        }

        public void GoState(string stateName,int layerIndex,float transitionTime = 0.25f)
        {
            m_animator.CrossFade(stateName, transitionTime, layerIndex);
        }
    }
}
