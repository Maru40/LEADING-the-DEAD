﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemyGeneratorManager : SingletonMonoBehaviour<AllEnemyGeneratorManager>
{
    List<EnemyGenerator> m_generators = new List<EnemyGenerator>();
    List<ZombieTank> m_tanks = new List<ZombieTank>();

    void Start()
    {
        m_generators = new List<EnemyGenerator>(FindObjectsOfType<EnemyGenerator>());
        m_tanks = new List<ZombieTank>(FindObjectsOfType<ZombieTank>());
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
        int count = 0;
        foreach (var generator in m_generators)
        {
            count += generator.NumClearServices;  //クリアに貢献したゾンビの数を取得する。
        }

        return count;
    }
}
