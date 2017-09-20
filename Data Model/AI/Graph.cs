using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph {

	Dictionary<Tile, PathNode<Tile>> graph;

	public Dictionary<Tile, PathNode<Tile>> Current {
		get { return graph; }
	}

	public Graph(Map map){

		graph = new Dictionary<Tile, PathNode<Tile>> ();

		for (int i = 0; i < map.Width; i++) {
			for (int j = 0; j < map.Height; j++) {

				Tile t = map.GetTileAt (i, j);
				graph.Add (t, new PathNode<Tile> (t));
			}
		}

		foreach (Tile t in graph.Keys) {
			PathNode<Tile> n = graph [t];

			Tile[] neighbourTiles = t.GetNeighbours ();

			n.edges = new PathNode<Tile>[neighbourTiles.Length];

			for (int i = 0; i < neighbourTiles.Length; i++) {
				if (neighbourTiles [i] != null) {
					n.edges [i] = graph [neighbourTiles [i]];
				} 
			}
		}
	}
}
