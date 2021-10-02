Shader "Custom/Horizon"
{
  Properties
  {
    _MainTex("Texture", 2D) = "white" {}
    _TexSize("Texture Size", Float) = 512
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

      sampler2D _MainTex;
      float _TexSize;
      float4 _MainTex_ST;

      v2f vert(appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        o.viewT = WorldSpaceViewDir(v.vertex);
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        return o;
      }

      fixed4 frag(v2f i) : SV_Target
      {
        if (i.viewT.y > 0 && i.worldPos.y > 0) {
          fixed dist = i.worldPos.y / i.viewT.y;
          fixed2 uv = fixed2(mod((i.worldPos.x - dist * i.viewT.x) / _TexSize, 1), mod((i.worldPos.z + dist * i.viewT.z) / _TexSize, 1));
          fixed4 col = tex2D(_MainTex, uv);
          return col;
        }
        return 0;
      }
      ENDCG
    }
  }
}
