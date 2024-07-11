Shader "Unlit/blurEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

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

            sampler2D _MainTex;
            float _BlurSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 col = fixed4(0,0,0,0);

                // 简单的模糊算法，通过对周围像素的采样和平均来实现
                int amount = 10;
                for (int x = -amount; x <= amount; x++)
                {
                    for (int y = -amount; y <= amount; y++)
                    {
                        col += tex2D(_MainTex, uv + float2(x, y) * _BlurSize);
                    }
                }

                col /= (amount * 2 + 1) * (amount * 2 + 1);

                return col;
            }
            ENDCG
        }
    }
}