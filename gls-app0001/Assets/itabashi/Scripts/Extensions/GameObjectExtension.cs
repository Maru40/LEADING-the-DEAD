using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject�N���X�̊g���N���X
/// </summary>
public static class GameObjectExtension
{
    /// <summary>
    /// �Q�[���I�u�W�F�N�g�̐e����q�܂ł��ׂĂ���GetComponent���ĕԂ�
    /// </summary>
    /// <typeparam name="T">Component�̌^</typeparam>
    /// <param name="gameObject">�Q�[���I�u�W�F�N�g</param>
    /// <returns></returns>
    public static T GetComponentInParentAndChildren<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();

        if (component)
        {
            return component;
        }

        component = gameObject.GetComponentInParent<T>();

        if (component)
        {
            return component;
        }

        component = gameObject.GetComponentInChildren<T>();

        return component;
    }

}
