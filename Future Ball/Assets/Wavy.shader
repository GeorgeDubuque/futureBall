// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Wavy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Scale ("Noise scale", Vector) = (1.0, 1.0, 1.0)
		_Offset ("Noise offset", Vector) = (1.0, 1.0, 1.0)
		_Color ("Color", Color) = (1.0, 1.0, 1.0)
        _CellSize ("Cell Size", Range(0, 2)) = 2
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Random.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float3 noise_uv : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float3 _Scale;
			float3 _Offset;
			float _CellSize;
			float4 _Color;

			float3 voronoiNoise(float2 value){
				float2 baseCell = floor(value);

				float minDistToCell = 10;
				[unroll]
				for(int x = -1; x <= 1; x++){
					for(int y = -1; y <= 1; y++){
						float2 cell = baseCell + float2(x, y);
						float2 cellPosition = cell + rand2dTo2d(cell); 
						float2 toCell = cellPosition - value;
						float distToCell = length(toCell);
						if(distToCell < minDistToCell){
							minDistToCell = distToCell;           
						}
					}
				}
				return minDistToCell;
			}

            v2f vert (appdata v)
            {
				v2f o;

				o.normal = v.normal;
				float noiseval = voronoiNoise(_Scale*(_Offset+v.vertex)/_CellSize);
				v.vertex += 0.1*float4(noiseval * o.normal, 0.0);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.noise_uv = v.vertex.xyz/v.vertex.w;

				return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = _Color;
                return col;
            }
            ENDCG
        }
    }
}
