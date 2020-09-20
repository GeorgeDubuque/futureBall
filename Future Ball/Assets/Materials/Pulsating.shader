Shader "Custom/Pulsating"
{
    Properties
    {
        _CellSize ("Cell Size", Range(0, 2)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        
        #include "Random.cginc"

        sampler2D _MainTex;
        float _CellSize;

        struct Input
        {
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float2 voronoiNoise(float2 value){
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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 value = IN.worldPos.xy / _CellSize;
            float noise = voronoiNoise(value);
            o.Albedo = noise;
        }

        ENDCG
    }
}
