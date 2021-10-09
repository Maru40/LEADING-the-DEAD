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

        if(!pickedUpObject)
        {
            return;
        }

        PutAway(pickedUpObject);
    }

    public void PutAway(PickedUpObject pickedUpObject)
    {
        Debug.Log("アイテムを拾いました");

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
}
