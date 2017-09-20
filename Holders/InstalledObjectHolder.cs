using UnityEngine;
using System.Collections.Generic;

public static class InstalledObjectHolder {

	static InstalledObject[] objs;
	static int maxObjID = 2;

	public static int MaxObjID {
		get {
			return maxObjID;
		}
	}

	public static void Init(){
		//TODO read from file
	
		objs = new InstalledObject[maxObjID];

		objs[0] = new InstalledObject(0, "Wall", Resources.Load <Sprite> ("Sprites/wall"), FurnitureValidation);
		objs[1] = new InstalledObject (1, "Tree", Resources.Load<Sprite> ("Sprites/Forest Assets/Trees/Tree 10"), NatureValidation, 10);
	}

	public static InstalledObject GetValue(int val){ 
		if (val < 0 || val > maxObjID) {
			return null;
		}
		return objs [val];
	}

	public static bool FurnitureValidation(Tile t){
		return t.Installed == null && (int)t.Type >= 1;//FIXME
			 
	}

	public static bool NatureValidation(Tile t){
		return t.Installed == null && (int)t.Type < 1;
	}
}
