using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Player
{
    public class PlayerBloodBagPutter : MonoBehaviour
    {
        [SerializeField]
        private PlayerStatusManager m_playerStatusManager;

        [SerializeField]
        private PlayerPickUpper m_playerPickUpper;

        [SerializeField]
        private Transform m_putBloodBagTransform;

        [SerializeField]
        private AudioSource m_audioSource;

        [SerializeField]
        private AudioClip m_audioClip;

        private GameControls m_gameControls;

        private Subject<Unit> m_puttingBloogBagSubject = new Subject<Unit>();
        private void Awake()
        {
            m_puttingBloogBagSubject
                .Where(_ => m_playerStatusManager.isControllValid)
                .Subscribe(_ => OnPutting());

            m_gameControls = new GameControls();

            m_gameControls.Player.PutBloodBag.performed += context => m_puttingBloogBagSubject.OnNext(Unit.Default);

            this.RegisterController(m_gameControls);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnPutting()
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

            m_audioSource.PlayOneShot(m_audioClip);
        }
    }
}