// Self-made shader for simple transparency transition. 
Shader "Custom/SimpleTransition"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Transparency("Transparency", Range(0.0,1.0)) = 1.0
	}
	SubShader
	{
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				
				float4 vertex : SV_POSITION;
				
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Transparency;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				// Assign the user-defined transparency value.
				col.a *= _Transparency;
				
				return col;
			}
			ENDCG
		}
	}
}
