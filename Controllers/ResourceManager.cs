using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

	static ResourceManager _instance;

	public static ResourceManager Instance {
		get {
			return _instance;
		}
	}

	Resource[] resources;
	public Resource[] VillageResources { get;}

	/////

	void Start () {
		if (_instance != null) {
			Debug.LogError ("There should not be more than one resource manager");
		}

		_instance = this;

		Resource.ResourceType[] resourceTypes = (Resource.ResourceType[]) System.Enum.GetValues (typeof(Resource.ResourceType));

		resources = new Resource[resourceTypes.Length];
			
		for(int i = 0; i < resourceTypes.Length; i++) {
			resources [i] = new Resource (resourceTypes [i]);
			ChangeVillageResourceVal (i, 0);
		}

	}

	public void ChangeVillageResourceVal(int index, int value){
		if((int)index >= 0 && (int)index < resources.Length){
			resources [index].Amount = value;
			UIControllerBuildMode.Instance.EditResourceValue ((int)index, value);
		}
	}
}
