using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TPS�J�����̈ړ��Ǘ��R���|�[�l���g
/// </summary>
public class TPSCameraMover : MonoBehaviour
{
    /// <summary>
    /// ���S�ɕ\������^�[�Q�b�g
    /// </summary>
    [SerializeField]
    private GameObject m_targetObject;

    /// <summary>
    /// �^�[�Q�b�g����̃J�����̍ő勗��
    /// </summary>
    [SerializeField]
    private float m_maxRange = 2.0f;

    /// <summary>
    /// ��]�X�s�[�h
    /// </summary>
    [SerializeField]
    private float m_rotateSpeed = 1.0f;

    /// <summary>
    /// X����]�̍ŏ��l
    /// </summary>
    [SerializeField]
    private float m_minRotateX = -90;
    /// <summary>
    /// X����]�̍ő�l
    /// </summary>
    [SerializeField]
    private float m_maxRotateX = 90;

    /// <summary>
    /// ��b�ɑ΂��Ẳ�]��
    /// </summary>
    private const float ROTATE_EULER = 45;

    /// <summary>
    /// X���ɑ΂����]
    /// </summary>
    private float m_rotateX;
    /// <summary>
    /// Y���ɑ΂����]
    /// </summary>
    private float m_rotateY;

    /// <summary>
    /// �Q�[���̓���
    /// </summary>
    private GameControls m_gameControls;

    private void OnValidate()
    {
        m_minRotateX = Mathf.Clamp(m_minRotateX, -90, 90);
        m_maxRotateX = Mathf.Clamp(m_maxRotateX, m_minRotateX, 90);
        m_minRotateX = Mathf.Clamp(m_minRotateX, -90, m_maxRotateX);
    }

    private void Awake()
    {
        m_gameControls = new GameControls();
        UpdatePosition();
    }

    private void OnEnable()
    {
        m_gameControls.Enable();
    }

    private void OnDisable()
    {
        m_gameControls.Disable();
    }

    private void OnDestroy()
    {
        m_gameControls.Disable();
    }

    private void UpdatePosition()
    {
        if(!m_targetObject)
        {
            Debug.LogError("�^�[�Q�b�g�Ɏw�肳��Ă���I�u�W�F�N�g�����݂��܂���B\n" + gameObject.name + " TargetObject");
            return;
        }

        var rotateVector2 = m_gameControls.Player.RotateViewPoint.ReadValue<Vector2>() * ROTATE_EULER * m_rotateSpeed * Time.deltaTime;

        m_rotateX += rotateVector2.y;
        m_rotateY += rotateVector2.x;

        m_rotateX = Mathf.Clamp(m_rotateX, m_minRotateX, m_maxRotateX);

        transform.rotation = Quaternion.Euler(m_rotateX, m_rotateY, 0);

        transform.position = m_targetObject.transform.position + -transform.forward * m_maxRange;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
}
