using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is designed to have only exclusively one of its child objects 
 * with a Display component active at any one time
 * 
 */ 

public class MutExPanels : MonoBehaviour {

	Display[] mutExObjs;

	public int startEnabled = 0;
	int currentEnabled = -1;

	void Start(){
		mutExObjs = GetComponentsInChildren<Display> (true);

		foreach (Display d in mutExObjs) {
			d.SetActive (false);
		}

		Switch (startEnabled);
	}

	public void Switch(int index){
		if (index < 0 || index >= mutExObjs.Length || index == currentEnabled) {
			Off ();
		} else {
			if (currentEnabled >= 0 && currentEnabled < mutExObjs.Length) {
				mutExObjs [currentEnabled].SetActive (false);
			}
			mutExObjs [index].SetActive (true);
			currentEnabled = index;
		}
	}

	public void Off(){
		if (currentEnabled >= 0 && currentEnabled < mutExObjs.Length) {
			mutExObjs [currentEnabled].SetActive (false);
		}
		currentEnabled = -1;
	}
}
