Shader "Unlit/Difference"
{
    Properties
    {
        _MainTex ("Current", 2D) = "white" {}
		_PrevTex ("Previous", 2D) = "white" {}
		_Threshold("Threshold", float) = 0.2
		_BlurAmount("BlurAmount", Range(0,0.2)) = 0.0005
		_BlurSamples ("BlurSamples", Range(3,7)) = 3
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
			sampler2D _PrevTex;

            float4 _MainTex_ST;
			float _Threshold;
			float _BlurAmount;
			int _BlurSamples;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 blur(sampler2D tex, v2f i) {
				half4 texcol = half4(0,0,0,0);
				float remaining = 1.0f;
				float coef = 1;
				float fI = 0;
				for (int j = 0; j < _BlurSamples; j++) {
					fI++;
					coef *= 0.32;
					texcol += tex2D(tex, float2(i.uv.x, i.uv.y - fI * _BlurAmount)) * coef;
					texcol += tex2D(tex, float2(i.uv.x - fI * _BlurAmount, i.uv.y)) * coef;
					texcol += tex2D(tex, float2(i.uv.x + fI * _BlurAmount, i.uv.y)) * coef;
					texcol += tex2D(tex, float2(i.uv.x, i.uv.y + fI * _BlurAmount)) * coef;

					remaining -= 4 * coef;
				}
				texcol += tex2D(tex, float2(i.uv.x, i.uv.y)) * remaining;

				return texcol;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colMain = blur(_MainTex, i);
				fixed4 colPrev = blur(_PrevTex, i);

				float grayMain = (colMain.r + colMain.g + colMain.b) / 3;
				float grayPrev = (colPrev.r + colPrev.g + colPrev.b) / 3;

				float difference = abs(grayMain - grayPrev);

				fixed4 col = fixed4(0, 0, 0, 1);

				if (difference > _Threshold) {
					col = fixed4(1, 1, 1, 1);
				}

                return col;
            }
            ENDCG
        }
    }
}
