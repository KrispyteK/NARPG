Shader "Unlit/Difference"
{
    Properties
    {
        _MainTex ("Current", 2D) = "white" {}
		_PrevTex ("Previous", 2D) = "white" {}
		_Threshold("Threshold", float) = 0.2
		_BlurSize("BlurSize", Range(0,1)) = 0.5
		_BlurAmount("BlurAmount", Range(0,100)) = 0.0005
		_BlurSamples ("BlurSamples", int) = 3
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
			float _BlurSize;
			int _BlurSamples;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			half4 blur(sampler2D tex, v2f i) {

				float2 uv = i.uv;

				half4 col = tex2D(tex, uv);
				int iteration = 0;

				for (int i = -_BlurSamples; i <= _BlurSamples; ++i) {
					for (int j = -_BlurSamples; j <= _BlurSamples; ++j) {
						float mul = _BlurSize / _BlurSamples;

						col += tex2D(tex, float2(uv.x + i * pow(mul,2), uv.y + j * pow(mul, 2)));

						iteration++;
					}
				}

				return col / iteration;
			}

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 colMain = blur(_MainTex, i);
				fixed4 colPrev = blur(_PrevTex, i);

				float grayMain = (colMain.r + colMain.g + colMain.b) / 3;
				float grayPrev = (colPrev.r + colPrev.g + colPrev.b) / 3;

				float difference = abs(grayMain - grayPrev);

				float dr = abs(colMain.r - colPrev.r);
				float dg = abs(colMain.g - colPrev.g);
				float db = abs(colMain.b - colPrev.b);

				float meanDifference = max(dr, max(dg, db));

				fixed4 col = fixed4(0, 0, 0, 1);

				if (dr > _Threshold || dg > _Threshold || db > _Threshold) {
					col = fixed4(1, 1, 1, 1);
				}

                return col;
            }
            ENDCG
        }
    }
}
