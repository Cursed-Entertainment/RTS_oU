Shader "ExperimentalShaders/Wireframe-Transparent-Culled"
{
    Properties
    {
        _WireThickness ("Wire Thickness", Range(0, 800)) = 100
        _WireSmoothness ("Wire Smoothness", Range(0, 20)) = 3
        _WireColor ("Wire Color", Color) = (0.0, 1.0, 0.0, 1.0)
        _BaseColor ("Base Color", Color) = (0.0, 0.0, 0.0, 0.0)
        _MaxTriSize ("Max Tri Size", Range(0, 200)) = 25
    }

    SubShader
    {
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Wireframe.cginc"

            ENDCG
        }
    }
}
