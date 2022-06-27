Shader "Custom/test"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "black" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		[Header(Adjust Effect Parameters)]
		_Speed("Effect Speed", Range(0, 30)) = 10
		_Start("Start", Range(0, 0.999)) = 0.5
		_TexCutoff("Texture Cutoff Alpha", Range(0, 1)) = 0.5
		_GlowCutoff("Glow Cutoff Alpha", Range(0, 1)) = 0.3
		[IntRange]_PixelLevel("Pixelization Level", Range(0, 512)) = 80
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

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float2 noise_uv : TEXCOORD1;
					float4 vertex : SV_POSITION;
					float4 objPos : TEXCOORD2;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _NoiseTex;
				float4 _NoiseTex_ST;
				fixed4 _GlowColor;
				fixed4 _Color;
				float _Speed;
				float _Start;
				float _TexCutoff;
				float _GlowCutoff;
				int _PixelLevel;

				//明るさのみ抽出
				float tex_brightness(fixed4 c)
				{
					return c.r * 0.3 + c.g * 0.59 + c.b * 0.11;
				}

				v2f vert(appdata v)
				{
					v2f o;
					o.objPos = v.vertex;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.noise_uv = TRANSFORM_TEX(v.uv, _NoiseTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float max_brightness = 0;
					float min_brightness = -1;

					// Set the y coordinate to [0, 1] in object space
					float y = i.objPos.y + 0.5;

					// Calculate the slope and offset of the fade-out equation
					float slope = (min_brightness - max_brightness) / (1 - _Start);
					float offset = max_brightness - slope * _Start;
					//時間経過*Speed
					float t = _Time.x * _Speed;
					//ノイズテクスチャを時間経過で動かして靡かせる
					float2 noise_uv = float2(i.noise_uv.x, i.noise_uv.y - t);
					//ばらけるパーツの大きさ
					noise_uv = floor(_PixelLevel * noise_uv) / _PixelLevel;
					//ノイズ用テクスチャの明るさだけ抽出
					float noise_alpha = tex_brightness(tex2D(_NoiseTex, noise_uv));

					float brightness = clamp(noise_alpha + slope * y + offset, 0, 1);
					//通常テクスチャが描画されるしきい値
					float tex_on = step(0.5, brightness);
					//光ってる部*通常の部分
					float glow_on = step(_GlowCutoff, brightness);

					return (tex2D(_MainTex, i.uv) * tex_on + glow_on * (1 - tex_on)) * _GlowColor;
				}

				ENDCG
			}
		}
}