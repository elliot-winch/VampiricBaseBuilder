using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : INode, IJobPassable{

	public enum TileType{ Dirt, WoodFloor, Last /*Custom null type */};

	TileType type;
	TileType originalType = TileType.Last;

	LooseObject loose;
	Action<Tile> onLooseUpdate;

	InstalledObject planned;
	InstalledObject installed;
	public bool inside = false;

	List<Villager> occupyingVillagers;

	Map map;
	int x;
	int y;

	float moveCost; //FIXME: to depend on flooring/material etc.

	Action<Tile> typeChange;
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

	//LooseObject Handling
	public LooseObject Loose {
		get {
			return loose;
		}
		set {
			loose = value;

			if (onLooseUpdate != null) {
				onLooseUpdate (this);
			}
		}
	}

	public void RegisterLooseCallback(Action<Tile> callback){
		onLooseUpdate += callback;
	}

	// // // // // 

	public InstalledObject Planned {
		get {
			return planned;
		}
		set {
			planned = value;
		} 
	}

	public InstalledObject Installed {
		get {
			return installed;
		}
		set {
			if ((installed == null && value != null) ) {
				planned = null;
				installed = value;

				this.moveCost = value.GetMoveCost ();
				this.CanMoveThrough = value.CanMoveThrough;
			} else if(installed != null && value == null){
				installed = null;

				//this.moveCost = tile.movecost
				//this.CanMoveThrough = tile.CANMOVETHOURHG
				this.CanMoveThrough = true;
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

	public bool CanMoveThrough { get; set; }

	public List<Villager> OccupyingVillagers {
		get {
			return occupyingVillagers;
		}
		set {
			occupyingVillagers = value;
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
		this.CanMoveThrough = true; //FIXME tileType.canMoveThrough

		this.OccupyingVillagers = new List<Villager> ();
	}

	public void RegisterTileTypeChangeCallback(Action<Tile> callback){
		typeChange += callback;
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

	public Tile NearestNeighbourTo(){
		return NearestNeighbourTo (this.X, this.Y);
	}

	public Tile NearestNeighbourTo(Tile t){
		return NearestNeighbourTo (t.X, t.Y);
	}

	//For calculating best place to walk for job. Might return null
	public Tile NearestNeighbourTo(int x, int y){
		Tile[] neighbours = GetNeighbours ();

		Tile currentMin = null;
		float min = Mathf.Infinity;
		float posMin;

		while (currentMin == null) {
			foreach (Tile tile in neighbours) {
				if (tile != null && tile.CanMoveThrough) {
					posMin = MyMath.SqrDistance (x, tile.X, y, tile.Y);
					if (posMin < min) {
						currentMin = tile;
						min = posMin;
					}
				}
			}
		}
			
		return currentMin;
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
