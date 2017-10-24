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


		SetMode ((int)BuildModes.None);
	}

	//Set Mode
	public enum BuildModes{ None, RemoveInstalled, DeletePlanned, BuildFloor, BuildMultiple, BuildSingle, InstallImmediate}
	//FIXME reorder

	int currentInstalledObjID;

	public void SetMode(int mode){

		BuildModes b = (BuildModes)mode;

		switch (b) {
		case BuildModes.None:
			buildMode = false;
			break;
		case BuildModes.RemoveInstalled:
			buildMode = true;
			dragMode = true;
			build = (tile) => { 
				if(tile.Installed != null){
					//Spawns a picture to mark deletion
					MapController.Instance.AddExtraGraphicalElement(tile, ExtraGraphicalElementHolder.Elements[1]);
					//RemoveInstalled job also removes deletion graphic
					JobController.Instance.AddJob(1f /*FIXME*/, new Job(tile, JobList.JobFunctions[(int)JobList.Jobs.RemoveInstalled]));
				}
				};
			ChangeObjID (-1);
			break;
		case BuildModes.DeletePlanned:
			buildMode = true;
			dragMode = true;
			build = (tile) => {
				if(tile.Planned != null){
					MapController.Instance.DestroyPlannedObjectGraphic(tile.Planned);
					JobController.Instance.CancelJob(tile);
				}
			};
			ChangeObjID (-1);
			break;
		case BuildModes.BuildFloor:
			buildMode = true;
			dragMode = true;
			build = (tile /*, Material*/) => {
				tile.Type = Tile.TileType.WoodFloor;
			};
			ChangeObjID (-1);

			break;
		case BuildModes.BuildMultiple:
			buildMode = true;
			dragMode = true;
			build = (tile /*, Material*/) => 
			{ 
				if(currentInstalledObjID >= 0){
					MapController.Instance.CreatePlannedObject(tile, currentInstalledObjID);
				}
			};
			break;
		case BuildModes.BuildSingle:
			buildMode = true;
			dragMode = false;
			build = (tile /*, Material*/) => 
			{ 
				if(currentInstalledObjID >= 0){
					MapController.Instance.CreatePlannedObject(tile, currentInstalledObjID);
				}
			};
			break;

		case BuildModes.InstallImmediate:
			buildMode = true;
			dragMode = true;
			build = (tile) => {
				if (currentInstalledObjID >= 0) {
					MapController.Instance.CreateObject (tile, currentInstalledObjID);
				}
			};
			break;
		default:
			Debug.LogError ("Button needs setting!");
			break;
		}

	}

	public void ChangeObjID(int objID){
		 if (objID >= InstalledObjectHolder.MaxObjID || objID < 0) {
			MouseManagerBuildMode.Instance.DefaultCursor();
		} else {
			currentInstalledObjID = objID;
			MouseManagerBuildMode.Instance.SetCursor (InstalledObjectHolder.GetObjInfo(objID), InstalledObjectHolder.GetSpriteHolder(objID));
		}
	}
}
