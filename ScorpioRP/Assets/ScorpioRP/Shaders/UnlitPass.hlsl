#ifndef SCORPIORP_UNLIT_PASS_INCLUDED
#define SCORPIORP_UNLIT_PASS_INCLUDED

Varings UnlitPassVertex(Attributes input)
{
    Varings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS = TransformWorldToHClip(positionWS);
    return output;
}

float4 UnlitPassFragment(Varings input) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input)
    float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
    return baseColor;
}

#endif
