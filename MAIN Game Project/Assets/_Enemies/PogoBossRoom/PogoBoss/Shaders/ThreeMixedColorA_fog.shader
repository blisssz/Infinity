Shader "Custom/ThreeMixedColorA_fog" {
// custom fog version
Properties {
		_Noise1Tex ("Base (RGB)", 2D) = "white" {}
		_Noise2Tex ("Base (RGB)", 2D) = "white" {}
		_Color1("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color2("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color3("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_FogColor("FogColor", Color) = (0.1, 0.1, 0.1, 1.0)
		_fac("iFactor", float) = 5
		
		
		_repeatFac1x("repeatFac1X", float) = 200
		_repeatFac1z("repeatFac1Z", float) = 200
		
		_repeatFac2x("repeatFac2X", float) = 100
		_repeatFac2z("repeatFac2Z", float) = 100
		
		_fogdist("fogDistance", float) = 200
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:mycolor

		sampler2D _Noise1Tex;
		sampler2D _Noise2Tex;
		
		half4 _Color1;
		half4 _Color2;
		half4 _Color3;
		half4 _FogColor;
		float _fac;
		
		float _repeatFac1x;
		float _repeatFac1z;
		
		float _repeatFac2x;
		float _repeatFac2z;
		
		float _fogdist;
		

		struct Input {
			float2 uv_Noise1Tex;
			float2 uv_Noise2Tex;
			float4 screenPos;
			float3 worldPos;
		};
		
		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color) {
			float myfog = min(length( - IN.screenPos.xyz)/_fogdist, 1);
			//float myfog = min(IN.screenPos.z/_fogdist, 1);
			
			// fix for the sqaure arround point lights
			#ifdef UNITY_PASS_FORWARDADD
	          _FogColor = 0;
	         #endif
			// gives weird results with pointlights. It suddenly changes brightness sometimes (without the #ifdef)
			color.rgb = 1 * lerp (color.rgb, _FogColor,  myfog);
			//color.a = 0;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_Noise1Tex, IN.worldPos.xz/ float2(_repeatFac1x, _repeatFac1z));//uv_Noise1Tex);
			//o.Albedo = c.rgb;
			//o.Alpha = c.a;
			
			half4 c2 = tex2D (_Noise2Tex, IN.worldPos.xz/ float2(_repeatFac2x, _repeatFac2z));// IN.uv_Noise2Tex);
			
			half4 c3 = (c + c2)/2;
			float i1 = floor(c.r*_fac)/_fac;
			float i2 = floor(c2.r*_fac)/_fac;
			
		//	float myfog = min(IN.screenPos.z/_fogdist, 1);
			//o.Albedo
			o.Albedo = lerp( lerp(_Color1, _Color2, i1), _Color3, i2);//, _FogColor, myfog)).rgb;
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
