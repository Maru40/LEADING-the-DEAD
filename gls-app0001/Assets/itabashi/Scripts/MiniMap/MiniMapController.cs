using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniMap
{
    public class MiniMapController : MonoBehaviour
    {
        [SerializeField]
        private Transform m_centerTransform;

        [SerializeField]
        private Transform m_cameraTransform;

        [SerializeField]
        private float m_mapSize = 100.0f;

        private List<MiniMapMarker> m_markers = new List<MiniMapMarker>();

        private RectTransform m_rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            float halfMapSize = m_mapSize * 0.5f;

            float rotateY = 0;

            if(m_cameraTransform)
            {
                rotateY = -m_cameraTransform.rotation.eulerAngles.y;
            }

            foreach(var marker in m_markers)
            {

                var localPosition = marker.transform.position - m_centerTransform.position;
                localPosition.y = 0;

                localPosition = Quaternion.AngleAxis(rotateY, Vector3.up) * localPosition;

                bool isOutOfRange = localPosition.sqrMagnitude > Mathf.Pow(halfMapSize, 2);

                Vector2 rectLocalPosition = new Vector2(localPosition.x, localPosition.z);
                rectLocalPosition /= halfMapSize;

                rectLocalPosition.x *= m_rectTransform.sizeDelta.x * 0.5f;
                rectLocalPosition.y *= m_rectTransform.sizeDelta.y * 0.5f;

                float minMarkerSize = Mathf.Min(marker.markerData.size, marker.outOfRangeMarkerData.size);
                float maxLength = m_rectTransform.sizeDelta.x * 0.5f - minMarkerSize * 0.5f;

                isOutOfRange = rectLocalPosition.sqrMagnitude > Mathf.Pow(maxLength, 2);

                if (isOutOfRange && marker.drawOutOfRange)
                {
                    rectLocalPosition = Vector2.ClampMagnitude(rectLocalPosition, maxLength);
                }

                marker.SetMakerLocalPosition(rectLocalPosition, isOutOfRange);
            }
        }

        public void AddMaker(MiniMapMarker maker)
        {
            m_markers.Add(maker);
        }

        public void RemoveMaker(MiniMapMarker maker)
        {
            m_markers.Remove(maker);
        }
    }
}