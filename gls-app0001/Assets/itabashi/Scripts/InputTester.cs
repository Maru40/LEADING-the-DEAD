using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    GameControls m_gameControls;

    // Start is called before the first frame update
    void Start()
    {
        m_gameControls = new GameControls();
        m_gameControls.Enable();
        m_gameControls.Player.Select.performed += context => Debug.Log("セレクト");
    }

    // Update is called once per frame
    void Update()
    {
        if(m_gameControls.Player.Guard.IsPressed())
        {
            Debug.Log("ガード");
        }

        var move = m_gameControls.Player.Move.ReadValue<Vector2>();

        transform.Translate(new Vector3(move.x, 0.0f, move.y));
    }
}
