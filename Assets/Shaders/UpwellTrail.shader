// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UpwellTrail"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.26
		[HDR]_TopEmission("TopEmission", Color) = (0,0,0,0)
		_FlowMap("Flow Map", 2D) = "white" {}
		_BaseTexture("Base Texture", 2D) = "white" {}
		_Intensity("Intensity", Float) = 0.1
		_Speed("Speed", Float) = 0.1
		_ColorSpaceOffset("Color Space Offset", Float) = 0
		[HDR]_BotEmission("BotEmission", Color) = (0,0,0,0)
		_Remap("Remap", Vector) = (0,1,0,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
		};

		uniform sampler2D _BaseTexture;
		uniform sampler2D _FlowMap;
		uniform float4 _FlowMap_ST;
		uniform float _Speed;
		uniform float _Intensity;
		uniform float4 _Remap;
		uniform float4 _BotEmission;
		uniform float4 _TopEmission;
		uniform float _ColorSpaceOffset;
		uniform float _Cutoff = 0.26;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_FlowMap = i.uv_texcoord * _FlowMap_ST.xy + _FlowMap_ST.zw;
			float2 temp_cast_0 = (_Speed).xx;
			float2 panner25 = ( _Time.y * temp_cast_0 + float2( 1,1 ));
			float2 uv_TexCoord23 = i.uv_texcoord + ( ( tex2D( _FlowMap, uv_FlowMap ).r + panner25 ) * _Intensity );
			float4 tex2DNode28 = tex2D( _BaseTexture, uv_TexCoord23 );
			float4 temp_cast_1 = (_Remap.x).xxxx;
			float4 temp_cast_2 = (_Remap.y).xxxx;
			float4 temp_cast_3 = (_Remap.z).xxxx;
			float4 temp_cast_4 = (_Remap.w).xxxx;
			float3 ase_worldPos = i.worldPos;
			float4 lerpResult57 = lerp( _BotEmission , _TopEmission , (( (ase_worldPos).yyyx + _ColorSpaceOffset )).xzxx);
			o.Emission = ( (temp_cast_3 + (tex2DNode28 - temp_cast_1) * (temp_cast_4 - temp_cast_3) / (temp_cast_2 - temp_cast_1)) * lerpResult57 ).rgb;
			o.Alpha = 1;
			clip( tex2DNode28.r - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
7;29;1906;1004;2944.341;-508.1979;1.455808;True;True
Node;AmplifyShaderEditor.Vector2Node;40;-1998.721,245.8282;Float;False;Constant;_Vector0;Vector 0;4;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;29;-2021.736,520.6905;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-2020.598,404.6613;Float;False;Property;_Speed;Speed;5;0;Create;True;0;0;False;0;0.1;0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;36;-1927.5,-70.17879;Float;True;Property;_FlowMap;Flow Map;2;0;Create;True;0;0;False;0;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;25;-1783.95,345.95;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;52;-1959.393,1502.545;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;37;-1520.195,154.4246;Float;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1432.227,341.9604;Float;False;Property;_Intensity;Intensity;4;0;Create;True;0;0;False;0;0.1;6.04;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;53;-1695.479,1347.233;Float;True;FLOAT4;1;1;1;0;1;0;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-1801.273,1127.786;Float;False;Property;_ColorSpaceOffset;Color Space Offset;6;0;Create;True;0;0;False;0;0;-1.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-1288.03,156.5626;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-1076.845,282.6341;Float;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-1447.815,1126.29;Float;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwizzleNode;56;-1191.031,1154.068;Float;True;FLOAT4;0;2;0;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;58;-1071.997,929.9551;Float;False;Property;_TopEmission;TopEmission;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0,1.584314,2.996078,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-783.976,-166.3259;Float;True;Property;_BaseTexture;Base Texture;3;0;Create;True;0;0;False;0;None;e28dc97a9541e3642a48c0e3886688c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-948.8315,668.8142;Float;False;Property;_BotEmission;BotEmission;7;1;[HDR];Create;True;0;0;False;0;0,0,0,0;1.403922,0,1.498039,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;51;-714.823,156.1498;Float;False;Property;_Remap;Remap;8;0;Create;True;0;0;False;0;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;57;-170.9985,1134.062;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-415.7115,160.2115;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-181.0608,158.571;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;12;493.9595,89.2655;Float;False;True;2;Float;ASEMaterialInspector;0;0;Unlit;UpwellTrail;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.26;True;True;0;True;Transparent;;Geometry;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;8;5;False;-1;1;False;-1;0;4;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;25;0;40;0
WireConnection;25;2;30;0
WireConnection;25;1;29;0
WireConnection;37;0;36;1
WireConnection;37;1;25;0
WireConnection;53;0;52;0
WireConnection;38;0;37;0
WireConnection;38;1;39;0
WireConnection;23;1;38;0
WireConnection;55;0;53;0
WireConnection;55;1;54;0
WireConnection;56;0;55;0
WireConnection;28;1;23;0
WireConnection;57;0;42;0
WireConnection;57;1;58;0
WireConnection;57;2;56;0
WireConnection;50;0;28;0
WireConnection;50;1;51;1
WireConnection;50;2;51;2
WireConnection;50;3;51;3
WireConnection;50;4;51;4
WireConnection;41;0;50;0
WireConnection;41;1;57;0
WireConnection;12;2;41;0
WireConnection;12;10;28;0
ASEEND*/
//CHKSM=79CA59087A4BB247E92112AE0AF0EA19A2C84BC9