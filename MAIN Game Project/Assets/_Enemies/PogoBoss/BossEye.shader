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
		ZWrite On
		ZTest LEqual
		//SeparateSpecular On
		
		CGPROGRAM
		#pragma surface surf MySpecular
	//	#include "UnityCG.cginc"
//Lambert
		sampler2D _MainTex;
		half4 _Color;
		float _fac;
		float _spec;
		
		half4 LightingMySpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
	        half3 h = normalize (lightDir + viewDir);

	        half diff = max (0, dot (s.Normal, lightDir));

	        float nh = max (0, dot (s.Normal, h));
	        float spec = pow (nh, _spec);

	        half4 c;
	        c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten * 2);
	        c.a = s.Alpha;
	        return c;
   		}
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 cc = half4(1,1,1,1) - (half4(1,1,1,1) - _Color)*_fac;// - _SubtractColor * _fac;
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			
			//o.Gloss = 230.2;
			//o.Specular = _spec;
			
			o.Albedo = c.rgb * cc.rgb;
			//o.Alpha = 1;
		//	o.Normal = UnpackNormal(half4(0,1,0,1));
			
			
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
