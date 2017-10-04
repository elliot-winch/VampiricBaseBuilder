using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerInventory  {

	LooseObject carrying;

	public LooseObject Carrying {
		get {
			return carrying;
		}
		set {
			if (IsSpace) {
				carrying = value;
			}
		}
	}

	public bool IsSpace{
		get{
			return carrying == null;
		}
	}
}
