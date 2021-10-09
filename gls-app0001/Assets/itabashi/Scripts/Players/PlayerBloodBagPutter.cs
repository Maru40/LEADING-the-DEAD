using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerBloodBagPutter : ItemUserBase
    {
        [SerializeField]
        private PlayerPickUpper m_playerPickUpper;

        [SerializeField]
        private Transform m_putBloodBagTransform;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override void OnUse()
        {
            var bloodBagList = m_playerPickUpper.GetPickedUpObjectList("BloodBag");

            if(bloodBagList.Count == 0)
            {
                return;
            }

            var bloodBag = m_playerPickUpper.TakeOut("BloodBag");
            bloodBag.transform.SetParent(null);
            bloodBag.gameObject.SetActive(true);

            bloodBag.transform.position = m_putBloodBagTransform.position;
            bloodBag.transform.rotation = m_putBloodBagTransform.rotation;
        }
    }
}