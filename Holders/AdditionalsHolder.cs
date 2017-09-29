using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalsHolder : MonoBehaviour {

	static AdditionalsHolder _instance;

	public static AdditionalsHolder Instance {
		get {
			return _instance;
		}
	}

	public GameObject[] AllAdditionals;

	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be two AdditionalsHolders");
		}

		_instance = this;
	}

}