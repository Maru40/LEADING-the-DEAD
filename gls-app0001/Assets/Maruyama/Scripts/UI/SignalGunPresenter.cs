using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class SignalGunPresenter : MonoBehaviour
{
    [SerializeField]
    private SignalGunCtrl m_signalGunCtrl = null;

    [SerializeField]
    private UseItemUI m_useItemUI = null;

    [SerializeField]
    private Image m_additiveImage = null;

    private void Start()
    {
        NullCheck();

        var coolTimeParcentage = m_signalGunCtrl.coolTimePercentageOnChanged;

        coolTimeParcentage.Where(parsent => parsent >= 1.0f)
            .Subscribe(_ => { m_useItemUI.isValidity = true;
                m_additiveImage.gameObject.SetActive(false); 
            }  )
            .AddTo(this);

        coolTimeParcentage.Where(parsent => parsent <= 1.0f && m_useItemUI.isValidity == true)
            .Skip(1)
            .Subscribe(_ => { m_useItemUI.isValidity = false;
                m_additiveImage.gameObject.SetActive(true);
            })
            .AddTo(this);

        coolTimeParcentage.Where(parsent => parsent <= 1.0f)
            .Subscribe(parsent => m_additiveImage.fillAmount = parsent)
            .AddTo(this);
    }

    private void NullCheck()
    {
        if (m_signalGunCtrl == null)
        {
            m_signalGunCtrl = GameObject.FindObjectOfType<SignalGunCtrl>();
        }
    }
}
