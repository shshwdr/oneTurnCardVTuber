Shader "Custom/DissolveGreyscaleToColor"
{
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0,1)) = 0
        _EdgeSoft ("Edge Softness", Range(0,1)) = 0.1
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Cull Off Lighting Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _Dissolve;
            float _EdgeSoft;
            float4 _MainTex_ST;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed noiseVal = tex2D(_NoiseTex, i.uv).r;
                // 计算灰度
                float gray = dot(col.rgb, fixed3(0.3, 0.59, 0.11));
                fixed3 greyscale = fixed3(gray, gray, gray);
                // 使用噪声值与 _Dissolve 参数比较，使用 smoothstep 实现柔和的边缘过渡
                float t = smoothstep(_Dissolve, _Dissolve + _EdgeSoft, noiseVal);
                fixed3 finalColor = lerp(greyscale, col.rgb, t);
                return fixed4(finalColor, col.a);
            }
            ENDCG
        }
    }
    FallBack "Sprites/Default"
}
