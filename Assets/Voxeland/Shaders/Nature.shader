﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Voxeland/Nature" 
{
	Properties 
	{
		[Enum(Off, 0, Front, 1, Back, 2)] _Culling("Culling", Int) = 1
		_Cutoff("Alpha Ref", Range(0, 1)) = 0.33
		_Color ("Color (RGB)", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}

		_SpecGlossMap("Spec Map (RGB), Smooth Map (A)", 2D) = "white" {}
		_Specular("Spec Value (RGB), Smooth Val (A)", Color) = (0,0,0,0.5)
		_AmbientPower("Ambient Power", Range(0.01, 2)) = 0.25

		_WindTex("Wind(XY)", 2D) = "bump" {}
		_WindSize("Wind Size", Range(0, 300)) = 50
		_WindSpeedX("Wind Speed X", Float) = 0.5
		_WindSpeedZ("Wind Speed Z", Float) = 2
		_WindStrength("Wind Strength", Range(0, 2)) = 0.33
	}

	SubShader 
	{
		Cull[_Culling]
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf StandardSpecular alphatest:_Cutoff vertex:vert 
		#pragma target 3.0

		#pragma shader_feature _NORMALMAP
		#pragma shader_feature _SPECGLOSSMAP
		#pragma shader_feature _WIND
		
		fixed4 _Color;

		sampler2D _MainTex;
		
		#if _NORMALMAP
		sampler2D _BumpMap;
		#endif

		#if _SPECGLOSSMAP
		sampler2D _SpecGlossMap;
		#endif

		fixed4 _Specular;
		half _AmbientPower;

		sampler2D _WindTex;
		float _WindSize;
		float _WindSpeedX;
		float _WindSpeedZ;
		float _WindStrength;


		struct Input {
			float2 uv_MainTex; //uv
			float4 color : COLOR; //ambient
		};


		void vert(inout appdata_full v, out Input o)
		{
			#if _WIND
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				worldPos.x += _WindSpeedX * _Time.y;
				worldPos.z += _WindSpeedZ * _Time.y;

				half4 windColor = tex2Dlod(_WindTex, float4(worldPos.x / _WindSize, worldPos.z / _WindSize, 0, 0));
				float3 windDir = UnpackNormal(windColor);
				float2 vertPosMod = windDir.xy * _WindStrength * v.texcoord1.y;
				v.vertex.xz += vertPosMod;
				v.vertex.y -= (abs(vertPosMod.x) + abs(vertPosMod.y)) / 4;
			#endif

			o.color = v.color;
			o.uv_MainTex = v.texcoord;
		}


		void surf (Input IN, inout SurfaceOutputStandardSpecular o) 
		{
			float2 texcoords = IN.uv_MainTex;

			//diffuse and alpha
			fixed4 color = tex2D(_MainTex, texcoords);
			o.Albedo = color.rgb * _Color * IN.color.rgb;
			o.Alpha = color.a;

			//specular, metallic and gloss
			//#if _SPECGLOSSMAP
			//_Specular *= 2;
			//_Specular *= tex2D(_SpecGlossMap, texcoords);
			//#endif

			o.Specular = _Specular.rgb * o.Albedo;
			//o.Metallic = _Specular.r;
			o.Smoothness = _Specular.a;

			//normal
			#if _NORMALMAP
			o.Normal = UnpackNormal(tex2D(_BumpMap, texcoords));
			#endif

			o.Occlusion = _AmbientPower;// * IN.color.a;
		}
		ENDCG

		// Pass to render object as a shadow caster
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#include "UnityCG.cginc"

			#pragma shader_feature _WIND

			struct v2f {
				V2F_SHADOW_CASTER;
				float2  uv : TEXCOORD1;
			};

			uniform float4 _MainTex_ST;
			sampler2D _WindTex;
			float _WindSize;
			float _WindSpeedX;
			float _WindSpeedZ;
			float _WindStrength;

			v2f vert(appdata_full v)
			{
				#if _WIND

								float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
								worldPos.x += _WindSpeedX * _Time.y;
								worldPos.z += _WindSpeedZ * _Time.y;

								half4 windColor = tex2Dlod(_WindTex, float4(worldPos.x / _WindSize, worldPos.z / _WindSize, 0, 0));
								float3 windDir = UnpackNormal(windColor);
								float2 vertPosMod = windDir.xy * _WindStrength * v.texcoord1.y;
								v.vertex.xz += vertPosMod;
								v.vertex.y -= (abs(vertPosMod.x) + abs(vertPosMod.y)) / 4;
				#endif

				v2f o;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);



				return o;
			}

			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;

			float4 frag(v2f i) : SV_Target
			{
				fixed4 texcol = tex2D(_MainTex, i.uv);
				clip(texcol.a - _Cutoff);

				SHADOW_CASTER_FRAGMENT(i)
				return texcol;
			}
			ENDCG
		}

		/*Pass
		{
			Name "TreeBillboard"
			Tags{ "Queue" = "AlphaTest" "RenderType" = "TreeBillboard" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0;
				//o.diff.rgb += ShadeSH9(half4(worldNormal,1));
				return o;
			}

			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a - _Cutoff);
				col *= (i.diff + UNITY_LIGHTMODEL_AMBIENT);
				col *= _Color;
				col.a = 1;
				return col;
			}
			ENDCG
		}*/

		/*Pass
		{
			Name "TreeTransparentCutout"
			Tags{ "Queue" = "AlphaTest" "RenderType" = "TreeTransparentCutout" }
			Fog {Mode Exp}

				CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff : COLOR0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.texcoord;
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0;
				//o.diff.rgb += ShadeSH9(half4(worldNormal,1));
				return o;
			}

			uniform sampler2D _MainTex;
			uniform fixed _Cutoff;
			uniform fixed4 _Color;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			clip(col.a - _Cutoff);
			col *= (i.diff + UNITY_LIGHTMODEL_AMBIENT);
			col *= _Color;
			col.a = 1;
			return col;
			}
				ENDCG
		}*/
	} 
	
	CustomEditor "VoxelandMaterialInspector"
}
