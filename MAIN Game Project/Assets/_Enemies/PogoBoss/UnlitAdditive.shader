Shader "Custom/UnlitAdditive" {
	Properties {
		_Color("Color", Color) = (0,0,0)
		//_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags {"Queue" = "Transparent"}//, "RenderType"="Additive" 
//		LOD 200
		Lighting Off
		Blend One One
		ZWrite Off
		
		Pass{		
			Color[_Color]	
		}
	} 
	FallBack "Unlit/Transparant"
}
