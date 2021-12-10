using UnityEngine;

public abstract class MissionCheckerBase<T> : MonoBehaviour
{
    [SerializeField]
    protected string m_descriptionFront;

    [SerializeField]
    protected string m_descriptionEnd;

    public abstract bool IsMissionClear(T checkValue);

    public abstract string GetDisctiptionText();
}
