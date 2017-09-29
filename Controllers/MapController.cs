using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	static MapController _instance;

	Map map;

	Dictionary<Tile, GameObject> plannedObjects;
	Dictionary<Tile, GameObject> installedObjects;
	Dictionary<Tile, GameObject> extraGraphicalElements;

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

	public Dictionary<Tile, GameObject> ExtraGraphicalElements {
		get {
			return extraGraphicalElements;
		}
	}
		
	void Start () {
		if (_instance != null) {
			Debug.LogError ("There should not be two map controllers");
		}
		_instance = this;

		plannedObjects = new Dictionary<Tile, GameObject> ();
		installedObjects = new Dictionary<Tile, GameObject> ();
        extraGraphicalElements = new Dictionary<Tile, GameObject> ();

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

				SpriteRenderer tile_sr = tile_go.AddComponent<SpriteRenderer> ();
				tile_sr.material = Resources.Load<Material> ("Materials/Lit2DMat");

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

		
	public void CreateObject(Tile baseTile, InstalledObject obj, bool plan){
		if (baseTile == null) {
			Debug.LogError ("Cannot init object on null tile");
			return;
		}

		//Destory plan objs
		if (!plan) {
			GameObject obj_to_destroy;
			for (int i = 0; i < obj.RelativeTiles.Length; i++) {
				if (plannedObjects.TryGetValue (GetTileAtWorldPos (baseTile.X + obj.RelativeTiles [i] [0], baseTile.Y + obj.RelativeTiles [i] [1]), out obj_to_destroy)) {
					plannedObjects.Remove (baseTile);
					Destroy (obj_to_destroy);
				}
			}
		} else {
			//Validate position
			for (int i = 0; i < obj.RelativeTiles.Length; i++) {
				Tile t = MapController.Instance.GetTileAtWorldPos (baseTile.X + obj.RelativeTiles [i] [0], baseTile.Y + obj.RelativeTiles [i] [1]);
				if (t == null || !obj.PlacementValidation (t)) {
					Debug.Log ("Cannot place plan here");
					return;
				}  
			}
		}

		//Foreach tile in obj
		baseTile.Planned = obj;	
		int[][] relativeTiles = obj.RelativeTiles;

		for (int i = 0; i < relativeTiles.Length; i++) {
			Tile currentTile = GetTileAtWorldPos (baseTile.X + relativeTiles[i][0], baseTile.Y + relativeTiles[i][1]);

			GameObject obj_go = new GameObject ();
			obj_go.name = obj.Name;
			obj_go.transform.position = currentTile.GetPosition ();

			obj_go.transform.SetParent (this.transform, true);

			SpriteRenderer obj_go_sr = obj_go.AddComponent<SpriteRenderer> ();

			obj_go_sr.sprite = InstalledObjectHolder.Sprites [obj.ID].Sprites [i];
			obj_go_sr.sortingLayerName = InstalledObjectHolder.Sprites [obj.ID].SortingLayer;
			obj_go_sr.sortingOrder = InstalledObjectHolder.Sprites [obj.ID].SortingOrder;
			obj_go_sr.material = Resources.Load<Material> ("Materials/Lit2DMat");

			if (plan) {
				obj_go_sr.color = new Color (obj_go_sr.color.r, obj_go_sr.color.g, obj_go_sr.color.b, obj_go_sr.color.a * 0.5f);
				plannedObjects.Add (currentTile, obj_go);
				currentTile.Planned = obj;
			} else {
				installedObjects.Add (currentTile, obj_go);
				currentTile.Installed = obj;
			}

		}
			
		if (plan) {
			JobController.Instance.AddJob (Time.realtimeSinceStartup, new Job (baseTile, JobList.JobFunctions [(int)obj.OnJobComplete]));
		} else {
			if (obj.SpawnAdd != null) {
				foreach (GameObject g in obj.SpawnAdd.Additions) {
					Debug.Log (g.name);

					Instantiate (g, installedObjects[baseTile].transform);
				}
			}
		}
	}

	public void AddExtraGraphicalElement(Tile t, ExtraGraphicalElement e){

		GameObject e_go = new GameObject ();
		e_go.name = e.Name;
		e_go.transform.position = t.GetPosition();
		SpriteRenderer e_go_sr = e_go.AddComponent<SpriteRenderer> ();
		e_go_sr.sprite = e.Sprite;
		e_go_sr.sortingLayerName = "UI";
		//Do I want these to be affected by light? Idk bro

		extraGraphicalElements.Add (t, e_go);
	}

	public bool RemoveExtraGraphicalElement(Tile t){
		GameObject obj_go_preexisting;

		if (extraGraphicalElements.TryGetValue (t, out obj_go_preexisting)) {
			extraGraphicalElements.Remove (t);
			Destroy (obj_go_preexisting);
			return true;
		}

		return false;
	}
}
