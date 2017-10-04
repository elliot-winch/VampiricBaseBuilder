using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

	public enum ResourceNames{
		Mud,
		Wood,
		Stone,
		Bricks,
		Wheat
	}

	Dictionary<ResourceNames, Resource> resources;

	public Dictionary<ResourceNames, Resource> Resources {
		get {
			return resources;
		}
	}

	public void ChangeResourceVal(ResourceNames index, int value){
		if((int)index >= 0 && (int)index < System.Enum.GetValues (typeof(ResourceNames)).Length){
			resources [index].Amount = value;
			UIControllerBuildMode.Instance.EditResourceValue ((int)index, value);
		}
	}

	/////

	void Start () {
		this.resources = new Dictionary<ResourceNames, Resource> ();

		System.Array names = System.Enum.GetValues (typeof(ResourceNames));
		Debug.Log (names.Length);
		for(int i = 0; i < names.Length; i++) {
			this.resources.Add ((ResourceNames) i, new Resource ());
			ChangeResourceVal ((ResourceNames) i, 0);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
