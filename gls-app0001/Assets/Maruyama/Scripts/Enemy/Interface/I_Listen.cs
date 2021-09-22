using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 聞くをするインターフェース
/// </summary>
public interface I_Listen
{
    /// <summary>
    /// 音を聞く
    /// </summary>
    public void Listen(FoundObject foundObject);
}
