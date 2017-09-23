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
	Tile currentTile;
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

	float baseMoveSpeed;

	public float BaseMoveSpeed{ 
		get { 
			return baseMoveSpeed;
		}
		set{ 
			if (value >= 0) {
				baseMoveSpeed = value;
			} else {
				Debug.Log ("Setting MoveSpeed to less than zero is not allowed");
			}
		}
	}

	public Vampire(float moveSpeed, Vector3 position){
		this.BaseMoveSpeed = moveSpeed;
		this.Position = position;
		this.currentTile = MapController.Instance.GetTileAtWorldPos (this.Position);
	}

	public void AssignMoveCallback(Action<Vampire> callback){
		onMove += callback;
	}

	public void Update(float time){
		UpdateMovement (time);
	}

	//For garbage collection purposes, this Vecotr2 is declared here and reused each frame

	Vector3 toMove = new Vector3();
	Vector3 tryMove = new Vector3();
	bool pressed = false;
	Tile[] tryTile = new Tile[4];

	void UpdateMovement(float time){
		pressed = false;

		toMove = Vector3.zero;

		if (Input.GetKey (up)) {
			toMove.y += time * BaseMoveSpeed / currentTile.MoveCost;
			pressed = true;
		}
		if (Input.GetKey (down)) {
			toMove.y -= time * BaseMoveSpeed / currentTile.MoveCost;
			pressed = true;
		}
		if (Input.GetKey (left)) {
			toMove.x -= time * BaseMoveSpeed / currentTile.MoveCost;
			pressed = true;
		}
		if (Input.GetKey (right)) {
			toMove.x += time * BaseMoveSpeed / currentTile.MoveCost;
			pressed = true;
		}

		if(pressed){
			tryMove = position + toMove;
			tryTile[0] = MapController.Instance.GetTileAtWorldPos (tryMove);
			tryTile[1] = MapController.Instance.GetTileAtWorldPos (tryMove.x, tryMove.y + 1);
			tryTile[2] = MapController.Instance.GetTileAtWorldPos (tryMove.x + 1, tryMove.y);
			tryTile[3] = MapController.Instance.GetTileAtWorldPos (tryMove.x + 1, tryMove.y + 1);


			foreach (Tile t in tryTile) {
				if (t.CanMoveThrough == false) {
					return;
				}
			}

			Position = toMove;
		}
	}
}
