using System;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject {

	/*
	 * public enum Material
	{
		Mud,
		Wood,
		Stone,
		Brick
	}
	 */ 
	
	string name;
	int id;

	int[][] relativeTiles;
	int height;

	public int Height {
		get {
			return height;
		}
	}

	/*
	 * Currently, relative tiles starts at the top left corner and works down then across. 
	 */ 
	float moveCost;
	bool canMoveThrough;

	InstalledObjectInteraction interaction;

	Func<Tile, bool> placementValidation;
	JobList.CombinedJobs onJobComplete;

	InstalledObjectPossibleJobs possibleJobs;

	InstalledObjectSpawnAdditional spawnAdd;

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


	public int[][] RelativeTiles {
		get {
			return relativeTiles;
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

	public Func<Tile, bool> PlacementValidation {
		get {
			return placementValidation;
		}
	}

	public InstalledObjectInteraction Interaction {
		get {
			return interaction;
		}
	}

	public JobList.CombinedJobs OnJobComplete {
		get {
			return onJobComplete;
		}
	}
    
	public InstalledObjectPossibleJobs PossibleJobs {
		get {
			return possibleJobs;
		}
	}

	public InstalledObjectSpawnAdditional SpawnAdd {
		get {
			return spawnAdd;
		}
	}

	public InstalledObject (int ID, string name, Func<Tile, bool> validation, 
		JobList.CombinedJobs onJobComplete, 
		bool canMoveThrough = false, float moveCost=Mathf.Infinity){

		this.id = ID;
		this.name = name;

		this.placementValidation = validation;
		this.onJobComplete = onJobComplete;
		this.canMoveThrough = canMoveThrough;
		this.moveCost = moveCost;

		this.relativeTiles = new int[][]{ new int[]{ 0, 0 } };
		this.height = 1;
	}

	public InstalledObject (int ID, string name, Func<Tile, bool> validation, 
		JobList.CombinedJobs onJobComplete, int width, int height, 
		bool canMoveThrough = false, float moveCost=Mathf.Infinity) 
		: this(ID, name, validation, onJobComplete, canMoveThrough, moveCost){

		SetMultiTiles (width, height);
		this.height = height;
	}

	public bool StandardValidation(Tile t){
		return true;
	}

	public void InitInteraction(){
		this.interaction = new InstalledObjectInteraction ();
	}

	public void InitPossibleJobs(){
		this.possibleJobs = new InstalledObjectPossibleJobs ();
	}

	public void InitSpawnAdditional(){
		this.spawnAdd = new InstalledObjectSpawnAdditional ();
	}

	public void SetMultiTiles(int[][] coords){
		//Specialist setting of tiles
		this.relativeTiles = coords;
	}

	public void SetMultiTiles(int width, int height){
		this.relativeTiles = new int[width * height][];

		int counter = 0;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				relativeTiles [counter++] = new int[]{ i, j };
			}
		}
	}
}
