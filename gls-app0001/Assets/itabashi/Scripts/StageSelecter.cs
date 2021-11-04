using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public class StageSelecter : MonoBehaviour
{
    public class SelectStageData
    {
        public StageData stageData;
        public int stageIndex;
        public bool isEdge;
        public SelectStageData(StageData stageData, int stageIndex,bool isEdge)
        {
            this.stageData = stageData;
            this.stageIndex = stageIndex;
            this.isEdge = isEdge;
        }
    }

    [SerializeField]
    private List<StageData> m_stageDatas;

    [SerializeField]
    private FocusChangeToPush m_focusChangeToPush;

    [SerializeField]
    private UISounder m_uiSounder;

    private int m_selectIndex = -1;

    private readonly Subject<SelectStageData> m_onSelectIndexDecrementSubject = new Subject<SelectStageData>();

    public System.IObservable<SelectStageData> OnSelectIndexDecrement => m_onSelectIndexDecrementSubject;

    private readonly Subject<SelectStageData> m_onSelectIndexIncrementSubject = new Subject<SelectStageData>();

    public System.IObservable<SelectStageData> OnSelectIndexIncrement => m_onSelectIndexIncrementSubject;

    private void Awake()
    {
        OnSelectIndexDecrement.Subscribe(_ => m_uiSounder.SelectPlay())
            .AddTo(this);

        OnSelectIndexIncrement.Subscribe(_ => m_uiSounder.SelectPlay())
            .AddTo(this);
    }

    public void SelectLeft()
    {
        if(m_selectIndex < 0)
        {
            return;
        }

        m_selectIndex = Mathf.Max(m_selectIndex - 1, -1);

        StageData stageData = m_selectIndex >= 0 ? m_stageDatas[m_selectIndex] : null;

        bool isEdge = m_selectIndex < 0;

        m_onSelectIndexDecrementSubject.OnNext(new SelectStageData(stageData, m_selectIndex, isEdge));
    }

    public void SelectRight()
    {
        if (m_selectIndex == m_stageDatas.Count - 1)
        {
            return;
        }

        m_selectIndex = Mathf.Min(m_selectIndex + 1, m_stageDatas.Count - 1);

        bool isEdge = m_selectIndex == m_stageDatas.Count - 1;

        m_onSelectIndexIncrementSubject.OnNext(new SelectStageData(m_stageDatas[m_selectIndex], m_selectIndex, isEdge));
    }

    public void StageIsSelect()
    {
        if(m_selectIndex < 0)
        {
            return;
        }

        m_uiSounder.SubmitPlay();
        m_focusChangeToPush.NextPushFocus();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(m_stageDatas[m_selectIndex].sceneObject);
    }
}
