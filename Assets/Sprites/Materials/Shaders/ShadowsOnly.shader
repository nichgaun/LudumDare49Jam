/*
Shader "Custom/ShadowsOnly"
{
  Properties
  {
    _ShadowStrength("Shadow Strength", Range (0, 1)) = 0.5 
  }
  SubShader
  {
    Tags{ "Queue" = "AlphaTest" }

    Pass
    {
      Tags{ "LightMode" = "ForwardBase" }
      Cull Back
      Blend SrcAlpha OneMinusSrcAlpha
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #pragma multi_compile_fwdbase

      #include "UnityCG.cginc"
      #include "AutoLight.cginc"

      struct appdata
      {
        float4 pos : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f
      {
        float4 pos : SV_POSITION;
        LIGHTING_COORDS(0, 1)
      };

      uniform float _ShadowStrength;

      v2f vert(appdata v)
      {
        v2f o;
        o.pos = UnityObjectToClipPos(v.pos);
        TRANSFER_VERTEX_TO_FRAGMENT(o);
        return o;
      }

      fixed4 frag(v2f i) : COLOR
      {
        return fixed4(0, 0, 0, (1 - LIGHT_ATTENUATION(i)) * _ShadowStrength);
      }
      ENDCG
    }
  }
}
*/

Shader "Custom/ShadowsOnly" {
  Properties{
    _Color("Main Color", Color) = (0, 0, 0, 0)
  }

  SubShader {
    Tags{ "Queue" = "AlphaTest" }
    LOD 200

    Cull Off
    Blend OneMinusSrcColor One
    CGPROGRAM
    #pragma surface surf Lambert keepalpha

    fixed4 _Color;

    struct Input {
      float4 color : COLOR;
    };

    void surf(Input IN, inout SurfaceOutput o) {
      fixed4 c = _Color;
      o.Albedo = c.rgb;
      o.Albedo *= IN.color.rgb;
      o.Alpha = c.a;
    }
  ENDCG
  }
  Fallback "VertexLit"
}
