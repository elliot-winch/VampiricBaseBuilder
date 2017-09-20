using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VampireModeController : MonoBehaviour {

	MonoBehaviour[] scripts;

	void Start(){
		scripts = gameObject.GetComponents<MonoBehaviour>();
	}

	public void PrepEnabled (){
		GetComponent<CameraControllerVampireMode> ().MoveToPlayer ();
	}

	public void OnEnabled(){
		GetComponent<UIControllerVampireMode> ().OpenCanvas ();

		foreach (MonoBehaviour m in scripts) {
			m.enabled = true;
		}
	}


	public void OnDisabled(){
		GetComponent<UIControllerVampireMode> ().CloseCanvas ();

		foreach (MonoBehaviour m in scripts) {
			m.enabled = false;
		}
	}
}
