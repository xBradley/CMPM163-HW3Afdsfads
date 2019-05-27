Shader "Custom/ParticleShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        //Define properties for Start and End Color
        _StartColor ("Start Color", Color) = (1,1,1,1)
        _EndColor ("End Color", Color) = (1,1,1,1)

    }
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Opaque" }
        LOD 100
        
        Blend One One
        ZWrite off
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            uniform float4 _StartColor;
            uniform float4 _EndColor;

            struct appdata {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
            };

            struct v2f {   
                float4 vertex : SV_POSITION;
                float3 uv : TEXCOORD0;
            };

           uniform sampler2D _MainTex;
          

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv; 
             
                return o;
            }

            float4 frag (v2f i) : SV_Target {
                //Get particle age percentage
                float age = i.uv.z;

                //Sample color from particle texture
                float4 texColor = tex2D(_MainTex, i.uv.xy);

                //Do a linear interpolation of start color and end color based on particle age percentage
                float4 finalColor = lerp(_StartColor, _EndColor, age) *     (texColor.a);

                return finalColor;
            }
            ENDCG
        }
    }
}
