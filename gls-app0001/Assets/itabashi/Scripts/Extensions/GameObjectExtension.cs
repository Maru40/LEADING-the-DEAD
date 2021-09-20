using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObjectクラスの拡張クラス
/// </summary>
public static class GameObjectExtension
{
    /// <summary>
    /// ゲームオブジェクトの親から子まですべてからGetComponentして返す
    /// </summary>
    /// <typeparam name="T">Componentの型</typeparam>
    /// <param name="gameObject">ゲームオブジェクト</param>
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
