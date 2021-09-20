using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class ObjectLauncher : MonoBehaviour
{
    /// <summary>
    /// ���ˑ��x
    /// </summary>
    [SerializeField]
    private float m_firingSpeed = 10.0f;

    /// <summary>
    /// ���ˑ��x
    /// </summary>
    [SerializeField]
    public float firingSpeed
    {
        set { m_firingSpeed = value; }
        get { return m_firingSpeed; }
    }

    /// <summary>
    /// ��]���w�肵�Ȃ��������i�s������forward�������邩�ǂ���
    /// </summary>
    [SerializeField]
    private bool m_rotationForward = true;

    [SerializeField]
    private bool m_isDrawPredictionLine = false;

    [Range(0.1f,1.0f)]
    [SerializeField]
    private float m_oneLineSecond = 0.1f;

    [Range(0.01f,1.0f)]
    [SerializeField]
    private float m_maxDrawLineLength = 1.0f;

    private LineRenderer m_lineRenderer;

    private List<Vector3> m_linePositions = new List<Vector3>();

    public bool isDrawPredictionLine
    {
        set
        {
            m_isDrawPredictionLine = value;
            m_lineRenderer.enabled = value;
        }
        get { return m_isDrawPredictionLine; }
    }

    private void Start()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(!isDrawPredictionLine)
        {
            return;
        }

        DrawPredictionLine();
    }

    /// <summary>
    /// �I�u�W�F�N�g�𔭎˂���
    /// </summary>
    /// <param name="throwableObjectPrefab">�I�u�W�F�N�g�̃v���n�u</param>
    public void Fire(ThrowableObject throwableObjectPrefab)
    {
        var rotation = m_rotationForward ? transform.rotation : Quaternion.identity;
        Fire(throwableObjectPrefab, rotation);
    }

    /// <summary>
    /// �I�u�W�F�N�g�𔭎˂���
    /// </summary>
    /// <param name="throwableObjectPrefab">�I�u�W�F�N�g�̃v���n�u</param>
    /// <param name="rotation">�I�u�W�F�N�g�̉�]���</param>
    public void Fire(ThrowableObject throwableObjectPrefab, Quaternion rotation)
    {
        var throwableObject = Instantiate(throwableObjectPrefab, transform.position, rotation);
        throwableObject.Throwing(new ThrowingData(transform.forward * m_firingSpeed));

        Debug.Log(Physics.gravity);
    }

    private void DrawPredictionLine()
    {
        m_linePositions.Clear();

        m_linePositions.Add(transform.position);

        float sumLength = 0.0f;

        Vector3 fireVector = transform.forward * m_firingSpeed;

        Vector3 beforePosition = transform.position;

        int count = 1;

        while(sumLength < m_maxDrawLineLength)
        {

            Vector3 position = new Vector3();

            float time = m_oneLineSecond * count;

            position.x = transform.position.x + fireVector.x * time;
            position.y = transform.position.y + fireVector.y * time - 0.5f * -Physics.gravity.y * time * time;
            position.z = transform.position.z + fireVector.z * time;

            RaycastHit raycastHit;

            if(Physics.Linecast(beforePosition,position,out raycastHit))
            {
                m_linePositions.Add(raycastHit.point);
                break;
            }

            m_linePositions.Add(position);

            sumLength += (position - beforePosition).magnitude;


            beforePosition = position;
            count++;
        }
        Debug.Log($"�O�ɏo�� {isDrawPredictionLine}");

        m_lineRenderer.positionCount = m_linePositions.Count;
        m_lineRenderer.SetPositions(m_linePositions.ToArray());
    }
}
