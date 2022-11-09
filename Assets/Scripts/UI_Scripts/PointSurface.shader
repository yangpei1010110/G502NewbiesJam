Shader "Custom/PointSurface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;

        // UNITY_INSTANCING_BUFFER_START(Props)
        // UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input input, inout SurfaceOutputStandard o)
        {
            // o.Albedo = input.worldPos;
            // o.Albedo = input.worldPos * 0.5f + 0.5f;
            // o.Albedo.rg = input.worldPos.xy;
            o.Albedo = input.worldPos * 0.5f + 0.5f;

            o.Albedo = saturate(o.Albedo);
            o.Smoothness = _Smoothness;
            // o.Alpha = 1.0f;
        }
        ENDCG
    }
    FallBack "Diffuse"
}