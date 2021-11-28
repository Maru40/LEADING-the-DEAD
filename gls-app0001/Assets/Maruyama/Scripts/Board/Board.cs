using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public struct VertexPositionColorTexture
    {
        public Vector3 position;
        public Color color;
        public Vector2 uv;

        public VertexPositionColorTexture(Vector3 position, Color color, Vector2 uv)
        {
            this.position = position;
            this.color = color;
            this.uv = uv;
        }
    }

    [System.Serializable]
    struct Parametor
    {
        public float width;
        public float height;
        public float depth;
        public float length;
        public int sides;
        public int heightSides;  //高さをどれだけ分けるかどうか
        public Color color;
    }

    [SerializeField]
    Parametor m_param = new Parametor();

    [SerializeField]
    float m_sides = 10.0f;

    [SerializeField]
    protected Material m_material;

    protected Mesh m_mesh;

    //protected List<VertexPositionColorTexture> m_vertices = new List<VertexPositionColorTexture>();
    protected List<Vector3> m_vertices = new List<Vector3>();  //頂点
    protected List<int> m_indices = new List<int>();           //頂点インデックス 
    protected List<Vector2> m_uvs = new List<Vector2>();       //UV
    protected List<Color> m_colors = new List<Color>();        //カラー
    protected List<Vector3> m_normals = new List<Vector3>();   //法線

    //test用
    WaitTimer m_waitTimer;

    protected virtual void Awake()
    {
        //Test();

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
        //CreateUVs();
        CreateIndices();
        //CreateColors();
        CreateNormals();

        //情報をメッシュに渡す。
        //m_mesh.vertices = m_vertices.ToArray();
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
        //var vertices = new Vector3[]{
        //    new Vector3(-1, -1, 0),
        //    new Vector3(-1, 1, 0),
        //    new Vector3(1, -1, 0),
        //    new Vector3(1, 1, 0)
        //};

        float fSides = (float)m_param.sides;  //横に欲しい頂点の数
        float width = m_param.width;
        float depth = m_param.depth;
        float halfHeight = m_param.height * 0.5f;              //縦の高さ
        float firstPosition = -width * 0.5f;
        var color = m_param.color;                 //カラー

        //var vertices = new List<Vector3>();
        var directVec = new Vector3(width, 0.0f, depth);
        //Verticesの作成
        for (int i = 0; i < m_param.sides + 1; i++)
        {
            float length = (m_param.length / fSides) * i;
            var position = directVec.normalized * length;
            var topUV = new Vector2(position.magnitude, 0.0f);   //上部のUV座標
            var bottomUV = new Vector2(position.magnitude, 1.0f);//下部のUV座標

            for(int j = 0 ; j < m_param.heightSides + 1; j++)
            {
                float heightNormalize = (m_param.height / m_param.heightSides);
                float heightRate = heightNormalize * j;
                float height = halfHeight - heightRate;
                var uv = new Vector3(position.magnitude, heightRate);

                m_vertices.Add(new Vector3(firstPosition + position.x, height, position.z));
                m_colors.Add(color);
                m_uvs.Add(uv);
            }

            //m_vertices.Add(new Vector3(firstPosition + position.x, +halfHeight, position.z));
            //m_vertices.Add(new Vector3(firstPosition + position.x, -halfHeight, position.z));

            //m_colors.Add(color);
            //m_colors.Add(color);

            //m_uvs.Add(topUV);
            //m_uvs.Add(bottomUV);
        }

        //m_vertices = vertices;
    }

    /// <summary>
    /// 頂点インデックスの生成
    /// </summary>
    protected virtual void CreateIndices()
    {
        //var baseIndices = new int[] {
        //    //0, 1, 2,
        //    //2, 1, 3
        //    0, 2, 1,
        //    1, 2, 3,
        //};

        var baseIndices = new List<int>();
        for(int i = 0; i < m_param.heightSides; i++)
        {
            baseIndices.Add(i);
            baseIndices.Add(m_param.heightSides + (i + 1));
            baseIndices.Add(i + 1);

            baseIndices.Add(i + 1);
            baseIndices.Add(m_param.heightSides + (i + 1));
            baseIndices.Add(m_param.heightSides + (i + 2));
        }

        //インディセスの作成
        for (int i = 0; i < m_param.sides; i++)
        {
            foreach(var baseIndex in baseIndices)
            {
                m_indices.Add(baseIndex + (i * (m_param.heightSides + 1)));  //ループのたびに、2プラスした数字を代入
            }
        }

        //m_indices = new List<int>(indices);
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
            new Color(1.0f, 1.0f, 0.0f, 1.0f),
            new Color(1.0f, 1.0f, 0.0f, 1.0f),
            new Color(1.0f, 1.0f, 0.0f, 1.0f),
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

    //アクセッサ・プロパティ-----------------------------------------------------------------------

    public Color[] Colors
    {
        get => m_colors.ToArray();
        set 
        {
            m_colors = new List<Color>(value);
            m_mesh.SetColors(Colors);
        }
    }

    public Vector3[] Vertices
    {
        get => m_vertices.ToArray();
        set
        {
            m_vertices = new List<Vector3>(value);
            m_mesh.vertices = m_vertices.ToArray();
        }
    }

}
