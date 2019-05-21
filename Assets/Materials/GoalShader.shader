// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Goal" {
	Properties {
		_MainTex ("_MainTex", 2D) = "white" {}
		_AlphaChTex ("_AlphaChTex", 2D) = "white" {}
	}
	SubShader {
		Tags {
			"IgnoreProjector"="True"
			"Queue"="Transparent"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}
		LOD 200
		Pass {
			Name "FORWARD"
			Tags {
				"LightMode"="ForwardBase"
			}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#define UNITY_PASS_FORWARDBASE
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			#pragma only_renderers d3d9 d3d11 glcore gles 
			#pragma target 3.0

			struct VertexInput {
				float4 vertex : POSITION;
				float2 texcoord0 : TEXCOORD0;
			};

			struct VertexOutput {
				float4 vertex : SV_POSITION;
				float2 texcoord0 : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
				float2 texcoord2 : TEXCOORD2;
				float2 texcoord3 : TEXCOORD3;
			};

			uniform float4 _Offset;
			uniform float4x4 _RotationChR;
			uniform float4x4 _RotationChG;
			uniform float4x4 _RotationChB;
			uniform float4 _Pos1;
			uniform float4 _Pos2;

			VertexOutput vert (VertexInput v) {
				VertexOutput o = (VertexOutput)0;

				float2 position1 = v.texcoord0;;
				float2 position2 = mul(position1 - float2(0.5, 0.5), _RotationChG) + float2(0.5, 0.5);
				
				o.texcoord0 = mul(position1 - float2(0.5, 0.5), _RotationChR) + float2(0.5, 0.5);
				o.texcoord1 = position2 + _Pos1.xy;
				o.texcoord2 = position2 + _Pos2.xy;
				o.texcoord3 = mul(position1 - float2(0.5, 0.5), _RotationChB) + float2(0.5, 0.5);

				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}

			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaChTex;

			float4 frag(VertexOutput i) : COLOR{

				float4 output = tex2D(_MainTex, i.texcoord0);

				float finalalpha =
					(tex2D(_AlphaChTex, i.texcoord1).r + tex2D(_AlphaChTex, i.texcoord2).g) * (tex2D(_AlphaChTex, i.texcoord3).b * tex2D(_AlphaChTex, i.texcoord3).a);

				//finalalpha = floor((finalalpha + _Cutoffs.z));

				output.w = finalalpha;

				if (output.w < 0.4) {
					discard;
				}
				else {
					output.w = 1;
				} 

				return output;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
