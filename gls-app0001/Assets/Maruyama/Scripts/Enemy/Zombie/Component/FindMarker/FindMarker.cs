using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMarker : MonoBehaviour
{
    [SerializeField]
    private GameObject m_createPositionObject = null;
    private Vector3 CreatePosition => m_createPositionObject.transform.position;

    [SerializeField]
    private GameObject m_createPrefab = null;
    [SerializeField]
    private Vector3 m_scale = Vector3.one;
    private GameObject m_marker;

    private void Start()
    {
        if (m_marker == null)
        {
            m_marker = CreateMarker(m_createPrefab);
        }
        
        m_marker.transform.localScale = m_scale;
        m_marker.SetActive(false);
    }

    private void OnDestroy()
    {
        //Debug.Log("◆◆マーカーデストロイ");
        Destroy(m_marker);
    }

    private void Update()
    {
        if (m_marker)
        {
            m_marker.transform.position = CreatePosition;
            if (!gameObject.activeSelf)  //ゲームオブジェクトが非表示なら
            {
                SetMarkerActive(false);
            }
        }
    }

    private GameObject CreateMarker(GameObject prefab)
    {
        var maker = Instantiate(prefab, CreatePosition, Quaternion.identity);

        return maker;
    }

    /// <summary>
    /// マーカーの状態を設定
    /// </summary>
    /// <param name="isActive"></param>
    public void SetMarkerActive(bool isActive)
    {
        m_marker?.SetActive(isActive);
    }

    public void ChangeMaker(GameObject prefab, bool isActive = true)
    {
        Destroy(m_marker);

        m_marker = CreateMarker(prefab);
        SetMarkerActive(isActive);
    }
}
