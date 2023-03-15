Shader "Hidden/Pixelize"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"
    }

        SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag
        #pragma target 3.0

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct Attributes
        {
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionHCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

        //SAMPLER(sampler_MainTex);
        //Texture2D _MainTex;
        //SamplerState sampler_MainTex;

        SamplerState sampler_point_clamp;

        uniform float2 _BlockCount;
        uniform float2 _BlockSize;
        uniform float2 _HalfBlockSize;
        uniform float2 _PixelOffset;
        


        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
            OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
            return OUT;
        }

        ENDHLSL

        Pass
        {
            Name "Pixelation"

            HLSLPROGRAM
            half4 frag(Varyings IN) : SV_TARGET
            {
                float2 blockPos = floor(IN.uv * _BlockCount + _PixelOffset); 
                float2 blockCenter = blockPos * _BlockSize + _HalfBlockSize;

                float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_point_clamp, blockCenter);
                //return float4(IN.uv.y * 3,1,1,1);

                // screenPos.xy will contain pixel integer coordinates.
                // use them to implement a checkerboard pattern that skips rendering
                // 4x4 blocks of pixels

                // checker value will be negative for 4x4 blocks of pixels
                // in a checkerboard pattern
                //IN.positionHCS.xy = floor(IN.positionHCS.xy * 0.25) * 0.5;
                //float checker = -frac(IN.positionHCS.r + IN.positionHCS.g);

                // clip HLSL instruction stops rendering a pixel if value is negative
                //clip(checker);

                // for pixels that were kept, read the texture and output it
                //half4 c = tex2D(sampler_MainTex, IN.uv);
                //return c;

                return tex;
            }
            ENDHLSL
        }


    }
}