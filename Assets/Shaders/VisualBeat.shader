Shader "Unlit/VisualBeat"
{
    Properties
    {
        _Size ("Size", Range(0, 1)) = 0
        _Side ("Side", Range(-1, 1)) = 1
        _Thickness ("Thickness", Range(0, 0.3)) = 0.1
        _Color ("Color", Color) = (1,0,0,0)
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

            float _Size;
            float _Side;
            float _Thickness;
            float4 _Color;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float2 coords = 2*(i.uv - 0.5);
                float sdf = length(coords) - (_Size - _Thickness);
                sdf = abs(sdf) - _Thickness;
                sdf /= fwidth(length(coords) - (_Size - _Thickness));

                sdf = 1-saturate(sdf);

                float4 col = float4(_Color.xyz, sdf) * (_Side * coords.x > 0);

                return col;
            }
            ENDCG
        }
    }
}
