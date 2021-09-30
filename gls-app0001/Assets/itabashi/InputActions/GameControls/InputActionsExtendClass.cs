using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputAction拡張メソッド用クラス
/// </summary>
public static class InputActionsExtendClass
{
    /// <summary>
    /// ボタンが押されているか
    /// </summary>
    /// <param name="inputAction">判定用データ</param>
    /// <returns>押されているならtrue</returns>
    public static bool IsPressed(this InputAction inputAction)
    {
        try
        {
            return inputAction.ReadValue<float>() > 0.0f;
        }
        catch(System.Exception)
        {
            return false;
        }
    }
}
