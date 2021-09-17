using System;
using System.Diagnostics.CodeAnalysis;

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BreadCrumb�p�̃R���|�[�l���g
/// </summary>
public class BreadCrumb : MonoBehaviour
{
    [SerializeField]
    int m_numBread = 30;  //Bread�̍ő吔

    [SerializeField]
    float m_addRange = 2.0f;  //�ǉ����鋗��

    List<Vector3> m_positions = new List<Vector3>();

    void Start()
    {
        AddPosition();
    }

    void Update()
    {
        //�O�񕪂���苗���͂Ȃꂽ��
        if (IsAddRange())
        {
            AddPosition();

            if (IsSizeOver())
            {
                RemoveOldPosition();
            }
        }
    }

    bool IsAddRange()
    {
        int index = m_positions.Count - 1;
        var beforePosition = m_positions[index];

        var toVec = beforePosition - transform.position;

        return toVec.magnitude > m_addRange ? true : false;
    }

    bool IsSizeOver()
    {
        return m_positions.Count > m_numBread ? true : false;
    }

    void AddPosition()
    {
        m_positions.Add(transform.position);
    }

    /// <summary>
    /// �Â��|�W�V�����̍폜
    /// </summary>
    void RemoveOldPosition()
    {
        m_positions.Remove(m_positions[0]);
    }


    //�A�N�Z�b�T---------------------------------------------------------------------

    /// <summary>
    /// �z��̒l���Q�Ɠn��
    /// </summary>
    /// <returns>�z��̎Q��</returns>
    public List<Vector3> GetPosisions()
    {
        return m_positions;
    }

    /// <summary>
    /// �z��̒l���R�s�[���āA�擾
    /// </summary>
    /// <returns>�z���l�n��</returns>
    public List<Vector3> GetCopyPositions()
    {
        return new List<Vector3>(m_positions);
    }

    /// <summary>
    /// Bread�̍ő吔���Z�b�g
    /// </summary>
    /// <param name="num">Bread�̍ő吔</param>
    public void SetNumBread(int num)
    {
        m_numBread = num;
    }

    /// <summary>
    /// Bread�̍ő吔���擾
    /// </summary>
    /// <returns>Bread�̍ő吔</returns>
    public int GetNumBread()
    {
        return m_numBread;
    }

    /// <summary>
    /// �ŐV�̃|�W�V�������擾����
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNewPosition()
    {
        int index = m_positions.Count - 1;
        return m_positions[index];
    }

    /// <summary>
    /// ���̃|�W�V�������擾����B
    /// </summary>
    /// <param name="beforePosition">�O�񕪂̃|�W�V����</param>
    /// <returns>���̃|�W�V����</returns>
    public Vector3? GetNextPosition(Vector3 beforePosition)
    {
        //�ő�̎�O�܂ŉ񂷁B
        for(int i = 0; i < m_positions.Count - 1; i++)
        {
            if(m_positions[i] == beforePosition)
            {
                return m_positions[++i];
            }
        }

        return null;
    }
}
