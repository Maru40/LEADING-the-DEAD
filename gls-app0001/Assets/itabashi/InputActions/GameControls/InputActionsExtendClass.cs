using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputAction�g�����\�b�h�p�N���X
/// </summary>
public static class InputActionsExtendClass
{
    /// <summary>
    /// �{�^����������Ă��邩
    /// </summary>
    /// <param name="inputAction">����p�f�[�^</param>
    /// <returns>������Ă���Ȃ�true</returns>
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
