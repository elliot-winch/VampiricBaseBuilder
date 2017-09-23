using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	static MapController _instance;

	Map map;

	Dictionary<Tile, GameObject> plannedObjects;
	Dictionary<Tile, GameObject> installedObjects;

	public Sprite floorSprite;

	public Map Map {
		get {
			return map;
		}
	}

	public static MapController Instance {
		get {
			return _instance;
		}
	}

	public Dictionary<Tile, GameObject> PlannedObjects {
		get {
			return plannedObjects;
		}
	}

	public Dictionary<Tile, GameObject> InstalledObjects {
		get {
			return installedObjects;
		}
	}
		
	void Start () {
		if (_instance != null) {
			Debug.LogError ("There should not be two map controllers");
		}
		_instance = this;

		plannedObjects = new Dictionary<Tile, GameObject> ();
		installedObjects = new Dictionary<Tile, GameObject> ();

		Init ();

		BuildMapTileType ();
	}

	void Init(){
		map = new Map ();

		for (int i = 0; i < map.Width; i++) {
			for (int j = 0; j < map.Height; j++) {
				Tile tile_data = map.GetTileAt (i, j);

				GameObject tile_go = new GameObject ();
				tile_go.name = "Tile_" + i + "_" + j;
				tile_go.transform.position = new Vector3 (tile_data.X, tile_data.Y);
				tile_go.transform.SetParent (this.transform, true);

				//FIXME: flooring is job
				tile_data.RegisterTileTypeChangeCallback ((tile) => { OnTileTypeChanged(tile, tile_go);} );

				tile_data.RegisterPlannedChangeCallback ( CreatePlannedObject );

				//FIXME: make general case, probs pass an object or type & use a lambda
				tile_data.RegisterInstalledChangeCallback ( CreateInstalledObject );

				tile_go.AddComponent<SpriteRenderer> ();
			}
		}
	}

	void BuildMapTileType(){
		//FIXME with biomes, random generation etc.
		for (int i = 0; i < map.Width; i++) {
			for (int j = 0; j < map.Height; j++) {
				map.GetTileAt (i, j).Type = Tile.TileType.Dirt;
			}
		}
	}

	public Tile GetTileAtWorldPos(Vector3 pos){
		int x = (int)pos.x;
		int y = (int)pos.y;

		return map.GetTileAt (x, y);
	}

	public Tile GetTileAtWorldPos(int x, int y){
		return map.GetTileAt(x, y);
	}

	public Tile GetTileAtWorldPos(float x, float y){
		return map.GetTileAt((int)x, (int)y);
	}

	void OnTileTypeChanged(Tile tile_data, GameObject tile_go){
		if (tile_data.Type == Tile.TileType.Last) {
			tile_go.GetComponent<SpriteRenderer> ().sprite = null;
			return;
		}

		tile_go.GetComponent<SpriteRenderer> ().sprite = TileTypeHolder.sprites [(int)tile_data.Type];
	}

		
	void CreatePlannedObject(Tile tile){
		if (tile == null) {
			Debug.LogError ("Cannot init object on null tile");
			return;
		}

		if (tile.Planned == null) {
			GameObject obj_go_preexisting;

			if (installedObjects.TryGetValue (tile, out obj_go_preexisting)) {
				installedObjects.Remove (tile);
				//FIXME: shouldn't destroy object, should start job to be dismantled
				Destroy (obj_go_preexisting);
			}
			return;
		}

		InstalledObject obj = tile.Planned;

		GameObject obj_go = new GameObject ();
		obj_go.name = obj.Name;
		obj_go.transform.position = new Vector3 (obj.BaseTile.X, obj.BaseTile.Y);

		obj_go.transform.SetParent (this.transform, true);

		SpriteRenderer obj_go_sr = obj_go.AddComponent<SpriteRenderer> ();
		obj_go_sr.sprite = obj.Sprite;
		obj_go_sr.sortingLayerName = "InstalledObject";

		obj_go_sr.color = new Color (obj_go_sr.color.r, obj_go_sr.color.g, obj_go_sr.color.b, obj_go_sr.color.a * 0.5f);

		plannedObjects.Add (tile, obj_go);

		//Add job
		JobController.Instance.AddJob (Time.realtimeSinceStartup, new Job(tile, JobList.JobFunctions[(int)obj.OnJobComplete] /* t.Planned.WorkToBuild*/));
	}

	public void CreateInstalledObject(Tile tile){
		if (tile == null) {
			Debug.LogError ("Cannot init object on null tile");
			return;
		}

		GameObject obj_toDestroy;

		if (plannedObjects.TryGetValue(tile, out obj_toDestroy)) {
			plannedObjects.Remove (tile);
			Destroy (obj_toDestroy);
		}

		if (tile.Installed == null) {
			GameObject obj_go_preexisting;

			if (installedObjects.TryGetValue (tile, out obj_go_preexisting)) {
				installedObjects.Remove (tile);
				Destroy (obj_go_preexisting);
			}
			return;
		}

		InstalledObject obj = tile.Installed;

		GameObject obj_go = new GameObject ();
		obj_go.name = obj.Name;
		obj_go.transform.position = tile.GetPosition ();

		obj_go.transform.SetParent (this.transform, true);

		SpriteRenderer obj_go_sr = obj_go.AddComponent<SpriteRenderer> ();
		obj_go_sr.sprite = obj.Sprite;
		obj_go_sr.sortingLayerName = obj.SortingLayer;
		obj_go_sr.sortingOrder = obj.SortingOrder;

		installedObjects.Add (tile, obj_go);
	}
}
