#ifndef SCORPIO_LIT_PASSS_INCLUDED
#define  SCORPIO_LIT_PASSS_INCLUDED

Varings LitPassVertex(Attributes input)
{
    Varings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS = TransformWorldToHClip(positionWS);
    output.normalWS   = TransformObjectToWorldNormal(input.normalOS);
    float4 baseST     = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
    output.baseUV     = input.baseUV * baseST.xy + baseST.zw;
    return output;
}

float4 LitPassFragment(Varings input) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input)
    float4 baseColor = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
    float4 baseMap   = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);
    float4 base      = baseMap * baseColor;
    
    #if defined(_CLIPPING)
    clip(base.a - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _Cutoff));
    #endif

    // base.rgb = normalize(input.normalWS);
    Surface surface;
    surface.albedo = base.rgb;
    surface.normal = normalize(input.normalWS);
    surface.alpha  = base.a;
    return float4(surface.albedo, surface.alpha);
}

#endif