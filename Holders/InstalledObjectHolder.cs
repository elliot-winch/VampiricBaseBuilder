using UnityEngine;
using System.Collections.Generic;

public static class InstalledObjectHolder {

	static InstalledObject[] objs;
	static int maxObjID = 3;

	public static int MaxObjID {
		get {
			return maxObjID;
		}
	}

	public static void Init(){
		//TODO read from file
	
		objs = new InstalledObject[maxObjID];

		objs[0] = new InstalledObject(0, "Wall", Resources.Load <Sprite> ("Sprites/wall"), FurnitureValidation, JobList.StandardJobs.StandardInstall);
		objs[1] = new InstalledObject (1, "Tree", Resources.Load<Sprite> ("Sprites/Forest Assets/Trees/Tree 10"), NatureValidation, JobList.StandardJobs.StandardInstall);

		Sprite doorSprite = (Sprite)(Resources.LoadAll <Sprite> ("Sprites/doors")).GetValue(21);
		Sprite doorOpenSprite = (Sprite)(Resources.LoadAll <Sprite> ("Sprites/doors")).GetValue(0);
		objs[2] = new InstalledObject (2, "Door", doorSprite, FurnitureValidation, JobList.StandardJobs.StandardInstall, true, 1.5f);
		objs[2].RegisterPrevInteractionCallback ( (tile) => { SwitchSprite(tile, doorSprite); });
		objs[2].RegisterInteractionCallback ( (tile) => { SwitchSprite(tile, doorOpenSprite); });
        
        objs[2].AddPossibleJob(JobList.StandardJobs.Lock);
		objs [2].AddPossibleJob (JobList.StandardJobs.Unlock);
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

	public static void SwitchSprite(Tile t , Sprite s){
		MapController.Instance.InstalledObjects [t].GetComponent<SpriteRenderer> ().sprite = s;
	}

	public static void PlayAnimation(Tile t/*Animation a*/){
		Debug.Log ("Playing animation");
	}
}
