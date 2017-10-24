using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {


	public enum ResourceType{
		Mud,
		Wood,
		Stone,
		Bricks,
		Wheat
	}

	ResourceType t;
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

	public ResourceType Type {
		get {
			return t;
		}
	}

	public Resource(int amount, ResourceType t){
		this.Amount = amount;
		this.t = t;
	}

	public Resource( ResourceType t){
		this.Amount = 0;
		this.t = t;
	}

	public void Combine(Resource r){
		if (r.Type == this.t) {
			if (r.Amount > 0) {
				this.Amount += r.Amount;
				r.Amount = 0;

			}
		}
	}
}
