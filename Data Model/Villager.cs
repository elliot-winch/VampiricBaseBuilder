using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager {

	static float RecalcPathCost = 100f;

	public string name = "Bob";

	//Movement
	float speed = 2f;
	float distCurrNext;
	float movePercentage = 0f;
	Tile currentTile;
	Tile nextTile;
	Tile destTile;
	Path currentPath;
	Action<Villager> onPositionChanged;

	//Jobs
	public Job CurrentJob;
	float JobWaitTime;
	Tile adjacentJobTile;

	public float X {
		get {
			return Mathf.Lerp (currentTile.X, nextTile.X, movePercentage);
		}
	}

	public float Y {
		get { 
			return Mathf.Lerp (currentTile.Y, nextTile.Y, movePercentage); 
		}
	}

	public Vector3 Position{
		get { return new Vector3 (X, Y, 0); }
	}

	public Tile CurrentTile {
		get {
			return currentTile;
		} 
		set {
			if (value == null) {
				Debug.Log ("Trying to set villager currentTile to null");
			}

			if (currentTile != null) {
				currentTile.OccupyingVillager = null;
			}
			currentTile = value;
			currentTile.OccupyingVillager = this;
		}
	}

	public bool HasPath{
		get {
			return this.currentPath != null;
		}
	}

	public Villager(Tile currentTile){
		this.CurrentTile = currentTile;
	}

	public void Start(){
		RecalcPathCost = 100f;
	}

	public void Update(float time){
		//if (!(destTile != null && destTile.OccupyingVillager != null)) {
		UpdateJob (time);

		UpdateMovement (time);
		//}
	}

	//Movement
	void UpdateMovement(float time){

		if (currentPath != null) {

			//Reached nextTile
			if (movePercentage >= 1) {
				movePercentage = 0;
				this.CurrentTile = this.nextTile;

				nextTile = currentPath.GetNextTile ();

				//Reached end of path case
				if (nextTile == null) {
					destTile = null;
					currentPath = null;
				}
				//If path has become blocked
				else if (nextTile.MoveCost >= RecalcPathCost || nextTile.MoveCost == Mathf.Infinity) {
					SetDest (destTile);
				} else {
					distCurrNext = MyMath.Distance (currentTile.X, nextTile.X, currentTile.Y, nextTile.Y);
				}
			}

			movePercentage += (speed * time) / distCurrNext;

			onPositionChanged (this);
		}
	}
		
	public void SetDest(Tile dest){
		if (dest == null) {
			Debug.Log ("Trying to set dest to null.");
			//It's probably nearest neighbour
		}

		if (dest.MoveCost == Mathf.Infinity) {
			SetDest (dest.NearestNeighbourTo (dest.X, dest.Y));
		}

		currentPath = new Path (MapController.Instance.Map, CurrentTile, dest);

		if (currentPath.IsNextTile()) {
			nextTile = currentPath.GetNextTile ();
			destTile = dest;
		} else {
			nextTile = null;
			destTile = null;
			currentPath = null;
		}
	}

	public void SetDest(int x, int y){
		SetDest(MapController.Instance.Map.GetTileAt (x, y));
	}

	public void SetDest(Vector3 dest){
		SetDest((int)dest.x, (int)dest.y);
	}

	public void RegisterPositionChangedCallback(Action<Villager> callback){
		onPositionChanged += callback;
	}

	//Jobs
	void UpdateJob(float time){
		//If no job is assigned, look for a job
		if (this.CurrentJob == null) {
			if (!JobController.Instance.JobQueue.IsEmpty) { //FIXME: loop through all queues in priortiy order
				this.CurrentJob = JobController.Instance.JobQueue.DequeueValue ();
			}

		} else {
			
			//If standing on the right tile, do the job
			if (CurrentTile.Equals(this.adjacentJobTile)) {
				//Job is performed regardless of outcome here
				if (this.CurrentJob.PerformJob (time)) {
					this.CurrentJob.OnCompleteJob (this.CurrentJob.Tile);
					this.CurrentJob = null;
					this.adjacentJobTile = null;
				} 

			} else {
				
				//If not in range and not travelling, start to travel
				if (currentPath == null) {
					Tile dest = this.CurrentJob.Tile.NearestNeighbourTo (this.CurrentTile.X , this.CurrentTile.Y);
					SetDest (dest);
					this.adjacentJobTile = dest;
				}

				//Else the villager is travelling but not close enough yet
			}
		}
	}
}
