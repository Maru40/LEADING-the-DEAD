using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace Utility
{
    public struct Trigger
    {
        private bool m_isValid;

        public bool isValid
        {
            get
            {
                if(m_isValid)
                {
                    m_isValid = false;
                    return true;
                }

                return false;
            }
        }

        public void Valid() { m_isValid = true; }
    }

    public class StateMachine<StateType,TransitionData>
        where StateType : Enum
        where TransitionData : struct
    {
        public interface I_State
        {
            void SetOnStart(Action onStart);
            void SetOnUpdate(Action onUpdate);
            void SetOnExit(Action onExit);

            void AddTransition(StateType nextState, Func<TransitionData, bool> transitionConditionFunc);
        }

        class State : I_State
        {
            readonly struct TransitionConditionData
            {
                public readonly StateType nextState;
                public readonly Func<TransitionData, bool> transitionCondition;

                public TransitionConditionData(StateType nextState, Func<TransitionData, bool> transitionCondition)
                {
                    this.nextState = nextState;
                    this.transitionCondition = transitionCondition;
                }
            }

            public readonly StateType stateType;

            public Action OnStart = null;
            public Action OnUpdate = null;
            public Action OnExit = null;

            private List<TransitionConditionData> m_transitionConditionDatas = new List<TransitionConditionData>();

            public State(StateType stateType)
            {
                this.stateType = stateType;
            }

            public void AddTransition(StateType nextState,Func<TransitionData, bool> transitionConditionFunc)
            {
                m_transitionConditionDatas.Add(new TransitionConditionData(nextState, transitionConditionFunc));
            }

            public bool TryTransition(in TransitionData transitionData, out StateType nextState)
            {
                foreach(var transitionConditionData in m_transitionConditionDatas)
                {
                    if(transitionConditionData.transitionCondition(transitionData))
                    {
                        nextState = transitionConditionData.nextState;
                        return true;
                    }
                }

                nextState = stateType;
                return false;
            }

            public void SetOnStart(Action onStart)
            {
                OnStart = onStart;
            }

            public void SetOnUpdate(Action onUpdate)
            {
                OnUpdate = onUpdate;
            }

            public void SetOnExit(Action onExit)
            {
                OnExit = onExit;
            }
        }

        private StateType m_startStateType = default(StateType);

        private bool m_isEnable = false;

        private StateType m_nowState = default(StateType);

        private bool m_isStart = true;

        public TransitionData transitionData;

        private Dictionary<StateType, State> m_stateDictionary = new Dictionary<StateType, State>();

        private List<IDisposable> m_disposables = new List<IDisposable>();

        public StateMachine()
        {
            m_disposables.Add(Observable.EveryUpdate().Subscribe(_ => Update()));
        }

        public I_State this[StateType stateType]
        {
            get
            {
                if (!m_stateDictionary.ContainsKey(stateType))
                {
                    m_stateDictionary.Add(stateType, new State(stateType));

                    if(m_stateDictionary.Count == 0)
                    {
                        m_nowState = stateType;
                    }
                }

                return m_stateDictionary[stateType];
            }
        }

        private void Update()
        {
            if(!m_isEnable)
            {
                return;
            }

            if(m_stateDictionary.Count == 0)
            {
                return;
            }

            var state = m_stateDictionary[m_nowState];

            if(state == null)
            {
                return;
            }

            if(m_isStart)
            {
                state.OnStart?.Invoke();

                m_isStart = false;
            }

            state.OnUpdate?.Invoke();

            StateType nextStateType;

            if(state.TryTransition(transitionData,out nextStateType))
            {
                var nextState = m_stateDictionary[nextStateType];

                state?.OnExit?.Invoke();

                m_isStart = true;

                m_nowState = nextStateType;
            }
        }

        private IEnumerator StateUpdate()
        {
            while(true)
            {
                Update();

                yield return null;
            }
        }

        public void Start()
        {
            m_isEnable = true;
        }

        public void Pause()
        {
            m_isEnable = false;
        }

        public void UnPause()
        {
            m_isEnable = true;
        }

        public void Finish()
        {
            foreach(var disposable in m_disposables)
            {
                disposable.Dispose();
            }
        }
    }
}