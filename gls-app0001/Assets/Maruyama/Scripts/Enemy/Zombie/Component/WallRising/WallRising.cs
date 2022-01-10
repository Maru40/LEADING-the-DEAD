using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁のぼり
/// </summary>
public class WallRising : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    //障害物に触れた時の処理
    public void CollsiionEnterAction(Collision collision)
    {
        if(collision.gameObject.tag == "T_Wall")
        {

        }
    }
}
