using UnityEngine;
using System.Collections;

using Plugins;

namespace Voxeland5
{
	[System.Serializable] public class LandTypeList : TypeList<BlockType> { }
	[System.Serializable] public class ConstructorTypeList : TypeList<ConstructorType> { }
	[System.Serializable] public class ObjectTypeList : TypeList<ObjectType> { }
	[System.Serializable] public class GrassTypeList : TypeList<GrassType> { }
	
	[System.Serializable]
	public class TypeList<T> where T : IBlockType
	{
		public T[] array = new T[0];
		public int selected = -1;

		//switching internal data types on change
		public bool changeBlockData = false;

		public void OnAddBlock (int n, object voxelandObj) 
		{ 
			Voxeland voxeland = (Voxeland)voxelandObj;

			if (typeof(T) == typeof(BlockType)) 
			{
				array[n] = (T)(object)(new BlockType() { name = "Block" });

				if (voxeland!=null)
				{
					ApplyToMaterial(voxeland.material);
					ApplyToMaterial(voxeland.farMaterial);
				}
			}
			else if (typeof(T) == typeof(ConstructorType)) array[n] = (T)(object)(new ConstructorType() { name = "Constructor" });
			else if (typeof(T) == typeof(ObjectType)) array[n] = (T)(object)(new ObjectType() { name = "Prefabs" });
			else if (typeof(T) == typeof(GrassType))
			{
				array[n] = (T)(object)(new GrassType() { name = "Grass" });
				if (voxeland!=null) ApplyToMaterial(voxeland.grassMaterial);
			}
			//I deadly wish static interface

			if (changeBlockData && voxelandObj!=null) 
			{
				if (typeof(T) != typeof(GrassType)) voxeland.data.InsertType((byte)n); //syncing blocks
				else voxeland.data.InsertGrassType((byte)n); //syncing grass
				voxeland.Rebuild();
			}
		}
		public void OnRemoveBlock (int n, object voxelandObj) 
		{
			Voxeland voxeland = (Voxeland)voxelandObj;

			if (changeBlockData && voxelandObj!=null) 
			{
				if (typeof(T) != typeof(GrassType)) voxeland.data.RemoveType((byte)n); //syncing blocks
				else voxeland.data.RemoveGrassType((byte)n); //syncing grass
				voxeland.Rebuild();
			}

			if (typeof(T) == typeof(BlockType) && voxeland!=null) 
			{
				ApplyToMaterial(voxeland.material);
				ApplyToMaterial(voxeland.farMaterial);
			}
			else if (typeof(T) == typeof(GrassType)) ApplyToMaterial(voxeland.grassMaterial);
		}
		public void OnSwitchBlock (int o, int n, object voxelandObj)
		{
			Voxeland voxeland = (Voxeland)voxelandObj;

			if (changeBlockData && voxelandObj!=null) 
			{
				if (typeof(T) != typeof(GrassType)) voxeland.data.SwitchType((byte)o, (byte)n); //syncing blocks
				else voxeland.data.SwitchGrassType((byte)o, (byte)n); //syncing grass
				voxeland.Rebuild();
			}

			if (typeof(T) == typeof(BlockType)  && voxeland!=null) 
			{
				ApplyToMaterial(voxeland.material);
				ApplyToMaterial(voxeland.farMaterial);
			}
			else if (typeof(T) == typeof(GrassType) && voxeland != null) ApplyToMaterial(voxeland.grassMaterial);
		}

		public void AddBlock (int n) { ArrayTools.Add(ref array, n); selected++; OnAddBlock(selected,null); } //to construct defaulty voxeland

		public void ApplyToMaterial (Material mat)
		{
			if (mat != null)
			for (int i=0; i<array.Length; i++) 
				array[i].ApplyToMaterial(mat, i); 
		}

		public void FillNames (ref string[] blockNames)
		{
			int namesCount = array.Length+4;
			if (blockNames == null || blockNames.Length != namesCount) blockNames = new string[namesCount];
			for (int i=0; i<namesCount-4; i++)
				blockNames[i] = array[i].name;
			blockNames[namesCount-3] = "Empty";
			blockNames[namesCount-1] = "Unlisted";
		}
	}


	public interface IBlockType
	{
		string name {get;set;}
		void OnGUI (Layout layout, bool selected, int num);
		void ApplyToMaterial (Material mat, int num);
	}

	[System.Serializable]
	public class BlockType : IBlockType
	{
		[SerializeField] private string _name;
		public string name {get {return _name;} set {_name=value;} }

		public Color color = new Color(0.77f, 0.77f, 0.77f, 1f);
		public Texture2D mainTex;
		public Texture2D bumpMap;
		#if RTP
		public Texture2D heightMap;
		#endif
		public float height = 1;
		public Vector2 metallic;
		public Vector2 glossiness;
		public bool  grass = true;
		//public Color grassTint = Color.white;
		//public float smooth = 1f; //not used

		public bool filledAmbient = true;

		public bool filledPrefab = false;
		public Transform[] prefabs = new Transform[0]; //new ObjectPool[0];

		public int guiHeight { get; set; }
		
		
		public void ApplyToMaterial (Material mat, int num)
		{
			if (mat.HasProperty("_MainTex"+num)) mat.SetTexture("_MainTex"+num, mainTex);
			if (mat.HasProperty("_BumpMap"+num)) mat.SetTexture("_BumpMap"+num, bumpMap);
			
			if (mat.HasProperty("_Height"+num)) mat.SetFloat("_Height"+num, height);
			if (mat.HasProperty("_SpecParams"+num)) mat.SetVector("_SpecParams"+num, new Vector4(metallic.x, metallic.y, glossiness.x, glossiness.y));
		}

		public void OnGUI (Layout layout, bool selected, int num)
		{
			if (!selected) { layout.Label(name); return; }

			name = layout.Field(name, style:layout.labelStyle); //layout.Field(name);

			layout.Par(14);
			layout.Label("Diff/height:", rect:layout.Inset(54f), fontSize:8);
			layout.Label("Normal:", rect:layout.Inset(54f), fontSize:8);

			layout.Par(54);
			mainTex = layout.Field(mainTex, rect:layout.Inset(54f));
			bumpMap = layout.Field(bumpMap, rect:layout.Inset(54f));

			layout.cursor.y -= 54;
			layout.margin += 108;
			layout.Field(ref height, "Height", fieldSize:0.67f);
			layout.Field(ref metallic, "Metallic", fieldSize:0.67f);
			layout.Field(ref glossiness, "Gloss", fieldSize:0.67f);
			//layout.Field(ref grass, "Has Grass", fieldSize:(25f/layout.field.width));
			layout.margin -= 108;
		}

		public void OnRTPGUI (Layout layout, bool selected, int num)
		{
			layout.Par(32);
			layout.Inset(3);
			layout.Icon(mainTex, rect:layout.Inset(32f), alphaBlend:false);

			if (selected) name = layout.Field(name, rect:layout.Inset(), style:layout.labelStyle);
			else layout.Label(name, rect:layout.Inset());
		}
	}

	[System.Serializable]
	public class ConstructorType : IBlockType
	{
		[SerializeField] private string _name;
		public string name {get {return _name;} set {_name=value;} }
		//public Constructor constructor;

		public void ApplyToMaterial (Material mat, int num) { }
		public void OnGUI (Layout layout, bool selected, int num) 
		{ 
		
		}
	}

	[System.Serializable]
	public class ObjectType : IBlockType
	{
		[SerializeField] private string _name;
		public string name {get {return _name;} set {_name=value;} }

		public Transform[] prefabs = new Transform[1];

		public void ApplyToMaterial (Material mat, int num) { }
		
		public int selectedPrefab = 0;

		public void OnGUI (Layout layout, bool selected, int num)
		{
			layout.Par();

			if (selected)
			{
				name = layout.Field(name, rect:layout.Inset(0.8f), style:layout.labelStyle);

				layout.DrawArrayAdd(ref prefabs, ref selectedPrefab, layout.Inset(0.1f), style:GUIStyle.none);
				layout.DrawArrayRemove(ref prefabs, ref selectedPrefab, layout.Inset(0.1f), style:GUIStyle.none);
				//for (int i=0; i<prefabs.Length; i++) if (prefabs[i] == null) prefabs[i] = new ObjectPool();
			
				layout.Par(3);

				for (int i=0; i<prefabs.Length; i++)
					layout.Field(ref prefabs[i]);
			}
			else layout.Label(name, rect:layout.Inset(0.99f));
		}
	}

	[System.Serializable]
	public class GrassType : IBlockType
	{
		[SerializeField] private string _name;
		public string name {get {return _name;} set {_name=value;} }

		public Mesh sourceMesh; //for editor only
		//public System.DateTime meshTimeStamp;
		public MeshWrapper[] meshes; 
		public Texture2D mainTex;
		public Texture2D bumpMap;
		
		public float height = 1;
		public float width = 1;
		public float elevation = 0;
		public float incline = 0.1f;
		public float random = 0.1f;
		public bool normalsFromTerrain = true;
		

		/*public bool CheckMesh ()
		{
			if (sourceMesh != null && (meshes==null || meshes.Length==0) ) return true;
			if ()
		}*/

		public void LoadMeshes ()
		{
			if (sourceMesh == null) { meshes = new MeshWrapper[0]; return; }

			meshes = new MeshWrapper[4];
			for (int i=0; i<4; i++)
			{
				meshes[i] = new MeshWrapper();
				meshes[i].ReadMesh(sourceMesh);
				meshes[i].RotateMirror(i*90, false);
			}
		}

		public void ApplyToMaterial (Material mat, int num)
		{
			if (mat.HasProperty("_MainTex"+num)) mat.SetTexture("_MainTex"+num, mainTex);
			if (mat.HasProperty("_BumpMap"+num)) mat.SetTexture("_BumpMap"+num, bumpMap);
		}

		public void OnGUI (Layout layout, bool selected, int num)
		{
			if (!selected) { layout.Label(name); return; }

			name =  layout.Field(name, style:layout.labelStyle);  //layout.Field(name);

			//layout.Field(ref sourceMesh, "Mesh");

			layout.Par();
			layout.Label("Mesh:", rect:layout.Inset(layout.field.width - 54f*2 - 20));
			layout.Label("Diffuse:", rect:layout.Inset(54f));
			layout.Label("Normal:", rect:layout.Inset(54f));

			layout.Par(54);
			Rect meshRect = layout.Inset(layout.field.width - 54f*2 - 20);
			meshRect.height = 20;
			layout.Field(ref sourceMesh, rect:meshRect);
			mainTex = layout.Field(mainTex, rect:layout.Inset(54f));
			bumpMap = layout.Field(bumpMap, rect:layout.Inset(54f));

			//layout.cursor.y -= 54;
			//layout.margin += 108;
			//layout.Field(ref height, "Height", fieldSize:0.67f);
			//layout.Field(ref width, "Width", fieldSize:0.67f);
			//layout.Field(ref elevation, "Elevation", fieldSize:0.67f);
			//layout.Field(ref incline, "Incline", fieldSize:0.67f);
			//layout.Field(ref random, "Random", fieldSize:0.67f);
			layout.Toggle(ref normalsFromTerrain, "Take Terrain Normals");
			//layout.margin -= 108;
		}
	}
}
