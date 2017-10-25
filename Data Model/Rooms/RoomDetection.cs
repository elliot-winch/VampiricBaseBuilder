using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomDetection {

	public static void RoomDetect(Tile startTile, Map map){
		Debug.Log ("called");
		//FIXME: change to some propert of installed object (eg torches shouldnt work as walls)

		//Based on tile type (ie must be floor to be inside) and must be total enclosed by wall
		//The value of the tile cannot be inside or outside if it is taken up by a wall

		if (startTile == null || startTile.Type != Tile.TileType.WoodFloor || startTile.Installed != null) {
			return;
		}

		List<Tile> potentialTilesToConvert = new List<Tile> ();

		Queue<Tile> q = new Queue<Tile> ();

		q.Enqueue (startTile);
		Tile n, w, e, working;
		Tile[] potentials;

		while (q.Count != 0) {

			Debug.Log (q.Peek ().GetPosition());

			n = q.Dequeue ();
			w = moveHorizontal(n, map, true);
			e = moveHorizontal (n, map, false);

			if (w == null || e == null) {
				foreach (Tile t in potentialTilesToConvert) {
					t.inside = false;
				}
				return;
			}

			for(int i = w.X; i <= e.X; i++){
				working = map.GetTileAt (i, w.Y);
				potentialTilesToConvert.Add(working);
				working.inside = true;
			}

			potentials = new Tile[] {
				map.GetTileAt (w.X, w.Y + 1),
				map.GetTileAt (e.X, e.Y + 1),
				map.GetTileAt (w.X, w.Y - 1),
				map.GetTileAt (e.X, e.Y - 1),
			};

			foreach (Tile u in potentials) {
				if(u != null && u.Type == Tile.TileType.WoodFloor && u.Installed == null && u.inside == false){
					q.Enqueue (u);
				}
			}
		}
	}

	static Tile moveHorizontal(Tile t, Map m, bool west){

		int moveAmount = 1;

		if (west) {
			moveAmount = -1;
		}

		Tile next;

		try{
			while((next = m.GetTileAt(t.X + moveAmount, t.Y)) != null && next.Installed == null){
				if (next.Type != Tile.TileType.WoodFloor) {
					return null;
				}

				t = next;
			}
		} catch (NullReferenceException e){
			Debug.Log ("Reached end of map: room counts as outside");
			return null;
		}
			
		return t;
	}
}