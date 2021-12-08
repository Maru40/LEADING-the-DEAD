using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

/// <summary>
/// プレイヤーキャラクターのアイテムを拾う機能のコンポーネント
/// </summary>
[DisallowMultipleComponent]
public class PlayerPickUpper : MonoBehaviour
{
    [SerializeField]
    private Canvas m_canvas;

    private const string PICKUP_OBJECTS_NAME = "PickUpObjects";

    Transform m_pickUpObjectsTransform;

    private ReactiveCollection<PickedUpObject> m_stackObjects = new ReactiveCollection<PickedUpObject>();

    public PickedUpObject stackObject => m_stackObjects.Empty() ? null : m_stackObjects.Front();

    public System.IObservable<int> stackObjectsCountOnChanged => m_stackObjects.ObserveCountChanged();

    private bool m_pickedUpDecision = false;

    public bool pickedUpDecition => m_pickedUpDecision;

    private readonly Subject<List<PickedUpObject>> m_decisionSubject = new Subject<List<PickedUpObject>>();

    private GameControls m_gameControls;

    private List<PickedUpObject> m_triggerPickedUpObjects = new List<PickedUpObject>();

    private int m_currentDisitionIndex = 0;

    private void Reset()
    {
        m_pickUpObjectsTransform = transform.Find(PICKUP_OBJECTS_NAME);

        if(!m_pickUpObjectsTransform)
        {
            m_pickUpObjectsTransform = new GameObject(PICKUP_OBJECTS_NAME).transform;
            m_pickUpObjectsTransform.SetParent(transform);
        }
    }

    private void Awake()
    {
        m_decisionSubject
            .Where(_ => m_canvas.gameObject.activeSelf)
            .Subscribe(list => Decision(list[m_currentDisitionIndex]))
            .AddTo(this);

        m_gameControls = new GameControls();
        this.RegisterController(m_gameControls);

        m_gameControls.Player.Select.performed += _ => m_decisionSubject.OnNext(m_triggerPickedUpObjects);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(m_pickUpObjectsTransform)
        {
            return;
        }

        m_pickUpObjectsTransform = transform.Find(PICKUP_OBJECTS_NAME);

        if (!m_pickUpObjectsTransform)
        {
            m_pickUpObjectsTransform = new GameObject(PICKUP_OBJECTS_NAME).transform;
            m_pickUpObjectsTransform.SetParent(transform);
        }
    }

    void UpdateCanvas()
    {
        var pickUpObjects = m_triggerPickedUpObjects.Where(pickUpObject => pickUpObject.IsValid() && pickUpObject.gameObject.activeInHierarchy);

        if (pickUpObjects.Count() == 0)
        {
            m_canvas.gameObject.SetActive(false);
            return;
        }

        float range = 999999999.0f;

        int count = 0;

        for (int i = 0; i < m_triggerPickedUpObjects.Count; ++i)
        {
            var pickUpObject = m_triggerPickedUpObjects[i];

            if (!pickUpObject.IsValid() || !pickUpObject.gameObject.activeInHierarchy)
            {
                continue;
            }

            float objectRange = (pickUpObject.transform.position - transform.position).sqrMagnitude;

            if (objectRange < range)
            {
                range = objectRange;
                m_currentDisitionIndex = count;

                Vector3 objectPosition = pickUpObject.transform.position;
                objectPosition.y += 0.5f;
                m_canvas.transform.position = objectPosition;
            }

            ++count;
        }

        m_pickedUpDecision = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateCanvas();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var pickedUpObject = collision.gameObject.GetComponent<PickedUpObject>();

        if(!pickedUpObject || pickedUpObject.pickedUpType != PickedUpObject.PickedUpType.Touch)
        {
            return;
        }

        PutAway(pickedUpObject);
    }

    public void PutAway(PickedUpObject pickedUpObject)
    {
        Debug.Log("アイテムを拾いました");

        var RadioStateManager = pickedUpObject.GetComponent<RadioStateManager>();

        if(RadioStateManager)
        {
            RadioStateManager.alarmSwitch = false;
        }

        var bloodBagManager = pickedUpObject.GetComponent<BloodBagManager>();
        bloodBagManager?.PickUp();

        m_stackObjects.Add(pickedUpObject);

        pickedUpObject.gameObject.transform.SetParent(m_pickUpObjectsTransform);

        pickedUpObject.gameObject.SetActive(false);
    }

    public PickedUpObject TakeOut()
    {
        return m_stackObjects.FrontPop();
    }

    public PickedUpObject TakeOut(string pickedUpObjectName)
    {
        var hitObjects = GetPickedUpObjectList(pickedUpObjectName);

        var popObject = hitObjects[0];
        m_stackObjects.Remove(popObject);

        return popObject;
    }

    public List<PickedUpObject> GetPickedUpObjectList(string pickedUpObjectName)
    {
        return m_stackObjects.Where(obj => obj.pickedUpObjectName == pickedUpObjectName).ToList();
    }

    public void DecisionTriggerEnter(Collider other)
    {
        var pickedUpObject = other.gameObject.GetComponent<PickedUpObject>();

        if (!pickedUpObject || !pickedUpObject.enabled || !pickedUpObject.gameObject.activeInHierarchy || pickedUpObject.pickedUpType != PickedUpObject.PickedUpType.Decision)
        {
            return;
        }

        m_canvas.gameObject.SetActive(true);

        m_triggerPickedUpObjects.Add(pickedUpObject);

        //m_possibleUI.AddSelectPossible("拾う", () => Decision(pickedUpObject), pickedUpObject.GetInstanceID());
    }

    private void Decision(PickedUpObject pickedUpObject)
    {
        m_triggerPickedUpObjects.Remove(pickedUpObject);
        PutAway(pickedUpObject);
        m_pickedUpDecision = true;
        UpdateCanvas();
        //m_possibleUI.RemoveSelectPossible("拾う", pickedUpObject.GetInstanceID());
    }

    public void DecisionTriggerExit(Collider other)
    {
        var pickedUpObject = other.gameObject.GetComponent<PickedUpObject>();

        if (!pickedUpObject || pickedUpObject.pickedUpType != PickedUpObject.PickedUpType.Decision)
        {
            return;
        }

        m_triggerPickedUpObjects.Remove(pickedUpObject);

        if(m_triggerPickedUpObjects.Count == 0)
        {
            m_canvas.gameObject.SetActive(false);
        };

        //m_possibleUI.RemoveSelectPossible("拾う", pickedUpObject.GetInstanceID());
    }
}
