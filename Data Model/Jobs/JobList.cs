using System;
using System.Collections.Generic;
using UnityEngine;

public static class JobList  {

	public enum Jobs
	{
		Idle,
		//Sleep,
		PickUp,
		PlaceLoose,
		StandardInstallBegin,
		StandardInstallEnd,
		Lock,
		Unlock,
		RemoveInstalled
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
		jobList [(int)Jobs.Idle] +=
			(Tile tile, Villager v) => 
		{
			//Idle or wandering code, but not for the end of the job
		};


		jobList.Add (New);
		jobList [(int)Jobs.PickUp] += 
			(Tile tile, Villager vil) => 
		{
			vil.Inventory.Carrying = tile.Loose;
			tile.Loose = null;
				
			Tile closestAvailableToPlace = InventoryManager.Instance.ClosestAvailableTile(vil.CurrentTile);

			if(closestAvailableToPlace != null){
				
				JobController.Instance.AddJob(1f /* FIXME */, new Job(closestAvailableToPlace, 
					(Tile t, Villager v) => {
							PlaceLoose(closestAvailableToPlace, v);
					}), 
					vil);
			}
		};

		jobList.Add (New);
		jobList [(int)Jobs.PlaceLoose] += PlaceLoose;

		jobList.Add (New);
		jobList [(int)Jobs.StandardInstallBegin] += MoveVillagersOutOfWay;

		jobList.Add (New);
		jobList [(int)Jobs.StandardInstallEnd] += MakeNewGraphicalObject;

		jobList.Add (New);
		jobList [(int)Jobs.Lock] += MoveVillagersOutOfWay;//FIXME
		jobList [(int)Jobs.Lock] += ToggleMoveThrough;
		jobList [(int)Jobs.Lock] += (Tile tile, Villager v) => {
			AddGraphic (tile, v, ExtraGraphicalElementHolder.Elements [0]);
		};
		jobList [(int)Jobs.Lock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (tile, v, (int)Jobs.Lock, false);
		};
		jobList [(int)Jobs.Lock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (tile, v, (int)Jobs.Unlock, true);
		};

		jobList.Add (New);
		jobList [(int)Jobs.Unlock] += ToggleMoveThrough;
		jobList [(int)Jobs.Unlock] += RemoveGraphic;
		jobList [(int)Jobs.Unlock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (tile, v, (int)Jobs.Unlock, false);
		};
		jobList [(int)Jobs.Unlock] += (Tile tile, Villager v) => {
			SetActivePossibleJob (
				tile, v, (int)Jobs.Lock, true);
		};

		jobList.Add (New);
		jobList [(int)Jobs.RemoveInstalled] += RemoveGraphic;
		jobList [(int)Jobs.RemoveInstalled] += (Tile t, Villager v) => {
			InstalledObject obj = t.Installed;
			Debug.Log(obj.Name);
			MapController.Instance.DestroyObjectGraphic(t.Installed);
		}; 

		//End of init
		for (int a = 0; a < jobList.Count; a++) {
			jobList[a] += (tile, villager) => {
				JobController.Instance.RemoveJob (tile);
			};
		}
	}

	static void New(Tile t, Villager v){
		//This method is to replace new Action<Tile, Villager>(), which for some unknown reason returned an error.
	}


	static void SetActivePossibleJob(Tile t, Villager v, int jobID, bool b){
		t.Installed.PossibleJobs.SetPossibleJobActive (jobID, b);
	}

	static void MakeNewGraphicalObject(Tile t , Villager v){
		if (t.OccupyingVillagers.Count <= 0) {
			MapController.Instance.CreateObject (t, t.Planned.ID);
		} 
	}

	static void MoveVillagersOutOfWay(Tile baseTile , Villager v){
		//MoveVillagersOutOfWay must come after making a new graphical object, else tile.Installed == null
		List<Tile> tiles = baseTile.Planned.Tiles;

		for (int i = 0; i < tiles.Count; i++) {
			if (tiles[i].OccupyingVillagers.Count > 0 && !tiles[i].Planned.CanMoveThrough) {
				Debug.Log ("Villager moved from " + tiles[i].GetPosition () + " to " +  tiles[i].NearestNeighbourTo (tiles[i].X, tiles[i].Y).GetPosition());

				foreach (Villager vil in tiles[i].OccupyingVillagers) {
					vil.CurrentTile = tiles[i].NearestNeighbourTo (tiles[i].X, tiles[i].Y);
				}
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

	static void PlaceLoose(Tile t, Villager v){
		if(v.Inventory.Carrying != null){
			
			v.Inventory.Carrying.CurrentTile = t;
			//v.Inv.Car is set to null by the above setter

			if (t.Installed.Name == "Stock Pile") { //FIXME
				ResourceManager.Instance.ChangeVillageResourceVal((int)v.Inventory.Carrying.Contents.Type, v.Inventory.Carrying.Contents.Amount);
			}
		}
	}

	/// ///////////

	public static void MakeTileStockPile(Tile t, Villager v){
		InventoryManager.Instance.AddSlot (t);
	}

//	public static void PickUpObject(Tile t, Villager v, LooseObject l){
//		
//	}
//
//	public static void PutDownObject(Tile t, Villager v){
//
//	}
}
