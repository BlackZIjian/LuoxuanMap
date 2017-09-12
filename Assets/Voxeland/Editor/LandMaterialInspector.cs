using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Plugins;

public class LandMaterialInspector : MaterialEditor
{
	Layout layout;
	bool[] openedChannels = new bool[20];

	public override void OnInspectorGUI ()
	{
		if (!isVisible) { base.OnInspectorGUI (); return; }
		
		Material mat = target as Material;
		
		if (layout == null) layout = new Layout();
		layout.margin = 0; layout.rightMargin = 0;
		layout.field = Layout.GetInspectorRect();
		layout.cursor = new Rect();
		layout.undoName =  "Material parameters change";
		layout.dragChange = true;
		layout.change = false;

		DrawMain(mat, layout);

		layout.Par(10);
		for (int i=0; i<openedChannels.Length; i++)
		{
			string layerName = "Layer " + i;
			if (mat.HasProperty("_MainTex"+i))
			{
				Texture mainTex = mat.GetTexture("_MainTex"+i);
				if (mainTex!=null) layerName = mainTex.name;
			}
			
			layout.Foldout(ref openedChannels[i], layerName);
			layout.margin += 5;
			if (openedChannels[i]) DrawLayer(mat, layout, i);
			layout.margin -= 5;
		}

		Layout.SetInspectorRect(layout.field);
	}

	public enum PreviewType { disabled=0, ambient=1, metallic=2, smoothness=3, directions=4, vertexBlends=5, finalBlends=6, define=7 };


	public static void DrawMain (Material mat, Layout layout)
	{
		layout.MatKeyword(mat, "_TRIPLANAR", "Triplanar");
		//layout.MatKeyword(mat, "_SPEC", "Metallic/Gloss");

		layout.Par(5);
		layout.MatField<float>(mat, "_Tile", "Tile");
		layout.MatField<float>(mat, "_BlendMapFactor", "Blend Map Factor");
		layout.MatField<float>(mat, "_BlendCrisp", "Blend Crispness");
		layout.MatField<float>(mat, "_Mips", "MipMap Factor");
		layout.MatField<float>(mat, "_AmbientOcclusion", "Ambient Occlusion");

		//far layer
		layout.Par(5);
		layout.MatKeyword(mat, "_DOUBLELAYER", "Far Layer");
		if (mat.IsKeywordEnabled("_DOUBLELAYER"))
		{
			layout.MatField<float>(mat, "_FarTile", "Far Tile");
			layout.MatField<float>(mat, "_FarStart", "Far Start");
			layout.MatField<float>(mat, "_FarEnd", "Far End");
			layout.disabled = false;
		}

		//horizon
		layout.Par(5);
		layout.MatKeyword(mat, "_HORIZON", "Horizon");
		if (mat.IsKeywordEnabled("_HORIZON"))
		{
			layout.MatField<float>(mat, "_HorizonHeightScale", "Horizon Height Scale");
			layout.MatField<Texture>(mat, "_HorizonHeightmap", "Horizon Heightmap");
			layout.MatField<Texture>(mat, "_HorizonTypemap", "Horizon Typemap");
			layout.MatField<Texture>(mat, "_HorizonVisibilityMap", "Horizon Visibility Map");
			layout.MatField<float>(mat, "_HorizonBorderLower", "Horizon Border Lower");
		}

		//preview
		layout.Par(5);
		if (!mat.HasProperty("_PreviewType")) { layout.disabled = true; layout.Field(PreviewType.disabled, "Preview"); layout.disabled = false; }
		else
		{
			PreviewType previewType = (PreviewType)mat.GetInt("_PreviewType");
			if (!mat.IsKeywordEnabled("_PREVIEW")) previewType = PreviewType.disabled;
			layout.Field(ref previewType, "Preview");
			if (layout.lastChange)
			{
				if (previewType == PreviewType.disabled) { mat.DisableKeyword("_PREVIEW"); mat.SetInt("_PreviewType", 0); }
				else { mat.EnableKeyword("_PREVIEW"); mat.SetInt("_PreviewType", (int)previewType); }
			}
			layout.disabled = false;
		}
	}

	public static void DrawLayer (Material mat, Layout layout, int num)
	{
		layout.Par(54);
		layout.MatField<Texture>(mat, "_MainTex"+num, rect:layout.Inset(54f));
		layout.MatField<Texture>(mat, "_BumpMap"+num, rect:layout.Inset(54f));

		layout.cursor.y -= 54;
		layout.margin += 108;
		layout.MatField<float>(mat, "_Height"+num, "Height", fieldSize:0.67f);
		layout.MatField<Vector2>(mat, "_SpecParams"+num, "Metallic", fieldSize:0.67f, zwOfVector4:false);
		layout.MatField<Vector2>(mat, "_SpecParams"+num, "Gloss", fieldSize:0.67f, zwOfVector4:true);
		layout.margin -= 108;
	}



}
