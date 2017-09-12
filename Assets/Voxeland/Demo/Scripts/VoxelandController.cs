using UnityEngine;
using System.Collections;
using Voxeland5;

using Plugins;

namespace VoxelandDemo
{

	public class VoxelandController : MonoBehaviour 
	{
		public Voxeland voxeland;
		public VoxelandDemo.CameraController cameraController;
		public VoxelandDemo.CharController charController;
		public CoordDir prevAimCoord;

		//gui
		public Canvas canvas;
		public GameObject helpPanel;
		public GameObject crosshair;
		public UnityEngine.UI.Toggle useMouselook;
		public UnityEngine.UI.Toggle useGravity;
		public UnityEngine.UI.Toggle useFullscreen;
		public GameObject fullscreenCheckmark;
		private int oldScreenWidth = 0;
		private int oldScreenHeight = 0;
		public UnityEngine.UI.Slider buildProgress;
		public GameObject autogeneratePleaseWait;

		//instrument selection
		public int selectedTool = 1;
		public UnityEngine.UI.Toggle vertGrassInstrument;
		public UnityEngine.UI.Toggle cliffInstrument;
		public UnityEngine.UI.Toggle grassInstrument;
		public UnityEngine.UI.Toggle yellowGrassInstrument;
		public UnityEngine.UI.Toggle pineInstrument;
		public UnityEngine.UI.Toggle torchInstrument;
		
		//disabling fullscreen and mouselook when loosing focus
		/*void OnApplicationFocus(bool focusStatus) 
		{
			if (!focusStatus && Screen.fullScreen)
			{
				useFullscreen.isOn = false;
				useMouselook.isOn = false;
			}
		}*/

		public void SwitchFullscreen ()
		{
			if (!Screen.fullScreen) 
			{ 
				oldScreenWidth = Screen.width;
				oldScreenHeight = Screen.height;
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
			}
			else
				Screen.SetResolution(oldScreenWidth, oldScreenHeight, false);
		}

		void OnEnable () 
		{
			//select cliff tool
			voxeland.grassTypes.selected=-1;	voxeland.landTypes.selected=0;		voxeland.objectsTypes.selected=-1;
			canvas.gameObject.SetActive(true);
		}

		void Update ()
		{
			//showing help panel
			if (Input.GetKeyDown(KeyCode.F1)) helpPanel.SetActive( !helpPanel.activeSelf );

			//selecting tool
			if (Input.GetKey("`")) { vertGrassInstrument.isOn = true;	voxeland.grassTypes.selected=1;		voxeland.landTypes.selected=-1;		voxeland.objectsTypes.selected=-1; }
			if (Input.GetKey("1")) { cliffInstrument.isOn = true; 		voxeland.grassTypes.selected=-1;	voxeland.landTypes.selected=0;		voxeland.objectsTypes.selected=-1; }
			if (Input.GetKey("2")) { grassInstrument.isOn = true;		voxeland.grassTypes.selected=-1;	voxeland.landTypes.selected=2;		voxeland.objectsTypes.selected=-1; }
			if (Input.GetKey("3")) { yellowGrassInstrument.isOn = true;	voxeland.grassTypes.selected=-1;	voxeland.landTypes.selected=3;		voxeland.objectsTypes.selected=-1; }
			if (Input.GetKey("4")) { pineInstrument.isOn = true; 		voxeland.grassTypes.selected=-1;	voxeland.landTypes.selected=-1;		voxeland.objectsTypes.selected=0; }
			if (Input.GetKey("5")) { torchInstrument.isOn = true; 		voxeland.grassTypes.selected=-1;	voxeland.landTypes.selected=-1;		voxeland.objectsTypes.selected=1; }

			//mouselook and gravity
			if (Input.GetKeyDown("g")) { useGravity.isOn = !useGravity.isOn; }
			if (Input.GetKeyDown("m")) { useMouselook.isOn = !useMouselook.isOn; }
			if (Input.GetKeyDown("f")) { SwitchFullscreen(); }
			//if (Input.GetKeyDown(KeyCode.Escape)) { useMouselook.isOn = false; useFullscreen.isOn = false; }

			charController.gravity = useGravity.isOn;
			cameraController.lockCursor = useMouselook.isOn;
			crosshair.SetActive(cameraController.lockCursor);

			//fullscreen - reverse order, setting toggle from current fullscreen state
			//useFullscreen.isOn = Screen.fullScreen;
			fullscreenCheckmark.SetActive(Screen.fullScreen);

			//displaing build progress
			float calculatedSum; float completeSum; float totalSum;
			ThreadWorker.GetProgresByTag("VoxelandChunk", out totalSum, out calculatedSum, out completeSum);
			buildProgress.maxValue = totalSum;
			buildProgress.value = completeSum;

			//editing
			if (cameraController.lockCursor || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) //IsPointerOverGameObject returns true if mouse hidden
			{
				//reading controls
				bool leftMouse = Input.GetMouseButtonDown(0);
				bool middleMouse = Input.GetMouseButtonDown(2);
				bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
				bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);

				//getting edit mode
				Voxeland.EditMode editMode = Voxeland.EditMode.none;
				if (leftMouse && !shift && !alt) editMode = Voxeland.EditMode.dig;
				else if (leftMouse && alt) editMode = Voxeland.EditMode.add;
				else if (leftMouse && shift) editMode = Voxeland.EditMode.replace;

				//getting aiming ray
				Ray aimRay;
				if (cameraController.lockCursor) aimRay = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0.5f));
				else aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);

				//aiming terrain block
				CoordDir aimCoord = voxeland.PointOut(aimRay);

				//if any change
				if (prevAimCoord != aimCoord || editMode != Voxeland.EditMode.none)
				{
					//highlight
					if (voxeland.highlight!=null)
					{
						if (aimCoord.exists) voxeland.Highlight(aimCoord, voxeland.brush, isEditing:editMode!=Voxeland.EditMode.none);
						else voxeland.highlight.Clear(); //clearing highlight if nothing aimed or voxeland not selected
					}

					//altering
					if (editMode!=Voxeland.EditMode.none && aimCoord.exists) 
					{
						voxeland.Alter(aimCoord, voxeland.brush, editMode, 
							landType:voxeland.landTypes.selected, 
							objectType:voxeland.objectsTypes.selected, 
							grassType:voxeland.grassTypes.selected);
					}

					prevAimCoord = aimCoord;
				} //if coord or button change
			}
		}

		public void SetSelectedTool (int tool) { selectedTool = tool; }
		public void SetMouseLook (bool val) { cameraController.lockCursor = val; }
		public void SetGravity (bool val) { cameraController.lockCursor = val; }

/*		public Voxeland.VoxelandTerrain land;
		public VoxelandDemo.CameraController cameraController;
		
		public int helpWidth = 400;
		public int helpHeight = 400;
		
		public int messageWidth = 600;
		public int messageHeight = 100;
		
		public bool displayHelp = true;
		public bool displaySave = false;
		public bool displayLoad = false;
		public bool displayNew = false;
		
		string helpText = "Voxeland Demo Quick Manual:\n\n"+
			"W,A,S,D: walk;\n"+
				"left click: add block;\n"+
				"right click: dig block;\n"+
				"~: select grass;\n"+
				"1: select Cliff;\n"+
				"2: select Mud;\n"+
				"3: select Torch;\n"+
				"4: select Tree;\n"+
				"[: increase brush size;\n"+
				"]: decrease brush size;\n"+
				"F5: save;\n"+
				"F6: load;\n"+
				"F1: hide/show this manual";
		
		void Update ()
		{
			if (land == null) land = FindObjectOfType(typeof(Voxeland.VoxelandTerrain)) as Voxeland.VoxelandTerrain;
			if (!displayHelp && !displaySave && !displayLoad && !displayNew)
			land.Edit(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0.5f)), 
			    add:( Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift) ),
			    dig:Input.GetMouseButtonDown(1),
			    smooth:false, 
				replace:( Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift)) )	);
		}
		
		void OnGUI () 
		{
			if (land == null) land = FindObjectOfType(typeof(Voxeland.VoxelandTerrain)) as Voxeland.VoxelandTerrain;
			if (cameraController == null) cameraController = FindObjectOfType(typeof(VoxelandDemo.CameraController)) as VoxelandDemo.CameraController;
			
			if (Input.GetKey("`")) land.selected = -1;
			if (Input.GetKey("1")) land.selected = 1;
			if (Input.GetKey("2")) land.selected = 2;
			if (Input.GetKey("3")) land.selected = 3;
			if (Input.GetKey("4")) land.selected = 4;
			if (Input.GetKey("5")) land.selected = 5;
			if (Input.GetKey("6")) land.selected = 6;
			if (Input.GetKey("7")) land.selected = 7;
			
			if (Input.GetKeyDown("]")) land.brushSize++;
			if (Input.GetKeyDown("[")) land.brushSize--;
			land.brushSize = Mathf.Max(land.brushSize,0);

			//help screen
			if (Input.GetKeyDown(KeyCode.F1)) displayHelp = !displayHelp;
			if (displayHelp)
			{
				GUI.BeginGroup(new Rect((Screen.width-helpWidth)/2, (Screen.height-helpHeight)/2, helpWidth, helpHeight));
				GUI.Box(new Rect(0, 0, helpWidth, helpHeight), "");
				GUI.Label(new Rect(20, 0, helpWidth-20, helpHeight), helpText);
				if (GUI.Button(new Rect(helpWidth/2-50, helpHeight-40, 100, 25), "OK")) displayHelp = false;
				GUI.EndGroup();
			}
			
			string guiText= null;
			if (land.selected > 0) guiText = "Selected: " + land.types[land.selected].name + "\nBrush size: " + land.brushSize;
			else guiText = "Selected: " + land.grass[-land.selected].name + "\nBrush size: " + land.brushSize;
			GUI.Box(new Rect(Screen.width-130, Screen.height-60, 110, 40), guiText);
			
			//save
			if (Input.GetKeyDown(KeyCode.F5)) 
			{
				using (System.IO.FileStream fs = new System.IO.FileStream(Application.persistentDataPath + "/VoxelandData.txt", System.IO.FileMode.Create))
					using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
						writer.Write(land.data.SaveToString());
				displaySave = true;
			}
			if (displaySave)
			{
				GUI.BeginGroup(new Rect((Screen.width-messageWidth)/2, (Screen.height-messageHeight)/2, messageWidth, messageHeight));
				GUI.Box(new Rect(0, 0, messageWidth, messageHeight), "Voxeland data was saved to:\n" + Application.persistentDataPath + "/VoxelandData.txt");
				if (GUI.Button(new Rect(messageWidth/2-50, messageHeight-40, 100, 25), "OK")) displaySave = false;
				GUI.EndGroup();
			}
			
			//load
			if (Input.GetKeyDown(KeyCode.F6)) 
			{
				using (System.IO.FileStream fs = new System.IO.FileStream(Application.persistentDataPath + "/VoxelandData.txt", System.IO.FileMode.Open))
					using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
						land.data.LoadFromString( reader.ReadToEnd() );
				land.chunks.Clear();
				land.Display(false);
				displayLoad = true;
			}
			if (displayLoad)
			{
				GUI.BeginGroup(new Rect((Screen.width-messageWidth)/2, (Screen.height-messageHeight)/2, messageWidth, messageHeight));
				GUI.Box(new Rect(0, 0, messageWidth, messageHeight), "Voxeland data was loaded");
				if (GUI.Button(new Rect(messageWidth/2-50, messageHeight-40, 100, 25), "OK")) displayLoad = false;
				GUI.EndGroup();
			}
			
			if (displayHelp || displaySave || displayLoad || displayNew) cameraController.lockCursor = false;
			else cameraController.lockCursor = true;
		}*/
	}

}
