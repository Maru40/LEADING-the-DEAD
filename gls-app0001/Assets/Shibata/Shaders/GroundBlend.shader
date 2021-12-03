Shader "Custom/GroundBlend"{
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SubTex ("Sub Texture", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}  
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _SubTex;
        sampler2D _MaskTex;

        struct Input {
            float2 uv_MainTex;
            float2 uv_SubTex;//バラバラに出来るように追加
            float2 uv_MaskTex;//バラバラに出来るように追加
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            fixed4 c1 = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 c2 = tex2D (_SubTex,  IN.uv_SubTex);
            fixed4 p  = tex2D (_MaskTex, IN.uv_MaskTex);
            o.Albedo = lerp(c1, c2, p);         
        }
        ENDCG
    }
    FallBack "Diffuse"
}