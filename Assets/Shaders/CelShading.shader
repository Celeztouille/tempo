Shader "Unlit/CelShading"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ShadowSize ("Shadow Size", Range(-1, 1)) = 0 
        _ShadowBlend ("Shadow Blend", Range(0, 1)) = 0.013
        _ShadowColor ("Shadow Color", Color) = (0.1766846, 0.1613119, 0.2358491, 1)
        _RimLightSize ("Rim Light Size", Range(0, 1)) = 0.385 
        _RimLightBlend ("Rim Light Blend", Range(0, 1)) = 0
        _RimColor ("Rim Color", Color) = (0.1460929, 0.4622642, 0.4558117, 1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}

        Pass
        {
            Tags { "LightMode" = "ForwardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShadowSize;
            float _ShadowBlend;
            float4 _ShadowColor;
            float _RimLightSize;
            float _RimLightBlend;
            float4 _RimColor;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                // Diffuse
                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz;
                float3 lambert = smoothstep(_ShadowSize, _ShadowSize + _ShadowBlend, dot(N, L));

                // Edge Light
                float fresnel = pow((1.0 - saturate(dot(normalize(i.normal), normalize(_WorldSpaceCameraPos - i.wPos)))), 1);

                float rimLightSize = 1 - _RimLightSize;
                float edgeStep = smoothstep(rimLightSize, rimLightSize + _RimLightBlend, fresnel);

                float3 celLightning = _ShadowColor * (1 - lambert) + _LightColor0 * lambert + edgeStep * _RimColor;

                
                float4 baseColor = tex2D(_MainTex, i.uv);
                return float4(baseColor * celLightning, 1);
            }
            ENDCG
        }

        Pass
        {
            Tags { "LightMode" = "ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
            
            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShadowSize;
            float _ShadowBlend;
            float4 _ShadowColor;
            float _RimLightSize;
            float _RimLightBlend;
            float4 _RimColor;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.wPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                // Diffuse
                float3 N = normalize(i.normal);
                float3 L = _WorldSpaceLightPos0.xyz;
                float3 lambert = smoothstep(_ShadowSize, _ShadowSize + _ShadowBlend, dot(N, L));

                // Edge Light
                float fresnel = pow((1.0 - saturate(dot(normalize(i.normal), normalize(_WorldSpaceCameraPos - i.wPos)))), 1);

                float rimLightSize = 1 - _RimLightSize;
                float edgeStep = smoothstep(rimLightSize, rimLightSize + _RimLightBlend, fresnel);

                float3 celLightning = _ShadowColor * (1 - lambert) + _LightColor0 * lambert + edgeStep * _RimColor;

                
                float4 baseColor = tex2D(_MainTex, i.uv);
                return float4(baseColor * celLightning, 1);
            }
            ENDCG
        }
    }
}
