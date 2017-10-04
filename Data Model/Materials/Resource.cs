using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : ILooseObject {

	int amount;

	public int Amount {
		get {
			return amount;
		} 
		set {
			if (value >= 0) {
				amount = value;
			}
		}
	}

	public Resource(int amount){
		this.Amount = amount;
	}

	public Resource(){
		this.Amount = 0;
	}

	public void Combine(Resource r){
		if (r.Amount > 0) {
			this.Amount += r.Amount;
			r.Amount = 0;

		}
	}
}
