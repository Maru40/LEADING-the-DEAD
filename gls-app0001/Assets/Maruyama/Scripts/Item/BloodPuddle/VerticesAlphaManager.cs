﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticesAlphaManager : MonoBehaviour
{
    Board m_board;

    [Header("全て透明なら削除するかどうか"),SerializeField]
    bool m_isAllAlphaDelete = true;

    private void Awake()
    {
        m_board = GetComponent<Board>();
    }

    private void Start()
    {
        //ChangeAlpha();
    }

    private void Update()
    {
        ChangeAlpha();
    }

    public void ChangeAlpha()
    {
        var vartices = m_board.Vertices;
        var colors = m_board.Colors;

        int index = 0;
        for (int i = 0; i < vartices.Length; i++)
        {
            //Debug.Log("■バーテックス範囲" + transform.position + vartices[i]);
            var rayObstacleLayerStrings = new string[] { "L_Obstacle", "L_Ground" };
            var layerIndex = LayerMask.GetMask(rayObstacleLayerStrings);
            const float maxRange = 0.1f;
            const float SphereRange = 0.0f;
            //Rayがhitしてなかったら非表示
            var colliders = Physics.OverlapSphere(transform.position, SphereRange, layerIndex);
            RaycastHit hit;

            var startPosition = transform.position + (transform.rotation * vartices[i]);
            //Debug.DrawRay(startPosition, transform.forward, new Color(1.0f, 0.0f, 0.0f, 1.0f));
            if (!Physics.Raycast(startPosition, transform.forward, out hit, maxRange, layerIndex))
                //&& colliders.Length == 0)
            {
                colors[i].a = 0.0f;
                index++;
                //vartices[i] = hit.point + (transform.forward * 0.1f);
            }
        }

        m_board.Vertices = vartices;
        m_board.Colors = colors;

        if(m_isAllAlphaDelete && vartices.Length >= index) //全て透明で削除するなら
        {
            Destroy(gameObject);
        }
    }
}