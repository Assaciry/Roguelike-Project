﻿Shader "Unlit/Circle"
{
    Properties
    {
        _Color("Color", Color) = (1.0,1.0,1.0,1.0)
        _Radius("Radius", Float) = 5.0
        _Width("Width", Range(0.001,0.05)) = 0.001
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Radius;
            float _Width;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv - float2(0.5,0.5);

                float halfLine = _Width * 0.5;
                float len = length(uv);
                float circle = step(_Radius, len + halfLine) - step(_Radius, len - halfLine);

                float3 color = circle * _Color.xyz;
                clip(color - 0.1);

                return float4(circle.xxx, 1.0);

            }
            ENDCG
        }
    }
}
