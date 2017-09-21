using System;
using System.Collections.Generic;
using UnityEngine;

public static class JobList  {

	static Action<Tile>[] jobList;

	public static Action<Tile>[] JobFunctions {
		get {
			return jobList;
		}
	}

	public static void Init(){
		jobList = new Action<Tile>[InstalledObjectHolder.MaxObjID];

		jobList [0] += TellTileToComplete;
		jobList [0] += MakeNewGraphicalObject;
		jobList [0] += MoveVillagersOutOfWay;
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
			t.OccupyingVillager.SetDest(t.NearestNeighbourTo(t.X, t.Y));
		}
	}

//	static void PlaceWall(Tile t){
//	}
}
