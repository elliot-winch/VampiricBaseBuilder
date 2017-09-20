using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	void Start(){
		gameObject.SetActive (false);
	}

	public void Clicked(){
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
