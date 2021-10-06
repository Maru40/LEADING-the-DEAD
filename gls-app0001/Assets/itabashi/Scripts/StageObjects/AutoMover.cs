using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMover : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeedPerSecond = 1.0f;

    [SerializeField]
    private bool m_isPositionLoop = false;

    [SerializeField]
    private List<Transform> m_transforms;

    private float m_countRange = 0.0f;

    private int m_nowIndex = 0;

    private bool m_isBack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = CalculatePosition();
    }

    private Vector3 CalculatePosition()
    {
        if(m_transforms.Count == 1)
        {
            return m_transforms[0].position;
        }

        if(IsNotMove())
        {
            return transform.position;
        }


        m_countRange += m_moveSpeedPerSecond * Time.deltaTime;

        int nextIndex = GetNextIndex();

        float length = 0.0f;

        while (true)
        {
            length = (m_transforms[nextIndex].position - m_transforms[m_nowIndex].position).magnitude;

            if(m_countRange < length)
            {
                break;
            }

            m_countRange -= length;

            m_nowIndex = nextIndex;
            nextIndex = GetNextIndex();

            Debug.Log($"始まり {m_nowIndex},終わり {nextIndex}");
        }

        return Vector3.Lerp(m_transforms[m_nowIndex].position, m_transforms[nextIndex].position, m_countRange / length);
    }

    private int GetNextIndex()
    {
        if (m_isPositionLoop)
        {
            return m_nowIndex < m_transforms.Count - 1 ? m_nowIndex + 1 : 0;
        }
        else
        {
            if(m_isBack)
            {
                if(m_nowIndex != 0)
                {
                    return m_nowIndex - 1;
                }

                m_isBack = false;
                return 1;
            }
            
            if(m_nowIndex < m_transforms.Count - 1)
            {
                return m_nowIndex + 1;
            }

            m_isBack = true;
            return m_nowIndex - 1;
        }
    }

    private bool IsNotMove()
    {
        if (m_transforms.Count == 0)
        {
            return true;
        }

        bool isSame = true;

        var initialPosition = m_transforms[0].position;

        foreach(var trans in m_transforms)
        {
            if(trans.position != initialPosition)
            {
                isSame = false;
                break;
            }
        }

        return isSame;
    }
}
