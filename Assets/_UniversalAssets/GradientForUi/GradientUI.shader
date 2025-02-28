Shader "UI/GradientURP"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {} // Основная текстура
        _Color1  ("Color 1", Color) = (1, 1, 1, 1)
        _Color2  ("Color 2", Color) = (0, 0, 0, 1)
        _Angle   ("Gradient Angle", Range(0, 360)) = 0
        _Offset  ("Gradient Offset", Range(-0.5, 0.5)) = 0
        _Smoothness ("Smoothness", Range(0.1, 5)) = 1.0 // Новый слайдер для регулировки плавности градиента
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "UIGradientPass"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            sampler2D _MainTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                half4  color  : COLOR;
            };

            half4 _Color1;
            half4 _Color2;
            float  _Angle;
            float  _Offset;
            float  _Smoothness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex.xyz);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Перевод угла в радианы
                float rad = radians(_Angle);
                // Определяем направление градиента
                float2 dir = float2(cos(rad), sin(rad));
                // Сдвигаем UV, чтобы центр элемента соответствовал значению 0.5, плюс дополнительное смещение
                float factor = saturate(dot(i.uv - 0.5, dir) + 0.5 + _Offset);
                // Применяем регулировку плавности перехода через степень
                float t = pow(factor, _Smoothness);
                // Интерполируем между двумя цветами
                half4 gradientCol = lerp(_Color1, _Color2, t);
                // Выборка цвета из основной текстуры
                half4 mainTexColor = tex2D(_MainTex, i.uv);
                // Перемножаем градиент, текстуру и вершинный цвет (UI позволяет задавать tint)
                half4 col = gradientCol * mainTexColor * i.color;
                return col;
            }
            ENDHLSL
        }
    }
    FallBack "UI/Default"
}
