using System;
using System.Collections.Generic;
using UnityEngine;

public class Job  {

	Tile tile;

	Action<Tile> onCompleteJob;

	//float originalJobTime ???
	float jobTime;

	public Tile Tile {
		get {
			return tile;
		}
	}

	public Action<Tile> OnCompleteJob {
		get {
			return onCompleteJob;
		}
	}

	public float JobTime {
		get {
			return jobTime;
		}
	}

	public Job(Tile tile, Action<Tile> job, float jobTime = 2f){
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
