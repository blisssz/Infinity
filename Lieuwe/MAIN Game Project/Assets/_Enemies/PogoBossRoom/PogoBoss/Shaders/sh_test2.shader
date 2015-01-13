Shader "Custom/sh_test2" {
	Properties {
		_Color1("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color2("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Color3("Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_FogColor("FogColor", Color) = (0.1, 0.1, 0.1, 1.0)
		_fac("iFactor", float) = 5
		_fogdist("fogDistance", float) = 200
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:mycolor
		
		half4 _Color1;
		half4 _Color2;
		half4 _Color3;
		half4 _FogColor;
		float _fac;
		float _fogdist;
		

		struct Input {
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
			
			float i1 = sin(IN.worldPos.x*0.05 + IN.worldPos.z*0.1+ sin(cos(IN.worldPos.x*0.1) + sin(IN.worldPos.y*0.1))*0.5+0.5);
			float i2 = sin(i1 + IN.worldPos.z*0.1 + IN.worldPos.x*0.1)*0.5+0.5;
			
			i2 = frac(i1);
			i1 = frac(i1/9+12.18);
			
			i1 = sin(sqrt(IN.worldPos.x*IN.worldPos.x + IN.worldPos.z * IN.worldPos.z)*0.12)*0.5+0.5;
			i2 = sin((abs(IN.worldPos.y)/20.0))*0.5+0.5; 
			
			
		//	float myfog = min(IN.screenPos.z/_fogdist, 1);
			//o.Albedo
			o.Albedo = lerp( lerp(_Color1.rgb, _Color2.rgb, i1), _Color3.rgb, i2);//, _FogColor, myfog)).rgb;
			o.Alpha = 1.0;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
