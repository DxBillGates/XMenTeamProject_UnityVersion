Shader "Unlit/shaderTest"
{
Properties{
    _MainTex("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1, 1, 1, 1)
    _Alpha("Alpha", Range(0, 1)) = 0
}
SubShader{
    Pass {
        //Queueの順番が正しくならないのでUnit側で設定が必要
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
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
            float3 viewDir : TEXCOORD1;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;

        v2f vert(appdata_full v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex);
            //ワールド行列に変換
            o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
            //カメラと頂点のディレクション
            o.viewDir = normalize(_WorldSpaceCameraPos - mul((float3x3)unity_ObjectToWorld, v.vertex));
            o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
            return o;
        }

        fixed4 _Color;
        fixed _Alpha;

        fixed4 frag(v2f i) : COLOR {
            //ディレクションと本線の内積が90度に近いほど光る(等倍)
            float val = 1 - abs(dot(i.viewDir, i.normal)) * _Alpha;
            return _Color * _Color.a * val * val * tex2D(_MainTex, i.uv);
        }

        ENDCG
    }
    }
        FallBack "Diffuse"
}