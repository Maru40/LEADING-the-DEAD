using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーキャラクターのアイテムを拾う機能のコンポーネント
/// </summary>
public class PlayerPickUpper : MonoBehaviour
{
    private const string PICKUP_OBJECTS_NAME = "PickUpObjects";

    Transform m_pickUpObjectsTransform;

    PickedUpObject m_stackObject;

    public PickedUpObject stackObject { get { return m_stackObject; } }

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
        if(m_stackObject)
        {
            Destroy(m_stackObject.gameObject);
        }

        Debug.Log("アイテムを拾いました");
        m_stackObject = pickedUpObject;

        m_stackObject.gameObject.transform.SetParent(m_pickUpObjectsTransform);

        m_stackObject.gameObject.SetActive(false);
    }

    public PickedUpObject TakeOut()
    {
        PickedUpObject takeOutObject = m_stackObject;
        m_stackObject = null;
        return takeOutObject;
    }
}
