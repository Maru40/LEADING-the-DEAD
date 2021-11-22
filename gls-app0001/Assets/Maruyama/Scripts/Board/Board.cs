using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField]
    protected Material m_material;

    protected Mesh m_mesh;

    protected List<Vector3> m_vertices = new List<Vector3>();  //頂点
    protected List<int> m_indices = new List<int>();           //頂点インデックス 
    protected List<Vector2> m_uvs = new List<Vector2>();       //UV
    protected List<Color> m_colors = new List<Color>();        //カラー
    protected List<Vector3> m_normals = new List<Vector3>();   //法線

    protected virtual void Awake()
    {
        CretaeMesh();
    }

    protected virtual void Update()
    {
        //OnDrawBoard();
    }

    protected virtual void OnDrawBoard()
    {
        Graphics.DrawMesh(m_mesh, transform.position, Quaternion.identity, m_material, 0);
    }

    //メッシュ生成--------------------------------------------------------------------------------

    private void CretaeMesh()
    {
        m_mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = m_mesh;

        CreateVertices();
        CreateUVs();
        CreateIndices();
        CreateColors();
        CreateNormals();

        //情報をメッシュに渡す。
        m_mesh.vertices = m_vertices.ToArray();
        m_mesh.triangles = m_indices.ToArray();
        m_mesh.uv = m_uvs.ToArray();
        m_mesh.SetColors(m_colors);
        //m_mesh.colors = m_colors.ToArray();
        m_mesh.normals = m_normals.ToArray();

        //m_mesh.RecalculateBounds();
    }

    /// <summary>
    /// 頂点データの生成
    /// </summary>
    protected virtual void CreateVertices()
    {
        var vertices = new Vector3[]{
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0)
        };

        m_vertices = new List<Vector3>(vertices);
    }

    /// <summary>
    /// 頂点インデックスの生成
    /// </summary>
    protected virtual void CreateIndices()
    {
        var indices = new int[] {
            0, 1, 2,
            2, 1, 3
        };

        m_indices = new List<int>(indices);
    }

    /// <summary>
    /// UVの生成
    /// </summary>
    protected virtual void CreateUVs()
    {
        var uvs = new Vector2[]{
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(1, 1),
        };

        m_uvs = new List<Vector2>(uvs);
    }

    protected virtual void CreateColors()
    {
        var colors = new Color[] {
            new Color(1.0f, 1.0f, 0.0f, 1.0f),
            new Color(1.0f, 1.0f, 0.0f, 0.0f),
            new Color(1.0f, 1.0f, 0.0f, 0.0f),
            new Color(1.0f, 1.0f, 0.0f, 0.0f),
        };

        m_colors = new List<Color>(colors);
    }

    /// <summary>
    /// 法線の生成
    /// </summary>
    protected virtual void CreateNormals()
    {
        for(int i = 0; i < m_vertices.Count; i++)
        {
            m_normals.Add(new Vector3(0, 0, 1));
        }
    }
}
