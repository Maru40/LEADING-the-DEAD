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

    //���������]���r������
    protected List<ThrongData> m_datas = new List<ThrongData>();

    private void Start()
    {
        CreateObjects();
    }

    void CreateObjects()
    {
        var random = new System.Random(System.DateTime.Now.Millisecond);

        for (int i = 0; i < m_numCreate; i++)
        {
            var createPosition = CalcuRandomPosition(random);
            CreateObject(createPosition);
        }
    }

    void CreateObject(Vector3 createPosition)
    {
        var obj = Instantiate(m_createObject, createPosition, Quaternion.identity, transform);
        
        m_datas.Add(new ThrongData(obj.GetComponent<EnemyVelocityMgr>(),
            obj.GetComponent<TargetMgr>(),
            obj.GetComponent<ThrongMgr>(),
            obj.GetComponent<RandomPlowlingMove>()
        ));
    }

    public Vector3 CalcuRandomPosition(System.Random random)
    {
        Vector3 minVec = -m_maxRandomRange;
        Vector3 maxVec =  m_maxRandomRange;
        Vector3 randomPosition = Vector3.zero;

        randomPosition.x = random.Next((int)minVec.x, (int)maxVec.x);
        randomPosition.y = random.Next((int)minVec.y, (int)maxVec.y);
        randomPosition.z = random.Next((int)minVec.z, (int)maxVec.z);
        return m_centerPosition + randomPosition;
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
