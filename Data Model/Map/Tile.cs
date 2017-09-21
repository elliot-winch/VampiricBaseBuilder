using System;
using UnityEngine;

public class Tile : INode{

	public enum TileType{ Dirt, WoodFloor, Last /*Custom null type */};

	TileType type;
	TileType originalType = TileType.Last;

	LooseObject loose;
	InstalledObject planned;
	InstalledObject installed;

	Villager occupyingVillager;

	Map map;
	int x;
	int y;

	float moveCost; //FIXME: to depend on flooring/material etc.

	Action<Tile> typeChange;
	//This is for the player placing an object (ie job generated)
	Action<Tile> plannedChange;
	//This is for the system placing an object (ie no job generated)
	Action<Tile> rawInstallChange;
	Action<Tile> moveCostChange;

	public int X {
		get {
			return x;
		}
	}
		
	public int Y {
		get {
			return y;
		}
	}

	//For INode
	public Vector3 GetPosition(){
		return new Vector3 (x, y);
	}
		
	public TileType Type {
		get {
			return type;
		}
		set {
			TileType oldType = type;
			type = value;

			if (typeChange != null && oldType != type) {
				typeChange (this);
			}
		}
	}

	public InstalledObject Planned {
		get {
			return planned;
		}
		set {
			if (planned != null) {
				Debug.Log ("Plan already exists here");

			} else if ((installed == null && value != null) || (installed != null && value == null)) {
				InstalledObject obj = planned;
				planned = value;
				if (plannedChange != null && obj != planned){
					plannedChange (this);
				}
			} 
		}
	}

	public InstalledObject Installed {
		get {
			return installed;
		}
		set {
			if ((installed == null && value != null) || (installed != null && value == null)) {
				Debug.Log (installed + " : " + value);
				InstalledObject obj = installed;
				installed = value;
				this.moveCost = value.MoveCost;



				if (rawInstallChange != null && obj != installed) {
					planned = null;
					rawInstallChange (this);
				}
			} else {
				if (value != null) {
					Debug.Log ("Object already installed here");
				}
			}
		}
	}

	public float MoveCost {
		get {
			return moveCost;
		}
		set {
			moveCost = value;
		}
	}

	public Villager OccupyingVillager {
		get {
			return occupyingVillager;
		}
		set {
			occupyingVillager = value;
		}
	}
		
	public Tile(Map m, int x, int y, TileType startingType, float moveCost = 1f){
		this.map = m;
		this.x = x;
		this.y = y;
		this.installed = null;

		this.originalType = startingType;
		this.Type = originalType;

		this.moveCost = moveCost;
	}

	public void RegisterTileTypeChangeCallback(Action<Tile> callback){
		typeChange += callback;
	}

	public void RegisterPlannedChangeCallback(Action<Tile> callback){
		plannedChange += callback;
	}

	public void RegisterInstalledChangeCallback(Action<Tile> callback){
		rawInstallChange += callback;
	}

	public void ResetTile(){
		this.Type = originalType;
	}

	public Tile[] GetNeighbours(){
		Tile[] neighbours = new Tile[8];

		neighbours[0] = map.GetTileAt (this.X, this.Y + 1);
		neighbours[1] = map.GetTileAt (this.X + 1, this.Y + 1);
		neighbours[2] = map.GetTileAt (this.X + 1, this.Y);
		neighbours[3] = map.GetTileAt (this.X + 1, this.Y - 1);
		neighbours[4] = map.GetTileAt (this.X, this.Y - 1);
		neighbours[5] = map.GetTileAt (this.X - 1, this.Y - 1);
		neighbours[6] = map.GetTileAt (this.X - 1, this.Y);
		neighbours[7] = map.GetTileAt (this.X - 1, this.Y + 1);

		return neighbours;
	}

	//For calculating best place to walk for job. Might return null.
	public Tile NearestNeighbourTo(int x, int y){
		Tile[] neighbours = GetNeighbours ();

		Tile currentMin = null;
		float min = Mathf.Infinity;
		float posMin;

		foreach (Tile tile in neighbours) {
			if (tile != null && tile.MoveCost != Mathf.Infinity) {
				posMin = MyMath.SqrDistance (x, tile.X, y, tile.Y);
				if (posMin < min) {
					currentMin = tile;
					min = posMin;
				}
			}
		}
			
		return currentMin;
	}

	public void PlannedComplete(){
		installed = planned;
		planned = null;

		if (installed != null) {
			this.moveCost = installed.MoveCost;
		} else {
			//this.moveCost = originalType.moveCost;//FIXME
		}
	}

	public bool Equals(Tile t){
		if (t == null) {
			return false;
		}

		try{
			if (t.X == this.X && t.Y == this.Y) {
				return true;
			}
		}catch (Exception e){
		}

		return false;
	}
}
