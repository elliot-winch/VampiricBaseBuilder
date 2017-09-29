using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

	static BuildingManager _instance;

	public static BuildingManager Instance {
		get {
			return _instance;
		}
	}
		
	bool buildMode;
	bool dragMode;
	Action<Tile> build;

	public bool BuildMode {
		get {
			return buildMode;
		}
	}

	public bool DragMode {
		get {
			return dragMode;
		}
	}

	public Action<Tile> Build {
		get {
			return build;
		}
	}

	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be two build controllers");
		}
		_instance = this;

		//Move this?
		InstalledObjectHolder.Init ();
		TileTypeHolder.Init ();
		ExtraGraphicalElementHolder.Init ();

		SetMode ((int)BuildModes.None);
	}

	//Set Mode
	public enum BuildModes{ None, DeleteAll, DeleteInstalled, BuildFloor, BuildMultiple, BuildSingle}

	InstalledObject currentInstalledObj;

	public void SetMode(int mode){

		BuildModes b = (BuildModes)mode;

		switch (b) {
		case BuildModes.None:
			buildMode = false;
			break;
		case BuildModes.DeleteAll:
			buildMode = true;
			dragMode = true;
			build = (tile) => { 
				tile.ResetTile();
				tile.Planned = null;
				};
			ChangeObj (-1);
			break;
		case BuildModes.DeleteInstalled:
			buildMode = true;
			dragMode = true;
			build = (tile) => {
				tile.Planned = null;
			};
			ChangeObj (-1);
			break;
		case BuildModes.BuildFloor:
			buildMode = true;
			dragMode = true;
			build = (tile /*, Material*/) => {
				tile.Type = Tile.TileType.WoodFloor;
			};
			ChangeObj (-1);

			break;
		case BuildModes.BuildMultiple:
			buildMode = true;
			dragMode = true;
			build = (tile /*, Material*/) => 
			{ 
				MapController.Instance.CreateObject(tile, currentInstalledObj, true);
			};
			break;
		case BuildModes.BuildSingle:
			buildMode = true;
			dragMode = false;
			build = (tile /*, Material*/) => 
			{ 
				MapController.Instance.CreateObject(tile, currentInstalledObj, true);
			};
			break;
		default:
			Debug.LogError ("Button needs setting!");
			break;
		}
	}

	public void ChangeObj(int objID){
		if (objID == -1) {
			currentInstalledObj = null;
		} else if (objID >= InstalledObjectHolder.MaxObjID || objID < 0 || InstalledObjectHolder.Installables[objID] == null) {
			throw new InvalidOperationException ("No object associated with this ID");
		} else {
			currentInstalledObj = InstalledObjectHolder.Installables[objID];
		}

		MouseManagerBuildMode.Instance.SetCursor (currentInstalledObj);
	}
}
