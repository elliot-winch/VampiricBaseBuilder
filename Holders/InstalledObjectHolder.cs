using UnityEngine;
using System;
using System.Collections.Generic;

public static class InstalledObjectHolder {

	static InstalledObjectInfo[] objs;
	static Dictionary<int, InstalledObjectInteractionInfo> interactionDictionary;
	static Dictionary<int, InstalledObjectSpawnAdditionalInfo> spawnAddDictionary;
	static Dictionary<int, ObjectPossibleJobsInfo> possJobsDictionary;
	static SpriteHolder[] sprites;
	static int maxObjID = 10;

	public static int MaxObjID {
		get {
			return maxObjID;
		}
	}

	public static SpriteHolder[] Sprites {
		get { return sprites; }
	}
		
	public static void Init(){
		interactionDictionary = new Dictionary<int, InstalledObjectInteractionInfo> ();
		spawnAddDictionary = new Dictionary<int, InstalledObjectSpawnAdditionalInfo> ();
		possJobsDictionary = new Dictionary<int, ObjectPossibleJobsInfo> ();

		//TODO read from file
	
		objs = new InstalledObjectInfo[maxObjID];
		sprites = new SpriteHolder[maxObjID];

		//Wall
		objs[0] = new InstalledObjectInfo(0, "Wall", FurnitureValidation, JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd]);
		sprites [0] = new SpriteHolder (Resources.LoadAll <Sprite> ("Sprites/Map/woodwall"), "InstalledObject", 50);

		//Tree
		objs[1] = new InstalledObjectInfo (1, "Tree", NatureValidation, JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd]);
		sprites [1] = new SpriteHolder ( Resources.LoadAll<Sprite> ("Sprites/Map/Forest Assets/Trees/Tree 10"), "InstalledObject", 50);

		//Door
		objs[2] = new InstalledObjectInfo (2, "Door", FurnitureValidation, JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd], 1, 1, null, true, 1.5f);
		sprites [2] = new SpriteHolder ( Resources.LoadAll <Sprite> ("Sprites/Map/doors"), "InstalledObject", 50);

		interactionDictionary.Add (2, new InstalledObjectInteractionInfo (2, (tile) => {
			SwitchSprite (tile, sprites [2].Sprites [21]);
		}, (tile) => {
			SwitchSprite (tile, sprites [2].Sprites [0]);
		}));
				

		possJobsDictionary.Add (2, new ObjectPossibleJobsInfo (2, new List<PossibleJob> () { 
			new PossibleJob ((int)JobList.Jobs.Lock, true),
			new PossibleJob ((int)JobList.Jobs.Unlock, false)
		}));
		//Doors start unlocked (i.e. cannot unlocked locked door)

		//Torch
		objs[3] = new InstalledObjectInfo(3, "Torch", FurnitureValidation, JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd], 1, 1, null, true, 1f);
		sprites [3] = new SpriteHolder ( Resources.LoadAll<Sprite>("Sprites/Map/torch"), "InstalledObject", 50);

		spawnAddDictionary.Add(3, new InstalledObjectSpawnAdditionalInfo(3, new List<GameObject>(){
			AdditionalsHolder.Instance.AllAdditionals[0]
		}));

		//Bed
		objs[4] = new InstalledObjectInfo(4, "Bed", FurnitureValidation, JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd], 1, 2);
		sprites [4] = new SpriteHolder (  Resources.LoadAll<Sprite> ("Sprites/Map/Forest Assets/Campsite/Bedroll 2"), "InstalledObject", 50);
		//Objects over multiple tiles!
		//Object with job list that only opens when you select a villager

		//Table
		objs[5] = new InstalledObjectInfo(5, "Table", FurnitureValidation, JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd]);
		sprites [5] = new SpriteHolder ( Resources.LoadAll<Sprite>("Sprites/Map/furnitureSprites")[13], "InstalledObject", 50);

		//Stock Pile (Tile)
		objs[6] = new InstalledObjectInfo(6, "Stock Pile", NatureValidation, 
			JobList.JobFunctions[(int)JobList.Jobs.StandardInstallEnd], 1, 1, JobList.MakeTileStockPile, true, 1f);
		sprites [6] = new SpriteHolder (Resources.Load<Sprite> ("Sprites/stockpilemarker"), "UI", 50);
	}


	public static InstalledObject CreateNewObject(int id, Tile baseTile){
		InstalledObject obj = new InstalledObject (objs [id], baseTile);

		if (interactionDictionary.ContainsKey (id)) {
			obj.InitInteraction (interactionDictionary[id]);
		}

		if (spawnAddDictionary.ContainsKey (id)) {
			obj.InitSpawnAdditional (spawnAddDictionary[id]);
		}

		if (possJobsDictionary.ContainsKey (id)) {
			obj.InitPossibleJobs (possJobsDictionary[id]);
		}

		return obj;
	}

	public static InstalledObjectInfo GetObjInfo(int id){
		return objs [id];
	}

	public static SpriteHolder GetSpriteHolder(int id){
		return sprites [id];
	}

	public static bool FurnitureValidation(Tile t){
		return t.Planned == null && t.Installed == null && t.Loose == null && (int)t.Type >= 1;//FIXME
			 
	}

	public static bool NatureValidation(Tile t){
		return t.Planned == null && t.Installed == null && t.Loose == null && (int)t.Type < 1;
	}

	public static void SwitchSprite(Tile t , Sprite s){
		MapController.Instance.InstalledObjects [t].GetComponent<SpriteRenderer> ().sprite = s;
	}

	public static void PlayAnimation(Tile t/*Animation a*/){
		Debug.Log ("Playing animation");
	}
}
	
public class InstalledObjectInfo{

	public string Name { get; }
	public int ID { get; }

	public int[][] RelativeTiles { get; }

	public int Height { get; }
	public int Width { get; }

	/*
	 * Currently, relative tiles starts at the top left corner and works down then across. 
	 */ 
	public float MoveCost { get; }
	public bool CanMoveThrough {get;}

	public Func<Tile, bool> PlacementValidation  {get;}
	public Action<Tile, Villager> OnInstallComplete  {get;}
	public Action<Tile, Villager> OnPlaced  {get;}

	public InstalledObjectInfo (int ID, string name, Func<Tile, bool> validation, 
		Action<Tile, Villager> onJobComplete, int height = 1, int width = 1, Action<Tile, Villager> onPlaced = null,
		bool canMoveThrough = false, float moveCost=Mathf.Infinity){

		this.ID = ID;
		this.Name = name;

		this.PlacementValidation = validation;
		this.OnInstallComplete = onJobComplete;
		this.OnPlaced = onPlaced;
		this.CanMoveThrough = canMoveThrough;
		this.MoveCost = moveCost;

		this.Height = height;
		this.Width = width;

		this.RelativeTiles = new int[width * height][];

		int counter = 0;
		for (int i = 0; i < width; i++) {
			for (int j = 0; j < height; j++) {
				this.RelativeTiles [counter++] = new int[]{ i, j };
			}
		}
	}
}

public class ObjectPossibleJobsInfo{
	public int ID { get; }

	public List<PossibleJob> PossibleJobs { get; }

	public ObjectPossibleJobsInfo(int id,  List<PossibleJob> list){
		this.ID = id;
		this.PossibleJobs = list;
	}
}

public class InstalledObjectInteractionInfo{
	public int ID { get; }

	public Action<Tile> PrevInteraction {get;}
	public Action<Tile> OnInteraction {get;}

	public InstalledObjectInteractionInfo(int id, Action<Tile> prev, Action<Tile> onI){
		this.ID = id;
		this.PrevInteraction = prev;
		this.OnInteraction = onI;
	}
}

public class InstalledObjectSpawnAdditionalInfo{
	public int ID { get; }

	public List<GameObject> Additions {get;}

	public InstalledObjectSpawnAdditionalInfo(int id, List<GameObject> list){
		this.ID = id;
		this.Additions = list;
	}

}

public class SpriteHolder{

	Sprite[] sprites;
	string sortingLayer;
	int sortingOrder;

	public Sprite Sprite {
		get {
			return sprites[0];
		}
	}

	public Sprite[] Sprites {
		get {
			return sprites;
		}
	}

	public string SortingLayer {
		get {
			return sortingLayer;
		}
	}

	public int SortingOrder {
		get {
			return sortingOrder;
		}
	}

	public SpriteHolder(Sprite[] sprite, string sl, int sortingOrder){
		this.sprites = sprite;
		this.sortingLayer = sl;
		this.sortingOrder = sortingOrder;
	}

	public SpriteHolder(Sprite sprite, string sl, int sortingOrder){
		this.sprites = new Sprite[1];
		this.sprites[0] = sprite;
		this.sortingLayer = sl;
		this.sortingOrder = sortingOrder;
	}
}
