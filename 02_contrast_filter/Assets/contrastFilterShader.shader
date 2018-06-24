Shader "Hidden/Custom/Contrast"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float _Contrast;


	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float factor = (1.01568f * (_Contrast + 1.0f)) / (1.0f * (1.01568f - _Contrast));
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		color = saturate(factor * (color - 0.5f) + 0.5f);
		color.w = 1.0f;

		/*
		float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
		color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
		*/
		return color;
	}

	ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

#pragma vertex VertDefault
#pragma fragment Frag

			ENDHLSL
		}
	}
}