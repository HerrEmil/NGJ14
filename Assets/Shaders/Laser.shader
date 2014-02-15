Shader "Custom/Laser" {
	Properties {
		_BaseColor ("Base Color (Color)", Color) = (0, 0, 0, 0)
		_AlphaTex ("Color (RGB) Alpha (A)", 2D) = "white"
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;
		sampler2D _AlphaTex;
		half4 _BaseColor;

		struct Input {
			float2 uv_AlphaTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_AlphaTex, IN.uv_AlphaTex + float2(_Time.x, _Time.z));
			o.Albedo = _BaseColor.rgb;
			o.Alpha = c.a * _BaseColor.a;
			o.Emission = _BaseColor.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
