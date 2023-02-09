// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "StandardVertexColor 2.0"
{
	Properties
	{
		_Albedo01("Albedo 01", 2D) = "white" {}
		_Metallic01("Metallic 01", 2D) = "black" {}
		[Normal]_NormalMap01("Normal Map 01", 2D) = "white" {}
		_NormalMap02Intensity("Normal Map 02 Intensity", Range( -10 , 10)) = 1
		_NormalMap01Intensity("Normal Map 01 Intensity", Range( -10 , 10)) = 1
		_VertexColorOffsetColor("Vertex Color Offset Color", Color) = (0.5,0.5,0.5,0)
		_Albedo02("Albedo 02", 2D) = "white" {}
		_Metallic02("Metallic 02", 2D) = "black" {}
		[Normal]_NormalMap02("Normal Map 02", 2D) = "white" {}
		_AlbedoBlendIntensity("Albedo Blend Intensity", Range( -1 , 1)) = 0
		_MetallicBlendIntensity("Metallic Blend Intensity", Range( -1 , 1)) = 0
		_SmoothnesBlendIntensity("Smoothnes Blend Intensity", Range( -1 , 1)) = 0
		_VertexColorIntensity("Vertex Color Intensity", Range( 0 , 10)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _NormalMap01Intensity;
		uniform sampler2D _NormalMap01;
		uniform float4 _NormalMap01_ST;
		uniform float _NormalMap02Intensity;
		uniform sampler2D _NormalMap02;
		uniform float4 _NormalMap02_ST;
		uniform float _VertexColorIntensity;
		uniform float4 _VertexColorOffsetColor;
		uniform sampler2D _Albedo01;
		uniform float4 _Albedo01_ST;
		uniform sampler2D _Albedo02;
		uniform float4 _Albedo02_ST;
		uniform float _AlbedoBlendIntensity;
		uniform sampler2D _Metallic01;
		uniform float4 _Metallic01_ST;
		uniform sampler2D _Metallic02;
		uniform float4 _Metallic02_ST;
		uniform float _MetallicBlendIntensity;
		uniform float _SmoothnesBlendIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap01 = i.uv_texcoord * _NormalMap01_ST.xy + _NormalMap01_ST.zw;
			float2 uv_NormalMap02 = i.uv_texcoord * _NormalMap02_ST.xy + _NormalMap02_ST.zw;
			float3 lerpResult35 = lerp( UnpackScaleNormal( tex2D( _NormalMap01, uv_NormalMap01 ) ,_NormalMap01Intensity ) , UnpackScaleNormal( tex2D( _NormalMap02, uv_NormalMap02 ) ,_NormalMap02Intensity ) , i.vertexColor.a);
			o.Normal = lerpResult35;
			float4 appendResult23 = (float4(i.vertexColor.r , i.vertexColor.g , i.vertexColor.b , 0.0));
			float2 uv_Albedo01 = i.uv_texcoord * _Albedo01_ST.xy + _Albedo01_ST.zw;
			float2 uv_Albedo02 = i.uv_texcoord * _Albedo02_ST.xy + _Albedo02_ST.zw;
			float4 lerpResult30 = lerp( tex2D( _Albedo01, uv_Albedo01 ) , tex2D( _Albedo02, uv_Albedo02 ) , ( _AlbedoBlendIntensity + i.vertexColor.a ));
			o.Albedo = ( ( ( _VertexColorIntensity * appendResult23 ) + _VertexColorOffsetColor ) * lerpResult30 ).rgb;
			float2 uv_Metallic01 = i.uv_texcoord * _Metallic01_ST.xy + _Metallic01_ST.zw;
			float4 tex2DNode12 = tex2D( _Metallic01, uv_Metallic01 );
			float2 uv_Metallic02 = i.uv_texcoord * _Metallic02_ST.xy + _Metallic02_ST.zw;
			float4 tex2DNode37 = tex2D( _Metallic02, uv_Metallic02 );
			float lerpResult36 = lerp( tex2DNode12.r , tex2DNode37.r , ( i.vertexColor.a + _MetallicBlendIntensity ));
			o.Metallic = lerpResult36;
			float lerpResult40 = lerp( tex2DNode12.a , tex2DNode37.a , ( i.vertexColor.a + _SmoothnesBlendIntensity ));
			o.Smoothness = lerpResult40;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=15401
241;90;1636;1092;2734.307;933.9977;2.296635;True;True
Node;AmplifyShaderEditor.VertexColorNode;9;-1900.899,-479.4429;Float;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;58;-1052.343,-888.3043;Float;False;Property;_VertexColorIntensity;Vertex Color Intensity;13;0;Create;True;0;0;False;0;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;23;-1039.064,-773.6409;Float;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1366.179,-186.6848;Float;False;Property;_AlbedoBlendIntensity;Albedo Blend Intensity;9;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-726.3124,-870.3769;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-872.3115,629.6835;Float;False;Property;_MetallicBlendIntensity;Metallic Blend Intensity;10;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-1550.727,374.8955;Float;False;Property;_NormalMap02Intensity;Normal Map 02 Intensity;3;0;Create;True;0;0;False;0;1;0;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-1902.032,4.457583;Float;False;Property;_NormalMap01Intensity;Normal Map 01 Intensity;4;0;Create;True;0;0;False;0;1;0;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;22;-839.3071,-281.3911;Float;True;Property;_Albedo01;Albedo 01;0;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-865.0535,-526.7458;Float;False;Property;_VertexColorOffsetColor;Vertex Color Offset Color;5;0;Create;True;0;0;False;0;0.5,0.5,0.5,0;0.6544118,0.6544118,0.6544118,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;29;-836.8768,-90.48637;Float;True;Property;_Albedo02;Albedo 02;6;0;Create;True;0;0;False;0;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-877.0674,1363.706;Float;False;Property;_SmoothnesBlendIntensity;Smoothnes Blend Intensity;11;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-958.8992,-194.2351;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-814.1081,363.6724;Float;True;Property;_NormalMap02;Normal Map 02;8;1;[Normal];Create;True;0;0;False;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;42;-351.8097,587.0624;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-559.8718,-617.7247;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;12;-875.5919,775.4559;Float;True;Property;_Metallic01;Metallic 01;1;0;Create;True;0;0;False;0;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-422.3785,1347.725;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;-935.1886,1042.607;Float;True;Property;_Metallic02;Metallic 02;7;0;Create;True;0;0;False;0;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;34;-1226.283,102.7061;Float;True;Property;_NormalMap01;Normal Map 01;2;1;[Normal];Create;True;0;0;True;0;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;30;-482.9114,-215.7496;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;36;-56.56427,809.1154;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-180.1431,-362.5789;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;35;-338.0948,158.7298;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;55;-1311.114,-395.8677;Float;False;Property;_Color0;Color 0;12;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;40;-176.8118,1200.344;Float;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;250.6261,175.2788;Float;False;True;2;Float;;0;0;Standard;StandardVertexColor 2.0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;1;False;-1;1;False;-1;0;0;False;-1;0;False;-1;4;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;9;1
WireConnection;23;1;9;2
WireConnection;23;2;9;3
WireConnection;59;0;58;0
WireConnection;59;1;23;0
WireConnection;32;0;33;0
WireConnection;32;1;9;4
WireConnection;6;5;60;0
WireConnection;42;0;9;4
WireConnection;42;1;41;0
WireConnection;27;0;59;0
WireConnection;27;1;28;0
WireConnection;44;0;9;4
WireConnection;44;1;43;0
WireConnection;34;5;53;0
WireConnection;30;0;22;0
WireConnection;30;1;29;0
WireConnection;30;2;32;0
WireConnection;36;0;12;1
WireConnection;36;1;37;1
WireConnection;36;2;42;0
WireConnection;25;0;27;0
WireConnection;25;1;30;0
WireConnection;35;0;34;0
WireConnection;35;1;6;0
WireConnection;35;2;9;4
WireConnection;40;0;12;4
WireConnection;40;1;37;4
WireConnection;40;2;44;0
WireConnection;0;0;25;0
WireConnection;0;1;35;0
WireConnection;0;3;36;0
WireConnection;0;4;40;0
ASEEND*/
//CHKSM=DE348A00084EB392C84A252F55F6D7179E94F628