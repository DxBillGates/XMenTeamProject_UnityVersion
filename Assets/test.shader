Shader "Custom/test"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Base Color", Color) = (1, 1, 1, 1)
		//�m�C�Y�e�N�X�`��
		_NoiseTex("Noise Texture", 2D) = "black" {}
		//���E���̐F
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		[Header(Adjust Effect Parameters)]
		//�������铮���̑���
		_Speed("Effect Speed", Range(0, 30)) = 10
		//�������邵�����l
		_Threshold("Threshold", Range(0, 0.999)) = 0.5
		//
		_TexCutoff("Texture Cutoff Alpha", Range(0, 1)) = 0.5
		//���E���̃����W
		_GlowCutoff("Glow Cutoff Alpha", Range(0, 1)) = 0.3
		//�����p�[�c�̑傫��
		[IntRange]_PixelLevel("Pixelization Level", Range(0, 512)) = 80
	}

		SubShader
		{
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			CGPROGRAM

			#pragma surface surf BlinnPhong fullforwardshadows vertex:vert alpha:fade
			#pragma target 3.0

			struct Input
			{
				float2 uv;
				float2 uv_NoiseTex;
				float3 objPos;
			};

			void vert(inout appdata_full v, out Input o)
			{
				UNITY_INITIALIZE_OUTPUT(Input, o);
				o.objPos = v.vertex;
			}

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			fixed4 _Color;
			fixed4 _GlowColor;
			float _Speed;
			float _Threshold;
			float _TexCutoff;
			float _GlowCutoff;
			int _PixelLevel;

			//���邳�̂ݒ��o
			float tex_brightness(fixed4 c)
			{
				return c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
			}

			void surf(Input input, inout SurfaceOutput o)
			{
				float max_brightness = 0;
				float min_brightness = -1;

				// Set the y coordinate to [0, 1] in object space
				float y = input.objPos.y + 0.5;

				// 
				float slope = (min_brightness - max_brightness) / (1 - _Threshold);
				float offset = max_brightness - slope * _Threshold;
				//���Ԍo��*Speed
				float time = _Time.x * _Speed;
				//���Ԍo�߂�UV��Y���X���C�h
				float2 noise_uv = float2(input.uv_NoiseTex.x, input.uv_NoiseTex.y - time);
				//�΂炯��p�[�c�̑傫��
				noise_uv = floor(_PixelLevel * noise_uv) / _PixelLevel;
				//�m�C�Y�p�e�N�X�`���̖��邳�������o
				float noise_alpha = tex_brightness(tex2D(_NoiseTex, noise_uv));
				
				float brightness = saturate(noise_alpha + slope * y + offset);
				//�ʏ�e�N�X�`�����`�悳��邵�����l
				float tex_on = step(0.5, brightness);
				//�����Ă镔*�ʏ�̕���
				float glow_on = step(_GlowCutoff, brightness) * (1 - tex_on);
				//
				o.Albedo = tex2D(_MainTex, input.uv) * _Color *(1 - glow_on);
				//
				o.Alpha = saturate(tex_on + glow_on);
				//���E���̌����Ă镔��
				o.Emission = _GlowColor * glow_on;
			}

			ENDCG
		}
}