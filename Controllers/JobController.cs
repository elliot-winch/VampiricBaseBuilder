using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobController : MonoBehaviour {

	static JobController _instance;

	PriorityQueue<float, Job> jobQueue;

	public static JobController Instance {
		get {
			return _instance;
		}
	}

	public PriorityQueue<float, Job> JobQueue {
		get {
			return jobQueue;
		}
	}
		
	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be two job controllers");
		}
		_instance = this;

		jobQueue = new PriorityQueue<float, Job> ();

		JobList.Init ();
	}

	public void AddJob(float priority, Job j){
		jobQueue.Enqueue (priority, j);
	}
}
