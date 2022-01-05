using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

/// <summary>
/// StatorBase(現在は継承だけ(機能は後々考える。))
/// </summary>
public abstract class StatorBase : MonoBehaviour
{
    public abstract void Reset();

    public abstract void ChangeState<EnumType>(EnumType type, int priority = 0) where EnumType : System.Enum;
}
