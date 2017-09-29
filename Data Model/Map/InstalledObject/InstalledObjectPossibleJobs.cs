using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObjectPossibleJobs  {

	//These are things villagers/the vampire can do to installed objects
	List<PossibleJob> possibleJobs;

	public List<PossibleJob> ListJobs {
		get {
			return possibleJobs;
		}
	}

	public InstalledObjectPossibleJobs(){
		possibleJobs = new List<PossibleJob> ();
	}

	public void AddPossibleJob(int jobID, bool t){
		possibleJobs.Add(new PossibleJob(jobID, t));
	}

	public void SetPossibleJobActive(int jobID, bool b){
		//contains
		for(int i = 0; i < possibleJobs.Count; i++){
			if(possibleJobs[i].possibleJobID == jobID){
				possibleJobs[i].active = b;
				return;
			}
		}
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
