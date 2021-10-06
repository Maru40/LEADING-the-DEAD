using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_BindedActiveArea
{
    public void Bind(BindActivateArea other);
    public void BindRelease(BindActivateArea other);
}
