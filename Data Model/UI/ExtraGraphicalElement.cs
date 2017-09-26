using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class ExtraGraphicalElement{

	string name;
	Sprite sprite;

	public string Name {
		get {
			return name;
		}
	}

	public Sprite Sprite {
		get {
			return sprite;
		}
	}

	public ExtraGraphicalElement(string name, Sprite s){
		this.name = name;
		this.sprite = s;
	}
}

