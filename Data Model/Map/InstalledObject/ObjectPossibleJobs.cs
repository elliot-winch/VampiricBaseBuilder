using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPossibleJobs  {

	//These are things villagers/the vampire can do to installed objects
	List<PossibleJob> possibleJobs;

	public List<PossibleJob> ListJobs {
		get {
			return possibleJobs;
		}
	}

	public ObjectPossibleJobs(){
		possibleJobs = new List<PossibleJob> ();
	}

	public ObjectPossibleJobs(List<PossibleJob> list){
		possibleJobs = list;
	}

	public void AddPossibleJob(int jobID, bool t){
		possibleJobs.Add(new PossibleJob(jobID, t));
	}

	public void SetPossibleJobActive(int jobID, bool b){
		possibleJobs [jobID].active = b;
	}
}
	
public class PossibleJob
{
	public int possibleJobID;
	public bool active;

	public PossibleJob(int i, bool active){
		this.possibleJobID = i;
		this.active = active;
	}
}
