using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerVampireMode : MonoBehaviour {

	Canvas canvas;

	void Start(){
		canvas = transform.GetChild (0).GetComponent<Canvas> ();
	}

	public void OpenCanvas(){
		canvas.gameObject.SetActive (true);
	}
	
	public void CloseCanvas(){
		canvas.gameObject.SetActive (false);
	}
}
