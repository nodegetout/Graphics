#ifndef SCORPIORP_UNLIT_INPUT_INCLUDED
#define  SCORPIORP_UNLIT_INPUT_INCLUDED

#include "../ShaderLibrary/Common.hlsl"

// CBUFFER_START(UnityPerMaterial)
//     float4 _BaseColor;
// CBUFFER_END

struct Attributes
{
    float3 positionOS : POSITION;
    float2 baseUV     : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varings
{
    float4 positionCS : SV_POSITION;
    float2 baseUV     : VAR_BASE_UV;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DEFINE_INSTANCED_PROP(float, _Cutoff)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

#endif