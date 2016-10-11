// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/CrystallShader"
{
	Properties
	{
		_Color( "Color", Color ) = ( 1.0,1.0,1.0,1.0 )
	}

		CGINCLUDE
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
		ENDCG

		SubShader
	{
		//Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		Tags{
		"LightMode" = "ForwardBase"
		"Queue" = "Transparent"
		//"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
	}
		Lighting On
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		//Cull Front
		Pass
	{
		
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase

		uniform half4 _Color;
	struct vertexOutput
	{
		float4 pos : SV_POSITION;
		fixed2 uv : TEXCOORD0;
		fixed3 wnorm : TEXCOORD1;
		fixed3 wrefl : TEXCOORD2;
		fixed3 ldir : TEXCOORD4;
		LIGHTING_COORDS( 3 , 4 )
	};
	vertexOutput vert( appdata_base v )
	{
		vertexOutput o;
		o.pos = mul( UNITY_MATRIX_MVP , v.vertex );
		o.uv = v.texcoord;
		//o.lightDir = normalize( ObjSpaceLightDir( v.vertex ) );
		//o.normal = normalize( v.normal ).xyz;
		float3 wpos = mul( unity_ObjectToWorld , v.vertex ).xyz;
		//float3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
		float3 view_dir = normalize( UnityWorldSpaceViewDir( wpos ) );
		fixed3 wnormal = UnityObjectToWorldNormal( normalize( v.normal ) );
		o.wrefl = reflect( -view_dir , wnormal );
		o.ldir = normalize( ObjSpaceLightDir( v.vertex ) );
		//o.viewdot = saturate( wnormal , )
		//o.ambient = ShadeSH9( half4( o.wnormal , 1 ) );
		TRANSFER_VERTEX_TO_FRAGMENT( o ); 
		return o;
	}
	half4 frag( vertexOutput i ) : COLOR
	{
		float3 L = normalize( i.ldir );
		float3 N = normalize( i.wnorm );
		half4 skyData = UNITY_SAMPLE_TEXCUBE( unity_SpecCube0 , i.wrefl );
		half3 skyColor = DecodeHDR( skyData , unity_SpecCube0_HDR );
		
		float spec = pow( saturate( dot( L , N ) ) , 10 );
		return half4( skyColor , 1 ) * _Color * spec;// +half4( NdotL )+_Color;
	}

		ENDCG
	}
	}
		FallBack "Diffuse"
}
