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
}
