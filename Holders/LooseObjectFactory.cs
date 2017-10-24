using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseObjectFactory : MonoBehaviour {

	static LooseObjectInfo[] objInfo;
	static SpriteHolder[] sprites;
	static int maxObjID = 1;

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
		//will load info from file

		objInfo = new LooseObjectInfo[maxObjID];
		sprites = new SpriteHolder[maxObjID];

		objInfo [0] = new LooseObjectInfo ("Log", 0, new Resource(10, Resource.ResourceType.Wood), true);
		sprites[0] = new SpriteHolder(Resources.Load <Sprite> ("Sprites/Map/log"),"LooseObject", 50);
	}

	public static LooseObject CreateLooseObject(int id, Tile t){
		LooseObject l = new LooseObject(objInfo[id].Name, id, objInfo[id].Contents, objInfo[id].PickUp, t);
		//FIXME
		l.InitPossibleJobs();
		l.PossibleJobs.AddPossibleJob ((int)JobList.Jobs.PickUp, true);

		return l;
	}

	//FIXME: move me
	public static bool StandardValidation(Tile t){
		return t.Planned == null && t.Installed == null && t.Loose == null;//FIXME
	}
}

class LooseObjectInfo{
	string name;
	int id; 
	Resource contents;
	bool pickUp;

	public string Name {
		get {
			return name;
		}
	}

	public int Id {
		get {
			return id;
		}
	}

	public Resource Contents {
		get {
			return contents;
		}
	}

	public bool PickUp {
		get {
			return pickUp;
		}
	}

	public LooseObjectInfo(string name, int id, Resource contents, bool pickUp){
		this.name = name;
		this.id = id;
		this.contents = contents;
		this.pickUp = pickUp;
	}
}