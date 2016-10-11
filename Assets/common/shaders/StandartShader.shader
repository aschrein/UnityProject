Shader "Custom/StandartShader"
{
	Properties
	{
		_Color( "Color", Color ) = ( 1.0,1.0,1.0,1.0 )
		_MainTex( "Base (RGB)", 2D ) = "white" {}
	}

		CGINCLUDE
#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Lighting.cginc"
		ENDCG

		SubShader
	{
		Tags{ "RenderType" = "Opaque" "LightMode" = "ForwardBase" }
		Lighting On
		Pass
		{
		
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fwdbase

			uniform half4 _Color;
			sampler2D _MainTex;
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
				fixed3 lightDir : TEXCOORD1;
				fixed3 normal : TEXCOORD2;
				half3 ambient : COLOR0;
				LIGHTING_COORDS( 3 , 4 )
			};
			vertexOutput vert( appdata_base v )
			{
				vertexOutput o;
				o.pos = mul( UNITY_MATRIX_MVP , v.vertex );
				o.uv = v.texcoord;
				o.lightDir = normalize( ObjSpaceLightDir( v.vertex ) );
				o.normal = normalize( v.normal ).xyz;
				half3 wnormal = UnityObjectToWorldNormal( o.normal );
				o.ambient = ShadeSH9( half4( wnormal , 1 ) );
				TRANSFER_VERTEX_TO_FRAGMENT( o );
				return o;
			}
			half4 frag( vertexOutput i ) : COLOR
			{
				float3 L = normalize( i.lightDir );
				float3 N = normalize( i.normal );

				float attenuation = LIGHT_ATTENUATION( i );
				//float4 ambient = UNITY_LIGHTMODEL_AMBIENT;

				float NdotL = saturate( dot( N, L ) );
				float4 diffuse = NdotL * _LightColor0 * attenuation;

				float4 albedo = tex2D( _MainTex , i.uv );


				return ( half4( i.ambient , 0 ) + diffuse ) * albedo * _Color;
			}

			ENDCG
		}
	}
		FallBack "Diffuse"
}
