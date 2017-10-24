using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : IJobPassable {

	static float RecalcPathCost = 100f;

	VillagerInfo v_info;

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
	PriorityQueue<float, Job> jobs;
	public Job CurrentJob;
	public bool AbleToWork; //Acts more like able to receive a new job

	float JobWaitTime;
	Tile adjacentJobTile;

	VillagerInventory inv;
	//FIXME put jobs into a signle class

	public float X {
		get {
			if (nextTile == null) {
				return currentTile.X;
			} else {
				return Mathf.Lerp (currentTile.X, nextTile.X, movePercentage);
			}
		}
	}

	public float Y {
		get { 
			if (nextTile == null) {
				return currentTile.Y;
			} else {
				return Mathf.Lerp (currentTile.Y, nextTile.Y, movePercentage); 
			}
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
			//Prev tile
			if (currentTile != null) {
				currentTile.OccupyingVillagers.Remove (this);
				if (currentTile.Installed != null && CurrentTile.Installed.Interaction != null) {
					currentTile.Installed.Interaction.PrevInteract (currentTile);
				}
			}
			//New current tile
			currentTile = value;
			currentTile.OccupyingVillagers.Add (this);

			if (currentTile.Installed != null && currentTile.Installed.Interaction != null) {
				currentTile.Installed.Interaction.Interact (currentTile);
			}

			if (onPositionChanged != null) {
				onPositionChanged (this);
			}
		}
	}

	public bool HasPath{
		get {
			return this.currentPath != null;
		}
	}

	public PriorityQueue<float, Job> Jobs {
		get {
			return jobs;
		}
	}

	public Villager(Tile currentTile, VillagerInfo info){
		this.CurrentTile = currentTile;
		this.v_info = info;
	}

	public Villager(Tile currentTile, string name, float age){
		this.CurrentTile = currentTile;
		this.v_info = new VillagerInfo (name, age);
	}

	public VillagerInfo Info {
		get {
			return v_info;
		}
	}

	public VillagerInventory Inventory {
		get {
			return inv;
		}
	}


	/// /////////

	public void Start(){
		inv = new VillagerInventory ();
		jobs = new PriorityQueue<float, Job> ();
		AbleToWork = true;
	}

	public void Update(float time){
		UpdateJob (time);

		UpdateMovement (time);
	}

	void UpdateMovement(float time){
		//I want to remove this but it is a temporary fix
		if (CurrentTile.CanMoveThrough == false) {
			this.CurrentTile = this.CurrentTile.NearestNeighbourTo ();
		}

		if (currentPath != null) {

			//Reached nextTile
			if (movePercentage >= 1) {
				movePercentage = 0;
				CurrentTile = nextTile;

				try {
					nextTile = currentPath.GetNextTile ();
				} catch (NullReferenceException e){
					Debug.Log (e.Message);
				}

				//Reached end of path case
				if (nextTile == null) {
					destTile = null;
					currentPath = null;

					if (CurrentJob != null && CurrentJob.OnStartJob != null) {
						CurrentJob.OnStartJob (CurrentJob.Tile, this);
					}
				}
					
				//If path has become impassable
				else if (nextTile.CanMoveThrough == false) {
					SetDest (destTile);
				} else {
					distCurrNext = MyMath.Distance (currentTile.X, nextTile.X, currentTile.Y, nextTile.Y);
				}
			}

			movePercentage += (speed * time) / (distCurrNext * CurrentTile.MoveCost);

			onPositionChanged (this);

		}
	}
		
	public void SetDest(Tile dest){
		if (dest == null) {
			Debug.Log ("Trying to set dest to null.");
			//It's probably nearest neighbour
		}
			
		currentPath = new Path (MapController.Instance.Map, CurrentTile, dest, true);

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
			if (!this.jobs.IsEmpty) { 
				this.CurrentJob = jobs.DequeueValue();
			} else {
				//IDLE as there are no jobs
				this.CurrentJob = new Job (this.CurrentTile, JobList.JobFunctions[(int)JobList.Jobs.Idle], 5f);
				this.adjacentJobTile = this.CurrentTile;
			}
		} else {
			//If standing on the right tile, do the job
			if (CurrentTile.Equals(this.adjacentJobTile)) {
				//Job is performed regardless of outcome here
				if (this.CurrentJob.PerformJob (time)) {
					this.CurrentJob.OnCompleteJob (this.CurrentJob.Tile, this);
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

	public void AddJob(float priority, Job j){
		if (AbleToWork && j != null) {
			jobs.Enqueue (priority /*  time a multipler*/, j);
		}
	}

	public void CancelJob(Job j){
		Debug.Log ("Cancelling job");
		if(j.Equals(CurrentJob)){
			this.CurrentJob = null;
			this.adjacentJobTile = null;
		} else if(jobs.Contains(j)){
			jobs.Cancel (j);
		}
	}
}
