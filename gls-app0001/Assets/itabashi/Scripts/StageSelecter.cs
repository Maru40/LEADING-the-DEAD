using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Manager;

public class StageSelecter : MonoBehaviour
{
    [SerializeField]
    private FocusChangeToPush m_focusChangeToPush;

    [SerializeField]
    private UISounder m_uiSounder;

    private readonly Subject<Unit> m_onSelectIndexDecrementSubject = new Subject<Unit>();

    public System.IObservable<Unit> OnSelectIndexDecrement => m_onSelectIndexDecrementSubject;

    private readonly Subject<Unit> m_onSelectIndexIncrementSubject = new Subject<Unit>();

    public System.IObservable<Unit> OnSelectIndexIncrement => m_onSelectIndexIncrementSubject;

    public readonly Subject<Unit> m_OnChangedIsTutorialSubject = new Subject<Unit>();

    public System.IObservable<Unit> OnChangedIsTutorial => m_OnChangedIsTutorialSubject;

    private void Awake()
    {
        OnSelectIndexDecrement.Subscribe(_ => m_uiSounder.SelectPlay())
            .AddTo(this);

        OnSelectIndexIncrement.Subscribe(_ => m_uiSounder.SelectPlay())
            .AddTo(this);
    }

    public void SelectLeft()
    {

        if(!GameStageManager.Instance.CanDecrement())
        {
            return;
        }

        GameStageManager.Instance.Decrement();

        m_onSelectIndexDecrementSubject.OnNext(Unit.Default);
    }

    public void SelectRight()
    {
        if(!GameStageManager.Instance.CanIncrement())
        {
            return;
        }

        GameStageManager.Instance.Increment();

        m_onSelectIndexIncrementSubject.OnNext(Unit.Default);
    }
    public void ChangeTutorial()
    {

        if(!GameStageManager.Instance.CanChangeTutorial())
        {
            return;
        }

        GameStageManager.Instance.ChangeTutorial();
        m_OnChangedIsTutorialSubject.OnNext(Unit.Default);
    }

    public void StageIsSelect()
    {
        if(!GameStageManager.Instance.isSelectStage)
        {
            return;
        }

        m_uiSounder.SubmitPlay();
        m_focusChangeToPush.NextPushFocus();
    }

    public void LoadScene()
    {
        GameSceneManager.Instance.LoadScene(GameStageManager.Instance.currentStageData.sceneObject);
    }
}
