using System;
using System.Collections.Generic;
using UnityEngine;

public class Vampire {

	float size = 0.9f;

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

			//change currentTile

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
		
	float toMove;
	Vector3 movement;
	bool pressed;

	//This might be able to be broken if you travel fast enough
	void UpdateMovement(float time){
		pressed = false;
		toMove = time * BaseMoveSpeed / currentTile.MoveCost;
		movement = Vector3.zero;

		if (Input.GetKey (up)) {
			pressed = true;
			if (((toMove + (position.y - (int)position.y) + size) > 0)) {
				Tile t1 =  MapController.Instance.GetTileAtWorldPos (position.x, position.y + size);
				Tile t2 =  MapController.Instance.GetTileAtWorldPos (position.x + size, position.y + size);
				if (t1 != null && t1.CanMoveThrough && t2 != null && t2.CanMoveThrough) {
					movement.y += toMove;
				}
			} else {
				movement.y += toMove;
			}
		}


		if (Input.GetKey (down)) {
			pressed = true;
			if (((position.y - (int)position.y) - toMove) < 0) {
				Tile t1 =  MapController.Instance.GetTileAtWorldPos (position.x, position.y - size);
				Tile t2 =  MapController.Instance.GetTileAtWorldPos (position.x + size, position.y - size);
				if (t1 != null && t1.CanMoveThrough && t2 != null && t2.CanMoveThrough) {
					movement.y -= toMove;
				}
			} else {
				movement.y -= toMove;
			}
		}


		if (Input.GetKey (right)) {
			pressed = true;
			if ((toMove + (position.x - (int)position.x) + size) > 0) {
				Tile t1 =  MapController.Instance.GetTileAtWorldPos (position.x + size, position.y );
				Tile t2 =  MapController.Instance.GetTileAtWorldPos (position.x + size, position.y + size);
				if (t1 != null && t1.CanMoveThrough && t2 != null && t2.CanMoveThrough) {
					movement.x += toMove;
				}
			} else {
				movement.x += toMove;
			}
		}


		if (Input.GetKey (left)) {
			pressed = true;
			if (((position.x - (int)position.x) - toMove) < 0) {
				Tile t1 =  MapController.Instance.GetTileAtWorldPos (position.x - size, position.y);
				Tile t2 =  MapController.Instance.GetTileAtWorldPos (position.x - size, position.y + size);
				if (t1 != null && t1.CanMoveThrough && t2 != null && t2.CanMoveThrough) {
					movement.x -= toMove;
				}
			} else {
				movement.x -= toMove;
			}
		}

		if (pressed) {
			Position = movement;
		}

	}
}
