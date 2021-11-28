using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class YesNoCheckUI : MonoBehaviour
{
    [SerializeField]
    private PopUpUI m_popUpUI;

    [SerializeField]
    private Button m_yesButton;

    [SerializeField]
    private Button m_noButton;

    private void Awake()
    {
        m_yesButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                m_popUpUI.Close();
            })
            .AddTo(this);

        m_noButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                m_popUpUI.Close();
            })
            .AddTo(this);
    }
}
