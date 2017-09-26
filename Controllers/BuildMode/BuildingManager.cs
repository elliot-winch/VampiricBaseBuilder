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
		
	public enum BuildModes{ None, DeleteAll, DeleteInstalled, BuildFloor, BuildInstalled/*, BuildDoor*/ }

	int currentInstalledObjID = 0;

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
			break;
		case BuildModes.DeleteInstalled:
			buildMode = true;
			dragMode = true;
			build = (tile) => {
				tile.Planned = null;
			};
			break;
		case BuildModes.BuildFloor:
			buildMode = true;
			dragMode = true;
			build =  (tile /*, Material*/) => { tile.Type = Tile.TileType.WoodFloor;};
			break;
		case BuildModes.BuildInstalled:
			buildMode = true;
			dragMode = true;
			build = (tile /*, Material*/) => { InstalledObjectHolder.GetValue(currentInstalledObjID).PlaceObjectPlan(tile);
				};
			break;
			/*
		case BuildMode.BuildDoor:
			buildMode = true;
			dragMode = false;
			break;
			*/
		default:
			Debug.LogError ("Button needs setting!");
			break;
		}

		Debug.Log (mode);
	}

	public void ChangeObj(int objID){
		if (currentInstalledObjID >= InstalledObjectHolder.MaxObjID) {
			throw new InvalidOperationException ("No object associated with this ID");
		} else {
			currentInstalledObjID = objID;
		}
	}
}
