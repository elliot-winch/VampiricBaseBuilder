using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

	Tile[,] tiles;
	int width;
	int height;

	Graph graph;

	public int Width{ get { return width; } }
	public int Height{ get { return height; } }

	public Graph Graph { get { return graph; } }

	public Map(int width = 100, int height = 100){
		this.width = width;
		this.height = height;

		tiles = new Tile[width, height];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				tiles [i, j] = new Tile (this, i, j, Tile.TileType.Last);//FIXME

			}
		}

		graph = new Graph (this);
	}

	public Tile GetTileAt(int x, int y){
		if (x < width && x >= 0 && y < height && y >= 0){
			return tiles [x, y];
		}

		return null;
	}
}
