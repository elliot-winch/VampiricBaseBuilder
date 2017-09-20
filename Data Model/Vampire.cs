using System;
using System.Collections.Generic;
using UnityEngine;

public class Vampire {

	KeyCode up = KeyCode.W;
	KeyCode left = KeyCode.A;
	KeyCode down = KeyCode.S;
	KeyCode right = KeyCode.D;

	public KeyCode[] getKeyCodes(){
		return new KeyCode[]{ up, left, down, right };
	}

	Vector3 position;
	Action<Vampire> onMove;

	public Vector3 Position {
		get {
			return position;
		} set {
			position = new Vector3 (position.x + value.x, position.y + value.y, 0f);

			if (onMove != null) {
				onMove (this);
			}
		}
	}

	float moveSpeed;

	public float MoveSpeed{ 
		get { 
			return moveSpeed;
		}
		set{ 
			if (value >= 0) {
				moveSpeed = value;
			} else {
				Debug.Log ("Setting MoveSpeed to less than zero is not allowed");
			}
		}
	}

	public Vampire(float moveSpeed, Vector3 position){
		this.MoveSpeed = moveSpeed;
		this.Position = position;
	}

	public void AssignMoveCallback(Action<Vampire> callback){
		onMove += callback;
	}

	public void Update(float time){
		UpdateMovement (time);
	}

	//For garbage collection purposes, this Vecotr2 is declared here and reused each frame

	Vector3 toMove = new Vector3();

	void UpdateMovement(float time){
		toMove = Vector3.zero;

		if (Input.GetKey (up)) {
			toMove.y += time * MoveSpeed;
		}
		if (Input.GetKey (down)) {
			toMove.y -= time * MoveSpeed;
		}
		if (Input.GetKey (left)) {
			toMove.x -= time * MoveSpeed;
		}
		if (Input.GetKey (right)) {
			toMove.x += time * MoveSpeed;
		}

		Position = toMove;
	}
}
