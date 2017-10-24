using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

	static MapController _instance;

	Map map;

	Dictionary<Tile, GameObject> plannedObjects;
	Dictionary<Tile, GameObject> installedObjects;
	Dictionary<Tile, GameObject> looseObjects;
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

	public Dictionary<Tile, GameObject> LooseObjects {
		get {
			return looseObjects;
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
		looseObjects = new Dictionary<Tile, GameObject> ();
        extraGraphicalElements = new Dictionary<Tile, GameObject> ();

		InitMap ();

		TileTypeHolder.Init ();
		LooseObjectFactory.Init ();

		//This order is important!!!
		JobList.Init ();
		InstalledObjectHolder.Init ();

		BuildMapTileType ();

		ExtraGraphicalElementHolder.Init ();
	
	}

	void InitMap(){
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

				tile_data.RegisterLooseCallback((tile) => 
					{
						MakeLooseObject(tile);
//						UIControllerBuildMode.Instance.UpdateInvUI();
					} 
				);

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
		
	public void CreatePlannedObject(Tile baseTile, int id){
		if (baseTile == null) {
			Debug.LogError ("Cannot init object on null tile");
			return;
		}

		InstalledObject obj = InstalledObjectHolder.CreateNewObject (id, baseTile);

		for (int i = 0; i < obj.Tiles.Count; i++) {
			if (!obj.PlacementValidation (obj.Tiles [i])) {
				Debug.Log ("Cannot place plan here");
				return;
			}
		}
			
		for (int i = 0; i < obj.Tiles.Count; i++) {

			GameObject obj_go = new GameObject ();
			obj_go.name = obj.Name;
			obj_go.transform.position = obj.Tiles[i].GetPosition();

			obj_go.transform.SetParent (this.transform, true);

			SpriteRenderer obj_go_sr = obj_go.AddComponent<SpriteRenderer> ();


			obj_go_sr.sprite = InstalledObjectHolder.Sprites [id].Sprites [i];
			obj_go_sr.sortingLayerName = InstalledObjectHolder.Sprites [id].SortingLayer;
			obj_go_sr.sortingOrder = InstalledObjectHolder.Sprites [id].SortingOrder;
			obj_go_sr.material = Resources.Load<Material> ("Materials/Lit2DMat");

			obj_go_sr.color = new Color (obj_go_sr.color.r, obj_go_sr.color.g, obj_go_sr.color.b, obj_go_sr.color.a * 0.5f);
			plannedObjects.Add (obj.Tiles[i], obj_go);
			obj.Tiles[i].Planned = obj;
		}
			
		JobController.Instance.AddJob (Time.realtimeSinceStartup, new Job (baseTile, obj.OnInstalledComplete));

	}

	public void CreateObject(Tile baseTile, int id){
		if (baseTile == null) {
			Debug.LogError ("Cannot init object on null tile");
			return;
		}

		InstalledObject obj;

		if (baseTile.Planned == null) {
			obj = InstalledObjectHolder.CreateNewObject (id, baseTile);
		} else {
			obj = baseTile.Planned;
		}

		DestroyPlannedObjectGraphic (baseTile.Planned);

		for (int i = 0; i < obj.Tiles.Count; i++) {
			GameObject obj_go = new GameObject ();
			obj_go.name = obj.Name;
			obj_go.transform.position = obj.Tiles[i].GetPosition ();

			obj_go.transform.SetParent (this.transform, true);

			SpriteRenderer obj_go_sr = obj_go.AddComponent<SpriteRenderer> ();

			obj_go_sr.sprite = InstalledObjectHolder.Sprites [id].Sprites [i];
			obj_go_sr.sortingLayerName = InstalledObjectHolder.Sprites [id].SortingLayer;
			obj_go_sr.sortingOrder = InstalledObjectHolder.Sprites [id].SortingOrder;
			obj_go_sr.material = Resources.Load<Material> ("Materials/Lit2DMat");

			installedObjects.Add (obj.Tiles[i], obj_go);
			obj.Tiles[i].Installed = obj;
		}
			
		if (obj.SpawnAdd != null) {
			foreach (GameObject g in obj.SpawnAdd.Additions) {
				Instantiate (g, installedObjects[baseTile].transform);
			}
		}
		if (obj.OnPlaced != null) {
			obj.OnPlaced (baseTile, null);
		}
	}


	/*
	 * Rather than a function that takes a tile, these two functions (above and below) should take an object and using its relative 
	 * tiles, delete all associated graphics
	 */

	public void DestroyPlannedObjectGraphic(InstalledObject obj){
		if (obj != null) {
			GameObject obj_to_destroy;
			for (int i = 0; i < obj.Tiles.Count; i++) {
				if (plannedObjects.TryGetValue (obj.Tiles[i], out obj_to_destroy)) {
					plannedObjects.Remove (obj.Tiles[i]);
					Destroy (obj_to_destroy);
					obj.Tiles[i].Planned = null;
				}
			}
		}
	}


	public void DestroyObjectGraphic(InstalledObject obj){
		if (obj != null) {
			GameObject obj_to_destroy;
			for (int i = 0; i < obj.Tiles.Count; i++) {
				if (installedObjects.TryGetValue (obj.Tiles[i], out obj_to_destroy)) {		
					installedObjects.Remove (obj.Tiles[i]);
					Destroy (obj_to_destroy);
					Debug.Log (obj.Tiles [i].Installed);
					obj.Tiles [i].Installed = null;
					Debug.Log (obj.Tiles [i].Installed);
				}
			}

			foreach(Tile t in obj.Tiles){
				Debug.Log (t.Installed);

			}
		}
	}

	public void MakeLooseObject(Tile t){
		LooseObject l = t.Loose;

		if (t.Loose == null) {
			GameObject obj_go_preexisting;

			if (looseObjects.TryGetValue (t, out obj_go_preexisting)) {
				looseObjects.Remove (t);
				Destroy (obj_go_preexisting);
			}

			return;
		}

		GameObject l_go = new GameObject ();
		l_go.name = l.Name;
		l_go.transform.position = t.GetPosition();
		SpriteRenderer l_go_sr = l_go.AddComponent<SpriteRenderer> ();
		l_go_sr.sprite = LooseObjectFactory.Sprites[l.ID].Sprite;
		l_go_sr.sortingLayerName = LooseObjectFactory.Sprites[l.ID].SortingLayer;
		l_go_sr.sortingOrder = LooseObjectFactory.Sprites [l.ID].SortingOrder;

		looseObjects.Add (t, l_go);
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
