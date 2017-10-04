using System;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	Action onDisplay;

	void Awake(){
		gameObject.SetActive (false);
		if (onDisplay != null) {
			onDisplay ();
		}
	}

	public void SetActive(bool b){
		if (b && onDisplay != null) {
			onDisplay ();
		}
		gameObject.SetActive (b);
	}

	public void ToggleActive(){
		gameObject.SetActive (!gameObject.activeSelf);
	}

	public void RegisterOnDisplay(Action callback){
		onDisplay += callback;
	}
}
