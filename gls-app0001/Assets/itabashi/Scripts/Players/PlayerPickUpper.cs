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
    private PossibleUI m_possibleUI;

    private const string PICKUP_OBJECTS_NAME = "PickUpObjects";

    Transform m_pickUpObjectsTransform;

    private ReactiveCollection<PickedUpObject> m_stackObjects = new ReactiveCollection<PickedUpObject>();

    public PickedUpObject stackObject => m_stackObjects.Empty() ? null : m_stackObjects.Front();

    public System.IObservable<int> stackObjectsCountOnChanged => m_stackObjects.ObserveCountChanged();

    private void Reset()
    {
        m_pickUpObjectsTransform = transform.Find(PICKUP_OBJECTS_NAME);

        if(!m_pickUpObjectsTransform)
        {
            m_pickUpObjectsTransform = new GameObject(PICKUP_OBJECTS_NAME).transform;
            m_pickUpObjectsTransform.SetParent(transform);
        }
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

    // Update is called once per frame
    void Update()
    {
        
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

        var cymbalMonkeyStateManager = pickedUpObject.GetComponent<CymbalMonkeyStateManager>();

        if(cymbalMonkeyStateManager)
        {
            cymbalMonkeyStateManager.alarmSwitch = false;
        }

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

        if (!pickedUpObject || pickedUpObject.pickedUpType != PickedUpObject.PickedUpType.Decision)
        {
            return;
        }

        m_possibleUI.AddSelectPossible("拾う", () => Decision(pickedUpObject), pickedUpObject.GetInstanceID());
    }

    private void Decision(PickedUpObject pickedUpObject)
    {
        PutAway(pickedUpObject);

        m_possibleUI.RemoveSelectPossible("拾う", pickedUpObject.GetInstanceID());
    }

    public void DecisionTriggerExit(Collider other)
    {
        var pickedUpObject = other.gameObject.GetComponent<PickedUpObject>();

        if (!pickedUpObject || pickedUpObject.pickedUpType != PickedUpObject.PickedUpType.Decision)
        {
            return;
        }

        m_possibleUI.RemoveSelectPossible("拾う", pickedUpObject.GetInstanceID());
    }
}
