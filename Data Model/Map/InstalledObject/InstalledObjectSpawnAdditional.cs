using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class may require the following additions:
 * 
 * Offset to spawn items at an offset to their parent
 * 
 */ 

public class InstalledObjectSpawnAdditional {

	List<GameObject> additions;

	public List<GameObject> Additions {
		get {
			return additions;
		}
	}

	public InstalledObjectSpawnAdditional(){
		additions = new List<GameObject> ();
	}
		
	public void AddAdditional(int id){
		if (id >= 0 && id < AdditionalsHolder.Instance.AllAdditionals.Length) {
			additions.Add (AdditionalsHolder.Instance.AllAdditionals [id]);
		}
	}
}
