// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'


Shader "Voxeland/Grass"
{
	Properties
	{
		[Enum(Off, 0, Front, 1, Back, 2)] _Culling("Culling", Int) = 0
		_Cutoff("Alpha Ref", Range(0, 1)) = 0.33

		_MainTex0("Albedo (RGB)", 2D) = "white" {}		_BumpMap0("Normals", 2D) = "bump" {}	_SpecParams0("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex1("Albedo (RGB)", 2D) = "white" {}		_BumpMap1("Normals", 2D) = "bump" {}	_SpecParams1("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex2("Albedo (RGB)", 2D) = "white" {}		_BumpMap2("Normals", 2D) = "bump" {}	_SpecParams2("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex3("Albedo (RGB)", 2D) = "white" {}		_BumpMap3("Normals", 2D) = "bump" {}	_SpecParams3("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex4("Albedo (RGB)", 2D) = "white" {}		_BumpMap4("Normals", 2D) = "bump" {}	_SpecParams4("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex5("Albedo (RGB)", 2D) = "white" {}		_BumpMap5("Normals", 2D) = "bump" {}	_SpecParams5("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex6("Albedo (RGB)", 2D) = "white" {}		_BumpMap6("Normals", 2D) = "bump" {}	_SpecParams6("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex7("Albedo (RGB)", 2D) = "white" {}		_BumpMap7("Normals", 2D) = "bump" {}	_SpecParams7("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex8("Albedo (RGB)", 2D) = "white" {}		_BumpMap8("Normals", 2D) = "bump" {}	_SpecParams8("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex9("Albedo (RGB)", 2D) = "white" {}		_BumpMap9("Normals", 2D) = "bump" {}	_SpecParams9("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex10("Albedo (RGB)", 2D) = "white" {}		_BumpMap10("Normals", 2D) = "bump" {}	_SpecParams10("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex11("Albedo (RGB)", 2D) = "white" {}		_BumpMap11("Normals", 2D) = "bump" {}	_SpecParams11("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex12("Albedo (RGB)", 2D) = "white" {}		_BumpMap12("Normals", 2D) = "bump" {}	_SpecParams12("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex13("Albedo (RGB)", 2D) = "white" {}		_BumpMap13("Normals", 2D) = "bump" {}	_SpecParams13("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex14("Albedo (RGB)", 2D) = "white" {}		_BumpMap14("Normals", 2D) = "bump" {}	_SpecParams14("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex15("Albedo (RGB)", 2D) = "white" {}		_BumpMap15("Normals", 2D) = "bump" {}	_SpecParams15("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex16("Albedo (RGB)", 2D) = "white" {}		_BumpMap16("Normals", 2D) = "bump" {}	_SpecParams16("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex17("Albedo (RGB)", 2D) = "white" {}		_BumpMap17("Normals", 2D) = "bump" {}	_SpecParams17("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex18("Albedo (RGB)", 2D) = "white" {}		_BumpMap18("Normals", 2D) = "bump" {}	_SpecParams18("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex19("Albedo (RGB)", 2D) = "white" {}		_BumpMap19("Normals", 2D) = "bump" {}	_SpecParams19("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex20("Albedo (RGB)", 2D) = "white" {}		_BumpMap20("Normals", 2D) = "bump" {}	_SpecParams20("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)

		_Mips("MipMap Factor", Float) = 0.6

		_PreviewType("Preview Type", Int) = 0

		_DistanceFadeZero("Distance Fade Zero", Float) = 50
		_DistanceFadeOne("Distance Fade One", Float) = 40

		_AmbientOcclusion("Ambient Occlusion", Float) = 1

		_WindTex("Wind(XY)", 2D) = "bump" {}
		_WindSize("Wind Size", Range(0, 300)) = 50
		_WindSpeedX("Wind Speed X", Float) = 0.5
		_WindSpeedZ("Wind Speed Z", Float) = 2
		_WindStrength("Wind Strength", Range(0, 2)) = 0.33
	}

	SubShader
	{
		Cull[_Culling]
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		//AlphaToMask On

		CGPROGRAM
		#pragma shader_feature _PREVIEW
		#pragma shader_feature _WIND

		#pragma surface surf Standard addshadow fullforwardshadows vertex:Vert nolightmap //alphatest:_Cutoff
		#pragma target 4.6


		#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
			#define TEX Texture2D
			#define SAMPLE(tex, uv, ddx, ddy) tex.SampleGrad(sampler_MainTex1, uv, ddx, ddy)
			#define NUM 20

			SamplerState sampler_MainTex1;
			#define LAYER(n)	Texture2D _MainTex##n;	Texture2D _BumpMap##n;	half _Height##n;	half4 _SpecParams##n;
			LAYER(0) LAYER(1) LAYER(2) LAYER(3) LAYER(4) LAYER(5) LAYER(6) LAYER(7) LAYER(8) LAYER(9) LAYER(10) LAYER(11) LAYER(12) LAYER(13) LAYER(14) LAYER(15) LAYER(16) LAYER(17) LAYER(18) LAYER(19) LAYER(20)

			#define MAIN_ARRAY { _MainTex0, _MainTex1, _MainTex2, _MainTex3,  _MainTex4, _MainTex5, _MainTex6, _MainTex7, _MainTex8, _MainTex9, _MainTex10,  _MainTex11, _MainTex12, _MainTex13, _MainTex14, _MainTex15, _MainTex16, _MainTex17,  _MainTex18, _MainTex19 }
			#define BUMP_ARRAY { _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3,  _BumpMap4, _BumpMap5, _BumpMap6, _BumpMap7, _BumpMap8, _BumpMap9, _BumpMap10,  _BumpMap11, _BumpMap12, _BumpMap13, _BumpMap14, _BumpMap15, _BumpMap16, _BumpMap17,  _BumpMap18, _BumpMap19 }
			#define SPEC_ARRAY { _SpecParams0, _SpecParams1, _SpecParams2, _SpecParams3, _SpecParams4, _SpecParams5, _SpecParams6, _SpecParams7, _SpecParams8, _SpecParams9, _SpecParams10, _SpecParams11, _SpecParams12, _SpecParams13, _SpecParams14, _SpecParams15, _SpecParams16, _SpecParams17, _SpecParams18, _SpecParams19 }
			#define BLEND_ARRAY { IN.blendsA.r, IN.blendsA.g, IN.blendsA.b, IN.blendsA.a, IN.blendsB.r, IN.blendsB.g, IN.blendsB.b, 0,0,0, 0,0,0,0,0, 0,0,0,0,0 }
		#else
			#define TEX sampler2D
			#define SAMPLE(tex, uv, ddx, ddy) tex2Dgrad(tex, uv, ddx, ddy)
			#define NUM 6

			#define LAYER(n)	sampler2D _MainTex##n;	sampler2D _BumpMap##n;	half _Height##n;	half4 _SpecParams##n;
			LAYER(0) LAYER(1) LAYER(2) LAYER(3) LAYER(4) LAYER(5) LAYER(6)

			#define MAIN_ARRAY { _MainTex0, _MainTex1, _MainTex2, _MainTex3,  _MainTex4, _MainTex5 }
			#define BUMP_ARRAY { _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3,  _BumpMap4, _BumpMap5 }
			#define SPEC_ARRAY { _SpecParams0,_SpecParams1, _SpecParams2, _SpecParams3, _SpecParams4, _SpecParams5 }
			#define BLEND_ARRAY { IN.blendsA.r, IN.blendsA.g, IN.blendsA.b, IN.blendsA.a, IN.blendsB.r, IN.blendsB.g }
		#endif

		half _Cutoff;
		half _PreviewType;
		float _Mips;
		float _DistanceFadeZero;
		float _DistanceFadeOne;
		float _AmbientOcclusion;

		#if _WIND
			sampler2D _WindTex;
			float _WindSize;
			float _WindSpeedX;
			float _WindSpeedZ;
			float _WindStrength;
		#endif

		struct Input {
			float4 color : COLOR; //tint + ambient
			float2 texcoord;
			float fade;
			//nointerpolation int type; //works only in dx11
			float type;
			half ambient;
		};


		void Vert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);

			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float camDist = length(worldPos - _WorldSpaceCameraPos.xyz);
			data.fade = 1 - (camDist - _DistanceFadeOne) / (_DistanceFadeZero - _DistanceFadeOne);
			//data.fade = dot(normalize(WorldSpaceViewDir(v.vertex)), v.tangent);

			//data.wPos = mul(unity_ObjectToWorld, v.vertex);
			//data.wNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0))); //world normal

			data.texcoord = v.texcoord;

			//type and ambient
			data.type = v.texcoord3.y;
			data.ambient = v.texcoord3.x;

			#if _WIND
				worldPos.x += _WindSpeedX * _Time.y;
				worldPos.z += _WindSpeedZ * _Time.y;

				half4 windColor = tex2Dlod(_WindTex, float4(worldPos.x / _WindSize, worldPos.z / _WindSize, 0, 0));
				float3 windDir = UnpackNormal(windColor);
				float2 vertPosMod = windDir.xy * _WindStrength * v.texcoord1.y;
				v.vertex.xz += vertPosMod;
				v.vertex.y -= (abs(vertPosMod.x) + abs(vertPosMod.y)) / 4;
			#endif
		}


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//using arrays of textures (maybe temporary)
			TEX tempDiffuses[NUM] = MAIN_ARRAY;
			TEX tempNormals[NUM] = BUMP_ARRAY;
			float type = IN.type;

			//sampling
			float2 yDDX = ddx(IN.texcoord)*_Mips;
			float2 yDDY = ddy(IN.texcoord)*_Mips;

			//albedo
			float4 albedo = 0;
			float maxType = type + 0.001; float minType = type - 0.001;
			for (int i=0; i<NUM; i++)
				if (i<maxType && i>minType) albedo = SAMPLE(tempDiffuses[i], IN.texcoord, yDDX, yDDY);
		
			o.Albedo = albedo;
			o.Alpha = albedo.a;

			//fading and clipping
			#if !_PREVIEW
			clip(albedo.a*IN.fade - _Cutoff);
			#endif

			//normal
			half4 normalColor = 0;
			for (int i=0; i<NUM; i++)
				normalColor = SAMPLE(tempNormals[i], IN.texcoord, yDDX, yDDY);
			o.Normal = UnpackNormal(normalColor);

			o.Occlusion = IN.ambient; // 1 - _AmbientOcclusion + IN.ambient*_AmbientOcclusion;

			//specular
			//TODO copy from land shader

			#if _PREVIEW
				if (_PreviewType == 1) o.Emission = o.Occlusion;
				if (_PreviewType == 2) o.Emission = o.Metallic;
				if (_PreviewType == 3) o.Emission = o.Smoothness;
				if (_PreviewType == 4) o.Emission = IN.fade;
				if (_PreviewType == 5) o.Emission = half3(IN.type/4, IN.type/12, IN.type/20);

				o.Alpha = 1;
				o.Albedo = 0;
				o.Metallic = 0;
				//o.Specular = 0;
				o.Smoothness = 0;
		//		o.Occlusion = 0;
				o.Normal = 0;
			#endif

		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "GrassMaterialInspector"
}
