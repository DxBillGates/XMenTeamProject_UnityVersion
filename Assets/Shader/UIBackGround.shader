Shader "Hidden/UIBackGround"
{
	Properties
	{
		_Threshold("Threshold",Range(0.001,0.03)) = 0.01
				_Color("Color", Color) = (1, 1, 1, 1)
	}

		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGB
		//背景をテクスチャにする
		GrabPass { "_GrabPassTexture" }

		Pass {

			CGPROGRAM
		   #pragma vertex vert
		   #pragma fragment frag

		   #include "UnityCG.cginc"


			sampler2D _GrabPassTexture;
			float _Threshold;
			fixed4 _Color;
			struct v2f {
				half4 vertex                : SV_POSITION;
				half2 grabPos               : TEXCOORD0;
			};

			v2f vert(float4 vertex : POSITION)
			{
				v2f o = (v2f)0;
				// まずUnityObjectToClipPos
				o.vertex = UnityObjectToClipPos(vertex);
				// GrabPassのテクスチャをサンプリングするUV座標はComputeGrabScreenPosで求める
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			fixed Gaussian(float2 drawUV, float2 pickUV, float sigma)
			{
				float d = distance(drawUV, pickUV);
				return exp(-(d * d) / (2 * sigma * sigma));
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 grabColor=tex2D(_GrabPassTexture, i.grabPos);

			float totalWeight = 0, _Sigma = _Threshold, _StepWidth = 0.001;
			float4 col = fixed4(0, 0, 0, 0);
			for (float py = -_Sigma * 2; py <= _Sigma * 2; py += _StepWidth)
			{
				for (float px = -_Sigma * 2; px <= _Sigma * 2; px += _StepWidth)
				{
					float2 pickUV = i.grabPos + float2(px, py);
					fixed weight = Gaussian(i.grabPos, pickUV, _Sigma);
					col += tex2D(_GrabPassTexture, pickUV) * weight;
					totalWeight += weight;
				}
			}
			col.rgb = col.rgb / totalWeight;
			return col*_Color;
			}
			ENDCG
		}
	}
}
