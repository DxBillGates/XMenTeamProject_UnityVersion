Shader "Unlit/postEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Intencity("Intencity",Range(-1.0, 1.0)) = 0.0
		_Radius("Radius",Range(0,1.0))=0.1
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
				float _Intencity;
				float _Radius;

				//ñæÇÈÇ≥ÇÃÇ›íäèo
				float tex_brightness(fixed4 c)
				{
					return c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
				}


				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					//åvéZÇ≈â~Çï`Ç≠
					fixed radius = 1 - _Radius;
					fixed r = 1 - distance(i.uv, fixed2(0.5, 0.5));
					fixed4 circle = step(radius, r);

					//òpã»
					float2 resultUv = i.uv;
					resultUv -= float2(0.5, 0.5);
					//òpã»ÇÃã≠Ç≥ÇÃåvéZ
					float distPower = pow(length(resultUv), _Intencity * tex_brightness(circle));
					resultUv *= float2(distPower,distPower);
					resultUv += float2(0.5 , 0.5);
					// sample the texture
					fixed4 col = tex2D(_MainTex, resultUv);

					return col;
				}
				ENDCG
			}
		}
}
