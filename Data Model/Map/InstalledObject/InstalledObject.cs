using System;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject{

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

	int height;
	int width;

	public int Height {
		get {
			return height;
		}
	}

	public int Width {
		get {
			return width;
		}
	}

	List<Tile> tiles;

	public List<Tile> Tiles{
		get {
			return tiles;
		}
	}

	/*
	 * Currently, relative tiles starts at the top left corner and works down then across. 
	 */ 
	float moveCost;
	bool canMoveThrough;

	InstalledObjectInteraction interaction;

	Func<Tile, bool> placementValidation;
	Action<Tile, Villager> onInstallComplete;
	Action<Tile, Villager> onPlaced;

	ObjectPossibleJobs possibleJobs;

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

	public Action<Tile, Villager> OnInstalledComplete {
		get {
			return onInstallComplete;
		}
	}

	public Action<Tile, Villager> OnPlaced {
		get {
			return onPlaced;
		}
	}
    
	public ObjectPossibleJobs PossibleJobs {
		get {
			return possibleJobs;
		}
	}

	public InstalledObjectSpawnAdditional SpawnAdd {
		get {
			return spawnAdd;
		}
	}

	public InstalledObject(InstalledObjectInfo info, Tile baseTile){
		this.id = info.ID;
		this.name = info.Name;

		this.placementValidation = info.PlacementValidation;
		this.onInstallComplete = info.OnInstallComplete;
		this.onPlaced = info.OnPlaced;
		this.canMoveThrough = info.CanMoveThrough;
		this.moveCost = info.MoveCost;

		this.height = info.Height;
		this.width = info.Width;

		this.tiles = new List<Tile> ();

		for (int i = 0; i < this.Width; i++) {
			for (int j = 0; j < this.Height; j++) {
				this.tiles.Add(MapController.Instance.GetTileAtWorldPos(baseTile.X + i, baseTile.Y + j));
			}
		}

		Debug.Log (tiles.Count);
		foreach (Tile t in tiles) {
			Debug.Log (t.GetPosition ());
		}
	}

//	public InstalledObject (int ID, string name, Func<Tile, bool> validation, 
//		Action<Tile, Villager> onJobComplete, Action<Tile, Villager> onPlaced = null,
//		bool canMoveThrough = false, float moveCost=Mathf.Infinity){
//
//		this.id = ID;
//		this.name = name;
//
//		this.placementValidation = validation;
//		this.onInstallComplete = onJobComplete;
//		this.onPlaced = onPlaced;
//		this.canMoveThrough = canMoveThrough;
//		this.moveCost = moveCost;
//
//		this.relativeTiles = new int[][]{ new int[]{ 0, 0 } };
//		this.height = 1;
//	}
//
//	public InstalledObject (int ID, string name, Func<Tile, bool> validation, 
//		Action<Tile, Villager> onJobComplete, Action<Tile, Villager> onPlaced, int width, int height, 
//		bool canMoveThrough = false, float moveCost=Mathf.Infinity) 
//		: this(ID, name, validation, onJobComplete, onPlaced, canMoveThrough, moveCost){
//
//		SetMultiTiles (width, height);
//		this.height = height;
//	}

	public bool StandardValidation(Tile t){
		return true;
	}

	public void InitInteraction(InstalledObjectInteractionInfo info){
		this.interaction = new InstalledObjectInteraction (info.PrevInteraction, info.OnInteraction);

	}

	public void InitPossibleJobs(ObjectPossibleJobsInfo info){
		this.possibleJobs = new ObjectPossibleJobs(info.PossibleJobs);
	}

	public void InitSpawnAdditional(InstalledObjectSpawnAdditionalInfo info){
		this.spawnAdd = new InstalledObjectSpawnAdditional(info.Additions);
	}
}
