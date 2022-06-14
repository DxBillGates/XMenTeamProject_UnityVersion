Shader "Unlit/shaderTest"
{
Properties{
    _MainTex("Texture", 2D) = "white" {}
    _Color("Color", Color) = (1, 1, 1, 1)
    _Alpha("Alpha", Range(0, 1)) = 0
}
SubShader{
    Pass {
        //Queue�̏��Ԃ��������Ȃ�Ȃ��̂�Unit���Őݒ肪�K�v
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
            //���[���h�s��ɕϊ�
            o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal.xyz));
            //�J�����ƒ��_�̃f�B���N�V����
            o.viewDir = normalize(_WorldSpaceCameraPos - mul((float3x3)unity_ObjectToWorld, v.vertex));
            o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
            return o;
        }

        fixed4 _Color;
        fixed _Alpha;

        fixed4 frag(v2f i) : COLOR {
            //�f�B���N�V�����Ɩ{���̓��ς�90�x�ɋ߂��قǌ���(���{)
            float val = 1 - abs(dot(i.viewDir, i.normal)) * _Alpha;
            return _Color * _Color.a * val * val * tex2D(_MainTex, i.uv);
        }

        ENDCG
    }
    }
        FallBack "Diffuse"
}