// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/Mobile_Additive_Stencil" {
Properties {
    _MainTex ("Particle Texture", 2D) = "white" {}
_Stencil("Stencil ID", Float) = 0
_StencilOp("_StencilOp", Float) = 0
_StencilComp("_StencilComp", Float) = 0
 _StencilReadMask(" _StencilReadMask", Float) = 0
 _StencilWriteMask(" _StencilWriteMask", Float) = 0
 _ColorMask(" _ColorMask", Float) = 0
}

Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
    Blend SrcAlpha One
    Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

    BindChannels {
        Bind "Color", color
        Bind "Vertex", vertex
        Bind "TexCoord", texcoord
    }

    SubShader {
        Pass {
            SetTexture [_MainTex] {
                combine texture * primary
            }
        }
    }
}
}
