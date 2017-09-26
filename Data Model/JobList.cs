using System;
using System.Collections.Generic;
using UnityEngine;

public static class JobList  {

	public enum StandardJobs
	{
		StandardInstall,
		Lock,
		Unlock
	}

	static List<Action<Tile>> jobList;

	public static List<Action<Tile>> JobFunctions {
		get {
			return jobList;
		}
	}

	public static void Init(){
		jobList = new List<Action<Tile>> ();


		jobList.Add(New);
		jobList [(int) StandardJobs.StandardInstall] += TellTileToComplete;
		jobList [(int) StandardJobs.StandardInstall] += MakeNewGraphicalObject;
		jobList [(int) StandardJobs.StandardInstall] += MoveVillagersOutOfWay;

		jobList.Add(New);
		jobList[(int) StandardJobs.Lock] += MoveVillagersOutOfWay;
		jobList[(int) StandardJobs.Lock] += ToggleMoveThrough;
		jobList[(int) StandardJobs.Lock] += AddGraphic;
		jobList [(int)StandardJobs.Lock] += (tile) => { SetActivePossibleJob(tile, StandardJobs.Lock, false);} ;
		jobList [(int)StandardJobs.Lock] += (tile) => { SetActivePossibleJob(tile, StandardJobs.Unlock, true);} ;

		jobList.Add(New);
		jobList[(int) StandardJobs.Unlock] += ToggleMoveThrough;
		jobList[(int) StandardJobs.Unlock] += RemoveGraphic;
		jobList [(int)StandardJobs.Unlock] += (tile) => { SetActivePossibleJob(tile, StandardJobs.Unlock, false);} ;
		jobList [(int)StandardJobs.Unlock] += (tile) => { SetActivePossibleJob(tile, StandardJobs.Lock, true);} ;
	
	}

	static void New(Tile t){
		//This method is to replace new Action<Tile>(), which for some unknown reason returned an error.
	}

	static void TellTileToComplete(Tile t){
		t.PlannedComplete ();
	}

	static void MakeNewGraphicalObject(Tile t){
		MapController.Instance.CreateInstalledObject (t);
	}

	//FIXME: what if multiple villagers in square?
	static void MoveVillagersOutOfWay(Tile t){
		if (t.OccupyingVillager != null) {
			Tile dest = t.NearestNeighbourTo (t.X, t.Y);
			t.OccupyingVillager.SetDest(t.NearestNeighbourTo(t.X, t.Y));

			Debug.Log ("Villager moved from " + t.OccupyingVillager.CurrentTile.GetPosition() + " to " + dest.X + " " + dest.Y);

		}
	}

//	static void PlaceWall(Tile t){
//	}

	static void ToggleMoveThrough(Tile t){
		t.CanMoveThrough = !t.CanMoveThrough;
	}

	static void AddGraphic(Tile t /* either int graphicID or ExtraGraphicalElement*/){
		if (MapController.Instance.ExtraGraphicalElements.ContainsKey(t) == false) {
			MapController.Instance.AddExtraGraphicalElement (t, ExtraGraphicalElementHolder.Elements [0]);
		}
	}

	static void RemoveGraphic(Tile t){
		if (MapController.Instance.ExtraGraphicalElements.ContainsKey (t) == true) {
			MapController.Instance.RemoveExtraGraphicalElement (t);
		}
	}

	static void SetActivePossibleJob(Tile t, JobList.StandardJobs job, bool b){
		t.Installed.SetPossibleJobActive (job, b);
	}
}
