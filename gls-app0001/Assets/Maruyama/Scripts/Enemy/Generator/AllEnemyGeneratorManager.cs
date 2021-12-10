using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyGeneratorManager : SingletonMonoBehaviour<AllEnemyGeneratorManager>
{
    List<EnemyGenerator> m_generators = new List<EnemyGenerator>();
    List<ZombieTank> m_tanks = new List<ZombieTank>();

    [Header("クリアに貢献できる時間"), SerializeField]
    float m_clearServicesTime = 5.0f;
    HashSet<GameObject> m_clearServices = new HashSet<GameObject>();  //クリアに貢献したオブジェクト
    List<GameObject> m_removeClearServices = new List<GameObject>();  //削除申請の出されたクリア貢献オブジェクト
    Dictionary<GameObject, GameTimer> m_clearServicesTimerDictionary = new Dictionary<GameObject, GameTimer>();

    void Start()
    {
        m_generators = new List<EnemyGenerator>(FindObjectsOfType<EnemyGenerator>());
        m_tanks = new List<ZombieTank>(FindObjectsOfType<ZombieTank>());
    }

    private void Update()
    {
        UpdateClearServicesTimers();
        RemoveClearServices();

        //Debug.Log("〇" + GetNumAllClearServices());
    }

    //貢献した数を時間で管理する。
    void UpdateClearServicesTimers()
    {
        System.Action removeAction = null;

        foreach (var pairs in m_clearServicesTimerDictionary)
        {
            var timer = pairs.Value;
            timer.UpdateTimer();
            if (timer.IsTimeUp)
            {
                removeAction += () => m_clearServicesTimerDictionary.Remove(pairs.Key);
            }
        }

        removeAction?.Invoke();
    }

    //貢献した数の削除
    void RemoveClearServices()
    {
        foreach (var remove in m_removeClearServices)
        {
            m_clearServices.Remove(remove);
        }
    }

    //ゲームフェード時に
    public void FadeOutEvent()
    {
        foreach (var generator in m_generators)
        {
            generator.RepawnPositoinAll();  //全てをリスポーンさせる。
            generator.IsInCameraCreate = false;  //カメラ内で湧かないようにする。
        }
    }

    //ゲームスタート時に呼びたいイベント
    public void GameStartEvent()
    {
        foreach (var generator in m_generators)
        {
            
        }

        foreach(var tank in m_tanks)
        {
            tank.GameStartEvent();
        }
    }

    //ゲーム終了時に呼びたいイベント
    public void GameClearEvent()
    {
        foreach(var generator in m_generators)
        {
            generator.ClearProcess();  //ゾンビを全員突撃させる。
        }
    }

    /// <summary>
    /// クリアに貢献した数を取得する。
    /// </summary>
    /// <returns></returns>
    public int GetNumAllClearServices()
    {
        return m_clearServices.Count;

        //int count = 0;
        //foreach (var generator in m_generators)
        //{
        //    count += generator.NumClearServices;  //クリアに貢献したゾンビの数を取得する。
        //}

        //return count;
    }

    public void AddClearServices(AttributeObject.DamageData data)
    {
        AddClearServices(data.obj);
    }

    public void AddClearServices(GameObject gameObj) {
        if (m_clearServices.Add(gameObj))  //追加されたら
        {
            //一定時間後に削除をする。
            m_clearServicesTimerDictionary[gameObj] = new GameTimer(m_clearServicesTime, () => {
                EndClearServices(gameObj);
            });
        }
        else
        {
            //時間リセット
            var timer = m_clearServicesTimerDictionary[gameObj];
            timer.ResetTimer(m_clearServicesTime, () => {
                EndClearServices(gameObj);
            });
        }
    }

    /// <summary>
    /// 貢献終了時
    /// </summary>
    void EndClearServices(GameObject gameObj)
    {
        m_removeClearServices.Add(gameObj);
    }

    public int GetAllDeashCount()
    {
        int count = 0;
        foreach(var generator in m_generators)
        {
            count += generator.DeathCount;
        }

        return count;
    }
}
