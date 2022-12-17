Shader "Unlit/ParticleToGeometry"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _Width ("_Width ", float) = 100
    }
    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType" = "Opaque"
            }
            LOD 200

            CGPROGRAM
            #pragma target 4.5
            #pragma vertex VS_Main
            #pragma geometry GS_Main
            #pragma fragment FS_Main
            #include "UnityCG.cginc"

            StructuredBuffer<float3> positions;
            uniform float3 targetPosition;
            
            uniform sampler2D _MainTex;
            uniform float _Width;

            struct appdata_input
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                uint id : SV_VertexID;
            };

            struct v2g
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct g2f
            {
                float4 pos : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            // Vertex Shader
            v2g VS_Main(appdata_input v)
            {
                v2g o = (v2g)0;
                float3 pos = positions[v.id] + targetPosition;
                v.vertex = float4(pos, 1.0);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.uv = v.uv;

                return o;
            }

            // Geometry Shader
            [maxvertexcount(6)]
            void GS_Main(point v2g input[1], inout TriangleStream<g2f> stream)
            {
                v2g p = input[0];

                float offsetX = _Width / _ScreenParams.x * p.pos.w * 0.5f;
                float offsetY = offsetX;
                float aspectReciprocal = _ScreenParams.y / _ScreenParams.x;
                offsetX *= aspectReciprocal;

                g2f pIn = (g2f)0;
                // 第一个
                pIn.pos = p.pos + float4(-offsetX, -offsetY, 0, 0);
                pIn.uv = half2(0, 0);
                stream.Append(pIn);
                pIn.pos = p.pos + float4(offsetX, -offsetY, 0, 0);
                pIn.uv = half2(1, 0);
                stream.Append(pIn);
                pIn.pos = p.pos + float4(-offsetX, offsetY, 0, 0);
                pIn.uv = half2(0, 1);
                stream.Append(pIn);
                stream.RestartStrip();

                // 第二个
                pIn.pos = p.pos + float4(-offsetX, offsetY, 0, 0);
                pIn.uv = half2(0, 1);
                stream.Append(pIn);
                pIn.pos = p.pos + float4(offsetX, -offsetY, 0, 0);
                pIn.uv = half2(1, 0);
                stream.Append(pIn);
                pIn.pos = p.pos + float4(offsetX, offsetY, 0, 0);
                pIn.uv = half2(1, 1);
                stream.Append(pIn);
                stream.RestartStrip();
            }

            // Fragment Shader
            float4 FS_Main(g2f i) : COLOR
            {
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}