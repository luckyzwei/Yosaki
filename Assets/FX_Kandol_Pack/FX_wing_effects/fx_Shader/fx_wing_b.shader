// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.06 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.06;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,dith:0,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:134,x:33042,y:32501,varname:node_134,prsc:2|emission-351-OUT;n:type:ShaderForge.SFN_Tex2d,id:7546,x:32101,y:32262,ptovrint:False,ptlb:wing,ptin:_wing,varname:node_7546,prsc:2,tex:e6e56781bdd132d4d84a705bfda0bf7c,ntxv:0,isnm:False|UVIN-8473-UVOUT;n:type:ShaderForge.SFN_Tex2d,id:3335,x:32202,y:32781,ptovrint:False,ptlb:mask,ptin:_mask,varname:node_3335,prsc:2,tex:02178d1001db3f24ba197af4a3c6cc4c,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:3177,x:32394,y:32334,varname:node_3177,prsc:2|A-7546-RGB,B-7546-A,C-8008-RGB,D-6812-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6812,x:32157,y:32487,ptovrint:False,ptlb:wing_intensity,ptin:_wing_intensity,varname:node_6812,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Panner,id:8473,x:31936,y:32262,varname:node_8473,prsc:2,spu:-1,spv:0|UVIN-6614-OUT,DIST-9079-OUT;n:type:ShaderForge.SFN_Time,id:5205,x:31417,y:32263,varname:node_5205,prsc:2;n:type:ShaderForge.SFN_Tex2d,id:245,x:31756,y:32622,ptovrint:False,ptlb:dust,ptin:_dust,varname:node_245,prsc:2,tex:21d26b3be32ee1d41b3a6653df9d257f,ntxv:0,isnm:False|UVIN-9924-UVOUT;n:type:ShaderForge.SFN_Panner,id:9924,x:31577,y:32617,varname:node_9924,prsc:2,spu:-1,spv:0|UVIN-4300-OUT,DIST-7302-OUT;n:type:ShaderForge.SFN_TexCoord,id:6521,x:31191,y:32498,varname:node_6521,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:4300,x:31406,y:32618,varname:node_4300,prsc:2|A-6521-UVOUT,B-7947-OUT;n:type:ShaderForge.SFN_ValueProperty,id:7947,x:31179,y:32690,ptovrint:False,ptlb:dust_UV,ptin:_dust_UV,varname:node_7947,prsc:2,glob:False,v1:2;n:type:ShaderForge.SFN_ValueProperty,id:2045,x:31427,y:32420,ptovrint:False,ptlb:wing_speed,ptin:_wing_speed,varname:node_2045,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:9079,x:31617,y:32346,varname:node_9079,prsc:2|A-5205-T,B-2045-OUT;n:type:ShaderForge.SFN_Multiply,id:2736,x:32390,y:32583,varname:node_2736,prsc:2|A-245-RGB,B-1487-OUT,C-3335-RGB,D-2884-RGB;n:type:ShaderForge.SFN_ValueProperty,id:1487,x:31953,y:32712,ptovrint:False,ptlb:dust_intensity,ptin:_dust_intensity,varname:node_1487,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:7302,x:31414,y:32823,varname:node_7302,prsc:2|A-7650-T,B-2600-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2600,x:31202,y:32976,ptovrint:False,ptlb:dust_speed,ptin:_dust_speed,varname:node_2600,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_Time,id:7650,x:31199,y:32827,varname:node_7650,prsc:2;n:type:ShaderForge.SFN_Color,id:8008,x:31938,y:32445,ptovrint:False,ptlb:wing_color,ptin:_wing_color,varname:node_8008,prsc:2,glob:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Multiply,id:351,x:32805,y:32589,varname:node_351,prsc:2|A-723-OUT,B-3335-RGB;n:type:ShaderForge.SFN_Add,id:723,x:32586,y:32427,varname:node_723,prsc:2|A-3177-OUT,B-2736-OUT;n:type:ShaderForge.SFN_Color,id:2884,x:31923,y:32884,ptovrint:False,ptlb:dust_color,ptin:_dust_color,varname:node_2884,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:6614,x:31432,y:32081,varname:node_6614,prsc:2|A-624-UVOUT,B-2953-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2953,x:31200,y:32201,ptovrint:False,ptlb:wing_UV,ptin:_wing_UV,varname:node_2953,prsc:2,glob:False,v1:1;n:type:ShaderForge.SFN_TexCoord,id:624,x:31183,y:31999,varname:node_624,prsc:2,uv:0;proporder:7546-2953-8008-6812-2045-245-2884-7947-1487-2600-3335;pass:END;sub:END;*/

Shader "Shader Forge/fx_wing_a" {
    Properties {
        _wing ("wing", 2D) = "white" {}
        _wing_UV ("wing_UV", Float ) = 1
        _wing_color ("wing_color", Color) = (1,1,1,1)
        _wing_intensity ("wing_intensity", Float ) = 1
        _wing_speed ("wing_speed", Float ) = 1
        _dust ("dust", 2D) = "white" {}
        _dust_color ("dust_color", Color) = (0.5,0.5,0.5,1)
        _dust_UV ("dust_UV", Float ) = 2
        _dust_intensity ("dust_intensity", Float ) = 1
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
            uniform float4 _wing_color;
            uniform float4 _dust_color;
            uniform float _wing_UV;
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
                float2 node_6614 = (i.uv0*_wing_UV);
                float2 node_8473 = (node_6614+(node_5205.g*_wing_speed)*float2(-1,0));
                float4 _wing_var = tex2D(_wing,TRANSFORM_TEX(node_8473, _wing));
                float4 node_7650 = _Time + _TimeEditor;
                float2 node_9924 = ((i.uv0*_dust_UV)+(node_7650.g*_dust_speed)*float2(-1,0));
                float4 _dust_var = tex2D(_dust,TRANSFORM_TEX(node_9924, _dust));
                float4 _mask_var = tex2D(_mask,TRANSFORM_TEX(i.uv0, _mask));
                float3 emissive = (((_wing_var.rgb*_wing_var.a*_wing_color.rgb*_wing_intensity)+(_dust_var.rgb*_dust_intensity*_mask_var.rgb*_dust_color.rgb))*_mask_var.rgb);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
