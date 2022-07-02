Shader "Unlit/test"
{
	Properties
	{
		_RelativeRefractionIndex("Relative Refraction Index", Range(0.0, 1.0)) = 0.67
		[PowerSlider(5)]_Distance("Distance", Range(0.0, 100.0)) = 10.0
	}

		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

		Cull Back
		ZWrite On
		ZTest LEqual
		ColorMask RGB
		//�w�i���e�N�X�`���ɂ���
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

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//���[���h��Ԃł̍��W
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex);
				//���[���h��Ԃł̖@��
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				//�J�����f�B���N�V����
				half3 viewDir = normalize(worldPos - _WorldSpaceCameraPos.xyz);
				// ���܃f�B���N�V����
				half3 refractDir = refract(viewDir, worldNormal, _RelativeRefractionIndex);
				// ���ܕ����̐�ɂ���ʒu���T���v�����O�ʒu�Ƃ���
				half3 samplingPos = worldPos + refractDir * _Distance;
				// �T���v�����O�ʒu���v���W�F�N�V�����ϊ�
				half4 samplingScreenPos = mul(UNITY_MATRIX_VP, half4(samplingPos, 1.0));
				// �r���[�|�[�g���W�n�ɕϊ�
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
