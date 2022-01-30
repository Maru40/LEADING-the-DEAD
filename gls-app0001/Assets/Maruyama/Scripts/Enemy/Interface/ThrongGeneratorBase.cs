using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrongGeneratorBase : GeneratorBase
{
    //生成したゾンビを持つ
    public abstract List<ThrongData> ThrongDatas { get; }
    public abstract int NumCreate { get; }
    public abstract int GetNumAlive();
    public abstract bool IsInCameraCreate { get; set; }
    public abstract void RepawnPositoinAll();
    public abstract Vector3 CalcuRandomPosition();

    protected ThrongData CalcuThrongData(GameObject obj)
    {
        var newData = new ThrongData(obj.GetComponent<EnemyVelocityManager>(),
            obj.GetComponent<TargetManager>(),
            obj.GetComponent<ThrongManager>(),
            obj.GetComponent<RandomPlowlingMove>(),
            obj.GetComponent<DropObjecptManager>(),
            obj.GetComponent<ClearManager_Zombie>(),
            obj.GetComponent<EnemyRespawnManager>(),
            obj.GetComponent<EnemyRotationCtrl>()
        );

        return newData;
    }

    protected int CalcuNumAlive(List<ThrongData> datas)
    {
        int count = 0;
        foreach (var data in datas)
        {
            if (data.gameObject.activeSelf)
            {
                count++;
            }
        }

        return count;
    }
}
