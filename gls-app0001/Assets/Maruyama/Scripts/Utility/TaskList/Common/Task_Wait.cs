using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Task_Wait : TaskNodeBase
{
    struct Parametor
    {
        public float time;
        public Action enter;
        public Action update;
        public Action exit;
    }

    Parametor m_param = new Parametor();

    public override void OnEnter()
    {

    }

    public override bool OnUpdate()
    {
        return true;
    }

    public override void OnExit()
    {

    }
}
