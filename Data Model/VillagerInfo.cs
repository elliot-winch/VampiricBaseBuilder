using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerInfo  {

	public const int NumFields = 2;

	string name;
	float age;

	public string Name {
		get {
			return name;
		}
	}

	public float Age {
		get {
			return age;
		}
	}

	//etc.

	public VillagerInfo(){
		this.name = "Dave";
		this.age = 0;
	}

	public VillagerInfo(string name, float age){
		this.name = name;
		this.age = age;

	}
}
