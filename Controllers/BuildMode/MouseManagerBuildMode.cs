using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManagerBuildMode : MonoBehaviour {

	static MouseManagerBuildMode _instance;

	public static MouseManagerBuildMode Instance {
		get {
			return _instance;
		}
	}

	public GameObject cursorPrefab;

	GameObject cursor;

	BuildingManager bm;

	Vector3 startMousePos;
	Vector3 mousePos;
	Vector3 dragStartPos;

	void Start(){
		if (_instance != null) {
			Debug.LogError ("Should not be more than one MouseManagerBuildMode");
		}

		_instance = this;

		bm = BuildingManager.Instance;
		cursor = Instantiate (cursorPrefab, Vector3.zero, Quaternion.identity);

		startMousePos = Input.mousePosition;
	}

	void Update(){
		mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		if (Input.GetMouseButtonDown (1)) {
			startMousePos = mousePos;
		}

		UpdateCursor ();
		NonBuildClicks ();

		if (!EventSystem.current.IsPointerOverGameObject()) {
			
			if (bm.BuildMode) {
				if (bm.DragMode) {
					Drag ();
				} else {
					Place ();
				}
			} 
		}
	}


	void UpdateCursor(){
		//Cursor
		Tile tile = MapController.Instance.GetTileAtWorldPos (mousePos);

		if (tile != null) {
			cursor.SetActive (true);
			cursor.transform.position = new Vector3 (tile.X, tile.Y);
		} else {
			cursor.SetActive (false);
		}
	}

	void NonBuildClicks(){
		//Right click
		if (Input.GetMouseButtonDown (1)) {
			Tile t = MapController.Instance.GetTileAtWorldPos(new Vector3((int)mousePos.x, (int)mousePos.y));
            
			if (t.OccupyingVillager != null) {
				UIControllerBuildMode.Instance.OpenVillagerPanel (t.OccupyingVillager.Info);
			} else if(t.Installed != null){
                //display jobs
				UIController.Instance.DisplayJobPanel(t, t.Installed.PossibleJobs);
				JobController.Instance.AddJob (Time.realtimeSinceStartup, new Job(t, JobList.JobFunctions[(int)t.Installed.PossibleJobs[0].possibleJob]));
            } else {
				UIControllerBuildMode.Instance.CloseVillagerPanel (); 
		    }
	   }
    }
    

	public bool DragCamera(float cameraMoveDrag){
		if (Input.GetMouseButton (1) || Input.GetMouseButton (2)) {

			Camera.main.transform.Translate ((startMousePos - mousePos) * cameraMoveDrag);

			return true;
		} 

		return false;
	}

	void Place(){
		if (Input.GetMouseButtonUp (0)) {
			Tile t = MapController.Instance.GetTileAtWorldPos(new Vector3((int)mousePos.x, (int)mousePos.y));
			if (t != null) {
				bm.Build(t);
			}
		}
	}

	void Drag(){
		//LEFT MOUSE BUTTON DRAG
		//Start drag
		if (Input.GetMouseButtonDown (0)) {
			dragStartPos = mousePos;
		}

		int start_x = (int)dragStartPos.x;
		int end_x = (int)mousePos.x;

		if (end_x < start_x) {
			int temp = end_x;
			end_x = start_x;
			start_x = temp;
		}

		int start_y = (int)dragStartPos.y;
		int end_y = (int)mousePos.y;

		if (end_y < start_y) {
			int temp = end_y;
			end_y = start_y;
			start_y = temp;
		}
		/*
		//While drag
		if (Input.GetMouseButton (0)) {
			for (int i = start_x; i <= end_x; i++) {
				for (int j = start_y; j <= end_y; j++) {
					Tile t = MapController.Instance.GetTileAtWorldPos(new Vector3(i,j,0));
					if (t != null) {
						
					}
				}
			}
		}
		*/
		//End drag
		if (Input.GetMouseButtonUp (0)) {
			for (int i = start_x; i <= end_x; i++) {
				for (int j = start_y; j <= end_y; j++) {
					Tile t = MapController.Instance.GetTileAtWorldPos(new Vector3(i,j,0));
					if (t != null) {
						bm.Build (t);
					}
				}
			}
		}
	}

	public void SetCursorActive(bool b){
		cursor.SetActive (b);
	}
}

