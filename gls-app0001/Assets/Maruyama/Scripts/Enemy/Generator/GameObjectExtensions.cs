using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MaruUtility
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// 指定されたインターフェイスを実装したコンポーネントを持つオブジェクトを検索する。
        /// </summary>
        public static List<T> FindObjectsOfInterface<T>(this GameObject gameObject) where T : class
        {
            var list = new List<T>();

            foreach (var component in GameObject.FindObjectsOfType<Component>())
            {
                var t = component as T;
                if (t != null)
                {
                    list.Add(t);
                }
            }

            return list;
        }
    }
}
