using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class UseItemUIPresenter : MonoBehaviour
{
    [SerializeField]
    private UseItemUI m_useItemUI;

    [SerializeField]
    private PlayerPickUpper m_playerPickUpper;

    private void Awake()
    {
        m_playerPickUpper.stackObjectsCountOnChanged.Subscribe(count => CountChangedEvent(count));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CountChangedEvent(int count)
    {
        m_useItemUI.isValidity = count > 0;
        m_useItemUI.count = count;
        
    }
}
