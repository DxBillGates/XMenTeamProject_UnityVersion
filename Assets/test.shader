Shader "Custom/test"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Base Color", Color) = (1, 1, 1, 1)
		//ノイズテクスチャ
		_NoiseTex("Noise Texture", 2D) = "black" {}
		//境界線の色
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		[Header(Adjust Effect Parameters)]
		//蒸発する動きの速さ
		_Speed("Effect Speed", Range(0, 30)) = 10
		//消失するしきい値
		_Threshold("Threshold", Range(0, 0.999)) = 0.5
		//
		_TexCutoff("Texture Cutoff Alpha", Range(0, 1)) = 0.5
		//境界線のレンジ
		_GlowCutoff("Glow Cutoff Alpha", Range(0, 1)) = 0.3
		//崩れるパーツの大きさ
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

			//明るさのみ抽出
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
				//時間経過*Speed
				float time = _Time.x * _Speed;
				//時間経過でUVのYをスライド
				float2 noise_uv = float2(input.uv_NoiseTex.x, input.uv_NoiseTex.y - time);
				//ばらけるパーツの大きさ
				noise_uv = floor(_PixelLevel * noise_uv) / _PixelLevel;
				//ノイズ用テクスチャの明るさだけ抽出
				float noise_alpha = tex_brightness(tex2D(_NoiseTex, noise_uv));
				
				float brightness = saturate(noise_alpha + slope * y + offset);
				//通常テクスチャが描画されるしきい値
				float tex_on = step(0.5, brightness);
				//光ってる部*通常の部分
				float glow_on = step(_GlowCutoff, brightness) * (1 - tex_on);
				//
				o.Albedo = tex2D(_MainTex, input.uv) * _Color *(1 - glow_on);
				//
				o.Alpha = saturate(tex_on + glow_on);
				//境界線の光ってる部分
				o.Emission = _GlowColor * glow_on;
			}

			ENDCG
		}
}