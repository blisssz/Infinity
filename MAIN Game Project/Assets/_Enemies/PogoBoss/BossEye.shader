Shader "Custom/BossEye" {
// multiplies a given color over a base texture
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,0,0)
		_fac("fac", Float) = 0
		_spec("specular", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		//SeparateSpecular On
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#include "UnityCG.cginc"
//Lambert
		sampler2D _MainTex;
		half4 _Color;
		float _fac;
		float _spec;
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 cc = half4(1,1,1,1) - (half4(1,1,1,1) - _Color)*_fac;// - _SubtractColor * _fac;
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			//o.Gloss = 230.2;
			//o.Specular = _spec;
			
			o.Albedo = c.rgb * cc.rgb;
			o.Alpha = c.a;
		//	o.Normal = UnpackNormal(half4(0,1,0,1));
			
			
		}
		ENDCG
	} 
	//FallBack "Diffuse"
}
