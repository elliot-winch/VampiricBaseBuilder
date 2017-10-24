using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

	static InventoryManager _instance;

	List<Tile> stockpileSlots;

	public static InventoryManager Instance {
		get {
			return _instance;
		}
	}

	void Start(){
		if (_instance != null) {
			Debug.Log ("There should not be more than one Inventory manager");
			return;
		}

		_instance = this;

		stockpileSlots = new List<Tile> ();
	}
		
	public void AddSlot(Tile t){
		if (t != null && t.Installed.Name == "Stock Pile" /*FIXME: better way to check installed obj*/) {
			stockpileSlots.Add (t);
		}
	}

	void RemoveSlot(Tile t){

	}

	public Tile ClosestAvailableTile(Tile t){
		return ClosestAvailableTile (t.X, t.Y);
	}

	public Tile ClosestAvailableTile(float X, float Y){
		Tile currentMin = null;
		float min = Mathf.Infinity;
		float posMin;

		foreach (Tile tile in stockpileSlots) {
			if (tile != null && tile.Loose == null) {
				posMin = MyMath.SqrDistance (X, tile.X, Y, tile.Y);
				if (posMin < min) {
					currentMin = tile;
					min = posMin;
				}
			}
		}
			
		return currentMin;
	}
}
