// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UpwellParticle"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "white" {}
		[HDR]_TopEmission("TopEmission", Color) = (0,0,0,0)
		_RemapSin("Remap Sin", Vector) = (-3,3,0.2,4)
		_Color0("Color 0", Color) = (0,0,0,0)
		[HDR]_BotEmission("BotEmission", Color) = (0,0,0,0)
		_ColorSpaceOffset("Color Space Offset", Float) = 0
		_Smoothness("Smoothness", Float) = 1
		_Fresnel("Fresnel", Vector) = (0,0,0,0)
		_FlickerSpeed("Flicker Speed", Float) = 1
		_Wavelength("Wavelength", Float) = 0
		[Toggle]_ActiveState("ActiveState", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color0;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _ActiveState;
		uniform float4 _BotEmission;
		uniform float4 _TopEmission;
		uniform float _ColorSpaceOffset;
		uniform float4 _Fresnel;
		uniform float _Wavelength;
		uniform float _FlickerSpeed;
		uniform float4 _RemapSin;
		uniform float _Smoothness;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _Color0.rgb;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 WorldPosition93 = ase_worldPos;
			float4 temp_output_62_0 = (( _ColorSpaceOffset + (WorldPosition93).yxxx )).xxxx;
			float4 lerpResult55 = lerp( _BotEmission , _TopEmission , temp_output_62_0);
			float4 lerpResult85 = lerp( (_BotEmission).rrba , (_TopEmission).rrga , temp_output_62_0);
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV79 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode79 = ( _Fresnel.y + _Fresnel.z * pow( 1.0 - fresnelNdotV79, _Fresnel.w ) );
			float Fresnel90 = fresnelNode79;
			float temp_output_33_0 = ( ase_worldPos.x + ase_worldPos.y + ase_worldPos.z );
			float mulTime17 = _Time.y * _FlickerSpeed;
			o.Emission = ( tex2D( _Albedo, uv_Albedo ) * lerp(lerpResult55,lerpResult85,_ActiveState) * Fresnel90 * (_RemapSin.z + (( sin( ( ( _Wavelength * temp_output_33_0 ) + ( mulTime17 * 0.5 ) ) ) + sin( ( ( temp_output_33_0 * _Wavelength ) + ( mulTime17 * 3.0 ) ) ) + sin( mulTime17 ) ) - _RemapSin.x) * (_RemapSin.w - _RemapSin.z) / (_RemapSin.y - _RemapSin.x)) ).rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = Fresnel90;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16700
-1913;23;1906;1050;3804.036;2096.468;3.652464;True;True
Node;AmplifyShaderEditor.CommentaryNode;96;-2469.757,575.416;Float;False;2670.133;1042.473;World position with Sin mapping;20;8;83;93;49;48;82;32;31;78;30;69;68;26;23;84;25;27;33;17;77;World Position;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;8;-2396.387,821.2822;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;77;-1950.688,1241.326;Float;False;Property;_FlickerSpeed;Flicker Speed;8;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;-2079.883,786.588;Float;False;WorldPosition;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;97;-2434.476,-326.3383;Float;False;1035.612;526.9298;Comment;5;72;62;94;89;63;Height ;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-2341.466,56.33453;Float;False;93;WorldPosition;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1388.427,801.8835;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-1373.746,1185.485;Float;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1962.816,1018.046;Float;False;Property;_Wavelength;Wavelength;9;0;Create;True;0;0;False;0;0;13.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1368.647,993.8638;Float;False;Constant;_Float0;Float 0;3;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;17;-1692.852,1210.072;Float;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;95;-2432.008,-1002.624;Float;False;862.1198;427.8083;Fresnel;3;80;79;90;Fresnel;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1130.05,717.9122;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;89;-2058.122,53.81413;Float;False;FLOAT4;1;0;0;0;1;0;FLOAT3;0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2314.22,-212.4522;Float;False;Property;_ColorSpaceOffset;Color Space Offset;5;0;Create;True;0;0;False;0;0;-1.55;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;98;-1094.734,-777.0424;Float;False;1890.773;1111.083;compoite ;10;70;36;7;88;37;86;87;85;91;55;Composite;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-1090.759,1116.411;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1111.438,994.7964;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1114.215,841.9637;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;63;-1796.797,30.42669;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;69;-860.1747,1060.999;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;-886.0969,887.0663;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;70;-994.605,-357.8891;Float;False;Property;_TopEmission;TopEmission;1;1;[HDR];Create;True;0;0;False;0;0,0,0,0;5.584314,0,5.992157,0.8235294;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;36;-1003.786,-550.8794;Float;False;Property;_BotEmission;BotEmission;4;1;[HDR];Create;True;0;0;False;0;0,0,0,0;0.4139203,0.88303,5.518937,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;80;-2374.96,-860.0929;Float;False;Property;_Fresnel;Fresnel;7;0;Create;True;0;0;False;0;0,0,0,0;0,-0.14,1.3,1.05;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;79;-2071.346,-845.8242;Float;False;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;86;-583.9467,-572.335;Float;False;FLOAT4;0;0;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwizzleNode;87;-589.893,-491.6356;Float;False;FLOAT4;0;0;1;3;1;0;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SwizzleNode;62;-1603.438,43.19302;Float;False;FLOAT4;0;0;0;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SinOpNode;31;-687.9454,1074.418;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;30;-689.0187,930.152;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;78;-1101.572,1269.729;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;90;-1818.008,-840.4259;Float;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;32;-386.6155,1049.547;Float;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;55;-186.667,-33.05635;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;49;-607.1318,1284.937;Float;False;Property;_RemapSin;Remap Sin;2;0;Create;True;0;0;False;0;-3,3,0.2,4;-3,3,0,1.34;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;85;-290.8623,-496.9651;Float;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;7;77.44186,-414.9418;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;48;-106.9034,1154.605;Float;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;91;105.4477,-28.47009;Float;False;90;Fresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ToggleSwitchNode;88;106.2219,-154.146;Float;False;Property;_ActiveState;ActiveState;10;0;Create;True;0;0;False;0;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;65;1266.606,-273.632;Float;False;Property;_Color0;Color 0;3;0;Create;True;0;0;False;0;0,0,0,0;0,0.326898,0.8207547,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;76;1305.75,-34.60092;Float;False;Property;_Smoothness;Smoothness;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;525.4326,-132.4315;Float;True;4;4;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;92;1331.081,72.25034;Float;False;90;Fresnel;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1577.694,-143.1903;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;UpwellParticle;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;-0.01;True;True;0;False;Transparent;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.053;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;93;0;8;0
WireConnection;33;0;8;1
WireConnection;33;1;8;2
WireConnection;33;2;8;3
WireConnection;17;0;77;0
WireConnection;83;0;82;0
WireConnection;83;1;33;0
WireConnection;89;0;94;0
WireConnection;26;0;17;0
WireConnection;26;1;27;0
WireConnection;23;0;17;0
WireConnection;23;1;25;0
WireConnection;84;0;33;0
WireConnection;84;1;82;0
WireConnection;63;0;72;0
WireConnection;63;1;89;0
WireConnection;69;0;84;0
WireConnection;69;1;26;0
WireConnection;68;0;83;0
WireConnection;68;1;23;0
WireConnection;79;1;80;2
WireConnection;79;2;80;3
WireConnection;79;3;80;4
WireConnection;86;0;36;0
WireConnection;87;0;70;0
WireConnection;62;0;63;0
WireConnection;31;0;69;0
WireConnection;30;0;68;0
WireConnection;78;0;17;0
WireConnection;90;0;79;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;32;2;78;0
WireConnection;55;0;36;0
WireConnection;55;1;70;0
WireConnection;55;2;62;0
WireConnection;85;0;86;0
WireConnection;85;1;87;0
WireConnection;85;2;62;0
WireConnection;48;0;32;0
WireConnection;48;1;49;1
WireConnection;48;2;49;2
WireConnection;48;3;49;3
WireConnection;48;4;49;4
WireConnection;88;0;55;0
WireConnection;88;1;85;0
WireConnection;37;0;7;0
WireConnection;37;1;88;0
WireConnection;37;2;91;0
WireConnection;37;3;48;0
WireConnection;0;0;65;0
WireConnection;0;2;37;0
WireConnection;0;4;76;0
WireConnection;0;9;92;0
ASEEND*/
//CHKSM=AECE7CB3547FE57B4F74439145C0065EE88FA4D5