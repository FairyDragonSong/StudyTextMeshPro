Shader "Unlit/ShowOneCharactor_SDF"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ColorMask("Color Mask", Float) = 15
        _Delta("Delta", Range(0, 0.2)) = 0

        _Bold("Bold", Range(0, 1)) = 0.5
        _UnderlayOffsetX("UnderlayOffsetX", Range(-0.02, 0.02)) = 0
        _UnderlayOffsetY("UnderlayOffsetY", Range(-0.02, 0.02)) = 0
        _UnderlayColor("UnderlayColor", Color) = (1, 1, 1, 1)

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
                    half2 underlayParam: TEXCOORD2;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float _Delta;
                float _Scale = 1;
                float _Bold;
                float _UnderlayOffsetX;
                float _UnderlayOffsetY;
                float4 _UnderlayColor;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    float w = o.vertex.w;
                    float scale = 100 / (w);
                    scale *= _Scale;
			        float halfscale = _Bold * scale;
                    o.param = half2(scale, halfscale);

                    float x = _UnderlayOffsetX;
                    float y = _UnderlayOffsetY;
                    float2 layerOffset = float2(x, y);
                    o.underlayParam = float2(v.uv + layerOffset);

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
			        half colorAlpha = saturate(col.a * i.param.x - i.param.y);
                    // 第二 end

                    float d = tex2D(_MainTex, i.underlayParam.xy).a * i.param.x;
                    float underLayAlpha = saturate(d - i.param.y) * (1 - colorAlpha);
                    
                    if (underLayAlpha > 0)
                        col.rgb = _UnderlayColor.rgb;

                    colorAlpha += underLayAlpha;

                    return float4(col.rgb, colorAlpha);
                    

                }
            ENDCG
        }
        }
}
