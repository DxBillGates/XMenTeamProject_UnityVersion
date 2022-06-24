Shader "Unlit/mask"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Threshold("threshold",Range(0.0,1.0)) = 0.0
		_Color("Color", Color) = (1,1,1,1)
		_OffsetX("OffsetControl",Float) = 0.0
	}
		SubShader
		{
		   Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off
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
				float4 _MainTex_ST;
				float _Threshold;
				float _OffsetX;
				fixed4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
				fixed4 mask = tex2D(_MainTex, i.uv + float2(_OffsetX,0));
				_Color.a = 1 - step(_Threshold,mask.r);
				return _Color;

				}
				ENDCG
			}
		}
}
