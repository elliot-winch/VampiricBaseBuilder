using System;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	Action onDisplay;

	void Awake(){
		SetActive (false);
	}

	public void SetActive(bool b){
		if (b && onDisplay != null) {
			onDisplay ();
			Debug.Log ("On display");
		}
		gameObject.SetActive (b);
	}

	public void ToggleActive(){
		SetActive (!gameObject.activeSelf);
	}

	public void RegisterOnDisplay(Action callback){
		onDisplay += callback;
	}
}
