Shader "Unlit/ShowOneCharactor_SDF"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ColorMask("Color Mask", Float) = 15
        _Delta("Delta", Range(0, 0.2)) = 0
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
            LOD 100

            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Delta;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // sample the texture
                    fixed4 col = fixed4(i.color.rgb, i.color.a * tex2D(_MainTex, i.uv).a);
                    // apply fog
                    UNITY_APPLY_FOG(i.fogCoord, col);

                    float rawAlpha = tex2D(_MainTex, i.uv).a;
                    //clip(rawAlpha - (0.5f - _Delta));

                    return float4(col.rgb, smoothstep(0.5f - _Delta, 0.5f + _Delta, rawAlpha));
            }
            ENDCG
        }
        }
}
