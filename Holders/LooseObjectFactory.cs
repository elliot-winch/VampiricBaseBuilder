using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooseObjectFactory : MonoBehaviour {

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


		sprites = new SpriteHolder[maxObjID];

		sprites[0] = new SpriteHolder(Resources.Load <Sprite> ("Sprites/Map/log"),"LooseObject", 50);
	}

	public static LooseObject CreateLooseObject(int id, Tile t){
		//FIXME
		return new LooseObject("Log", 0, new Resource(10), true, t);
	}


	public static bool StandardValidation(Tile t){
		return t.Planned == null && t.Installed == null;//FIXME
	}
}

