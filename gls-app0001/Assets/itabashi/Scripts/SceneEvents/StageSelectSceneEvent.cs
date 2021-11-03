using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


class InputDirection
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    static public Direction GetDirection(Vector2 moveInput)
    {
        if(moveInput.sqrMagnitude == 0)
        {
            return Direction.None;
        }

        if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            return moveInput.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            return moveInput.y > 0 ? Direction.Up : Direction.Down;
        }
    }
}

public class StageSelectSceneEvent : MonoBehaviour
{
    [SerializeField]
    private SceneObject m_titleScene;

    [SerializeField]
    private StageSelecter m_stageSelecter;

    private UIControls m_uiControls;

    private bool m_isMoveInputed = false;

    private Dictionary<InputDirection.Direction, System.Action> m_directionToActionTable =
        new Dictionary<InputDirection.Direction, System.Action>();

    private void Awake()
    {
        m_uiControls = new UIControls();

        m_uiControls.UI.Submit.performed += context => OnSelect();

        m_uiControls.UI.Cancel.performed += context => BackTitle();

        m_uiControls.UI.Navigate.performed += context => OnMove(context);
        m_uiControls.UI.Navigate.canceled += context => MoveEnd();

        this.RegisterController(m_uiControls);

        m_directionToActionTable.Add(InputDirection.Direction.Left, OnLeft);
        m_directionToActionTable.Add(InputDirection.Direction.Right, OnRight);
        m_directionToActionTable.Add(InputDirection.Direction.Up, () => { });
        m_directionToActionTable.Add(InputDirection.Direction.Down, () => { });
        m_directionToActionTable.Add(InputDirection.Direction.None, () => { });
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BackTitle()
    {
        SceneManager.LoadScene(m_titleScene);
    }

    private void MoveEnd()
    {
        m_isMoveInputed = false;
    }

    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        if(m_isMoveInputed)
        {
            return;
        }

        m_isMoveInputed = true;

        m_directionToActionTable[InputDirection.GetDirection(callbackContext.ReadValue<Vector2>())].Invoke();
    }

    private void OnLeft()
    {
        if (enabled)
        {
            m_stageSelecter.SelectLeft();
        }
    }

    private void OnRight()
    {
        if (enabled)
        {
            m_stageSelecter.SelectRight();
        }
    }

    private void OnSelect()
    {
        if(enabled)
        {
            m_stageSelecter.StageIsSelect();
        }
    }
}
