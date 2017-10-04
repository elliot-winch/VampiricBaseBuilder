using System;
using System.Collections.Generic;
using UnityEngine;

public static class JobList  {

	public enum CombinedJobs
	{
		//Idle,
		//Sleep,
		StandardInstall,
		Lock,
		Unlock,
	}

	static List<Action<Tile, Villager>> jobList;
	//static List<Action<Villager>> mobile JobsList;

	public static List<Action<Tile, Villager>> JobFunctions {
		get {
			return jobList;
		}
	}

	public static void Init(){
		//jobsList is just jobs on tiles atm
		jobList = new List<Action<Tile, Villager>> ();

		jobList.Add (New);
		jobList [(int)CombinedJobs.StandardInstall] += MakeNewGraphicalObject;
		jobList [(int)CombinedJobs.StandardInstall] += MoveVillagersOutOfWay;

		jobList.Add (New);
		jobList [(int)CombinedJobs.Lock] += MoveVillagersOutOfWay;
		jobList [(int)CombinedJobs.Lock] += ToggleMoveThrough;
		jobList [(int)CombinedJobs.Lock] += (Tile tile, Villager v) => {
			AddGraphic (tile, v, ExtraGraphicalElementHolder.Elements [0]);
		};
		jobList [(int)CombinedJobs.Lock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (tile, v, (int)CombinedJobs.Lock, false);
		};
		jobList [(int)CombinedJobs.Lock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (tile, v, (int)CombinedJobs.Unlock, true);
		};

		jobList.Add (New);
		jobList [(int)CombinedJobs.Unlock] += ToggleMoveThrough;
		jobList [(int)CombinedJobs.Unlock] += RemoveGraphic;
		jobList [(int)CombinedJobs.Unlock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (tile, v, (int)CombinedJobs.Unlock, false);
		};
		jobList [(int)CombinedJobs.Unlock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (
				tile, v, (int)CombinedJobs.Lock, true);
		};

	}

	static void New(Tile t, Villager v){
		//This method is to replace new Action<Tile>(), which for some unknown reason returned an error.
	}

	static void MakeNewGraphicalObject(Tile t , Villager v){
		MapController.Instance.CreateObject (t, t.Planned, false);
	}

	static void MoveVillagersOutOfWay(Tile baseTile , Villager v){
		//MoveVillagersOutOfWay must come after making a new graphical object, else tile.Installed == null
		int[][] relativeTiles = baseTile.Installed.RelativeTiles;

		for (int i = 0; i < relativeTiles.Length; i++) {
			Debug.Log (relativeTiles [i] [0]);
			Tile t = MapController.Instance.GetTileAtWorldPos (baseTile.X + relativeTiles [i] [0], baseTile.Y + relativeTiles [i] [1]);

			if (t.OccupyingVillager != null && !t.Planned.CanMoveThrough) {
				Tile occupied = t.OccupyingVillager.CurrentTile;
				Debug.Log ("Villager moved from " + occupied.GetPosition () + " to " +  occupied.NearestNeighbourTo (occupied.X, occupied.Y).GetPosition());
				t.OccupyingVillager.CurrentTile = occupied.NearestNeighbourTo (occupied.X, occupied.Y);
			} 
		}
	}

	static void ToggleMoveThrough(Tile t, Villager v){
		t.CanMoveThrough = !t.CanMoveThrough;
	}

	static void AddGraphic(Tile t, Villager v,  ExtraGraphicalElement e){
		
		Debug.Log ("Adding graphic @ " + t.X + " " + t.Y);
		if (MapController.Instance.ExtraGraphicalElements.ContainsKey(t) == false) {
			MapController.Instance.AddExtraGraphicalElement (t, e);
		}
	}

	static void RemoveGraphic(Tile t, Villager v){

		Debug.Log ("Removing graphic @ " + t.X + " " + t.Y);

		if (MapController.Instance.ExtraGraphicalElements.ContainsKey (t)) {
			MapController.Instance.RemoveExtraGraphicalElement (t);
		}
	}

	static void SetActivePossibleJob(Tile t, Villager v, int jobID, bool b){
		t.Installed.PossibleJobs.SetPossibleJobActive (jobID, b);
	}

	public static void PickUpObject(Tile t, Villager v, LooseObject l){
		
	}

	public static void PutDownObject(Tile t, Villager v){

	}
}
