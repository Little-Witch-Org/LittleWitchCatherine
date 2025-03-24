Shader "Unlit/CrumpledPaperEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CrumpledTex ("Crumpled Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            sampler2D _CrumpledTex;
            float _Intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 crumpledColor = tex2D(_CrumpledTex, i.uv);
                fixed4 mainColor = tex2D(_MainTex, i.uv);

                // Смешиваем цвета с учетом интенсивности
                fixed4 finalColor = lerp(mainColor, crumpledColor, _Intensity);
                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}