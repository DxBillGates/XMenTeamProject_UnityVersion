Shader "Custom/dome"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "black" {}
		//ベースカラー
		_Color("Color", Color) = (1, 1, 1, 1)
		//境界線部分のカラー
		_GlowColor("Glow Color", Color) = (1, 1, 1, 1)
		[Header(Adjust Effect Parameters)]
		//パーティクルの動きの速さ
		_Speed("Effect Speed", Range(0, 30)) = 10
		//しきい値
		_Threshold("Threshold", Range(0, 0.999)) = 0.5
		//メインテクスチャの部分の強さ(強いと_Thresholdが0でもノイズが映る)
		_TexCutoff("Texture Cutoff Alpha", Range(0, 1)) = 0.5
		//境界線の大きさ
		_GlowCutoff("Glow Cutoff Alpha", Range(0, 1)) = 0.3
		//境界線のピクセルの大きさ
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

				//明るさのみ抽出
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
					//ワールド行列に変換
					o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
					//カメラと頂点のディレクション
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
					//時間経過*Speed
					float t = _Time.x * _Speed;
					//ノイズテクスチャを時間経過で動かして靡かせる
					float2 noise_uv = float2(i.noise_uv.x, i.noise_uv.y - t);
					//ばらけるパーツの大きさ
					noise_uv = floor(_PixelLevel * noise_uv) / _PixelLevel;
					//ノイズ用テクスチャの明るさだけ抽出
					float noise_alpha = tex_brightness(tex2D(_NoiseTex, noise_uv));

					float brightness = clamp(noise_alpha + slope * i.objPos.y + offset, 0, 1);
					//通常テクスチャが描画されるしきい値
					float tex_on = step(_TexCutoff, brightness);

					//光ってる部*通常の部分
					float glow_on = step(_GlowCutoff, brightness);
					
					/*ドームの見た目の計算*/
					//ディレクションと法線の内積が0に近いほど色がつく
					float val = 1 - abs(dot(i.dir, i.normal)) * _Alpha;
					//テクスチャと計算結果と_Colorをかける

					return (tex2D(_MainTex, i.uv) * val * val * tex_on + glow_on * (1 - tex_on)) * _GlowColor;
				}

				ENDCG
			}
		}
			FallBack "Diffuse"
}