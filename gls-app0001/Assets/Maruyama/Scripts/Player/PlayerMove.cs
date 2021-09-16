using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float m_speed;

    // Update is called once per frame
    void Update()
    {
        MoveProcess();      
    }

    void MoveProcess()
    {
        //float x = Input.GetAxis("Horizontal");
        //float z = Input.GetAxis("Vertical");

        //var moveVec = new Vector3(x,0.0f,z);

        //transform.position += moveVec * m_speed * Time.deltaTime;
    }
}
