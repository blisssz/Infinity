Shader "Custom/UnlitColor" {
	Properties {
		_Color("Color", Color) = (0,0,0)
		//_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
//		LOD 200
		//Blend SrcAlpha D
		Blend Off
		Pass{
			Color[_Color]
		}
	} 
	FallBack "Unlit/Transparant"
}
