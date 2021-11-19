using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MeatUIPresenter : MonoBehaviour
{
    [SerializeField]
    UseItemUI m_meatUI;

    [SerializeField]
    Player.PlayerMeatPutter m_meatPutter;

    private void Awake()
    {
        m_meatUI.count = m_meatPutter.HaveMeatCount;

        m_meatPutter.OnHaveMeatCountChanged
            .Subscribe(count => m_meatUI.count = count)
            .AddTo(this);
    }
}
