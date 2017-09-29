using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObjectInteraction {

	Action<Tile> prevInteraction; //How all entities interact with this installed object when they've just stepped off it
	Action<Tile> onInteraction;  //How all entities interact with this installed object 

	//When a villager just finished moving through this tile, what happens?
	public void RegisterPrevInteractionCallback(Action<Tile> callback){
		prevInteraction += callback;
	}

	//When a villager moves through this tile, what happens?
	public void RegisterInteractionCallback(Action<Tile> callback){
		onInteraction += callback;
	}

	public void PrevInteract(Tile t){
		if (prevInteraction != null) {
			prevInteraction (t);
		}
	}

	public void Interact(Tile t){
		if (onInteraction != null) {
			onInteraction (t);
		}
	}
}
