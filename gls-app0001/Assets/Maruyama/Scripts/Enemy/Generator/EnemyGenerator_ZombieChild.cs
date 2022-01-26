using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MaruUtility;

public class EnemyGenerator_ZombieChild : ThrongGeneratorBase
{
    public struct CreateObjectData
    {
        public GameObject gameObj;
        public Vector3 createPosition;

        public CreateObjectData(GameObject gameObj, Vector3 createPosition)
        {
            this.gameObj = gameObj;
            this.createPosition = createPosition;
        }
    }

    [SerializeField]
    private GameObject m_createObject = null;

    [SerializeField]
    private List<GameObject> m_positionObjects = new List<GameObject>();

    //生成したオブジェクトのデータ
    private List<CreateObjectData> m_createObjectDatas = new List<CreateObjectData>();

    //集団データ
    private List<ThrongData> m_throngDatas = new List<ThrongData>();
    public override List<ThrongData> ThrongDatas => m_throngDatas;

    private bool m_isInCameraCreate = false;

    private void Start()
    {
        CreateObjects();
    }

    private void CreateObjects()
    {
        foreach (var positionObject in m_positionObjects)
        {
            var createPosition = positionObject.transform.position;
            var newObject = Instantiate(m_createObject, createPosition, Quaternion.identity);
            newObject.gameObject.SetActive(true);
            newObject.transform.SetParent(this.transform);

            //データリストの更新
            m_createObjectDatas.Add(new CreateObjectData(newObject, createPosition));
            m_throngDatas.Add(CalcuThrongData(newObject));
        }
    }

    //仮想関数の実装-----------------------------------------------------------------------------------

    public override int NumCreate => m_createObjectDatas.Count;
    public override int GetNumAlive() => CalcuNumAlive(m_throngDatas);
    public override bool IsInCameraCreate 
    { 
        get => m_isInCameraCreate; 
        set => m_isInCameraCreate = value;
    }

    public override Vector3 CalcuRandomPosition()
    {
        var obj = MyRandom.RandomList(m_positionObjects);
        return obj.transform.position;
    }

    public override void RepawnPositoinAll()
    {
        foreach(var data in m_createObjectDatas)
        {
            data.gameObj.transform.position = data.createPosition;
        }
    }
}
