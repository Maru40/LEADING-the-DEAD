using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnState_RandomPlowling : EnemyStateNodeBase<EnemyBase>
{
    GameObject m_target = null;

    EyeSearchRange m_eyeRange;

    public EnState_RandomPlowling(EnemyBase owner)
        :base(owner)
    { }

    public override void OnStart()
    {
        var owner = GetOwner();
        AddChangeComp(owner.GetComponent<RandomPlowlingMove>(), true, false);

        ChangeComps(EnableChangeType.Start);

        //���̑��R���|�[�l���g�̎擾
        m_eyeRange = owner.GetComponent<EyeSearchRange>();

        //�^�[�Q�b�g�̎擾
        m_target = owner.GetComponent<TargetMgr>().GetNowTarget();
        if(m_target == null)
        {
            var target = GameObject.Find("Player");
            var targetMgr = owner.GetComponent<TargetMgr>();
            targetMgr?.SetNowTarget(GetType(), target);

            m_target = target;
        }
    }

    public override void OnUpdate()
    {
        Debug.Log("RandomPlowling");

        //���E�ɓG����������X�e�[�g��؂�ւ���B
        if (m_eyeRange)
        {
            if (m_eyeRange.IsInEyeRange(m_target))
            {
                var owner = GetOwner();

                var chase = owner.GetComponent<I_Chase>();
                if (chase != null){
                    chase.ChangeState();
                }
            }
        }
        else
        {
            Debug.Log("EnState_RomdomPlowling :: " + " Update :" + "EyeSearchRange�R���|�[�l���g�����݂��܂��� ");
        }
    }

    public override void OnExit()
    {


        ChangeComps(EnableChangeType.Exit);
    }
}
