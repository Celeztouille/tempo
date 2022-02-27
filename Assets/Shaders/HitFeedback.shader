Shader "Unlit/HitFeedback"
{
    Properties
    {
        _PerfectTex ("Perfect", 2D) = "white" {}
        _GoodTex ("Good", 2D) = "white" {}
        _MissTex ("Miss", 2D) = "white" {}

        _SliderPerfect ("Perfect Slider", Range(0, 1)) = 0
        _SliderGood ("Good Slider", Range(0, 1)) = 0
        _SliderMiss ("Miss Slider", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _PerfectTex;
            float4 _PerfectTex_ST;
            sampler2D _GoodTex;
            float4 _GoodTex_ST;
            sampler2D _MissTex;
            float4 _MissTex_ST;

            float _SliderPerfect;
            float _SliderGood;
            float _SliderMiss;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _PerfectTex);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float4 colP = tex2D(_PerfectTex, i.uv);
                float4 colG = tex2D(_GoodTex, i.uv);
                float4 colM = tex2D(_MissTex, i.uv);


                return lerp(0, colP, _SliderPerfect) + lerp(0, colG, _SliderGood) + lerp(0, colM, _SliderMiss);
            }
            ENDCG
        }
    }
}
