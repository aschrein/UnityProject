Shader "Pixelization/OutlineShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		frame_width( "frame_width" , float ) = 0
		frame_height( "frame_height" , float ) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D depth_buffer;
			uniform float frame_width;
			uniform float frame_height;
			fixed sampleDepth( fixed2 uv )
			{
				fixed udepth = tex2D( depth_buffer , uv ).r;
				float f = 1000.0;
				float n = 0.3;
				float z = ( 2 * n ) / ( f + n - udepth * ( f - n ) );
				return z;
			}
			float depthLaplace( fixed2 uv , fixed2 duv )
			{
				fixed d00 = sampleDepth( uv + fixed2( duv.x , 0 ) );
				fixed d01 = sampleDepth( uv + fixed2( -duv.x , 0 ) );
				fixed d10 = sampleDepth( uv + fixed2( 0 , duv.y ) );
				fixed d11 = sampleDepth( uv + fixed2( 0 , -duv.y ) );
				return ( d00 + d01 + d10 + d11 - 4.0 * sampleDepth( uv ) ) / duv / duv;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float dl = depthLaplace( i.uv , fixed2( 1 / frame_width , 1 / frame_height ) );
				dl = dl * 4.0;///pow( dl * 0.00005 , 2 );
				float outline = step( saturate( dl ) , 0.1 );
				/*if( ( dl) > 10.0 )
				{
					return fixed4( 0.0 , 0.0 , 0 , 0 );
				}*/
				// just invert the colors
				//col = 1 - col;
				return //fixed4( dl , dl , dl , dl );
				col * outline;
			}
			ENDCG
		}
	}
}
