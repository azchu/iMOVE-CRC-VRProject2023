// Self-made shader for the flashing effect of a trigger.
Shader "Custom/FlashingIndicator" {
 Properties {
     _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
     _Speed("Speed",float) = 1.0
 }
 
 SubShader {
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 100
     
     ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 
     
     Pass {  
         CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
 
             struct appdata_t {
                 float4 vertex : POSITION;
                 float2 texcoord : TEXCOORD0;
             };
 
             struct v2f {
                 float4 vertex : SV_POSITION;
                 half2 texcoord : TEXCOORD0;
                 UNITY_FOG_COORDS(1)
             };
 
             sampler2D _MainTex;
             float4 _MainTex_ST;
             float _Speed;

             v2f vert (appdata_t v)
             {
                 v2f o;
                 o.vertex = UnityObjectToClipPos(v.vertex);
                 o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                 UNITY_TRANSFER_FOG(o,o.vertex);
                 return o;
             }
             
             fixed4 frag (v2f i) : SV_Target
             {
                 fixed4 col = tex2D(_MainTex, i.texcoord);
				 
				 // Modify the transparency with time flow.
                 col.a = col.a * sin(_Time.y * _Speed);

                 UNITY_APPLY_FOG(i.fogCoord, col);
                 return col;
             }
         ENDCG
     }
 }
 
 }