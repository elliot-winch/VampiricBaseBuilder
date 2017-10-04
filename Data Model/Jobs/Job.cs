using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * This class represents an actual, realised job that exists on a Tile and in the job queue.
 * 
 * 
 */

public class Job {

	Tile tile;

	Action<Tile, Villager> onCompleteJob;

	//float originalJobTime ???
	float jobTime;

	public Tile Tile {
		get {
			return tile;
		}
	}

	public bool Active { get; set; }

	public Action<Tile, Villager> OnCompleteJob {
		get {
			return onCompleteJob;
		}
	}

	public float JobTime {
		get {
			return jobTime;
		}
	}

	public Job(Tile tile, Action<Tile, Villager> job, float jobTime = 2f){
		this.Active = true;
		this.tile = tile;
		this.onCompleteJob = job;
		this.jobTime = jobTime;
	}

	//Return true when complete
	public bool PerformJob(float deltaTime){
		if ((jobTime -= deltaTime) <= 0) {
			return true;
		}

		return false;
	}
}
