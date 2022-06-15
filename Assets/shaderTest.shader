Shader "Unlit/shaderTest"
{
Properties{
    _MainTex("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1, 1, 1, 1)
    _Alpha("Alpha", Range(0, 1)) = 0
}
SubShader{
    Pass {
        //Queueの順番が正しくならないのでUnity側で設定が必要
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
        Blend One One
        Cull Off
        ZWrite Off

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag

        #include "UnityCG.cginc"

        struct v2f {
            float4 pos : SV_POSITION;
            float3 normal : NORMAL;
            float2 uv : TEXCOORD0;
            float3 dir : TEXCOORD1;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;

        v2f vert(appdata_full v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            //ワールド行列に変換
            o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
            //カメラと頂点のディレクション
            o.dir = normalize(_WorldSpaceCameraPos - mul((float3x3)unity_ObjectToWorld, v.vertex));
            //タイリングとオフセットの値をテクスチャのUVに適応させる
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }

        fixed4 _Color;
        fixed _Alpha;

        fixed4 frag(v2f i) : COLOR {
            //ディレクションと法線の内積が0に近いほど色がつく
            float val = 1 - abs(dot(i.dir, i.normal)) * _Alpha;
            //テクスチャと計算結果と_Colorをかけ合わせる
            return _Color * _Color.a * val * val* val * tex2D(_MainTex, i.uv);
        }

        ENDCG
    }
    }
        FallBack "Diffuse"
}