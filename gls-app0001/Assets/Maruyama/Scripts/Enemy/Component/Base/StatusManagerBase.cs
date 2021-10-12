using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusManagerBase : MonoBehaviour
{
    BuffManager m_buffManager = new BuffManager();

    public void SetBuffParametor(BuffParametor parametor)
    {
        m_buffManager.SetParametor(parametor);
    }
    public BuffParametor GetBuffParametor()
    {
        return m_buffManager.GetParametor();
    }
}
