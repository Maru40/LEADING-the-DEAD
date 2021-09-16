using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    GameControls m_gameControls;

    /// <summary>
    /// プレイヤーキャラクターの移動速度
    /// </summary>
    [SerializeField]
    private float m_moveSpeed = 1.0f;

    private void Awake()
    {
        m_gameControls = new GameControls();
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var moveVector2 = m_gameControls.Player.Move.ReadValue<Vector2>();

        moveVector2 *= Time.deltaTime * m_moveSpeed;

        if (moveVector2.sqrMagnitude == 0.0f)
        {
            return;
        }


        var moveVector3 = new Vector3(moveVector2.x, 0.0f, moveVector2.y);

       transform.rotation = Quaternion.LookRotation(moveVector3);

        transform.Translate(new Vector3(0.0f, 0.0f, moveVector3.magnitude));

    }
}
