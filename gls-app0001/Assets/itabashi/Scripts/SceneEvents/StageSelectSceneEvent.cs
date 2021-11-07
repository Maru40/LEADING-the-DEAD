using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Manager;

public class StageSelectSceneEvent : MonoBehaviour
{
    [SerializeField]
    private SceneObject m_titleScene;

    [SerializeField]
    private StageSelecter m_stageSelecter;

    private Dictionary<MoveDirection, System.Action> m_directionToActionTable =
        new Dictionary<MoveDirection, System.Action>();

    private void Awake()
    {

        m_directionToActionTable.Add(MoveDirection.Left, OnLeft);
        m_directionToActionTable.Add(MoveDirection.Right, OnRight);
        m_directionToActionTable.Add(MoveDirection.Up, () => { });
        m_directionToActionTable.Add(MoveDirection.Down, () => { });
        m_directionToActionTable.Add(MoveDirection.None, () => { });
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackTitle()
    {
        GameSceneManager.Instance.LoadScene(m_titleScene);
    }

    public void OnMove(BaseEventData eventData)
    {
        var axisEventData = eventData as AxisEventData;

        if(axisEventData == null)
        {
            return;
        }

        m_directionToActionTable[axisEventData.moveDir].Invoke();
    }

    public void OnLeft()
    {
        if (enabled)
        {
            m_stageSelecter.SelectLeft();
        }
    }

    public void OnRight()
    {
        if (enabled)
        {
            m_stageSelecter.SelectRight();
        }
    }

    public void OnSelect()
    {
        if(enabled)
        {
            m_stageSelecter.StageIsSelect();
        }
    }
}
