using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
    where T : MonoBehaviour 
{
    public static T sm_instance;

    public static bool Exists => sm_instance != null;

    public static T Instance
    {
        get
        {
            if(sm_instance == null)
            {
                sm_instance = FindObjectOfType<T>();
                if(sm_instance == null)
                {
                    Debug.Log(typeof(T) + "、がGameSceneに追加されていません");
                }
            }

            return sm_instance;
        }
    }

    protected void Awake()
    {
        if(this != Instance)
        {
            Debug.Log(GetType() + "、は生成されているためコンポーネントを削除しました。\n" +
                "アタッチされているオブジェクトは " + Instance.gameObject.GetType() + "です。");
            Destroy(this);
        }
    }
}
