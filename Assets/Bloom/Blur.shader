Shader "Unlit/Blur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Threshold("Threshold",Range(0.001,0.03)) = 0.01

	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

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
				float4 _MainTex_ST;
				float _Threshold;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed Gaussian(float2 drawUV, float2 pickUV, float sigma)
				{
					float d = distance(drawUV, pickUV);
					return exp(-(d * d) / (2 * sigma * sigma));
				}
				fixed4 frag(v2f i) : SV_Target
				{
				   float totalWeight = 0,_Sigma = _Threshold, _StepWidth= 0.001;
				float4 col = fixed4(0, 0, 0, 0);
				for (float py = -_Sigma * 2; py <= _Sigma * 2; py += _StepWidth)
				{
					for (float px = -_Sigma * 2; px <= _Sigma * 2; px += _StepWidth)
					{
						float2 pickUV = i.uv + float2(px, py);
						fixed weight = Gaussian(i.uv, pickUV, _Sigma);
						col += tex2D(_MainTex, pickUV) * weight;
						totalWeight += weight;
					}
				}
				col.rgb = col.rgb / totalWeight;
				return col;

				}
				ENDCG
			}
		}
}
