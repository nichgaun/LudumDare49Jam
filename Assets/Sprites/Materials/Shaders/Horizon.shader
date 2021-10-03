Shader "Custom/Horizon"
{
  Properties
  {
    _GrassTex("Grass Texture", 2D) = "white" {}
    _RoadTex("Road Texture", 2D) = "white" {}
    _TexSize("Texture Size", Float) = 512
    _XSign("X sign", Float) = -1
    _ZSign("Z sign", Float) = 1
    _GradientStart("Gradient Start", Color) = (1, 1, 1, 1)
    _GradientEnd("Gradient End", Color) = (0, 0, 0, 0)
    _GradientLength("Gradient Length", Float) = 8
    _SunColor("Sun Color", Color) = (1, 1, 1, 1)
    _SunSize("Sun Size", Float) = 4
    _SunGlowSize("Sun Glow Size", Float) = 2
  }
  SubShader
  {
    Tags{ "RenderType" = "Opaque" }
    LOD 100

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #define mod(x, y) (x - y * floor(x / y))

      #include "UnityCG.cginc"

      struct appdata
      {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f
      {
        float4 vertex : SV_POSITION;
        float2 uv : TEXCOORD0;
        float3 viewT : TEXCOORD1;
        float3 worldPos : TEXCOORD3;
      };

      sampler2D _GrassTex;
      sampler2D _RoadTex;
      float4 _RoadTex_ST;
      float _TexSize;
      float _XSign;
      float _ZSign;
      float4 _GradientStart;
      float4 _GradientEnd;
      float _GradientLength;
      float _SunSize;
      float _SunGlowSize;
      float4 _SunColor;

      v2f vert(appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _RoadTex);
        o.viewT = WorldSpaceViewDir(v.vertex);
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        return o;
      }

      fixed4 frag(v2f i) : SV_Target
      {
        fixed4 col;
        if (i.viewT.y > 0) {
          fixed dist = i.worldPos.y / i.viewT.y;
          fixed2 uv = fixed2((i.worldPos.x + _XSign * dist * i.viewT.x) / _TexSize + 0.5f, (i.worldPos.z - _ZSign * dist * i.viewT.z) / _TexSize + 0.5f);
          if (uv.y > 0 && uv.y < 1) {
            col = tex2D(_RoadTex, uv) * 1.2f;
          } else {
            col = tex2D(_GrassTex, uv);
          }
        } else {
          fixed2 skyPos = fixed2(i.viewT.x, -i.viewT.y);
          float gradientProg = min(skyPos.y / _GradientLength, 1);
          col = (1 - gradientProg) * _GradientStart + gradientProg * _GradientEnd;
          float sunDist = skyPos.x * skyPos.x + skyPos.y * skyPos.y;
          float variedSunGlowSize = (1 + 0.2 * _SinTime.w) * _SunGlowSize;
          if (sunDist < _SunSize) {
            col += _SunColor;
          } else if (sunDist < _SunSize + variedSunGlowSize) {
            col += _SunColor * (0.5 - abs(0.5 - (sunDist - _SunSize) / variedSunGlowSize));
          }
        }
        return col;
      }
      ENDCG
    }
  }
}
