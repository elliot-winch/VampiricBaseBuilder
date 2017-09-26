using System;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject {
	
	string name;
	int id;

	Sprite sprite;
	string sortingLayer;
	int sortingOrder;

	int width;
	int height;
	float moveCost;
	bool canMoveThrough;
	Action<Tile> prevInteraction; //How all entities interact with this installed object when they've just stepped off it
	Action<Tile> onInteraction;  //How all entities interact with this installed object 

	Tile baseTile;
	Func<Tile, bool> placementValidation;
	JobList.StandardJobs onJobComplete;
    
    //These are things villagers/the vampire can do to installed objects
	List<PossibleJob> possibleJobs;

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

	public Sprite Sprite {
		get {
			return sprite;
		}
	}

	public string SortingLayer {
		get {
			return sortingLayer;
		}
	}

	public int SortingOrder {
		get {
			return sortingOrder;
		}
	}

	public Tile BaseTile {
		get {
			return baseTile;
		}
	}

	public virtual float GetMoveCost(){ 
		return moveCost;
	}

	public void SetMoveCost(float val){
		this.moveCost = val;
	}

	public bool CanMoveThrough {
		get {
			return canMoveThrough;
		}
		set {
			canMoveThrough = value;
		}
	}

	public JobList.StandardJobs OnJobComplete {
		get {
			return onJobComplete;
		}
	}
    
	public List<PossibleJob> PossibleJobs {
		get {
			return possibleJobs;
		}
	}
    
	public void AddPossibleJob(int jobID, bool t){
		possibleJobs.Add(new PossibleJob(jobID, t));
    }

	public void SetPossibleJobActive(int jobID, bool b){
		//contains
	
		for(int i = 0; i < possibleJobs.Count; i++){
			if(possibleJobs[i].possibleJobID == jobID){
				possibleJobs[i].active = b;
				return;
			}
		}
	}

	public InstalledObject (int ID, string name, Sprite sprite, Func<Tile, bool> validation, 
		JobList.StandardJobs onJobComplete, bool canMoveThrough = false, float moveCost=Mathf.Infinity, int sortingOrder = 50, 
		string sortingLayer = "InstalledObject", int width=1, int height=1){

		this.id = ID;
		this.name = name;

		this.sprite = sprite;
		this.sortingLayer = sortingLayer;
		this.sortingOrder = sortingOrder;

		this.placementValidation = validation;
		this.onJobComplete = onJobComplete;
		this.canMoveThrough = canMoveThrough;
		this.moveCost = moveCost;
		this.width = width;
		this.height = height;
        
		possibleJobs = new List<PossibleJob>();
	}

	public void PlaceObjectPlan(Tile t){
		if (t != null && placementValidation(t)) {
			this.baseTile = t;

			t.Planned = this;
		}
	}

	public void PlaceObjectRaw(Tile t){
		if (placementValidation(t)) {
			this.baseTile = t;

			t.Installed = this;
		}
	}

	public bool StandardValidation(Tile t){
		return true;
	}

	//When a villager just finished moving through this tile, what happens?
	public void RegisterPrevInteractionCallback(Action<Tile> callback){
		prevInteraction += callback;
	}

	//When a villager moves through this tile, what happens?
	public void RegisterInteractionCallback(Action<Tile> callback){
		onInteraction += callback;
	}

	public void PrevInteract(Tile t){
		if (prevInteraction != null) {
			prevInteraction (t);
		}
	}

	public void Interact(Tile t){
		if (onInteraction != null) {
			onInteraction (t);
		}
	}
}

public class PossibleJob
{
	public int possibleJobID;
	public bool active;

	public PossibleJob(int i, bool active){
		this.possibleJobID = i;
		this.active = active;
	}
}
