using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraGraphicalElementHolder : MonoBehaviour {

	static int numElements = 2;
	static ExtraGraphicalElement[] elements;

	public static ExtraGraphicalElement[] Elements {
		get {
			return elements;
		}
	}

	public static void Init(){
		//Load from file
		elements = new ExtraGraphicalElement[numElements];

		elements [0] = new ExtraGraphicalElement ("Lock Symbol", Resources.Load <Sprite> ("Sprites/UI/lock"));
		elements [1] = new ExtraGraphicalElement ("Deletion Marker", Resources.Load<Sprite> ("Sprites/UI/deletion"));
	}

	public ExtraGraphicalElement GetElement(int id){
		return elements [id];
	}
}