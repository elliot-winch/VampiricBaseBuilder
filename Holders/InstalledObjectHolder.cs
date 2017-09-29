using UnityEngine;
using System.Collections.Generic;

public static class InstalledObjectHolder {

	static InstalledObject[] objs;
	static SpriteHolder[] sprites;
	static int maxObjID = 6;

	public static InstalledObject[] Installables {
		get {
			return objs;
		}
	}

	public static int MaxObjID {
		get {
			return maxObjID;
		}
	}

	public static SpriteHolder[] Sprites {
		get {
			return sprites;
		}
	}

	public static void Init(){
		//TODO read from file
	
		objs = new InstalledObject[maxObjID];
		sprites = new SpriteHolder[maxObjID];

		//Wall
		objs[0] = new InstalledObject(0, "Wall", FurnitureValidation, JobList.StandardJobs.StandardInstall);
		sprites [0] = new SpriteHolder (Resources.LoadAll <Sprite> ("Sprites/Map/woodwall"), "InstalledObject", 50);

		//Tree
		objs[1] = new InstalledObject (1, "Tree", NatureValidation, JobList.StandardJobs.StandardInstall);
		sprites [1] = new SpriteHolder ( Resources.LoadAll<Sprite> ("Sprites/Map/Forest Assets/Trees/Tree 10"), "InstalledObject", 50);

		//Door
		objs[2] = new InstalledObject (2, "Door", FurnitureValidation, JobList.StandardJobs.StandardInstall, true, 1.5f);
		sprites [2] = new SpriteHolder ( Resources.LoadAll <Sprite> ("Sprites/Map/doors"), "InstalledObject", 50);

		objs[2].InitInteraction ();
		objs[2].Interaction.RegisterPrevInteractionCallback ( (tile) => { SwitchSprite(tile, sprites[2].Sprites[21]); });
		objs[2].Interaction.RegisterInteractionCallback ( (tile) => { SwitchSprite(tile, sprites[2].Sprites[0]); });
        
		objs[2].InitPossibleJobs ();
		objs[2].PossibleJobs.AddPossibleJob((int)JobList.StandardJobs.Lock, true); 
		objs[2].PossibleJobs.AddPossibleJob((int)JobList.StandardJobs.Unlock, false);
		//Doors start unlocked (i.e. cannot unlocked locked door)

		//Torch
		objs[3] = new InstalledObject(3, "Torch", FurnitureValidation, JobList.StandardJobs.StandardInstall, true, 1f);
		sprites [3] = new SpriteHolder ( Resources.LoadAll<Sprite>("Sprites/Map/torch"), "InstalledObject", 50);
		objs[3].InitSpawnAdditional ();
		objs[3].SpawnAdd.AddAdditional(0);

		//Bed
		objs[4] = new InstalledObject(4, "Bed", FurnitureValidation, JobList.StandardJobs.StandardInstall, 1, 2);
		sprites [4] = new SpriteHolder (  Resources.LoadAll<Sprite> ("Sprites/Map/Forest Assets/Campsite/Bedroll 2"), "InstalledObject", 50);
		//Objects over multiple tiles!
		//Object with job list that only opens when you select a villager
	}

	public static InstalledObject GetValue(int val){ 
		if (val < 0 || val > maxObjID) {
			return null;
		}
		return objs [val];
	}

	public static bool FurnitureValidation(Tile t){
		return t.Planned == null && t.Installed == null && (int)t.Type >= 1;//FIXME
			 
	}

	public static bool NatureValidation(Tile t){
		return t.Planned == null && t.Installed == null && (int)t.Type < 1;
	}

	public static void SwitchSprite(Tile t , Sprite s){
		MapController.Instance.InstalledObjects [t].GetComponent<SpriteRenderer> ().sprite = s;
	}

	public static void PlayAnimation(Tile t/*Animation a*/){
		Debug.Log ("Playing animation");
	}
}

public class SpriteHolder{

	Sprite[] sprite;
	string sortingLayer;
	int sortingOrder;

	public Sprite[] Sprites {
		get {
			return sprite;
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
		this.sprite = sprite;
		this.sortingLayer = sl;
		this.sortingOrder = sortingOrder;
	}
}
