using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_createObject = null;

    [SerializeField]
    protected int m_numCreate = 30;

    [SerializeField]
    protected Vector3 m_centerPosition = new Vector3();  //��������Ƃ��̒��S�_
    [SerializeField]
    protected Vector3 m_maxRandomRange = new Vector3();  //�����_���ɐ������鎞�̍ő勗��

    [SerializeField]
    protected GameObject m_target = null;
    [SerializeField]
    protected float m_outRange = 15.0f;

    //���������]���r������
    protected List<ThrongData> m_datas = new List<ThrongData>();

    protected virtual void Start()
    {
        if(m_target == null)
        {
            m_target = GameObject.Find("Player");
        }

        CreateObjects();
    }

    void CreateObjects()
    {
        var random = new System.Random(System.DateTime.Now.Millisecond);

        for (int i = 0; i < m_numCreate; i++)
        {
            var createPosition = CalcuRandomPosition(m_target);
            CreateObject(createPosition);
        }
    }

    void CreateObject(Vector3 createPosition)
    {
        var obj = Instantiate(m_createObject, createPosition, Quaternion.identity, transform);
        CreateObjectAdjust(obj);  //����
        
        m_datas.Add(new ThrongData(obj.GetComponent<EnemyVelocityMgr>(),
            obj.GetComponent<TargetManager>(),
            obj.GetComponent<ThrongManager>(),
            obj.GetComponent<RandomPlowlingMove>()
        ));
    }

    /// <summary>
    /// �������K�v�ȃI�u�W�F�N�g��n���āA����
    /// </summary>
    /// <param name="obj">�����������I�u�W�F�N�g</param>
    protected virtual void CreateObjectAdjust(GameObject obj) { }


    /// <summary>
    /// �^�[�Q�b�g���痣�ꂽ�ꏊ���A�����_���ɕԂ��B
    /// </summary>
    /// <param name="target">�^�[�Q�b�g</param>
    /// <returns>�����_���Ȉʒu</returns>
    public Vector3 CalcuRandomPosition(GameObject target)
    {
        return UtilityRandomPosition.OutRangeOfTarget(target, m_outRange, m_maxRandomRange, m_centerPosition);
    }

    //�A�N�Z�b�T---------------------------------------

    /// <summary>
    /// ��������I�u�W�F�N�g�Ɠn���ꂽ�I�u�W�F�N�g������prefab�Ȃ�
    /// </summary>
    /// <param name="gameObj">��r�Ώۂ̃I�u�W�F�N�g</param>
    /// <returns>�����Ȃ�true</returns>
    public bool IsEqualCreateObject(GameObject gameObj)
    {
        return m_createObject.GetType() == gameObj.GetType() ? true : false;
    }

    public List<ThrongData> GetThrongDatas()
    {
        return m_datas;
    }

    public GameObject GetCreateObject()
    {
        return m_createObject;
    }

    /// <summary>
    /// �����_���ɐ�������Ƃ��̍ő勗��
    /// </summary>
    /// <returns>�ő勗��</returns>
    public Vector3 GetMaxRandomRange()
    {
        return m_maxRandomRange;
    }

    public Vector3 GetCenterPosition()
    {
        return m_centerPosition;
    }
}
