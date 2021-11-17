using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PossibleUI : MonoBehaviour
{
    class PossibleEvent
    {
        public string displayName;
        public Action selectEvent;
        public int instanceID;

        public PossibleEvent(string displayName,Action selectEvent,int instanceID)
        {
            this.displayName = displayName;
            this.selectEvent = selectEvent;
            this.instanceID = instanceID;
        }

        public PossibleEvent(string displayName,int instanceID)
        {
            this.displayName = displayName;
            this.instanceID = instanceID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            PossibleEvent possibleEvent = obj as PossibleEvent;

            if(possibleEvent == null)
            {
                return false;
            }

            return displayName == possibleEvent.displayName &&
                instanceID == possibleEvent.instanceID;
        }
    }

    private List<PossibleEvent> m_possibleEvents = new List<PossibleEvent>();

    [SerializeField]
    private Button m_button = null;

    [SerializeField]
    private Text m_text = null;

    private int m_index = 0;

    private GameControls m_gameControls = null;

    private void Awake()
    {
        UpdateSelect();

        m_gameControls = new GameControls();
        this.RegisterController(m_gameControls);
        
        m_gameControls.Player.PutBloodBag.performed += context => PushSelectEvent();
    }

    public void AddSelectPossible(string displayName,Action selectEvent,int instanceID)
    {
        m_possibleEvents.Add(new PossibleEvent(displayName, selectEvent, instanceID));

        UpdateSelect();
    }

    public void RemoveSelectPossible(string displayName,int instanceID)
    {
        m_possibleEvents.Remove(new PossibleEvent(displayName, instanceID));

        UpdateSelect();
    }

    public void UpdateSelect()
    {
        if (m_possibleEvents.Count == 0)
        {
            m_index = 0;
            m_button.gameObject.SetActive(false);
            return;
        }

        m_button.gameObject.SetActive(true);

        //EventSystem.current.SetSelectedGameObject(m_button.gameObject);

        m_index = Mathf.Clamp(m_index, 0, m_possibleEvents.Count - 1);

        var selectPossible = m_possibleEvents[m_index];

        m_text.text = selectPossible.displayName;
    }

    public void PushSelectEvent()
    {
        if(m_button == null)
        {
            return;
        }

        if (m_button.gameObject.activeInHierarchy)
        {
            m_possibleEvents[m_index]?.selectEvent();
        }
    }
}
