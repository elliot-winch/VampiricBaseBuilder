using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileTypeHolder/* : IHolder*/ {

	public static Sprite[] sprites;

	public static void Init(){
		sprites = new Sprite[(int)Tile.TileType.Last]; 

		sprites[(int)Tile.TileType.Dirt] = Resources.Load <Sprite> ("Sprites/tempDirt");
		sprites[(int)Tile.TileType.WoodFloor] = Resources.Load <Sprite> ("Sprites/floor");
	}
}
