// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Voxeland/Land" 
{
	Properties 
	{
		_MainTex0("Albedo (RGB)", 2D) = "black" {}		_BumpMap0("Normals", 2D) = "bump" {}	_Height0("Height", Float) = 1	_SpecParams0("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex1("Albedo (RGB)", 2D) = "black" {}		_BumpMap1("Normals", 2D) = "bump" {}	_Height1("Height", Float) = 1	_SpecParams1("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex2("Albedo (RGB)", 2D) = "black" {}		_BumpMap2("Normals", 2D) = "bump" {}	_Height2("Height", Float) = 1	_SpecParams2("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex3("Albedo (RGB)", 2D) = "black" {}		_BumpMap3("Normals", 2D) = "bump" {}	_Height3("Height", Float) = 1	_SpecParams3("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex4("Albedo (RGB)", 2D) = "black" {}		_BumpMap4("Normals", 2D) = "bump" {}	_Height4("Height", Float) = 1	_SpecParams4("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex5("Albedo (RGB)", 2D) = "black" {}		_BumpMap5("Normals", 2D) = "bump" {}	_Height5("Height", Float) = 1	_SpecParams5("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex6("Albedo (RGB)", 2D) = "black" {}		_BumpMap6("Normals", 2D) = "bump" {}	_Height6("Height", Float) = 1	_SpecParams6("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex7("Albedo (RGB)", 2D) = "black" {}		_BumpMap7("Normals", 2D) = "bump" {}	_Height7("Height", Float) = 1	_SpecParams7("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex8("Albedo (RGB)", 2D) = "black" {}		_BumpMap8("Normals", 2D) = "bump" {}	_Height8("Height", Float) = 1	_SpecParams8("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex9("Albedo (RGB)", 2D) = "black" {}		_BumpMap9("Normals", 2D) = "bump" {}	_Height9("Height", Float) = 1	_SpecParams9("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex10("Albedo (RGB)", 2D) = "black" {}		_BumpMap10("Normals", 2D) = "bump" {}	_Height10("Height", Float) = 1	_SpecParams10("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex11("Albedo (RGB)", 2D) = "black" {}		_BumpMap11("Normals", 2D) = "bump" {}	_Height11("Height", Float) = 1	_SpecParams11("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex12("Albedo (RGB)", 2D) = "black" {}		_BumpMap12("Normals", 2D) = "bump" {}	_Height12("Height", Float) = 1	_SpecParams12("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex13("Albedo (RGB)", 2D) = "black" {}		_BumpMap13("Normals", 2D) = "bump" {}	_Height13("Height", Float) = 1	_SpecParams13("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex14("Albedo (RGB)", 2D) = "black" {}		_BumpMap14("Normals", 2D) = "bump" {}	_Height14("Height", Float) = 1	_SpecParams14("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex15("Albedo (RGB)", 2D) = "black" {}		_BumpMap15("Normals", 2D) = "bump" {}	_Height15("Height", Float) = 1	_SpecParams15("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex16("Albedo (RGB)", 2D) = "black" {}		_BumpMap16("Normals", 2D) = "bump" {}	_Height16("Height", Float) = 1	_SpecParams16("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex17("Albedo (RGB)", 2D) = "black" {}		_BumpMap17("Normals", 2D) = "bump" {}	_Height17("Height", Float) = 1	_SpecParams17("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex18("Albedo (RGB)", 2D) = "black" {}		_BumpMap18("Normals", 2D) = "bump" {}	_Height18("Height", Float) = 1	_SpecParams18("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)
		_MainTex19("Albedo (RGB)", 2D) = "black" {}		_BumpMap19("Normals", 2D) = "bump" {}	_Height19("Height", Float) = 1	_SpecParams19("SpecParams", Vector) = (-0.75,0.25,-0.35,1.5)

		_Tile("Tile", Float) = 0.1
		_FarTile("Far Tile", Float) = 0.01
		_FarStart("Far Start", Float) = 10
		_FarEnd("Far End", Float) = 100

		_BlendMapFactor("Blend Map Factor", Float) = 1
		_BlendCrisp("Blend Crispness", Float) = 2

		_Mips("MipMap Factor", Float) = 0.6
		_AmbientOcclusion("Ambient Occlusion", Float) = 1

		_PreviewType("Preview Type", Int) = 0

		_HorizonHeightScale("Horizon Height Scale", Float) = 200
		_HorizonHeightmap("Horizon Heightmap", 2D) = "black" {}
		_HorizonTypemap("Horizon Typemap", 2D) = "black" {}
		_HorizonVisibilityMap("Horizon Visibility Map", 2D) = "white" {}
		_HorizonBorderLower("Horizon Border Lower", Float) = 2
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma shader_feature _TRIPLANAR
		#pragma shader_feature _DOUBLELAYER
		#pragma shader_feature _PREVIEW
		#pragma shader_feature _HORIZON

		#pragma surface surf Standard addshadow fullforwardshadows vertex:Vert nolightmap

		//#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)
		#pragma target 4.6
		//#else
		//#pragma target 3.0
		//#endif

		#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) 
			#define TEX Texture2D
			#define SAMPLE(tex, uv, ddx, ddy) tex.SampleGrad(sampler_MainTex0, uv, ddx, ddy)
			#define NUM 16

			SamplerState sampler_MainTex0;
			#define LAYER(n)	Texture2D _MainTex##n;	Texture2D _BumpMap##n;	half _Height##n;	half4 _SpecParams##n;
			LAYER(0) LAYER(1) LAYER(2) LAYER(3) LAYER(4) LAYER(5) LAYER(6) LAYER(7) LAYER(8) LAYER(9) LAYER(10) LAYER(11) LAYER(12) LAYER(13) LAYER(14) LAYER(15)

			#define MAIN_ARRAY { _MainTex0, _MainTex1, _MainTex2, _MainTex3, _MainTex4, _MainTex5, _MainTex6, _MainTex7, _MainTex8, _MainTex9, _MainTex10, _MainTex11, _MainTex12, _MainTex13, _MainTex14, _MainTex15 }
			#define BUMP_ARRAY { _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3, _BumpMap4, _BumpMap5, _BumpMap6, _BumpMap7, _BumpMap8, _BumpMap9, _BumpMap10, _BumpMap11, _BumpMap12, _BumpMap13, _BumpMap14, _BumpMap15 }
			#define SPEC_ARRAY { _SpecParams0, _SpecParams1, _SpecParams2, _SpecParams3, _SpecParams4, _SpecParams5, _SpecParams6, _SpecParams7, _SpecParams8, _SpecParams9, _SpecParams10, _SpecParams11, _SpecParams12, _SpecParams13, _SpecParams14, _SpecParams15 }
			#define HEIGHT_ARRAY { _Height0, _Height1, _Height2, _Height3, _Height4, _Height5, _Height6, _Height7, _Height8, _Height9, _Height10, _Height11, _Height12, _Height13, _Height14, _Height15 }
			#define BLEND_ARRAY { IN.blendsA.r, IN.blendsA.g, IN.blendsA.b, IN.blendsA.a, IN.blendsB.r, IN.blendsB.g, IN.blendsB.b, IN.blendsB.a, IN.blendsC.r, IN.blendsC.g, IN.blendsC.b, IN.blendsC.a, IN.blendsD.r, IN.blendsD.g, IN.blendsD.b, IN.blendsD.a }
		
		#elif defined(SHADER_API_GLCORE) || defined(SHADER_API_GLES3)
			#define TEX Texture2D
			#define SAMPLE(tex, uv, ddx, ddy) tex.SampleGrad(sampler_MainTex0, uv, ddx, ddy)
			#define NUM 6

			SamplerState sampler_MainTex0;
			#define LAYER(n)	Texture2D _MainTex##n;	Texture2D _BumpMap##n;	half _Height##n;	half4 _SpecParams##n;
			LAYER(0) LAYER(1) LAYER(2) LAYER(3) LAYER(4) LAYER(5)

			#define MAIN_ARRAY { _MainTex0, _MainTex1, _MainTex2, _MainTex3, _MainTex4, _MainTex5 }
			#define BUMP_ARRAY { _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3, _BumpMap4, _BumpMap5 }
			#define SPEC_ARRAY { _SpecParams0, _SpecParams1, _SpecParams2, _SpecParams3, _SpecParams4, _SpecParams5 }
			#define HEIGHT_ARRAY { _Height0, _Height1, _Height2, _Height3, _Height4, _Height5 }
			#define BLEND_ARRAY { IN.blendsA.r, IN.blendsA.g, IN.blendsA.b, IN.blendsA.a, IN.blendsB.r, IN.blendsB.g }

		#else
			#define TEX sampler2D
			#define SAMPLE(tex, uv, ddx, ddy) tex2Dgrad(tex, uv, ddx, ddy)
			#define NUM 4

			#define LAYER(n)	sampler2D _MainTex##n;	sampler2D _BumpMap##n;	half _Height##n;	half4 _SpecParams##n;
			LAYER(0) LAYER(1) LAYER(2) LAYER(3) LAYER(4)

			#define MAIN_ARRAY { _MainTex0, _MainTex1, _MainTex2, _MainTex3 }
			#define BUMP_ARRAY { _BumpMap0, _BumpMap1, _BumpMap2, _BumpMap3 }
			#define SPEC_ARRAY { _SpecParams0, _SpecParams1, _SpecParams2, _SpecParams3 }
			#define HEIGHT_ARRAY { _Height0, _Height1, _Height2, _Height3 }
			#define BLEND_ARRAY { IN.blendsA.r, IN.blendsA.g, IN.blendsA.b, IN.blendsA.a }
		#endif

		half _PreviewType;

		float _Tile;
		float _FarTile; 
		float _FarStart; 
		float _FarEnd;

		float _BlendMapFactor;
		float _BlendCrisp;
		float _Mips;
		float _AmbientOcclusion;

		half _Heights[8];

		struct appdata_hbtt {
			float4 vertex : POSITION; 
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
		};

		struct Input {
			//float4 color : COLOR; //ambient
			float3 wNormal;
			float3 wPos;
			half ambient;

			#if _HORIZON
				half2 wTexcoord; //should be float, but it does not work in DX9
				half visibility;
			#endif

			half4 blendsA; half4 blendsB;
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
		 	half4 blendsC; half4 blendsD; //half4 blendsE; half4 blendsF;
			#endif
		};

		#if _HORIZON
			float _HorizonHeightScale;
			sampler2D _HorizonHeightmap;
			sampler2D _HorizonTypemap;
			sampler2D _HorizonVisibilityMap; 
			float _HorizonBorderLower;
		#endif

		inline float4 GetTangent(float3 worldNormal)
		{
			float4 tangent;
			float3 absWorldNormal = abs(worldNormal);

			if (absWorldNormal.z >= absWorldNormal.x && absWorldNormal.z >= absWorldNormal.y)
			{
				if (worldNormal.z>0) tangent = float4(-1, 0, 0, -1);
				else tangent = float4(1, 0, 0, -1);
			}
			else if (absWorldNormal.y >= absWorldNormal.x && absWorldNormal.y >= absWorldNormal.z)
			{
				if (worldNormal.y>0) tangent = float4(0, 0, -1, -1);
				else tangent = float4(0, 0, 1, -1); 
			}
			else //if (absWorldNormal.x >= absWorldNormal.x && absWorldNormal.y >= absWorldNormal.z)
			{
				if (worldNormal.x>0) tangent = float4(0, 0, 1, -1);
				else tangent = float4(0, 0, -1, -1);
			}
			return tangent;
		}



		void Vert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);

			//blends
			//data.blendsA = v.color;
			data.blendsA = half4(
				((int)v.tangent.x >> 0) & 0xF,
				((int)v.tangent.x >> 4) & 0xF,
				((int)v.tangent.x >> 8) & 0xF,
				((int)v.tangent.x >> 12) & 0xF) / 16;
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)
			data.blendsB = half4(
				((int)v.tangent.x >> 16) & 0xF,
				((int)v.tangent.x >> 20) & 0xF,
				((int)v.tangent.y >> 0) & 0xF,
				((int)v.tangent.y >> 4) & 0xF) / 16;
			#endif
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
			data.blendsC = half4(
				((int)v.tangent.y >> 8) & 0xF,
				((int)v.tangent.y >> 12) & 0xF,
				((int)v.tangent.y >> 16) & 0xF,
				((int)v.tangent.y >> 20) & 0xF) / 16;
			data.blendsD = half4(
				((int)v.tangent.z >> 0) & 0xF,
				((int)v.tangent.z >> 4) & 0xF,
				((int)v.tangent.z >> 8) & 0xF,
				((int)v.tangent.z >> 12) & 0xF) / 16;
			/*data.blendsE = half4(
				((int)v.tangent.z >> 16) & 0xF,
				((int)v.tangent.z >> 20) & 0xF,
				((int)v.tangent.w >> 0) & 0xF,
				((int)v.tangent.w >> 4) & 0xF) / 16;
			data.blendsF = half4(
				((int)v.tangent.w >> 8) & 0xF,
				((int)v.tangent.w >> 12) & 0xF,
				((int)v.tangent.w >> 16) & 0xF,
				((int)v.tangent.w >> 20) & 0xF) / 16;*/
			#endif

			//pos, normal, tangent, ambient
			data.wPos = mul(unity_ObjectToWorld, v.vertex);
			data.wNormal = normalize(mul(unity_ObjectToWorld, float4(v.normal, 0))); //world normal
			v.tangent = GetTangent(data.wNormal); //vertex tangent
			data.ambient = v.texcoord3.x; 

			#if _HORIZON
				//height
				half4 heightColor = tex2Dlod(_HorizonHeightmap, float4(v.texcoord.xy, 0, 0));
				v.vertex.y = (heightColor.r*250 + heightColor.g)*256;

				//visibility and border
				float4 visibilityDirs = float4(
					tex2Dlod(_HorizonVisibilityMap, float4(v.texcoord.x+0.001, v.texcoord.y, 0, 0)).a,
					tex2Dlod(_HorizonVisibilityMap, float4(v.texcoord.x-0.001, v.texcoord.y, 0, 0)).a,
					tex2Dlod(_HorizonVisibilityMap, float4(v.texcoord.x, v.texcoord.y+0.001, 0, 0)).a,
					tex2Dlod(_HorizonVisibilityMap, float4(v.texcoord.x, v.texcoord.y-0.001, 0, 0)).a ); 
				data.wPos.x += (visibilityDirs.x - visibilityDirs.y)*_HorizonBorderLower;
				data.wPos.z += (visibilityDirs.z - visibilityDirs.w)*_HorizonBorderLower;

				data.visibility = (visibilityDirs.x+ visibilityDirs.y+ visibilityDirs.z+ visibilityDirs.w)*4; //if >0 then visible, if <1 then border
				if (data.visibility < 0.999) v.vertex.y -= _HorizonBorderLower * (1-data.visibility);

				//types
				int type = tex2Dlod(_HorizonTypemap, float4(v.texcoord.xy, 0, 0)).a * 256;
				data.blendsA = half4(type==0? 1:0, type==1? 1:0, type==2? 1:0, type==3? 1:0);
				#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)
				data.blendsB = half4(type==4? 1:0, type==5? 1:0, type==6? 1:0, type==7? 1:0);
				#endif
				#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)
				data.blendsC = half4(type == 8 ? 1 : 0, type == 9 ? 1 : 0, type == 10 ? 1 : 0, type == 11 ? 1 : 0);
				data.blendsD = half4(type==12? 1:0, type==13? 1:0, type==14? 1:0, type==15? 1:0);
				//data.blendsE = half4(type==16? 1:0, type==17? 1:0, type==18? 1:0, type==19? 1:0);
				#endif

				//uv
				data.wTexcoord = v.texcoord;

				//filling ambient with white
				data.ambient = 1;
			#endif
		}

		inline half4 SoftLight(half4 a, half4 b, float percent)
		{
			half4 sl = (1 - 2 * b)*a*a + 2 * a*b;

			float bPercent = saturate((percent - 0.5) * 2);
			float aPercent = saturate(((1 - percent) - 0.5) * 2);

			return a*aPercent + b*bPercent + sl*(1 - aPercent - bPercent);
			//return half4(aPercent, bPercent, 1 - aPercent - bPercent,1);
		}


		inline half4 TriplanarSample(TEX tex, float2 yUV, float2 zUV, float2 xUV, float3 absDirections)
		{
			half4 color = half4(0, 0, 0, 0);

			float2 yDDX = ddx(yUV)*_Mips; float2 yDDY = ddy(yUV)*_Mips;
			#if _TRIPLANAR
			float2 zDDX = ddx(zUV)*_Mips; float2 zDDY = ddy(zUV)*_Mips;
			float2 xDDX = ddx(xUV)*_Mips; float2 xDDY = ddy(xUV)*_Mips;

			if (absDirections.y > 0.001) color += SAMPLE(tex, yUV, yDDX, yDDY) * absDirections.y;
			if (absDirections.z > 0.001) color += SAMPLE(tex, zUV, zDDX, zDDY) * absDirections.z;
			if (absDirections.x > 0.001) color += SAMPLE(tex, xUV, xDDX, xDDY) * absDirections.x;

			//color = lerp(tex2Dgrad(tex, zUV, zDDX, zDDY), tex2Dgrad(tex, xUV, xDDX, xDDY), absDirections.x);
			//color = SoftLight(color, tex2Dgrad(tex, yUV, yDDX, yDDY), absDirections.y);
			#else
			color += SAMPLE(tex, yUV, yDDX, yDDY);
			#endif
			
			return color;
		}

		inline half4 FarTriplanarSample(TEX tex, float3 pos, float3 directions, float dist)
		{
			//switching directions to avoid texture inversion
			float2 yUV = float2(directions.y>0 ? -pos.z : pos.z, pos.x);
			float2 zUV = float2(directions.z>0 ? -pos.x : pos.x, pos.y);
			float2 xUV = float2(directions.x<0 ? -pos.z : pos.z, pos.y);

			float3 absDirections = abs(directions);

			#if _DOUBLELAYER
				float percent = (dist - _FarStart) / (_FarEnd - _FarStart);
				percent = saturate(percent);
				
				if (percent > 0.999999f) return TriplanarSample(tex, yUV*_FarTile, zUV*_FarTile, xUV*_FarTile, absDirections);
				else if (percent < 0.000001f) return TriplanarSample(tex, yUV*_Tile, zUV*_Tile, xUV*_Tile, absDirections);
				else
				{
					half4 color = TriplanarSample (tex, yUV*_Tile, zUV*_Tile, xUV*_Tile, absDirections);
					half4 farColor = TriplanarSample(tex, yUV*_FarTile, zUV*_FarTile, xUV*_FarTile, absDirections);
					return SoftLight(color, farColor, percent);
				}
			#else
				return TriplanarSample(tex, yUV*_Tile, zUV*_Tile, xUV*_Tile, absDirections);
			#endif
		}


		void surf (Input IN, inout SurfaceOutputStandard o) 
		{


			//clipping horizon vizibility
			#if _HORIZON
			//clip(IN.visibility-0.01);
			#endif

			//using arrays of textures (maybe temporary)
			TEX tempDiffuses[NUM] = MAIN_ARRAY; 
			TEX tempNormals[NUM] = BUMP_ARRAY;

			//directions
			float3 worldNormal = IN.wNormal;  //WorldNormalVector(IN, fixed3(0, 0, 1)); //cannot get IN.worldNormal directly because shader writes to o.Normal
			//worldNormal.y += 0.1f;  
			float3 normalPow = abs(pow(worldNormal, 8)) * worldNormal; //las multiply is needed to set + or -
			float3 directions = normalPow / (abs(normalPow.x) + abs(normalPow.y) + abs(normalPow.z));

			//finding camera distance (for 2 layer blend)
			float dist = distance(IN.wPos, _WorldSpaceCameraPos);

			//per-vertex types
			float blends[NUM] = BLEND_ARRAY;

			//getting colors array
			half4 albedoColors[NUM];
			for (int i = 0; i<NUM; i++)
				if (blends[i] > 0.000001)
					albedoColors[i] = FarTriplanarSample(tempDiffuses[i], IN.wPos, directions, dist); //(tempDiffuses[0], sx, sy, sz, dist);
				else albedoColors[i] = half4(0,0,0,0);
			
			//sampling default texture
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)
			//albedoColors[0] += _MainTex0.SampleGrad(sampler_MainTex0, float2(0, 0), 100, 100) * 0;
			#endif

			//mixing in heighmaps
			float heights[NUM] = HEIGHT_ARRAY;
			for (int i = 0; i<NUM; i++)
			{ 
				float vb = blends[i] * _BlendMapFactor*_BlendCrisp*2; //_BlendMapFactor*_BlendCrisp*2 to avoid too low values
				float hm = pow(albedoColors[i].a*heights[i], _BlendMapFactor) * _BlendMapFactor*_BlendCrisp*2;
				blends[i] = vb*hm; //vb*vb + 2 * vb*hm*(1 - vb);
				blends[i] = pow(blends[i], _BlendCrisp);
			}

			//normalizing blend
			float sum = 0;
			for (int i = 0; i<NUM; i++) sum += blends[i];
			for (int i = 0; i<NUM; i++) blends[i] /= sum;

			//setting final values
			o.Albedo = half3(0,0,0);
			for (int i=0; i<NUM; i++) o.Albedo += albedoColors[i] * blends[i];

			//normals
			half4 normalColor = half4(0,0,0,0);
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)
			for (int i = 0; i<NUM-1; i++)
			#else
			for (int i = 0; i<NUM; i++)
			#endif
				if (blends[i] > 0.000001)
					normalColor += FarTriplanarSample(tempNormals[i], IN.wPos, directions, dist) * blends[i]; //(tempNormals[i], sx, sy, sz, dist) * blends[i]; 
			o.Normal = UnpackNormal(normalColor);


			//calculating specular
			half4 tempSpecParams[NUM] = SPEC_ARRAY;
			for (int i = 0; i < NUM; i++)
			{
				half metal = dot(albedoColors[i], float3(0.3, 0.58, 0.12));
				float gloss = metal;

				metal = metal*tempSpecParams[i].y + tempSpecParams[i].x;
				gloss = gloss*tempSpecParams[i].w + tempSpecParams[i].z;

				//metal = (metal - 0.5f) * tempSpecParams[i].y + 0.5f; //older way to ajust - using bightness/contrast
				//metal = metal + tempSpecParams[i].x;
				
				o.Metallic += metal * blends[i];
				o.Smoothness += gloss * blends[i];
			}
			o.Metallic = saturate(o.Metallic);
			o.Smoothness = saturate(o.Smoothness);

			o.Occlusion = 1-_AmbientOcclusion + IN.ambient*_AmbientOcclusion;


			#if _HORIZON
				//calculating normal
				half4 heightColor = tex2D(_HorizonHeightmap, IN.wTexcoord);

				//visibility
				if (IN.visibility<0.01 || heightColor.r+heightColor.g<0.01) clip(-1);

				float3 baseNormal = float3(0,0,1);
				baseNormal.x = (heightColor.a - 0.5f)*2;
				baseNormal.y = -(heightColor.b - 0.5f)*2;
				baseNormal.z = sqrt(1 - saturate(dot(o.Normal.xy, o.Normal.xy)));

				o.Normal = baseNormal + float3(o.Normal.x, o.Normal.y, 0);
				o.Normal = normalize(o.Normal);
			#endif

			#if _PREVIEW
				if (_PreviewType == 1) o.Emission = o.Albedo;
				if (_PreviewType == 2) o.Emission = o.Metallic;
				if (_PreviewType == 3) o.Emission = o.Smoothness;
				if (_PreviewType == 4) o.Emission = abs(directions);

				#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3)
				if (_PreviewType == 5) o.Emission = IN.blendsB;
				#else
				if (_PreviewType == 5) o.Emission = IN.blendsA;
				#endif

				#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3)
				if (_PreviewType == 6) o.Emission = half3(blends[2], blends[3], blends[4]);
				#else
				if (_PreviewType == 6) o.Emission = half3(blends[0], blends[1], blends[2]); 
				#endif
				
				#if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X) || defined(SHADER_API_GLES3)
				if (_PreviewType == 7) o.Emission = half3(1,0,0);
				#elif defined(SHADER_API_GLCORE)
				if (_PreviewType == 7) o.Emission = half3(0,1,0);
				#else
				if (_PreviewType == 7) o.Emission = half3(0,0,1);
				#endif

				o.Alpha = 0;
				o.Albedo = 0;
				o.Metallic = 0;
				//o.Specular = 0;
				o.Smoothness = 0;
				o.Occlusion = 0;
				//o.Normal = 0;
				
			#endif
		}
		ENDCG
	}
	FallBack "Diffuse"
	CustomEditor "LandMaterialInspector"
}
