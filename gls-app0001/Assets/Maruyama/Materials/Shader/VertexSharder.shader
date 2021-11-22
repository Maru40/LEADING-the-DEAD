Shader "Unlit/VertexSharder"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    //êFÇÃÇ›ìKâûÇ≥ÇπÇΩÇ¢
    //SubShader
    //{
    //  Pass
    //  {
    //    CGPROGRAM
    //    #pragma vertex vert
    //    #pragma fragment frag
    //    #include "UnityCG.cginc"

    //    struct appdata
    //    {
    //      float4 vertex : POSITION;
    //      float4 color: COLOR;
    //    };

    //    struct v2f
    //    {
    //      float4 vertex : SV_POSITION;
    //      float4 color : COLOR;
    //    };

    //    void vert(in appdata v, out v2f o)
    //    {
    //      o.vertex = UnityObjectToClipPos(v.vertex);
    //      o.color = v.color;
    //    }

    //    void frag(in v2f i, out float4 col : SV_Target)
    //    {
    //      col = i.color;
    //    }
    //    ENDCG
    //  }
    //}

    SubShader
    {
        //Tags { "RenderType"= "Fade" }
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            //    struct appdata
            //    {
            //      float4 vertex : POSITION;
            //      float4 color: COLOR;
            //    };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            //struct v2f
            //{
            //  float4 vertex : SV_POSITION;
            //  float4 color : COLOR;
            //};

            sampler2D _MainTex;
            float4 _MainTex_ST;

            //v2f vert (appdata v)
            //{
            //    v2f o;
            //    o.vertex = UnityObjectToClipPos(v.vertex);
            //    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            //    UNITY_TRANSFER_FOG(o,o.vertex);
            //    o.color = v.color;
            //    return o;
            //}

            void vert(in appdata v, out v2f o)
            {
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                o.color = v.color;
            }

            //fixed4 frag (v2f i) : SV_Target
            //{
            //    // sample the texture
            //    fixed4 col = tex2D(_MainTex, i.uv);
            //    // apply fog
            //    UNITY_APPLY_FOG(i.fogCoord, col);
            //    return col;
            //}
            //ENDCG

            void frag(in v2f i, out float4 col : SV_Target)
            {
                col = tex2D(_MainTex, i.uv)
                UNITY_APPLY_FOG(i.fogCoord, col);
                col *= i.color;
            }
            ENDCG
        }
    }
}
