Shader "Unlit/test"
{
	Properties
	{
		_RelativeRefractionIndex("Relative Refraction Index", Range(0.0, 1.0)) = 0.67
		[PowerSlider(5)]_Distance("Distance", Range(0.0, 100.0)) = 10.0
		_Radius("Radius",Range(0.0,10.0)) = 0

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

			struct appdata {
				half4 vertex                : POSITION;
				half4 texcoord              : TEXCOORD0;
				half3 normal                : NORMAL;
			};

			struct v2f {
				half4 vertex                : SV_POSITION;
				half2 samplingViewportPos   : TEXCOORD0;
			};

			sampler2D _GrabPassTexture;
			float _RelativeRefractionIndex;
			float _Distance;
			float _Radius;

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//ワールド空間での座標
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				//ワールド空間での法線
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				//カメラディレクション
				half3 viewDir = normalize(worldPos - _WorldSpaceCameraPos.xyz);

				//カメラからの内積
				float val = 1 - abs(dot(-viewDir, v.normal));

				// 屈折ディレクション
				half3 refractDir = refract(viewDir, worldNormal, _RelativeRefractionIndex);
				// 屈折方向の先にある位置をサンプリング位置とする
				half3 samplingPos = worldPos + refractDir * _Distance * pow(val, _Radius);
				// サンプリング位置をプロジェクション変換
				half4 samplingScreenPos = mul(UNITY_MATRIX_VP, half4(samplingPos, 1.0));
				// ビューポート座標系に変換
				o.samplingViewportPos = (samplingScreenPos.xy / samplingScreenPos.w) * 0.5 + 0.5;
			   #if UNITY_UV_STARTS_AT_TOP
					o.samplingViewportPos.y = 1.0 - o.samplingViewportPos.y;
			   #endif

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return tex2D(_GrabPassTexture, i.samplingViewportPos);
			}
			ENDCG
		}
	}
}
