using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeController : MonoBehaviour {

	bool isBuildMode;

	public KeyCode switchModes = KeyCode.M;
	public float switchTime = 2f;

	VampireModeController vampireControllerObj;
	BuildModeController buildControllerObj;

	bool canSwitch = true;

	void Start(){
		vampireControllerObj = transform.GetChild(0).GetComponentInChildren<VampireModeController> ();
		buildControllerObj = transform.GetChild(1).GetComponentInChildren<BuildModeController> ();

		vampireControllerObj.OnDisabled ();
		buildControllerObj.OnEnabled ();
		isBuildMode = true;
	}


	void Update(){
		if(Input.GetKeyDown(KeyCode.M) && canSwitch){
			StartCoroutine ("SwitchModes");
		}
	}

	IEnumerator SwitchModes(){
		canSwitch = false;

		if(isBuildMode){
			buildControllerObj.OnDisabled ();
			vampireControllerObj.PrepEnabled ();
		} else {
			//buildControllerObj.PrepEnabled ();
			vampireControllerObj.OnDisabled ();
		}

		yield return new WaitForSeconds (switchTime);

		if(isBuildMode){
			vampireControllerObj.OnEnabled ();
			isBuildMode = false;
		} else {
			buildControllerObj.OnEnabled ();
			isBuildMode = true;
		}

		canSwitch = true;
	}
}
