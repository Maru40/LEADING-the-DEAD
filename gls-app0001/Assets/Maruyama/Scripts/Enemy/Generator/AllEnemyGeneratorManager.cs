using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class AllEnemyGeneratorManager : SingletonMonoBehaviour<AllEnemyGeneratorManager>
{
    [SerializeField]
    private Color m_gizmosColor;
    [SerializeField]
    private GameObject m_barriade = null;
    [SerializeField]
    private GameObject m_player = null;

    private List<ThrongGeneratorBase> m_generators = new List<ThrongGeneratorBase>();
    private List<ThrongData> m_allThrongDatas = new List<ThrongData>();
    private List<ZombieTank> m_tanks = new List<ZombieTank>();

    [Header("クリアに貢献できる時間"), SerializeField]
    private float m_clearServicesTime = 5.0f;
    private HashSet<GameObject> m_clearServices = new HashSet<GameObject>();  //クリアに貢献したオブジェクト
    private List<GameObject> m_removeClearServices = new List<GameObject>();  //削除申請の出されたクリア貢献オブジェクト
    private Dictionary<GameObject, GameTimer> m_clearServicesTimerDictionary = new Dictionary<GameObject, GameTimer>();

    [Header("クリア演出に参加できるゾンビの距離"), SerializeField]
    private float m_clearMovieEntryRange = 8.0f;

    private void Start()
    {
        m_generators = new List<ThrongGeneratorBase>(FindObjectsOfType<ThrongGeneratorBase>());
        m_tanks = new List<ZombieTank>(FindObjectsOfType<ZombieTank>());

        if (m_barriade == null)
        {
            m_barriade = FindObjectOfType<BarricadeDurability>().gameObject;
        }

        SettingThrongDatas();
    }

    private void Update()
    {
        UpdateClearServicesTimers();
        RemoveClearServices();
    }

    public List<ThrongData> GetAllThrongDatas()
    {
        if (m_allThrongDatas.Count == 0)
        {
            SettingThrongDatas();
        }

        return m_allThrongDatas; 
    }
    


    private List<ThrongData> SettingThrongDatas()
    {
        foreach (var generator in m_generators)
        {
            foreach(var data in generator.ThrongDatas)
            {
                m_allThrongDatas.Add(data);
            }
        }

        return m_allThrongDatas;
    }

    /// <summary>
    /// 貢献した数を時間で管理する。
    /// </summary>
    private void UpdateClearServicesTimers()
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

    /// <summary>
    /// 貢献した数の削除
    /// </summary>
    private void RemoveClearServices()
    {
        foreach (var remove in m_removeClearServices)
        {
            m_clearServices.Remove(remove);
        }
    }

    /// <summary>
    /// ゲームフェード時に呼びたいイベント
    /// </summary>
    public void FadeOutEvent()
    {
        foreach (var generator in m_generators)
        {
            generator.RepawnPositoinAll();  //全てをリスポーンさせる。
            generator.IsInCameraCreate = false;  //カメラ内で湧かないようにする。
        }
    }

    /// <summary>
    /// ゲームスタート時に呼びたいイベント
    /// </summary>
    public void GameStartEvent()
    {
        foreach(var tank in m_tanks)
        {
            tank.GameStartEvent();
        }
    }

    //ゲーム終了時に呼びたいイベント
    public void GameClearEvent()
    {
        m_player.GetComponent<FoundObject>().enabled = false;

        //if(m_generators.Count == 0) {
        //    return;
        //}

        foreach(var generator in m_generators)
        {
            var datas = generator.ThrongDatas;
            
            foreach(var data in datas)
            {
                data.targetMgr.SetNowTarget(GetType(), null);

                if (Calculation.IsRange(m_barriade, data.gameObject, m_clearMovieEntryRange))
                {
                    data.clearManager?.ClearProcess();
                }
                else
                {
                    data.gameObject.SetActive(false);
                }
            }
        }

        foreach (var tank in m_tanks)
        {
            tank.GetComponent<TargetManager>()?.SetNowTarget(GetType(), null);
            //tank.GameClearEvent();
        }
    }

    /// <summary>
    /// クリアに貢献した数を取得する。
    /// </summary>
    /// <returns></returns>
    public int GetNumAllClearServices()
    {
        return m_clearServices.Count;
    }

    /// <summary>
    /// クリアに貢献した敵の追加
    /// </summary>
    /// <param name="data"></param>
    public void AddClearServices(AttributeObject.DamageData data)
    {
        AddClearServices(data.obj);
    }

    /// <summary>
    /// クリアに貢献した敵の追加
    /// </summary>
    /// <param name="gameObj"></param>
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
    private void EndClearServices(GameObject gameObj)
    {
        m_removeClearServices.Add(gameObj);
    }

    /// <summary>
    /// 死亡した数の取得
    /// </summary>
    /// <returns></returns>
    public int GetAllDeashCount()
    {
        int count = 0;
        foreach(var generator in m_generators)
        {
            count += generator.DeathCount;
        }

        return count;
    }

    /// <summary>
    /// ゾンビの総数
    /// </summary>
    /// <returns></returns>
    public int GetNumAllZombie()
    {
        int count = 0;
        foreach(var generator in m_generators)
        {
            count += generator.NumCreate;
        }

        return count;
    }

    /// <summary>
    /// 現在生きているゾンビの総数
    /// </summary>
    /// <returns></returns>
    public int GetNumAllActiveZombie()
    {
        int count = 0;
        foreach (var generator in m_generators)
        {
            count += generator.GetNumAlive();
        }

        return count;
    }

    //プレイヤーを発見しているゾンビの数
    public int GetNumFindPlayerZombie()
    {
        int count = 0;
        foreach(var generator in m_generators)
        {
            var datas = generator.ThrongDatas;
            foreach(var data in datas)
            {
                if(data.targetMgr.GetNowTargetType() == FoundObject.FoundType.Player)
                {
                    count++;
                }
            }
        }

        return count;
    }

    //Gizmo------------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        DrawGizmo();
    }

    private void DrawGizmo()
    {
        Gizmos.color = m_gizmosColor;
        //var maxRandomRange = m_maxRandomRange * 2.0f;
        if(m_barriade == null)
        {
            return;
        }

        Gizmos.DrawSphere(m_barriade.transform.position, m_clearMovieEntryRange);
        //Gizmos.DrawCube(m_gizmosCenter, Vector3.one * 10.0f);
        //Gizmos.DrawCube(m_centerPosition, maxRandomRange);
    }
}
