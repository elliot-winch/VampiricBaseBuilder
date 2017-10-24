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

	Resource contents;
	//Might be an installed object or resource FIXME

	Action<Tile> placed;

	ObjectPossibleJobs possibleJobs;

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
			if (value != null) {
				currentTile = value;
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
				JobController.Instance.AddJob (1f, new Job(CurrentTile, JobList.JobFunctions[(int)JobList.Jobs.PickUp]));
			}
		}
	}

	public ObjectPossibleJobs PossibleJobs {
		get {
			return possibleJobs;
		}
	}

	public Resource Contents {
		get {
			return contents;
		}
	}

	public LooseObject(string name, int id, /*Func<Tile, bool> placementValidation,*/ Resource contents, bool pickUp, Tile t){
		this.name = name;
		this.id = id;
		//this.placementValidation = placementValidation;
		this.contents = contents;

		//Pickup needs to be set after tile
		this.CurrentTile = t;
		this.PickUp = pickUp;

	}

	public void InitPossibleJobs(){
		this.possibleJobs = new ObjectPossibleJobs ();
	}
//
//	public void RegisterPlacedCallback(Action<Tile> callback){
//		placed += callback;
//	}

}
