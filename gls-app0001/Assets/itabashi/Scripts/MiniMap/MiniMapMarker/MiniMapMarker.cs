using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMap
{
    /// <summary>
    /// ミニマップのマーカークラス
    /// </summary>
    public class MiniMapMarker : MonoBehaviour
    {
        [System.Serializable]
        public struct MarkerData
        {
            [SerializeField, CustomLabel("表示スプライト")]
            public Sprite sprite;

            [SerializeField, CustomLabel("色")]
            public Color color;

            [SerializeField, CustomLabel("サイズ"), Min(0.0f)]
            public float size;

            public MarkerData(Sprite sprite, Color color, float size)
            {
                this.sprite = sprite;
                this.color = color;
                this.size = size;
            }
        }

        [SerializeField]
        private MiniMapController m_mapController;

        [SerializeField]
        private MarkerData m_markerData = new MarkerData(null, Color.white, 10.0f);

        public MarkerData markerData { get { return m_markerData; } }

        [SerializeField]
        private bool m_isForwardToUp = false;

        [SerializeField]
        private bool m_drawOutOfRange = false;

        public bool drawOutOfRange { get { return m_drawOutOfRange; } }

        [SerializeField]
        private MarkerData m_outOfRangeMarkerData = new MarkerData(null, Color.white, 10.0f);

        public MarkerData outOfRangeMarkerData { get { return m_markerData; } }

        [SerializeField]
        private bool m_isOutwardToUp = false;

        private GameObject m_markerObject;

        private Image m_markerImage;

        private RectTransform m_markerTransform;

        private bool m_isOutOfRange = false;

        private bool m_isActive = true;

        private Camera m_camera = null;

        private void Reset()
        {
            m_mapController = FindObjectOfType<MiniMapController>();
        }

        private void Awake()
        {
            if (!m_mapController)
            {
                m_mapController = FindObjectOfType<MiniMapController>();
            }

            if (!m_mapController)
            {
                Debug.LogError("MiniMapControllerが見つかりません");
                return;
            }

            m_markerObject = CreateMarkerObject();
            m_markerObject.transform.SetParent(m_mapController.transform.Find("Markers"));

            m_markerImage = m_markerObject.GetComponent<Image>();
            m_markerTransform = m_markerObject.GetComponent<RectTransform>();

            m_mapController.AddMaker(this);

            m_camera = Camera.main;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            if (!m_isOutOfRange)
            {
                m_isActive = true;
                m_markerObject?.SetActive(true);
            }
        }

        private void OnDisable()
        {
            m_isActive = false;

            if (m_markerObject != null)
            {
                m_markerObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            m_mapController.RemoveMaker(this);

            Destroy(m_markerObject);
        }

        private GameObject CreateMarkerObject()
        {
            var markerObject = new GameObject($"{name}Maker");

            markerObject.SetActive(enabled);

            var image = markerObject.AddComponent<Image>();
            image.color = m_markerData.color;
            image.sprite = m_markerData.sprite;



            var rectTransform = markerObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(m_markerData.size, m_markerData.size);
            return markerObject;
        }

        private void UpdateMarkerObject(in MarkerData markerData)
        {
            m_markerImage.sprite = markerData.sprite;
            m_markerImage.color = markerData.color;

            m_markerTransform.sizeDelta = new Vector2(markerData.size, markerData.size);
        }

        public void SetMakerLocalPosition(Vector2 localPosition, bool isOutOfRange)
        {
            if (!enabled)
            {
                return;
            }

            m_markerObject.transform.localPosition = localPosition;

            Vector3 upVector;

            if (isOutOfRange)
            {
                upVector = m_isOutwardToUp ? new Vector3(localPosition.x, localPosition.y, 0.0f) : new Vector3();
            }
            else
            {
                if(m_isForwardToUp)
                {
                    upVector = new Vector3(transform.forward.x, transform.forward.z, 0).normalized;

                    upVector = Quaternion.Euler(0,0, m_camera.transform.rotation.eulerAngles.y) * upVector;
                }
                else
                {
                    upVector = new Vector3();
                }
            }

            m_markerTransform.up = upVector;

            var setMarkerData = isOutOfRange ? m_outOfRangeMarkerData : m_markerData;

            UpdateMarkerObject(setMarkerData);

            if (m_isActive && !m_drawOutOfRange)
            {
                m_markerObject.SetActive(!isOutOfRange);
            }

            m_isOutOfRange = isOutOfRange;
        }
    }
}
