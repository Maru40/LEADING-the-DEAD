using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public abstract class StatusManagerBase : MonoBehaviour
{
    [Serializable]
    public struct Status
    {
        public float hp;
        public float damageIntervalTime;  //ダメージを受けた後の無敵時間

        public Status(float hp, float damageIntervalTime)
        {
            this.hp = hp;
            this.damageIntervalTime = damageIntervalTime;
        }
    }

    private BuffManager m_buffManager = new BuffManager();

    [SerializeField]
    protected Status m_status = new Status(1.0f, 3.0f);

    //仮想関数----------------------------------------------------------

    public abstract void Damage(AttributeObject.DamageData data);

    //アクセッサ--------------------------------------------------------

    public void SetBuffParametor(BuffParametor parametor)
    {
        m_buffManager.SetParametor(parametor);
    }
    public BuffParametor GetBuffParametor()
    {
        return m_buffManager.GetParametor();
    }

    public void SetStatus(Status status)
    {
        m_status = status;
    }
    public Status GetStatus()
    {
        return m_status;
    }

    public void AddStatus(Status status)
    {
        m_status.hp += status.hp;
        m_status.damageIntervalTime += status.damageIntervalTime;
    }
}
