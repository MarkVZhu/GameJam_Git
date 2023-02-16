Shader "Unlit/NewUnlitShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Wave("Wave",float) = 60
        _Speed("Speed",float) = 1
        _BackgroundAlpha("BackgroundAlpha",float) = 0.3
        _InnerColor("InnerColor", Color) = (1,1,1,1)
        _BorderColor("BorderColor", Color) = (1,1,1,1)
        _BackgroundColor("BackgroundColor", Color) = (1,1,1,1)

    }
    SubShader {
        Tags {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
        }
        LOD 100
        Tags {
            "LightMode"="ForwardBase"
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 posWS : TEXCOORD1;
                // float4 lrud : TEXCOORD2;
            };

            UNITY_DEFINE_INSTANCED_PROP(float4, _lrud)

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float4    _InnerColor;
            float4    _BorderColor;
            float4    _BackgroundColor;
            float     _Wave;
            float     _Speed;
            float     _BackgroundAlpha;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.posWS = mul(unity_ObjectToWorld, v.vertex).xyz;

                // float3 pos1 = float3(0, 0, 1);
                // float3 pos2 = float3(1, 1, 0);
                // float3 pos1w = mul(unity_ObjectToWorld, pos1).xyz;
                // float3 pos2w = mul(unity_ObjectToWorld, pos2).xyz;
                // pos2w = float3(100000, 100000, 100000);
                // o.lrud.xz = pos1w.xy;
                // o.lrud.yw = pos2w.xy;

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float dis(float3 posWS, float4 lrud)
            {
                float dis1 = min(abs(posWS.x - lrud.x), abs(posWS.x - lrud.y)) * 3;
                dis1 = clamp(dis1, 0, 1);
                float dis2 = min(abs(posWS.y - lrud.z), abs(posWS.y - lrud.w)) * 3;
                dis2 = clamp(dis2, 0, 1);
                return sqrt(dis1 * dis2);
                // return pow(dis1* dis2,1.0/4);
                // return 2-2.0/dis1;
                // return dis1;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 lrud = UNITY_ACCESS_INSTANCED_PROP(Props, _lrud);
                float  distance = dis(i.posWS, lrud);
                float  a = clamp(10 * (distance - 0.33), 0, 1);
                // sample the texture
                float4 inner = _InnerColor;
                inner.w *= clamp(1.2 * sin(_Wave * (i.posWS.x + i.posWS.y + _Speed * _Time.y)), 0, 1) * a;
                inner.w = pow(inner.w, 3);

                float4 border = _BorderColor;
                border.w *= clamp(1 - pow(5 * (distance - 0.3), 6), 0, 1);

                float4 mix = border * border.w + inner * (1 - border.w);


                // fixed4 col = mix;

                float4 col = _BackgroundColor;
                col.w = _BackgroundAlpha * a;
                col = col * (1 - mix.w) + mix * mix.w;
                // col.w = mix.w + (1 - mix.w) * col.w;
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}