Shader "Custom/Laser" {
	Properties {
		_BaseColor ("Base Color (Color)", Color) = (0, 0, 0, 0)
		_AlphaTex ("Alpha (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _AlphaTex;
		half4 _BaseColor;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_AlphaTex, IN.uv_MainTex);
			o.Albedo = c.rgb * _BaseColor;
			o.Alpha = c.a * _BaseColor.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
