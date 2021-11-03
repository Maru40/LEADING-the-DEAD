using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UniRx;

public class TimeEventStateMachineBehaviour : StateMachineBehaviour
{
    private Subject<Unit> m_onStateEnterSubject = new Subject<Unit>();

    public System.IObservable<Unit> onStateEntered => m_onStateEnterSubject;

    private Subject<Unit> m_onStateExitSubject = new Subject<Unit>();

    public System.IObservable<Unit> onStateExited => m_onStateExitSubject;

    private Subject<float> m_onTimeSubject = new Subject<float>();

    private bool m_isInit = false;

    public System.IObservable<UniRxIObservableExtension.BeforeAfterData<float>> onTimeEvent => m_onTimeSubject.BeforeAfter();

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_onStateEnterSubject.OnNext(Unit.Default);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_onStateExitSubject.OnNext(Unit.Default);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!m_isInit)
        {
            m_onTimeSubject.OnNext(-1.0f);
            m_isInit = true;
        }

        if(float.IsInfinity(stateInfo.length))
        {
            return;
        }

        m_onTimeSubject.OnNext(stateInfo.normalizedTime % 1.0f * stateInfo.length);

    }

}
