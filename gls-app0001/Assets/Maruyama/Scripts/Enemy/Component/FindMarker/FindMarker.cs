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
        m_marker = Instantiate(m_createPrefab, CreatePosition, Quaternion.identity);
        m_marker.transform.localScale = m_scale;
        m_marker.SetActive(false);
    }

    private void Update()
    {
        if (m_marker)
        {
            m_marker.transform.position = CreatePosition;
        }
    }

    /// <summary>
    /// マーカーの状態を設定
    /// </summary>
    /// <param name="isActive"></param>
    public void SetMarkerActive(bool isActive)
    {
        Debug.Log("△見つかった");
        m_marker?.SetActive(isActive);
    }

}
