using System;
using UnityEngine;

public class BuildModeController : MonoBehaviour {

	MonoBehaviour[] scripts;

	void Start(){
		scripts = gameObject.GetComponents<MonoBehaviour>();
	}


	public void OnEnabled(){
		GetComponent<UIControllerBuildMode> ().OpenCanvas ();

		foreach (MonoBehaviour m in scripts) {
			m.enabled = true;
		}

		GetComponent<MouseManagerBuildMode> ().SetCursorActive (true);
	}


	public void OnDisabled(){
		GetComponent<UIControllerBuildMode> ().CloseCanvas ();

		foreach (MonoBehaviour m in scripts) {
			m.enabled = false; 
		}

		GetComponent<MouseManagerBuildMode> ().SetCursorActive (false);

	}
}

