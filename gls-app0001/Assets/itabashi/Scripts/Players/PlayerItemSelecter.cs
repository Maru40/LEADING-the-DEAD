using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerItemSelecter : MonoBehaviour
    {
        [SerializeReference]
        private ItemUserBase m_leftItemUser;

        [SerializeField]
        private ItemUserBase m_rightItemUser;

        [SerializeField]
        private ItemUserBase m_upItemUser;

        [SerializeField]
        private ItemUserBase m_downItemUser;

        private ItemUserBase m_nowItemUser;

        private GameControls m_gameControls;

        private void Awake()
        {
            m_gameControls = new GameControls();

            m_gameControls.Player.ItemSelectUp.performed += _ => ChangeUseItemObject(m_upItemUser);
            m_gameControls.Player.ItemSelectDown.performed += _ => ChangeUseItemObject(m_downItemUser);
            m_gameControls.Player.ItemSelectLeft.performed += _ => ChangeUseItemObject(m_leftItemUser);
            m_gameControls.Player.ItemSelectRight.performed += _ => ChangeUseItemObject(m_rightItemUser);

            //m_gameControls.Player.UseItem.performed += _ => m_nowItemUser?.Use();

            this.RegisterController(m_gameControls);
        }

        private void ChangeUseItemObject(ItemUserBase changeItemUser)
        {
            if(m_nowItemUser)
            {
                m_nowItemUser.isUse = false;
            }

            if(changeItemUser)
            {
                changeItemUser.isUse = true;
            }

            m_nowItemUser = changeItemUser;
        }
    }
}
