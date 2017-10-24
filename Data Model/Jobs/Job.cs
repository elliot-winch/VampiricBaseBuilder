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

	Action<Tile, Villager> onStartJob;
	Action<Tile, Villager> onCompleteJob;

	float jobTime;

	public Tile Tile {
		get {
			return tile;
		}
	}

	public bool Active { get; set; }
	bool started;
	public bool Started { get { return started; } }

	public Villager VillagerAssociated { get; set; } 

	public Action<Tile, Villager> OnStartJob {
		get {
			return onStartJob;
		}
	}

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

	public Job(Tile tile, Action<Tile, Villager> start, Action<Tile, Villager> end, float jobTime = 2f){
		this.Active = true;
		this.tile = tile;
		this.onStartJob = start;
		this.onCompleteJob = end;
		this.jobTime = jobTime;
	}

	public Job(Tile tile, Action<Tile, Villager> end, float jobTime = 2f) : this(tile, null, end, jobTime){}

	//Return true when complete
	public bool PerformJob(float deltaTime){
		
		if ((jobTime -= deltaTime) <= 0) {
			return true;
		}

		return false;
	}
}
