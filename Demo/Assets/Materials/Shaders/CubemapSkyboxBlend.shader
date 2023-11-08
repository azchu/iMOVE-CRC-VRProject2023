// Shader of blending skybox. Online Source.
// Reference: https://answers.unity.com/questions/616078/shader-blending-2-cubemaps.html
Shader "Skybox/CubemapSkyboxBlend" {
     Properties {
         _Tint ("Tint Color", Color) = (.5, .5, .5, 1)
         _Tint2 ("Tint Color 2", Color) = (.5, .5, .5, 1)
         [Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
         _Rotation ("Rotation", Range(0, 360)) = 0
         _BlendCubemaps ("Blend Cubemaps", Range(0, 1)) = 0.5
         [NoScaleOffset] _Tex ("Cubemap (HDR)", Cube) = "grey" {}
         [NoScaleOffset] _Tex2 ("Cubemap (HDR) 2", Cube) = "grey" {}
         _Transparency("Transparency", Range(0.0,1.0)) = 1.0
     }
     SubShader {
         Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
         Cull Off ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
      
         Pass {
          
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
      
             #include "UnityCG.cginc"
      
             samplerCUBE _Tex;
             samplerCUBE _Tex2;
             float _BlendCubemaps;
             half4 _Tex_HDR;
             half4 _Tint;
             half4 _Tint2;
             half _Exposure;
             float _Rotation;
             float _Transparency;

             float4 RotateAroundYInDegrees (float4 vertex, float degrees)
             {
                 float alpha = degrees * UNITY_PI / 180.0;
                 float sina, cosa;
                 sincos(alpha, sina, cosa);
                 float2x2 m = float2x2(cosa, -sina, sina, cosa);
                 return float4(mul(m, vertex.xz), vertex.yw).xzyw;
             }
          
             struct appdata_t {
                 float4 vertex : POSITION;
                 float3 normal : NORMAL;
             };
      
             struct v2f {
                 float4 vertex : SV_POSITION;
                 float3 texcoord : TEXCOORD0;
             };
      
             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(RotateAroundYInDegrees(v.vertex, _Rotation));
                 o.texcoord = v.vertex;
                 return o;
             }
      
             fixed4 frag (v2f i) : SV_Target
             {
                 float4 env1 = texCUBE (_Tex, i.texcoord);
                 float4 env2 = texCUBE (_Tex2, i.texcoord);
                 float4 env = lerp( env2, env1, _BlendCubemaps );
                 float4 tint = lerp( _Tint, _Tint2, _BlendCubemaps );
                 half3 c = DecodeHDR (env, _Tex_HDR);
                 c = c * tint.rgb * unity_ColorSpaceDouble;
                 c *= _Exposure;
                 return half4(c, _Transparency);
             }
             ENDCG
         }
     }
     Fallback Off
 }