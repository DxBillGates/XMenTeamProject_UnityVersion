Shader "Custom/dome"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "black" {}
		//�x�[�X�J���[
		_Color("Color", Color) = (1, 1, 1, 1)
		//���E�������̃J���[
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		[Header(Adjust Effect Parameters)]
		//�p�[�e�B�N���̓����̑���
		_Speed("Effect Speed", Range(0, 30)) = 10
		//�������l
		_Threshold("Threshold", Range(0, 0.999)) = 0.5
		//���C���e�N�X�`���̕����̋���(������_Threshold��0�ł��m�C�Y���f��)
		_TexCutoff("Texture Cutoff Alpha", Range(0, 1)) = 0.5
		//���E���̑傫��
		_GlowCutoff("Glow Cutoff Alpha", Range(0, 1)) = 0.3
		//���E���̃s�N�Z���̑傫��
		[IntRange]_PixelLevel("Pixelization Level", Range(0, 512)) = 80
		//
		_Alpha("Alpha", Range(0, 1)) = 0
	}

		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}
			Cull Off
			Blend One One
			Fog { Mode Off }
			Lighting Off
			ZWrite Off


			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 noise_uv : TEXCOORD1;
					float4 vertex : SV_POSITION;
					float4 objPos : TEXCOORD2;
					float3 normal : NORMAL;
					float3 dir : TEXCOORD3;

				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _NoiseTex;
				float4 _NoiseTex_ST;
				fixed4 _GlowColor;
				fixed4 _Color;
				float _Speed;
				float _Threshold;
				float _TexCutoff;
				float _GlowCutoff;
				int _PixelLevel;
				float _Alpha;

				//���邳�̂ݒ��o
				float tex_brightness(fixed4 c)
				{
					return c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
				}

				v2f vert(appdata_full v)
				{
					v2f o;
					o.objPos = v.vertex;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.noise_uv = TRANSFORM_TEX(v.texcoord, _NoiseTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					//���[���h�s��ɕϊ�
					o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
					//�J�����ƒ��_�̃f�B���N�V����
					o.dir = normalize(_WorldSpaceCameraPos - mul((float3x3)unity_ObjectToWorld, v.vertex));

					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float max_brightness = 0;
					float min_brightness = -1;

					float slope = (min_brightness - max_brightness) / (1 - _Threshold);
					float offset = max_brightness-0.5 - slope * _Threshold;
					//���Ԍo��*Speed
					float t = _Time.x * _Speed;
					//�m�C�Y�e�N�X�`�������Ԍo�߂œ��������r������
					float2 noise_uv = float2(i.noise_uv.x, i.noise_uv.y - t);
					//�΂炯��p�[�c�̑傫��
					noise_uv = floor(_PixelLevel * noise_uv) / _PixelLevel;
					//�m�C�Y�p�e�N�X�`���̖��邳�������o
					float noise_alpha = tex_brightness(tex2D(_NoiseTex, noise_uv));

					float brightness = clamp(noise_alpha + slope * i.objPos.y + offset, 0, 1);
					//�ʏ�e�N�X�`�����`�悳��邵�����l
					float tex_on = step(_TexCutoff, brightness);

					//�����Ă镔*�ʏ�̕���
					float glow_on = step(_GlowCutoff, brightness);
					
					/*�h�[���̌����ڂ̌v�Z*/
					//�f�B���N�V�����Ɩ@���̓��ς�0�ɋ߂��قǐF����
					float val = 1 - abs(dot(i.dir, i.normal)) * _Alpha;
					//�e�N�X�`���ƌv�Z���ʂ�_Color��������

					return (tex2D(_MainTex, i.uv) * val * val * tex_on + glow_on * (1 - tex_on)) * _GlowColor;
				}

				ENDCG
			}
		}
			FallBack "Diffuse"
}