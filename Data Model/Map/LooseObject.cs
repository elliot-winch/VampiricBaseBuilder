using System;
using System.Collections.Generic;
using UnityEngine;

public class LooseObject {

	string name;
	int id;

	Tile currentTile;

	bool pickUp;

//	Func<Tile, bool> placementValidation;
	//Do different looseobjects need different placement validations? The answer is yes, but not right now

	ILooseObject contents;
	//Might be an installed object or resource

	Action<Tile> placed;

	public string Name {
		get {
			return name;
		}
	}

	public int ID {
		get {
			return id;
		}
	}

	public Tile CurrentTile {
		get {
			return currentTile;
		}
		set {
			currentTile = value;

			if (value != null) {
				value.Loose = this;
			}
		}
	}

	public bool PickUp {
		get {
			return pickUp;
		}
		set {
			pickUp = value;

			if (pickUp) {
				JobController.Instance.AddJob (Time.realtimeSinceStartup, new Job (currentTile,
					(Tile tile, Villager v) =>
					{
						v.Inventory.Carrying = tile.Loose;
						tile.Loose = null;
					}
				));
			}
		}
	}

	public ILooseObject Contents {
		get {
			return contents;
		}
	}

	public LooseObject(string name, int id, /*Func<Tile, bool> placementValidation,*/ ILooseObject contents, bool pickUp, Tile t){
		this.name = name;
		this.id = id;
		//this.placementValidation = placementValidation;
		this.contents = contents;

		//Pickup needs to be set after tile
		this.CurrentTile = t;
		this.PickUp = pickUp;

	}
//
//	public void RegisterPlacedCallback(Action<Tile> callback){
//		placed += callback;
//	}

}
