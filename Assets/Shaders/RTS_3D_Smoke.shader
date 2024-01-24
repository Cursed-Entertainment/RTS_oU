Shader "RTS_Engine/RTS_Shader" {
    Properties {
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
        _Emission ("Emissive Color", Color) = (0, 0, 0, 0)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.7
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader {
        Tags {
            "Queue" = "Overlay"
        }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        fixed4 _SpecColor;
        fixed4 _Emission;
        float _Shininess;

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            o.Albedo = c.rgb;
            o.Alpha = c.a;
            o.Specular = _SpecColor.rgb * _Shininess;
            o.Emission = _Emission.rgb;

            o.Normal = float3(0, 0, 1);
        }
        ENDCG
    }

    Fallback "Diffuse"
}
