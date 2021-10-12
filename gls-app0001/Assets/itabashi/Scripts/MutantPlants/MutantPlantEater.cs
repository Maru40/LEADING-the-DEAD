using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MutantPlantEater : MonoBehaviour
{

    [SerializeField,Min(0.0f)]
    private float m_headSpeed = 1.0f;

    [SerializeField, Min(0.0f)]
    private float m_eatCoolTime = 5.0f;

    [SerializeField]
    private float m_maxSumWeight = 10;

    [SerializeField, ReadOnly]
    private float m_nowSumWeight = 0.0f;

    [SerializeField]
    private MutantPlantHead m_head;

    [SerializeField]
    private bool m_isEeatable = true;

    private Vector3 m_collisionEatenObjectPosition;

    private Subject<Unit> m_healEatableSubject = new Subject<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        Observable.FromCoroutine(FindEatenObject)
            .Where(_ => m_head.sumEatenWeight == 0)
            .Subscribe(_ => m_isEeatable = true)
            .AddTo(this);

        m_healEatableSubject.Delay(System.TimeSpan.FromSeconds(m_eatCoolTime))
            .Subscribe(_ => m_isEeatable = true)
            .AddTo(this);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFindEatenObject(EatenObject eatenObject)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        EatenObject eatenObject = other.GetComponent<EatenObject>();

        if(eatenObject && m_isEeatable)
        {
            Debug.Log("発見");
            m_isEeatable = false;
            m_collisionEatenObjectPosition = eatenObject.transform.position;
            m_head.isEatable = true;
            StartCoroutine("FindEatenObject", eatenObject);
        }
    }

    private IEnumerator FindEatenObject()
    {
        Vector3 headBeginPosition = m_head.transform.position;
        Vector3 headEndPosition = m_collisionEatenObjectPosition;

        Vector3 headVector = headEndPosition - headBeginPosition;

        float m_count = 0.0f;

        float m_addCount = m_headSpeed * Time.deltaTime;

        m_count += m_addCount;

        while(m_count < Mathf.PI)
        {
            m_head.transform.position = headBeginPosition + headVector * Mathf.Sin(m_count);

            m_count += m_addCount;

            yield return null;
        }

        m_head.transform.position = headBeginPosition;
        var addWeight = m_head.sumEatenWeight;
        m_nowSumWeight += addWeight;
        m_head.SumWeightClear();

        m_head.isEatable = false;

        if(addWeight == 0)
        {
            m_isEeatable = true;
            yield break;
        }

        if(m_nowSumWeight < m_maxSumWeight)
        {
            m_healEatableSubject.OnNext(Unit.Default);
        }
    }

}
