using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;
using UniRx;

public class SignalGunPresenter : MonoBehaviour
{
    [SerializeField]
    SignalGunCtrl m_signalGunCtrl = null;

    [SerializeField]
    UseItemUI m_useItemUI = null;

    [SerializeField]
    GameObject m_additiveImage = null;

    void Start()
    {
        var coolTimeParcentage = m_signalGunCtrl.coolTimePercentageOnChanged;

        coolTimeParcentage.Where(parsent => parsent >= 1.0f)
            .Subscribe(_ => { m_useItemUI.isValidity = true; }  )
            .AddTo(this);

        coolTimeParcentage.Where(parsent => parsent <= 1.0f && m_useItemUI.isValidity == true)
            .Skip(1)
            .Subscribe(_ => m_useItemUI.isValidity = false)
            .AddTo(this);
    }

}
