﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : MonoBehaviour, IUpdateableWithTime {

	static VampireController _instance;

	public static VampireController Instance {
		get {
			return _instance;
		}
	}

	Vampire playerVampire;

	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be two vampire controllers");
		}
		_instance = this;
			
		SpawnPlayer (new Vector3(10f, 10f, 0f));
	}

	public void SpawnPlayer(Vector3 pos){
		playerVampire = new Vampire (2f, pos);

		//Based on hierarchy

		GameObject v_go = new GameObject ();
		v_go.name = "Player"; //FIXME
		v_go.transform.position = playerVampire.Position;

		v_go.transform.SetParent (this.transform, true);

		SpriteRenderer v_go_sr = v_go.AddComponent<SpriteRenderer> ();
		v_go_sr.sprite = Resources.LoadAll<Sprite>("Sprites/spriteSheet1")[20];//FIXME
		v_go_sr.sortingLayerName = "Creatures";

		v_go_sr.material = Resources.Load<Material> ("Materials/Lit2DMat");

		playerVampire.AssignMoveCallback ( (vampire) => {ChangeVampirePosition(vampire, v_go);} );

		GetComponent<CameraControllerVampireMode> ().Begin (this.playerVampire);

	}

	public void UpdateWithTime(float time){
		if (playerVampire != null) {
			playerVampire.Update (time);
		}
	}

	//For IUpdateableWithTime inteface
	public bool IsActive(){
		return this.enabled;
	}

	void ChangeVampirePosition(Vampire v, GameObject v_go){
		v_go.transform.position = v.Position;
	}
}
