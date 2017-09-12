
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Plugins;

namespace Voxeland5 
{
	[CustomEditor(typeof(Voxeland))]
	public class VoxelandEditor : Editor
	{
		[System.NonSerialized] private CoordDir prevAimCoord = new CoordDir(false);
		[System.NonSerialized] private int mouseButton = -1;
		[System.NonSerialized] private bool gaugeDisplayed = false; //for repainting the last final frame of progress gauge
		public enum FixedInfinite { Fixed, Infinite }
		
		Voxeland voxeland;
		Layout layout;
		//MaterialEditor mainMatEditor;
		//MaterialEditor farMatEditor;
		//MaterialEditor grassMatEditor;
		//MaterialEditor highlightMatEditor;

		[System.NonSerialized] private string[] blockNames;
		[System.NonSerialized] private string[] objectNames;
		[System.NonSerialized] private string[] grassNames;

		public void OnSceneGUI ()
		{	
			//if (Event.current.type == EventType.Layout) return;

			//updating Inspector GUI if thread is working (it lags less if done from OnSceneGUI... it's so unity)
			if (ThreadWorker.IsWorking("Voxeland") || gaugeDisplayed) Repaint();

			Voxeland voxeland = (Voxeland)target;
			if (voxeland.data == null) return;

			//disabling selection
			if (voxeland.guiLockSelection) HandleUtility.AddDefaultControl( GUIUtility.GetControlID(FocusType.Passive) ); 

			//hiding wireframe
			voxeland.transform.ToggleDisplayWireframe(!voxeland.guiHideWireframe);

			//getting mouse button
			if (Event.current.type == EventType.mouseDown) mouseButton = Event.current.button;
			if (Event.current.rawType == EventType.mouseUp) mouseButton = -1;

			//getting mouse pos
			SceneView sceneview = UnityEditor.SceneView.lastActiveSceneView;
			if (sceneview==null || sceneview.camera==null) return;
			Vector2 mousePos = Event.current.mousePosition;
			mousePos = new Vector2(mousePos.x/sceneview.camera.pixelWidth, mousePos.y/sceneview.camera.pixelHeight);
			#if UNITY_5_4_OR_NEWER 	
			mousePos *= EditorGUIUtility.pixelsPerPoint;
			#endif
			mousePos.y = 1 - mousePos.y;

			//aiming
			Ray aimRay = sceneview.camera.ViewportPointToRay(mousePos);
			CoordDir aimCoord = voxeland.PointOut(aimRay);

			//focusing on brush
			if(Event.current.commandName == "FrameSelected")
			{ 
				Event.current.Use();
				if (aimCoord.exists) UnityEditor.SceneView.lastActiveSceneView.LookAt(
					voxeland.transform.TransformPoint( aimCoord.vector3 ), 
					UnityEditor.SceneView.lastActiveSceneView.rotation,
					Mathf.Max(voxeland.brush.extent*6, 20), 
					UnityEditor.SceneView.lastActiveSceneView.orthographic, 
					false);
				else 
				{
					Coord rectCenter = voxeland.chunks.deployedRects[0].Center * voxeland.chunkSize;
					int rectExtend = voxeland.chunks.deployedRects[0].size.x * voxeland.chunkSize;
					int height; byte temp;
					voxeland.data.GetTopTypePoint(rectCenter.x, rectCenter.z, out height, out temp);
					UnityEditor.SceneView.lastActiveSceneView.LookAt(
						voxeland.transform.TransformPoint( new Vector3(rectCenter.x, height, rectCenter.z) ), 
						UnityEditor.SceneView.lastActiveSceneView.rotation,
						rectExtend, 
						UnityEditor.SceneView.lastActiveSceneView.orthographic, 
						false);
				}
			}

			//if any change
			if (prevAimCoord != aimCoord || mouseButton==0)
			{
				//getting edit mode
				Voxeland.EditMode editMode = Voxeland.EditMode.none;
				bool buttonDown = false;
				if (!voxeland.continuousPainting) buttonDown = Event.current.type==EventType.mouseDown && Event.current.button==0;
				else buttonDown = mouseButton==0;
				if (buttonDown && !Event.current.alt)
				{
					if (Event.current.control && Event.current.shift) editMode = voxeland.controlShiftEditMode;
					else if (Event.current.shift) editMode = voxeland.shiftEditMode;
					else if (Event.current.control) editMode = voxeland.controlEditMode;
					else editMode = voxeland.standardEditMode;
				}

				//highlight
				if (voxeland.highlight!=null) // && Event.current.type!=EventType.KeyDown && Event.current.type!=EventType.mouseDrag) //do not redraw highlight on alt pressed
				{
					if (aimCoord.exists) voxeland.Highlight(aimCoord, voxeland.brush, isEditing:editMode!=Voxeland.EditMode.none);
					else voxeland.highlight.Clear(); //clearing highlight if nothing aimed or voxeland not selected
				}

				//altering
				if (editMode!=Voxeland.EditMode.none && aimCoord.exists) 
				{
					//if (voxeland.landTypes.selected>=0)  voxeland.Alter(aimCoord, voxeland.brush, editMode, (byte)voxeland.landTypes.selected);
					//if (voxeland.objectsTypes.selected>=0)  voxeland.AlterObjects(aimCoord, voxeland.brush, editMode, (byte)voxeland.objectsTypes.selected);
					//if (voxeland.grassTypes.selected>=0) voxeland.AlterGrass(aimCoord, voxeland.brush, editMode, (byte)voxeland.grassTypes.selected);

					voxeland.Alter(aimCoord, voxeland.brush, editMode, landType:voxeland.landTypes.selected, objectType:voxeland.objectsTypes.selected, grassType:voxeland.grassTypes.selected);
				}
				
				prevAimCoord = aimCoord;
				
				//SceneView.lastActiveSceneView.camera.Render(); //forcing repaint
				SceneView.lastActiveSceneView.Repaint();
			} //if coord or button change

		}


		//repainting gui to make a animated indicator
		private void OnInspectorUpdate () 
		{ 	
			if (ThreadWorker.IsWorking("Voxeland")) Repaint();
		}

		public override void OnInspectorGUI ()
		{
			voxeland = (Voxeland)target;

			//assigning voxeland to mapmagic window
			#if MAPMAGIC
			if (MapMagic.MapMagicWindow.instance != null && MapMagic.MapMagicWindow.instance.mapMagic != (MapMagic.IMapMagic)voxeland && voxeland.data != null && voxeland.data.generator != null && voxeland.data.generator.mapMagicGens != null)
				MapMagic.MapMagicWindow.Show(voxeland.data.generator.mapMagicGens, voxeland, forceOpen:false);
			#endif

			if (layout == null) layout = new Layout();
			layout.margin = 0; layout.rightMargin = 0;
			layout.field = Layout.GetInspectorRect();
			layout.cursor = new Rect();
			layout.undoObject = voxeland;
			layout.undoName =  "Voxeland settings change";
			layout.dragChange = true;
			layout.change = false;
			layout.delayed = true;

			#region Progress

				layout.Par();

				if (ThreadWorker.IsWorking("Voxeland"))
				{
					float calculatedSum; float completeSum; float totalSum;
					ThreadWorker.GetProgresByTag("VoxelandChunk", out totalSum, out calculatedSum, out completeSum);

					if (totalSum>10) 
					{
						Rect gaugeRect = layout.Inset(0.7f);
						layout.Gauge(0, "", gaugeRect);
						layout.Gauge(1, "", new Rect(gaugeRect.x, gaugeRect.y, gaugeRect.width * calculatedSum/totalSum, gaugeRect.height), disabled:true);
						layout.Gauge(1, "", new Rect(gaugeRect.x, gaugeRect.y, gaugeRect.width * completeSum/totalSum, gaugeRect.height));

					//	Rect cursor = layout.cursor;
				//		layout.Gauge(calculatedSum/totalSum, "", layout.Inset(0.7f), disabled:true);
					//	layout.cursor = cursor;
				//		layout.Gauge(calculatedSum/totalSum, "", layout.Inset(0.7f));
						//layout.Gauge(progress, "Progress: " + (int)completeSum + "(" + calculatedSum + ")" + "/" + (int)totalSum, layout.Inset(0.7f));
					//	layout.cursor = cursor;
						layout.Label("Progress: " + (int)completeSum + "(" + calculatedSum + ")" + "/" + (int)totalSum, gaugeRect);
					}
					else layout.Label("Progress: building", layout.Inset(0.7f));
					gaugeDisplayed = true;

					Repaint();
				}
				else 
				{
					layout.Label("Progress: complete", layout.Inset(0.7f));
					gaugeDisplayed = false;
				}

				if (layout.Button("Rebuild", layout.Inset(0.3f))) voxeland.Rebuild();

			#endregion

			layout.margin = 10;

			#region Brush
				layout.Par(5); layout.Foldout(ref voxeland.guiBrush, "Brush");
				if (voxeland.guiBrush) 
				{
					layout.Field(ref voxeland.brush.form, "Form");
					 voxeland.brush.extent = layout.Field(voxeland.brush.extent, "Extent", min:0, max: voxeland.brush.maxExtent, slider:true);
					layout.Toggle(ref voxeland.brush.round, "Round");
			
					layout.Par(5);
					if (voxeland.brush.form==Brush.Form.stamp)
					{
						 layout.Field(ref voxeland.brush.getStamp, "Get Stamp");
						if (voxeland.brush.getStamp)
						{
							layout.Par();
							layout.Label("Min:",rect:layout.Inset(0.25f));
							layout.Field(ref voxeland.brush.getStampMin.x, "X", rect:layout.Inset(0.25f));
							layout.Field(ref voxeland.brush.getStampMin.y, "Y", rect:layout.Inset(0.25f));
							layout.Field(ref voxeland.brush.getStampMin.z, "Z", rect:layout.Inset(0.25f));

							layout.Par();
							layout.Label("Max:",rect:layout.Inset(0.25f));
							layout.Field(ref voxeland.brush.getStampMax.x, "X", rect:layout.Inset(0.25f));
							layout.Field(ref voxeland.brush.getStampMax.y, "Y", rect:layout.Inset(0.25f));
							layout.Field(ref voxeland.brush.getStampMax.z, "Z", rect:layout.Inset(0.25f));
						}
					}

					layout.Par(5);
					layout.Field(ref voxeland.standardEditMode, "Standard Edit");
					layout.Field(ref voxeland.controlEditMode, "Control Mode");
					layout.Field(ref voxeland.shiftEditMode, "Shift Mode");
					layout.Field(ref voxeland.controlShiftEditMode, "Control+Shift Mode");

					layout.Par(5);
					layout.Toggle(ref voxeland.continuousPainting, "Continuous Painting");
				}
			#endregion

			#region Types
			layout.Par(5); layout.Foldout(ref voxeland.guiBlocks, "Land Blocks");
			if (voxeland.guiBlocks) 
			{ 
				layout.margin += 5; layout.rightMargin += 5;

				TypeList<BlockType> types = voxeland.landTypes;

				#if RTP
				bool rtpMat = false;
				if (voxeland.material!=null && voxeland.material.shader.name.Contains("Relief Pack")) 
				{
					rtpMat = true;

					for (int i=0; i<Mathf.Min(voxeland.landTypes.array.Length,4); i++)
					{
						if (voxeland.material.HasProperty("_SplatA"+i.ToString()))
							voxeland.landTypes.array[i].mainTex = (Texture2D)voxeland.material.GetTexture("_SplatA"+i.ToString());
					}
				}
				#endif

				for (int i=0; i<types.array.Length; i++)
				{
					#if RTP
					if (rtpMat)
					{
						if (layout.DrawWithBackground(types.array[i].OnRTPGUI, active:i==types.selected))
							voxeland.Select(i, typeof(BlockType));
					}
					else
					{
						if (layout.DrawWithBackground(types.array[i].OnGUI, active:i==types.selected))
							voxeland.Select(i, typeof(BlockType));
					}

					if (layout.lastChange && !rtpMat) 
					{
						voxeland.landTypes.ApplyToMaterial(voxeland.material);
						voxeland.landTypes.ApplyToMaterial(voxeland.farMaterial);
					}

					#else
					if (layout.DrawWithBackground(types.array[i].OnGUI, active:i==types.selected))
						voxeland.Select(i, typeof(BlockType));

					if (layout.lastChange) 
					{
						voxeland.landTypes.ApplyToMaterial(voxeland.material);
						voxeland.landTypes.ApplyToMaterial(voxeland.farMaterial);
					}
					#endif
				}

				layout.Par(3); layout.Par();
				layout.DrawArrayAdd(ref types.array, ref types.selected, layout.Inset(0.15f), onAdded:types.OnAddBlock, caller:voxeland);
				layout.DrawArrayRemove(ref types.array, ref types.selected, layout.Inset(0.15f), onRemoved:types.OnRemoveBlock, caller:voxeland);
				layout.DrawArrayUp(ref types.array, ref types.selected, layout.Inset(0.15f), reverseOrder:false, onSwitch:types.OnSwitchBlock, caller:voxeland);
				layout.DrawArrayDown(ref types.array, ref types.selected, layout.Inset(0.15f), reverseOrder:false, onSwitch:types.OnSwitchBlock, caller:voxeland);

				layout.Inset(0.05f);

				layout.Field(ref types.changeBlockData, "Sync Data", rect:layout.Inset(0.35f), fieldSize:0.15f);

				layout.margin -= 5; layout.rightMargin -= 5;
			}

			layout.Par(5); layout.Foldout(ref voxeland.guiObjects, "Object Blocks");
			if (voxeland.guiObjects) 
			{ 
				layout.margin += 5; layout.rightMargin += 5;

				TypeList<ObjectType> types = voxeland.objectsTypes;

				for (int i=0; i<types.array.Length; i++)
					if (layout.DrawWithBackground(types.array[i].OnGUI, active:i==types.selected))
						voxeland.Select(i, typeof(ObjectType));

				layout.Par(3); layout.Par();
				layout.DrawArrayAdd(ref types.array, ref types.selected, layout.Inset(0.15f), onAdded:types.OnAddBlock, caller:voxeland);
				layout.DrawArrayRemove(ref types.array, ref types.selected, layout.Inset(0.15f), onRemoved:types.OnRemoveBlock, caller:voxeland);
				layout.DrawArrayUp(ref types.array, ref types.selected, layout.Inset(0.15f), reverseOrder:false, onSwitch:types.OnSwitchBlock, caller:voxeland);
				layout.DrawArrayDown(ref types.array, ref types.selected, layout.Inset(0.15f), reverseOrder:false, onSwitch:types.OnSwitchBlock, caller:voxeland);

				layout.Inset(0.05f);

				layout.Field(ref types.changeBlockData, "Sync Data", rect:layout.Inset(0.35f), fieldSize:0.15f);

				layout.margin -= 5; layout.rightMargin -= 5;
			}

			layout.Par(5); layout.Foldout(ref voxeland.guiGrass, "Grass");
			if (voxeland.guiGrass) 
			{
				layout.margin += 5; layout.rightMargin += 5;

				TypeList<GrassType> types = voxeland.grassTypes;

				for (int i=0; i<types.array.Length; i++)
				{
					if (layout.DrawWithBackground(types.array[i].OnGUI, active:i==types.selected))
						voxeland.Select(i, typeof(GrassType));

					if (layout.lastChange) 
					{
						for (int g=0; g<voxeland.grassTypes.array.Length; g++) voxeland.grassTypes.array[g].LoadMeshes();
						voxeland.grassTypes.ApplyToMaterial(voxeland.grassMaterial);
					}
				}

				layout.Par(3); layout.Par();
				layout.DrawArrayAdd(ref types.array, ref types.selected, layout.Inset(0.15f), onAdded:types.OnAddBlock, caller:voxeland);
				layout.DrawArrayRemove(ref types.array, ref types.selected, layout.Inset(0.15f), onRemoved:types.OnRemoveBlock, caller:voxeland);
				layout.DrawArrayUp(ref types.array, ref types.selected, layout.Inset(0.15f), reverseOrder:false, onSwitch:types.OnSwitchBlock, caller:voxeland);
				layout.DrawArrayDown(ref types.array, ref types.selected, layout.Inset(0.15f), reverseOrder:false, onSwitch:types.OnSwitchBlock, caller:voxeland);

				layout.Inset(0.05f);

				layout.Field(ref types.changeBlockData, "Sync Data", rect:layout.Inset(0.35f), fieldSize:0.15f);

				layout.margin -= 5; layout.rightMargin -= 5;
			}
			#endregion

			#region Distances
			layout.Par(5); layout.Foldout(ref voxeland.guiDistances, "Mode and Ranges");
			if (voxeland.guiDistances)
			{
				layout.fieldSize = 0.4f;
				layout.Field(ref voxeland.editorSizeMode, "Editor Mode");
				layout.Field(ref voxeland.playmodeSizeMode, "Playmode Mode");
				
				layout.Par(5);
				if (voxeland.editorSizeMode!=Voxeland.SizeMode.DynamicInfinite || voxeland.playmodeSizeMode!=Voxeland.SizeMode.DynamicInfinite)
				{
					layout.Field(ref voxeland.terrainSize, "Terrain Size");
					voxeland.terrainSize = ((int)(voxeland.terrainSize/voxeland.chunkSize)) * voxeland.chunkSize;
				}

				bool prevChange = layout.change;
				layout.change = false;

				if (voxeland.editorSizeMode!=Voxeland.SizeMode.Static || voxeland.playmodeSizeMode!=Voxeland.SizeMode.Static)
				{
					layout.Par(5);
					layout.Label("Terrain Ranges:");
					if (voxeland.guiDistancesFullControl)
					{
						layout.fieldSize = 0.2f;
						voxeland.guiDistancesMax = layout.Field(voxeland.guiDistancesMax, "Max Distance", delayed:true);
						layout.fieldSize = 0.8f;
					
						layout.Par(5);
						layout.Label("Chunks");
						DrawRange("Create", ref voxeland.createRange, 0.01f, voxeland.guiDistancesMax, voxeland.guiDistancesMax);
						DrawRange("Remove", ref voxeland.removeRange, voxeland.createRange, voxeland.guiDistancesMax, voxeland.guiDistancesMax);

						layout.Par(5);
						layout.Label("Voxel Meshes");
						DrawRange("Low Detail", ref voxeland.voxelLoRange,	voxeland.voxelHiRange,	voxeland.createRange,	voxeland.guiDistancesMax);
						DrawRange("High Detail", ref voxeland.voxelHiRange,	0.01f,						voxeland.voxelLoRange,	voxeland.guiDistancesMax);
						DrawRange("Collision", ref voxeland.collisionRange,	0.01f,						voxeland.voxelLoRange,	voxeland.guiDistancesMax);
						DrawRange("Objects", ref voxeland.objectsRange,		0.01f,						voxeland.createRange,	voxeland.guiDistancesMax);
						DrawRange("Grass", ref voxeland.grassRange,			0.01f,						voxeland.voxelLoRange,	voxeland.guiDistancesMax);
						DrawRange("Remove", ref voxeland.voxelRemoveRange,	voxeland.voxelLoRange,	voxeland.createRange,	voxeland.guiDistancesMax);

						layout.Par(5);
						layout.Label("Sculpt Areas");
						DrawRange("Generate", ref voxeland.areaGenerateRange, 0.01f, voxeland.areaRemoveRange, voxeland.areaRemoveRange);
						DrawRange("Remove", ref voxeland.areaRemoveRange, voxeland.areaGenerateRange, voxeland.areaRemoveRange, voxeland.areaRemoveRange);
					}

					else 
					{
						layout.fieldSize = 0.8f;
					
						DrawRange("Voxel Mesh", ref voxeland.voxelLoRange,	0.01f,	300, 300);
						DrawRange("Objects", ref voxeland.objectsRange,		0.01f,	300, 300);

						layout.Par(5);
						DrawRange("Collision", ref voxeland.collisionRange,	0.01f,	voxeland.voxelLoRange,	voxeland.guiDistancesMax);
						DrawRange("Grass", ref voxeland.grassRange,			0.01f,	voxeland.voxelLoRange,	voxeland.guiDistancesMax);
						DrawRange("High Detail", ref voxeland.voxelHiRange,	0.01f,	voxeland.voxelLoRange,	voxeland.guiDistancesMax);

						voxeland.createRange = Mathf.Max(voxeland.voxelLoRange, voxeland.objectsRange);
						voxeland.removeRange = voxeland.createRange + voxeland.chunkSize*2+1;
						voxeland.voxelRemoveRange = voxeland.voxelLoRange + voxeland.chunkSize*2+1;

						voxeland.areaGenerateRange = voxeland.createRange + voxeland.data.areaSize;
						voxeland.areaRemoveRange = voxeland.areaGenerateRange + voxeland.data.areaSize*2;
					}

					layout.Toggle(ref voxeland.guiDistancesFullControl, "Advanced");
					layout.fieldSize = 0.5f;
				}

				if (layout.change) voxeland.ForceUpdate();
				layout.change = prevChange;

				//voxeland.voxelHiRange = layout.Field(new Vector2(0, voxeland.voxelHiRange), "Hi Range", min:0, limit:false, max:maxRange, slider:true, quadratic:true).y;
				//voxeland.voxelLoRange = layout.Field(new Vector2(voxeland.voxelHiRange, voxeland.voxelLoRange), "Lo Range", min:0, limit:false, max:maxRange, slider:true, quadratic:true).y;
			}
			#endregion

			#region Settings
			layout.Par(5); layout.Foldout(ref voxeland.guiSettings, "Settings");
			if (voxeland.guiSettings)
			{
				layout.margin += 5;
				layout.fieldSize = 0.4f;

				//data label
				layout.Par(5);
				voxeland.data = layout.ScriptableAssetField(voxeland.data, construct:null, savePath:null, fieldSize:0.8f);

				//margins
				layout.Par(5);
				layout.Field(ref voxeland.meshMargin, "Mesh Margin");
				layout.Field(ref voxeland.ambientMargin, "Ambient Margin");
				layout.Field(ref voxeland.ambientFade, "Ambient Fade", min:0, max:1);
				layout.Field(ref voxeland.normalsSmooth, "Smooth Normals", min:0, max:5);
				layout.Field(ref voxeland.relaxStrength, "Relax Strength", min:0, max:3);
				layout.Field(ref voxeland.relaxIterations, "Relax Iterations", min:0, max:10);
				if (voxeland.relaxIterations > voxeland.meshMargin)
				{
					layout.Par(30);
					layout.Label("Consider using Mesh Margin value higher than Relax Iterations to avoid visible seams between chunks.", rect:layout.Inset(), helpbox:true);
				}
				layout.Field(ref voxeland.encodeChannelsToColors, "Color Channels (RTP Mode)");
				layout.Field(ref voxeland.playmodeEdit, "Edit in Playmode");
				

				//threads
				layout.Par(5);
				layout.Field(ref ThreadWorker.multithreading, "Multithreading");

				if (ThreadWorker.multithreading)
				{
					layout.Par();
					layout.Field(ref ThreadWorker.maxThreads, "Max Threads", rect:layout.Inset(0.75f), fieldSize:0.2f, disabled:ThreadWorker.autoMaxThreads);
					layout.Toggle(ref ThreadWorker.autoMaxThreads, "Auto",rect:layout.Inset(0.25f));
				}
				else layout.Field(ref ThreadWorker.maxThreads, "Max Coroutines");
				 
				layout.Field(ref ThreadWorker.maxApplyTime, "Max Apply Time");

				//other
				layout.Par(5);
				layout.Field(ref voxeland.guiLockSelection, "Lock Selection");
				layout.Field(ref voxeland.guiHideWireframe, "Hide Frame");
				if (layout.lastChange) voxeland.transform.ToggleDisplayWireframe(!voxeland.guiHideWireframe);
				layout.Field(ref voxeland.hideColliderWire, "Hide Collider Wire");
				layout.Field(ref voxeland.brush.maxExtent, "Max Brush Size");
				if (voxeland.brush.maxExtent>8) { layout.Par(30); layout.Label("Large brush extent can slow down Blob Brush display", layout.Inset(), helpbox:true); }

				layout.Par(5);
				layout.Field(ref voxeland.saveMeshes, "Save Meshes with Scene");
				layout.Field(ref voxeland.saveNonpinnedAreas, "Save Non-pinned Areas");

				layout.Par(5);
				layout.Field(ref voxeland.chunkName, "Default Chunk Name");
				layout.Field(ref voxeland.copyLayersTags, "Copy Layers and Tags to Chunk");
				layout.Field(ref voxeland.copyComponents, "Copy Components to Chunk");

				//horizon
				layout.Par(10);
				bool useHorizon = layout.Toggle(voxeland.horizon!=null, "Horizon Mesh");
				if (useHorizon && voxeland.horizon==null) { voxeland.horizon = Horizon.Create(voxeland); voxeland.RefreshMaterials(); }
				if (!useHorizon && voxeland.horizon!=null) GameObject.DestroyImmediate(voxeland.horizon.gameObject);
				if (voxeland.horizon!=null)
				{
					voxeland.horizon.meshFilter.sharedMesh = layout.Field(voxeland.horizon.meshFilter.sharedMesh, "Mesh");
					layout.Field(ref voxeland.horizon.meshSize, "Mesh Bounding Box Size");
					layout.Field(ref voxeland.horizon.scale, "Scale");
					layout.Field(ref voxeland.horizon.textureResolutions, "Texture Resolution");
				}
				else
				{
					layout.Field<Mesh>(null, "Mesh", disabled:true);
					layout.Field(60, "Mesh Bounding Box Size", disabled:true);
					layout.Field(1, "Scale", disabled:true);
					layout.Field(512, "Texture Resolution", disabled:true);
				}

				//floaty origin solution
				layout.Par(10);
				layout.Toggle(ref voxeland.shift, "Shift World (Floating Point Solution)");
				layout.Field(ref voxeland.shiftThreshold, "Shift Threshold", disabled:!voxeland.shift);

				//debug
				BuildTargetGroup buildGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
				string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildGroup);
				
				bool debug = false;
				if (defineSymbols.Contains("WDEBUG;") || defineSymbols.EndsWith("WDEBUG")) debug = true;
				
				layout.Par(10);
				layout.Toggle(ref debug, "Debug (Requires re-compile)");
				if (layout.lastChange) 
				{
					if (debug)
					{
						defineSymbols += (defineSymbols.Length!=0? ";" : "") + "WDEBUG";
					}
					else
					{
						defineSymbols = defineSymbols.Replace("WDEBUG","");  
						defineSymbols = defineSymbols.Replace(";;", ";"); 
					}
					PlayerSettings.SetScriptingDefineSymbolsForGroup(buildGroup, defineSymbols);
				}
			}
			#endregion



			#region Material
			layout.Par(5); layout.Foldout(ref voxeland.guiMaterial, "Materials");
			if (voxeland.guiMaterial)
			{
				layout.margin += 10;
				layout.fieldSize = 0.5f;

				layout.Par();
				layout.Foldout(ref voxeland.guiMaterialMain, "Main Material", rect:layout.Inset(0.5f), bold:false);
				layout.Field(ref voxeland.material, rect:layout.Inset(0.5f));
				if (layout.lastChange) { voxeland.RefreshMaterials(); } // mainMatEditor = null; }
				if (voxeland.guiMaterialMain)
				{
					layout.margin += 15;
					if (voxeland.material != null) LandMaterialInspector.DrawMain(voxeland.material, layout);
					layout.margin -= 15;
					layout.Par(10);
				}

				layout.Par(3);
				layout.Par();
				layout.Foldout(ref voxeland.guiMaterialFar, "Horizon Material", rect:layout.Inset(0.5f), bold:false);
				layout.Field(ref voxeland.farMaterial, rect:layout.Inset(0.5f));
				if (layout.lastChange) voxeland.RefreshMaterials();
				if (voxeland.guiMaterialFar)
				{
					layout.margin += 15;
					if (voxeland.farMaterial != null) LandMaterialInspector.DrawMain(voxeland.farMaterial, layout);
					layout.margin -= 15;
					layout.Par(10);
				}

				layout.Par(3);
				layout.Par();
				layout.Foldout(ref voxeland.guiMaterialGrass, "Grass Material", rect:layout.Inset(0.5f), bold:false);
				layout.Field(ref voxeland.grassMaterial, rect:layout.Inset(0.5f));
				if (layout.lastChange) voxeland.RefreshMaterials();
				if (voxeland.guiMaterialGrass)
				{
					layout.margin += 15;
					if (voxeland.grassMaterial != null) GrassMaterialInspector.DrawMain(voxeland.grassMaterial, layout);
					layout.margin -= 15;
					layout.Par(10);
				}

				layout.Par(3);
				if (voxeland.highlight == null) layout.Field<Material>(null, "Highlight Material", disabled:true);
				else layout.Field(voxeland.highlight.material, "Highlight Material");

				layout.margin -= 10;
			}
			#endregion

			#region Generate
			layout.Par(5); layout.Foldout(ref voxeland.guiGenerate, "Generate");
			if (voxeland.guiGenerate)
			{
				layout.margin += 10;

				if (voxeland.data == null) layout.Label("No proper Voxeland data to generate");
				else
				{	
					Generator generator = voxeland.data.generator;
					bool change = layout.change;
					layout.change = false;
				
					layout.Field(ref generator.generatorType, "Generator Type");

					voxeland.landTypes.FillNames(ref blockNames);
					voxeland.objectsTypes.FillNames(ref objectNames);
					voxeland.grassTypes.FillNames(ref grassNames);
			
					if (generator.generatorType == Generator.GeneratorType.Planar)
					{
						generator.planarGen.OnGUI(layout, blockNames);
					}

					if (generator.generatorType == Generator.GeneratorType.Noise)
					{
						generator.noiseGen.OnGUI(layout, blockNames);
						generator.curveGen.OnGUI(layout, blockNames);
						generator.slopeGen.OnGUI(layout, blockNames);
						generator.cavityGen.OnGUI(layout, blockNames);
						generator.blurGen.OnGUI(layout, blockNames);
						generator.stainsGen.OnGUI(layout, blockNames);
						generator.scatterGen.OnGUI(layout, objectNames, blockNames);
						generator.grassGen.OnGUI(layout, grassNames, blockNames);
					}

					else if (generator.generatorType == Generator.GeneratorType.MapMagic)
					{
						#if MAPMAGIC
						MapMagic.VoxelandOutput.voxeland = voxeland;
						MapMagic.VoxelandGrassOutput.voxeland = voxeland;
						MapMagic.VoxelandObjectsOutput.voxeland = voxeland;

						//data field
						layout.Par(5);
						layout.fieldSize = 0.7f;
						generator.mapMagicGens = layout.ScriptableAssetField(generator.mapMagicGens, construct:MapMagic.GeneratorsAsset.DefaultVoxeland, savePath: null);
						if (layout.lastChange) 
							MapMagic.MapMagicWindow.Show(generator.mapMagicGens, voxeland, forceOpen:false, asBiome:false);
				
						//show editor button
						layout.Par(5);
						layout.Par(22);
						if (layout.Button("Show Editor", rect:layout.Inset(1f), disabled:generator.mapMagicGens==null))
							MapMagic.MapMagicWindow.Show(generator.mapMagicGens, voxeland, forceOpen:true);
					
						#else
						layout.Par(40);
						layout.Label("MapMagic World Generator does not seems to be installed. If you sure that you have it try restarting Unity to add it's define symbol.", rect:layout.Inset(), helpbox:true);
						#endif
					}

					else if (generator.generatorType == Generator.GeneratorType.Heightmap)
					{
						generator.standaloneHeightGen.OnGUI(layout, blockNames);
					}

					if (generator.generatorType != Generator.GeneratorType.Planar)
					{
						layout.Par(10);
				
						layout.Field(ref generator.seed, "Seed"); if (layout.lastChange) generator.change = true;
						#if MAPMAGIC
						if (layout.lastChange) voxeland.ClearResults();
						#endif
						layout.Field(ref generator.heightFactor, "Height"); if (layout.lastChange) generator.change = true;
						layout.Toggle(ref generator.saveResults, "Save Interim Results");
						layout.Toggle(ref generator.instantGenerate, "Instant Generate");
						layout.Toggle(ref generator.polish, "Polish");
				
						layout.Par();
						layout.Toggle(ref generator.removeThinLayers, "Remove Thin Layers", rect:layout.Inset(0.8f));
						layout.Field(ref generator.minLayerThickness, rect:layout.Inset(0.2f));

						if (layout.change) voxeland.Generate();
						layout.change = change;
					}

					layout.Par(5);

					string readyIcon = "Voxeland_Success"; int readyAnimFrames = 0;
					if (ThreadWorker.IsWorking("VoxelandGenerate")) { readyIcon = "Voxeland_Loading"; readyAnimFrames=12; Repaint(); }

					layout.Par(24);
					if (layout.Button("Generate", rect:layout.Inset(), icon:readyIcon, iconAnimFrames:readyAnimFrames)) 
						voxeland.Generate();
				}

				layout.margin -= 10;
			}
			#endregion

			#region About
			layout.Par(5); layout.Foldout(ref voxeland.guiAbout, "About");
			if (voxeland.guiAbout)
			{
				Rect savedCursor = layout.cursor;
				
				layout.margin = 20;
				layout.Par(100, padding:0);
				layout.Icon("VoxelandIcon", layout.Inset(50,padding:0));

				layout.cursor = savedCursor;
				layout.margin = 80;

				layout.Label("Voxeland " + Voxeland.version.ToVersionString() + " " + Voxeland.versionState.ToVersionString());
				layout.Label("by Denis Pahunov");
				
				layout.Par(10);
				layout.Label(" - Wiki", url:"https://gitlab.com/denispahunov/voxeland/wikis/home");
				layout.Label(" - Forums", url:"https://forum.unity3d.com/threads/voxeland-voxel-terrain-tool.187741/");
				layout.Label(" - Issues / Ideas", url:"https://gitlab.com/denispahunov/voxeland/issues"); 

				/*layout.Par(10);
				layout.Label("Other Products:");

				layout.Par(5);
				layout.Label("MapMagic", url:"https://gitlab.com/denispahunov/voxeland/wikis/home");
				layout.Label("Node based infinite terrain generator");*/

			}
			#endregion

			Layout.SetInspectorRect(layout.field);
		}

		public void DrawRange (string label, ref float src, float min, float max, float gaugeMax)
		{
			layout.Par();
			layout.Label(label,rect:layout.Inset(0.25f));
			//layout.Gauge(Mathf.Sqrt(src/max),null,rect:layout.Inset(0.5f));
			//layout.Gauge(src/gaugeMax,null,rect:layout.Inset(0.5f));

			//rects
			float gaugeWidth = 0.55f;
			Rect gaugeRect = layout.Inset(gaugeWidth);
			Rect afterGauge = layout.cursor;

			//field
			layout.cursor = afterGauge;
			src = layout.Field(src, rect:layout.Inset(0.2f), min:min, max:max);
			if (layout.lastChange)
			{
				if (src < min) src = min;
				if (src > max) src = max;
				if (src > gaugeMax) src = gaugeMax;
			}

			//gauge background
			layout.Gauge(src/gaugeMax, null, rect:gaugeRect, disabled:true);

			//gauge itself
			gaugeRect.position = new Vector2(gaugeRect.x + (min/gaugeMax) * gaugeRect.width, gaugeRect.y);
			gaugeRect.width = ((max-min)/gaugeMax)*gaugeRect.width;
			if (gaugeRect.width < 4) gaugeRect.width = 4;
			
			float delta = max-min;
			if (delta < 1f) delta = 1f;
			layout.Gauge((src-min)/delta, null, rect:gaugeRect);
		}

		[MenuItem ("GameObject/3D Object/Voxeland Static")]
		static void CreateStaticVoxeland () { CreateVoxeland(false); }

		[MenuItem ("GameObject/3D Object/Voxeland Infinite")]
		static void CreateInfiniteVoxeland () { CreateVoxeland(true); }

		static void CreateVoxeland (bool infinite=true) 
		{
			GameObject go = new GameObject();
			go.name = "Voxeland";

			Voxeland voxeland = go.AddComponent<Voxeland>();
			voxeland.chunks = new ChunkGrid<Chunk>();
			voxeland.chunkSize = 30;
			
			//adding empty layer 
			voxeland.landTypes = new LandTypeList();

			//ArrayTools.Add(ref voxeland.landTypes.array, -1);
			//voxeland.landTypes.OnAddBlock(0,null); 
			//voxeland.landTypes.array[0].name = "Empty";

			ArrayTools.Add(ref voxeland.landTypes.array);
			voxeland.landTypes.OnAddBlock(0,null); 
			voxeland.landTypes.array[0].mainTex = Extensions.ColorTexture(2,2,Color.gray);

			voxeland.landTypes.selected = 0;

			//adding empty grass
			voxeland.grassTypes = new GrassTypeList();
			voxeland.grassTypes.selected = -1;

			//objects
			voxeland.objectsTypes = new ObjectTypeList();
			voxeland.objectsTypes.selected = -1;

			//data
			voxeland.data = ScriptableObject.CreateInstance<Data>(); 
			voxeland.data.areas = new ChunkGrid<Data.Area>();
			voxeland.data.areaSize = 512;

			//materials
			voxeland.material = Voxeland.GetDefaultLandMaterial();
			voxeland.farMaterial = Voxeland.GetDefaultFarMaterial();
			voxeland.grassMaterial = Voxeland.GetDefaultGrassMaterial();
			
			//highlight
			if (voxeland.highlight == null) //could be created on de-prefab in onenable
			{
				voxeland.highlight = Highlight.Create(voxeland);
				voxeland.highlight.material = Voxeland.GetDefaultHighlightMaterial(); //new Material( Shader.Find("Voxeland/Highlight") );
			}

			voxeland.RefreshMaterials();

			//static and infinite
			if (infinite)
			{
				voxeland.playmodeSizeMode = Voxeland.SizeMode.DynamicInfinite;
				voxeland.editorSizeMode = Voxeland.SizeMode.DynamicInfinite;

				voxeland.saveMeshes = false;

				voxeland.horizon = Horizon.Create(voxeland);
			}
			else
			{
				voxeland.playmodeSizeMode = Voxeland.SizeMode.Static;
				voxeland.editorSizeMode = Voxeland.SizeMode.Static;

				voxeland.saveMeshes = true;  
			}

			//registering undo
			Undo.RegisterCreatedObjectUndo (go, "Voxeland Create");
			EditorUtility.SetDirty(go);
		}
	}


	public class NewDataWindow : EditorWindow
	{
		Layout layout;
		public Voxeland voxeland;

		public int areaRes = 100;
		public int areaSize = 32;

		public void OnGUI ()
		{
			this.name = "New Data";
			if (layout == null) layout = new Layout();
			layout.margin = 0; layout.rightMargin = 0;
			layout.field = Layout.GetInspectorRect();
			layout.cursor = new Rect();
			layout.undoObject = voxeland;
			layout.undoName =  "Voxeland settings change";
			layout.dragChange = false;
			layout.change = false;

			layout.Field(ref areaSize, "Area Size");
			layout.Field(ref areaRes, "Areas Count");
			layout.Label("Total terrain size: " + areaRes*areaSize + "x" + areaRes*areaSize);

			layout.Par(5);
			if (layout.Button("Reset Terrain"))
			{
				voxeland.data = ScriptableObject.CreateInstance<Data>();
				//voxeland.data.Init(areaSize, areaRes);
			}
		}
	}

	public class MeshesOnSceneSaveRemover : UnityEditor.AssetModificationProcessor 
	{
		static string[] OnWillSaveAssets (string[] paths) 
		{
			bool savingScene = false;
			for (int i=0; i<paths.Length; i++)
				if (paths[i].Contains(".unity")) { savingScene = true; break; }
			if (!savingScene) return paths;

			foreach (Voxeland voxeland in Voxeland.instances)
			{
				if (!voxeland.saveMeshes)
				{
					voxeland.chunks.RemoveNonPinned();
					voxeland.chunks.deployedRects = null;
					//voxeland.currentCamPoses = null;
				}
				if (!voxeland.saveNonpinnedAreas)
				{
					voxeland.data.areas.RemoveNonPinned();
					voxeland.data.areas.deployedRects = null;
					//voxeland.currentCamPoses = null;  
				}
			}
			return paths;
		}
	}


}//namespace