using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TPSカメラの移動管理コンポーネント
/// </summary>
public class TPSCameraMover : MonoBehaviour
{
    /// <summary>
    /// 中心に表示するターゲット
    /// </summary>
    [SerializeField]
    private GameObject m_targetObject;

    /// <summary>
    /// ターゲットからのカメラの最大距離
    /// </summary>
    [SerializeField]
    private float m_maxRange = 2.0f;

    /// <summary>
    /// 回転スピード
    /// </summary>
    [SerializeField]
    private float m_rotateSpeed = 1.0f;

    /// <summary>
    /// X軸回転の最小値
    /// </summary>
    [SerializeField]
    private float m_minRotateX = -90;
    /// <summary>
    /// X軸回転の最大値
    /// </summary>
    [SerializeField]
    private float m_maxRotateX = 90;

    [SerializeField]
    private LayerMask m_hitLayer;

    /// <summary>
    /// 一秒に対しての回転量
    /// </summary>
    private const float ROTATE_EULER = 45;

    /// <summary>
    /// X軸に対する回転
    /// </summary>
    private float m_rotateX;
    /// <summary>
    /// Y軸に対する回転
    /// </summary>
    private float m_rotateY;

    /// <summary>
    /// ゲームの入力
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
            Debug.LogError("ターゲットに指定されているオブジェクトが存在しません。\n" + gameObject.name + " TargetObject");
            return;
        }

        var rotateVector2 = m_gameControls.Player.RotateViewPoint.ReadValue<Vector2>() * ROTATE_EULER * m_rotateSpeed * Time.deltaTime;

        m_rotateX += rotateVector2.y;
        m_rotateY += rotateVector2.x;

        m_rotateX = Mathf.Clamp(m_rotateX, m_minRotateX, m_maxRotateX);

        transform.rotation = Quaternion.Euler(m_rotateX, m_rotateY, 0);

        float range = m_maxRange;

        if (Physics.Raycast(m_targetObject.transform.position, -transform.forward, out RaycastHit hitInfo, m_maxRange, m_hitLayer))
        {
            range = (hitInfo.point - m_targetObject.transform.position).magnitude;
        }

        Debug.DrawLine(m_targetObject.transform.position, transform.position + -transform.forward * range);

        transform.position = m_targetObject.transform.position + -transform.forward * range;
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
