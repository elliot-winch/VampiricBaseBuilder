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
	int currentEnabled;

	void Start(){
		mutExObjs = GetComponentsInChildren<Display> (true);

		foreach (Display d in mutExObjs) {
			d.SetActive (false);
		}

		mutExObjs [startEnabled].SetActive (true);
		currentEnabled = startEnabled;
	}

	public void Switch(int index){
		mutExObjs [currentEnabled].SetActive (false);
		mutExObjs [index].SetActive (true);
		currentEnabled = index;
	}
}
