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
        m_gameControls.Player.Select.performed += context => Debug.Log("ÉZÉåÉNÉg");
    }

    // Update is called once per frame
    void Update()
    {
        var move = m_gameControls.Player.Move.ReadValue<Vector2>();

        transform.Translate(new Vector3(move.x, 0.0f, move.y));
    }
}
