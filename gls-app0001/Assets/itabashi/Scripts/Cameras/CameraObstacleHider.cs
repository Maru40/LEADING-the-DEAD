using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



static class MeshExtention
{
    public static void ChangeAlpha(this MeshRenderer meshRenderer, float alpha)
    {
        var color = meshRenderer.material.color;
        color.a = alpha;
        meshRenderer.material.color = color;
    }
}

public class CameraObstacleHider : MonoBehaviour
{
    private struct HitRenderer
    {
        public MeshRenderer meshRenderer;
        public RaycastHit raycastHit;

        public HitRenderer(MeshRenderer meshRenderer,RaycastHit raycastHit)
        {
            this.meshRenderer = meshRenderer;
            this.raycastHit = raycastHit;
        }
    }

    [SerializeField]
    private Transform m_targetTransform;

    [SerializeField]
    private List<LayerMask> m_hitLayers;

    private LayerMask m_hitLayer;

    private List<HitRenderer> m_hitMeshRenderers = new List<HitRenderer>();
    private List<HitRenderer> m_beforeHitMeshRenderers = new List<HitRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        m_hitLayer = CreateHitLayerMask(m_hitLayers.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(!m_targetTransform)
        {
            return;
        }

        float distance = (transform.position - m_targetTransform.position).magnitude;

        m_beforeHitMeshRenderers = new List<HitRenderer>(m_hitMeshRenderers);

        m_hitMeshRenderers.Clear();

        foreach (var hitData in Physics.RaycastAll(transform.position, transform.forward, distance, m_hitLayer))
        {
            var meshRenderer = hitData.collider.gameObject.GetComponent<MeshRenderer>();

            if(meshRenderer)
            {
                m_hitMeshRenderers.Add(new HitRenderer(meshRenderer, hitData));
            }
        }

        m_beforeHitMeshRenderers.RemoveAll(renderer => renderer.meshRenderer == null);

        foreach(var exitHitRenderer in m_beforeHitMeshRenderers.Except(m_hitMeshRenderers))
        {
            exitHitRenderer.meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }

        foreach(var hitRenderer in m_hitMeshRenderers)
        {
            hitRenderer.meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }

    }

    private LayerMask CreateHitLayerMask(params LayerMask[] layerMasks)
    {
        LayerMask hitLayerMask = 0;

        foreach(var layerMask in layerMasks)
        {
            hitLayerMask |= layerMask;
        }

        return hitLayerMask;
    }

    private Color ChangeAlpha(Color color,float alpha)
    {
        color.a = alpha;
        return color;
    }
}
