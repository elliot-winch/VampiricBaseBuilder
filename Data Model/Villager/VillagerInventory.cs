using System;
using System.Collections.Generic;
using UnityEngine;

public class VillagerInventory  {

	LooseObject carrying;
	Action onCarryingChanged;

	public LooseObject Carrying {
		get {
			return carrying;
		}
		set {
			carrying = value;
			onCarryingChanged ();
		}
	}

	public void Drop(Tile t){
		t.Loose = this.Carrying;
		this.Carrying = null;
		onCarryingChanged ();
	}

	public void RegisterOnCarryingChangedCallback(Action callback){
		onCarryingChanged += callback;
	}
}
