using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class NumBloodManager : MonoBehaviour
{
    [SerializeField]
    private float m_endTime = 3.0f;

    private List<BloodBagManager> m_bloodBags = new List<BloodBagManager>();
    private List<BloodPuddleManager> m_bloodPuddles = new List<BloodPuddleManager>();

    private int NumBloodBag => m_bloodBags.Count;

    private System.Action m_updateAction = null;
    private GameTimer m_timer = new GameTimer();

    //private Text m_text = null;
    [SerializeField, Header("数字を管理するためのNumberImage")]
    private NumberImage m_numberImage = null;

    private void Awake()
    {
        //m_text = GetComponent<Text>();
        //m_numberImage = GetComponent<NumberImage>();
        m_bloodBags = new List<BloodBagManager>(FindObjectsOfType<BloodBagManager>());
    }

    private void Start()
    {
        m_timer.ResetTimer(m_endTime);
        ChangeNumber();
        //m_text.text = NumBloodBag.ToString();
    }

    private void Update()
    {
        if (m_updateAction != null)
        {
            m_updateAction.Invoke();
        }
    }

    /// <summary>
    /// 血袋の削除を通知
    /// </summary>
    /// <param name="bloodBag"></param>
    public void RemoveBloodBag(BloodBagManager bloodBag)
    {
        m_bloodBags.Remove(bloodBag);
        ChangeNumber();
        //m_text.text = NumBloodBag.ToString();

        if (NumBloodBag == 0)
        {
            //血を回収する。
            m_bloodPuddles = new List<BloodPuddleManager>(FindObjectsOfType<BloodPuddleManager>());
            m_updateAction = UpdateBloodPuddleManager;
        }
    }

    //液体の監視
    private void UpdateBloodPuddleManager()
    {
        System.Action action = null;

        foreach(var puddle in m_bloodPuddles)
        {
            if(puddle == null)
            {
                action += () => m_bloodPuddles.Remove(puddle);
            }
        }

        action?.Invoke();

        if(m_bloodPuddles.Count == 0) { //血が0なら
            m_updateAction = TimerUpdate;
        }
    }

    private void TimerUpdate()
    {
        m_timer.UpdateTimer();

        if(m_timer.IsTimeUp)
        {
            GameOver();
        }
    }

    //ゲームオーバー処理
    private void GameOver()
    {
        Debug.Log("GameOver");
    }

    private void ChangeNumber()
    {
        m_numberImage.value = NumBloodBag;
    }
}
