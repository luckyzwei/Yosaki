// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:134,x:33062,y:32405,varname:node_134,prsc:2|emission-5785-OUT;n:type:ShaderForge.SFN_Tex2d,id:7546,x:31836,y:32194,ptovrint:False,ptlb:wing,ptin:_wing,varname:node_7546,prsc:2,tex:e6e56781bdd132d4d84a705bfda0bf7c,ntxv:0,isnm:False|UVIN-8473-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3335,x:31989,y:32496,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_3335,prsc:2,tex:02178d1001db3f24ba197af4a3c6cc4c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3177,x:32129,y:32266,varname:node_3177,prsc:2|A-7546-RGB,B-7546-A,C-8008-RGB;n:type:ShaderForge.SFN_Multiply,id:8988,x:32619,y:32296,varname:node_8988,prsc:2|A-3177-OUT,B-3335-RGB,C-6812-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6812,x:32368,y:32469,ptovrint:False,ptlb:wing_intensity,ptin:_wing_intensity,varname:node_6812,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Panner,id:8473,x:31658,y:32201,varname:node_8473,prsc:2,spu:-1,spv:0|UVIN-6143-OUT,DIST-9079-OUT;n:type:ShaderForge.SFN_Time,id:5205,x:31284,y:32230,varname:node_5205,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:245,x:31999,y:32706,ptovrint:False,ptlb:dust,ptin:_dust,varname:node_245,prsc:2,tex:21d26b3be32ee1d41b3a6653df9d257f,ntxv:0,isnm:False|UVIN-9924-UVOUT;n:type:ShaderForge.SFN_Panner,id:9924,x:31820,y:32701,varname:node_9924,prsc:2,spu:-1,spv:0|UVIN-4300-OUT,DIST-7302-OUT;n:type:ShaderForge.SFN_TexCoord,id:6521,x:31448,y:32651,varname:node_6521,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:4300,x:31649,y:32702,varname:node_4300,prsc:2|A-6521-UVOUT,B-7947-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7947,x:31463,y:32845,ptovrint:False,ptlb:dust_UV,ptin:_dust_UV,varname:node_7947,prsc:2,glob:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:2045,x:31287,y:32396,ptovrint:False,ptlb:wing_speed,ptin:_wing_speed,varname:node_2045,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:9079,x:31484,y:32313,varname:node_9079,prsc:2|A-5205-T,B-2045-OUT;n:type:ShaderForge.SFN_Add,id:5785,x:32814,y:32511,varname:node_5785,prsc:2|A-8988-OUT,B-1774-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1487,x:32012,y:32924,ptovrint:False,ptlb:dust_intensity,ptin:_dust_intensity,varname:node_1487,prsc:2,glob:False,v1:5;n:type:ShaderForge.SFN_Multiply,id:7302,x:31657,y:32907,varname:node_7302,prsc:2|A-7650-T,B-2600-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2600,x:31445,y:33060,ptovrint:False,ptlb:dust_speed,ptin:_dust_speed,varname:node_2600,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Time,id:7650,x:31442,y:32911,varname:node_7650,prsc:2;n:type:ShaderForge.SFN_Color,id:7807,x:32332,y:32882,ptovrint:False,ptlb:dust_color,ptin:_dust_color,varname:node_7807,prsc:2,glob:False,c1:1,c2:0.25,c3:0.25,c4:1;n:type:ShaderForge.SFN_Multiply,id:1774,x:32628,y:32690,varname:node_1774,prsc:2|A-6340-OUT,B-7807-RGB;n:type:ShaderForge.SFN_Multiply,id:6340,x:32349,y:32686,varname:node_6340,prsc:2|A-3177-OUT,B-245-RGB,C-3335-RGB,D-1487-OUT;n:type:ShaderForge.SFN_Color,id:8008,x:31841,y:32383,ptovrint:False,ptlb:wing_color,ptin:_wing_color,varname:node_8008,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_TexCoord,id:9124,x:31128,y:32047,varname:node_9124,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:6143,x:31372,y:32015,varname:node_6143,prsc:2|A-9124-UVOUT,B-9278-OUT;n:type:ShaderForge.SFN_ValueProperty,id:9278,x:31105,y:32234,ptovrint:False,ptlb:wing_uv,ptin:_wing_uv,varname:node_9278,prsc:2,glob:False,v1:1;proporder:7546-8008-9278-6812-2045-245-7807-7947-1487-2600-3335;pass:END;sub:END;*/

Shader "Shader Forge/fx_wing_a" {
    Properties {
        _wing ("wing", 2D) = "white" {}
        _wing_color ("wing_color", Color) = (1,1,1,1)
        _wing_uv ("wing_uv", Float ) = 1
        _wing_intensity ("wing_intensity", Float ) = 1
        _wing_speed ("wing_speed", Float ) = 1
        _dust ("dust", 2D) = "white" {}
        _dust_color ("dust_color", Color) = (1,0.25,0.25,1)
        _dust_UV ("dust_UV", Float ) = 2
        _dust_intensity ("dust_intensity", Float ) = 5
        _dust_speed ("dust_speed", Float ) = 1
        _mask ("mask", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _wing; uniform float4 _wing_ST;
            uniform sampler2D _mask; uniform float4 _mask_ST;
            uniform float _wing_intensity;
            uniform sampler2D _dust; uniform float4 _dust_ST;
            uniform float _dust_UV;
            uniform float _wing_speed;
            uniform float _dust_intensity;
            uniform float _dust_speed;
            uniform float4 _dust_color;
            uniform float4 _wing_color;
            uniform float _wing_uv;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
/////// Vectors:
////// Lighting:
////// Emissive:
                float4 node_5205 = _Time + _TimeEditor;
                float2 node_8473 = ((i.uv0*_wing_uv)+(node_5205.g*_wing_speed)*float2(-1,0));
                float4 _wing_var = tex2D(_wing,TRANSFORM_TEX(node_8473, _wing));
                float3 node_3177 = (_wing_var.rgb*_wing_var.a*_wing_color.rgb);
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float4 node_7650 = _Time + _TimeEditor;
                float2 node_9924 = ((i.uv0*_dust_UV)+(node_7650.g*_dust_speed)*float2(-1,0));
                float4 _dust_var = tex2D(_dust,TRANSFORM_TEX(node_9924, _dust));
                float3 emissive = ((node_3177*_mask_var.rgb*_wing_intensity)+((node_3177*_dust_var.rgb*_mask_var.rgb*_dust_intensity)*_dust_color.rgb));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
