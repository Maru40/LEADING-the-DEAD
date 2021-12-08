using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUpEventer : MonoBehaviour
{
    [SerializeField]
    private PopUpUI m_popUpUI;

    [SerializeField]
    private Image m_changedImage;

    [SerializeField]
    private Text m_changedText;

    [SerializeField]
    private GameStateManager m_gameStageManager;

    [SerializeField]
    private Sprite m_sprite;

    [SerializeField, TextArea(1, 4)]
    private string m_text;

    public void PopUpEvent()
    {
        m_gameStageManager.ChangeForcedPause();

        m_changedImage.sprite = m_sprite;
        m_changedText.text = m_text;

        m_popUpUI.PopUp();
    }

}
