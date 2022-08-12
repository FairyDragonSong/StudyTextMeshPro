Shader "Unlit/ShowOneCharactor_SDF"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ColorMask("Color Mask", Float) = 15
        _Delta("Delta", Range(0, 0.2)) = 0

        // 放缩时保证边界
        _Scale("Scale", Float) = 1
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
                    half2 param: TEXCOORD1;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Delta;
                float _Scale = 1;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    float w = o.vertex.w;
                    float scale = 100 / (w);
                    scale *= _Scale;
			        float halfscale = 0.5 * scale;
                    o.param = half2(scale, halfscale);

                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.color = v.color;
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // 获取当前像素颜色值
                    fixed4 col = fixed4(i.color.rgb, i.color.a * tex2D(_MainTex, i.uv).a);

                    UNITY_APPLY_FOG(i.fogCoord, col);

                    // 第一 
                    //float alpha = tex2D(_MainTex, i.uv).a;
                    //return float4(col.rgb, step(0.5, alpha));
                    //return float4(col.rgb, smoothstep(0.5 - _Delta, 0.5 + _Delta, alpha));
                    // 第一 end

                    // 第二
                    half alpha = col.a;
			        half aa = saturate(alpha * i.param.x - i.param.y);
                    return float4(col.rgb, aa);
                    // 第二 end
                }
            ENDCG
        }
        }
}
