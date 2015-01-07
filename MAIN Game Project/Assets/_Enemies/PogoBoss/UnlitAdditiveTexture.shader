Shader "Custom/UnlitAdditiveTexture" {
	Properties {
		_Color("Color", Color) = (0,0,0)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//_Fac ("Fac", float) = 1.0
	}
	SubShader {
		Tags {"Queue" = "Transparent"}//, "RenderType"="Additive" 
//		LOD 200
		Lighting Off
		Blend One One
		ZWrite Off
		
		Pass{		
			 SetTexture[_MainTex] {
			constantColor [_Color]
			Combine texture * constant
			}
		}
	} 
	FallBack "Unlit/Transparant"
}
