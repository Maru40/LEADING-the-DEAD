using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public static class ReactiveCollectionExtension
{
    /// <summary>
    /// コレクションの先頭要素を取得する
    /// </summary>
    /// <typeparam name="T">コレクションの型</typeparam>
    /// <param name="collection">コレクション</param>
    /// <returns>取得した要素</returns>
    public static T Front<T>(this ReactiveCollection<T> collection)
    {
        return collection[0];
    }

    /// <summary>
    /// コレクションの先頭要素を取り出す
    /// </summary>
    /// <typeparam name="T">コレクションの型</typeparam>
    /// <param name="collection">コレクション</param>
    /// <returns>取り出した要素</returns>
    public static T FrontPop<T>(this ReactiveCollection<T> collection)
    {
        var result = collection[0];
        collection.RemoveAt(0);
        return result;
    }

    /// <summary>
    /// コレクションの最後の要素を取得する
    /// </summary>
    /// <typeparam name="T">コレクションの型</typeparam>
    /// <param name="collection">コレクション</param>
    /// <returns>取得した要素</returns>
    public static T Back<T>(this ReactiveCollection<T> collection)
    {
        return collection[collection.Count - 1];
    }

    /// <summary>
    /// コレクションの最後の要素を取り出す
    /// </summary>
    /// <typeparam name="T">コレクションの型</typeparam>
    /// <param name="collection">コレクション</param>
    /// <returns>取り出した要素</returns>
    public static T BackPop<T>(this ReactiveCollection<T> collection)
    {
        var result = collection[collection.Count - 1];
        collection.RemoveAt(collection.Count - 1);
        return result;
    }

    public static bool Empty<T>(this ReactiveCollection<T> collection)
    {
        return collection.Count == 0;
    }
}
