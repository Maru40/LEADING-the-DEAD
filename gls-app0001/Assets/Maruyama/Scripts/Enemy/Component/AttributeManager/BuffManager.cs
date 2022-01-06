using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuffParametor
{
    public AngerManager.RiseParametor angerParam;
    public TargetManager.BuffParametor targetParam;

    public BuffParametor(AngerManager.RiseParametor angerParam)
    {
        this.angerParam = angerParam;
        this.targetParam = new TargetManager.BuffParametor(1.0f);
    }

    /// <summary>
    /// speedのバフを渡してくれる。
    /// </summary>
    public float SpeedBuffMultiply
    {
        get {
            //Debug.Log("angerBuff: " + angerParam.speed) ;
            //Debug.Log("targetBuff: " + targetParam.speed);
            //Debug.Log("BuffSpeed: " + angerParam.speed * targetParam.speed);
            return angerParam.speed * targetParam.speed; 
        }
    }
}

public class BuffManager
{
    private BuffParametor m_param = new BuffParametor();

    public BuffManager()
        :this(new BuffParametor(new AngerManager.RiseParametor(1.0f,1.0f, 1.0f)))
    { }

    public BuffManager(BuffParametor param)
    {
        m_param = param;
    }

    //アクセッサ------------------------------------------------------------------------------

    public void SetParametor(BuffParametor parametor)
    {
        m_param = parametor;
    }
    public BuffParametor GetParametor()
    {
        return m_param;
    }
}
