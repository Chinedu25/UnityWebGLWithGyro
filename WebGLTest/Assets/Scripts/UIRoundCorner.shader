// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UI/UIRoundCorner"
{
    Properties
    {
       [HideInInspector] _MainTex("Texture", 2D) = "white" {}

    // --- Mask support ---
    [HideInInspector] _StencilComp("Stencil Comparison", Float) = 8
    [HideInInspector] _Stencil("Stencil ID", Float) = 0
    [HideInInspector] _StencilOp("Stencil Operation", Float) = 0
    [HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
    [HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
    [HideInInspector] _ColorMask("Color Mask", Float) = 15
    [HideInInspector] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
        _ColorA("ColorA", Color) = (1,1,1,1)
        _ColorB("ColorB", Color) = (1,1,1,1)
        _ColorMix("ColorMix", Range(0.001, 2.000)) = 0.5
        _WidthHeightRadius("WidthHeightRadius", Vector) = (100,100,0,0)
        _GradientAngle("GradientAngle", Range(-3.60, 3.60)) = 0
        [Toggle] _UseGradient("UseGradient", Float) = 0
    }
    SubShader
    {

         Tags
            {
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

              Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]
        LOD 100
        

        //Texture Pass
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

         float AntialiasedCutoff(float distance) {
                float distanceChange = fwidth(distance) * 0.5;
                return smoothstep(distanceChange, -distanceChange, distance);
            }

    // SDF Function for rectangle
    float rectangle(float2 samplePosition, float2 halfSize) {
        // X component represents signed distance to closest vertical edge of rectangle
        // Same for Y but for closest horizontal edge
        // HalfSize represents two distances from each axis of 2d space to a сorresponding edge
        float2 distanceToEdge = abs(samplePosition) - halfSize;
        // max(n, 0) to remove distances that signed with minus (distances inside rectangle)
        // length to calculate distance from outside (distances that > 0) to rectangle
        float outsideDistance = length(max(distanceToEdge, 0));
        // max(x,y) is a cheap way to calculate distance to closest edge inside rectangle
        // with min we just make sure that inside distances would not impact on outside distances
        float insideDistance = min(max(distanceToEdge.x, distanceToEdge.y), 0);
        return outsideDistance + insideDistance;
    }

    // An extension of rectangle() function to modify signed distance (effect as a wrap around rectangle)
    float roundedRectangle(float2 samplePosition, float absoluteRound, float2 halfSize) {
        // subtrancting value from final distance effects like a wrap around rectangle, so
        // the solution is to decrease actual rectangle with `absoluteRound` value 
        // and then make an effect of wrap with size of `absoluteRound`
        return rectangle(samplePosition, halfSize - absoluteRound) - absoluteRound;
    }


    float CalcAlpha(float2 samplePosition, float2 size, float radius) {
        // -.5 = translate origin of samplePositions from (0, 0) to (.5, .5)
        // because for Image component (0,0) is bottom-right, not a center
        // * size = scale samplePositions to localSpace of Image with this size
        float2 samplePositionTranslated = (samplePosition - .5) * size;
        float distToRect = roundedRectangle(samplePositionTranslated, radius * .5, size * .5);
        return AntialiasedCutoff(distToRect);
    }
            struct appdata
            {
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;  // set from Image component property
  
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _WidthHeightRadius;
            fixed4 _ColorA;
            fixed4 _ColorB;
            float _ColorMix;
            fixed4 _TextureSampleAdd;
            float _GradientAngle;
            float _UseGradient;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col = (tex2D(_MainTex, i.uv) + _TextureSampleAdd) * i.color;
                fixed4 color = lerp(_ColorA, _ColorB, (i.uv.y * sin(_GradientAngle) + i.uv.x * cos(_GradientAngle)) / _ColorMix);


                col = _UseGradient > 0 ? color * col : col;

                float alpha = CalcAlpha(i.uv, _WidthHeightRadius.xy, _WidthHeightRadius.z);

                col.a = min(col.a, alpha);
                return col;
            }
            ENDCG
        }
    }
}
