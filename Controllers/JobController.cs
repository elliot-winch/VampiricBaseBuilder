using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobController : MonoBehaviour {

	static JobController _instance;

	public static JobController Instance {
		get {
			return _instance;
		}
	}

	Dictionary<Tile, Job> currentJobs;
		
	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be two job controllers");
		}
		_instance = this;

		currentJobs = new Dictionary<Tile, Job> ();

		LooseObjectFactory.CreateLooseObject (0, MapController.Instance.GetTileAtWorldPos (15, 15));

	}

	public void AddJob(float priority, Job j, Villager v){
		if(currentJobs.ContainsKey(j.Tile)){
			Debug.Log ("Adding job where one already exists");
			return;
		}

		v.AddJob (priority, j);
		currentJobs.Add (j.Tile, j);
		j.VillagerAssociated = v;
	}

	public void AddJob(float priority, Job j){
		if(currentJobs.ContainsKey(j.Tile)){
			Debug.Log ("Adding job where one already exists");
			return;
		}

		Villager[] vils = VillagerManager.Instance.Villagers;

		//Scheduling jobs
		int leastJobs = int.MaxValue;
		Villager curLeast = null;
		foreach (Villager v in vils) {
			if (v.AbleToWork /*&& is the right type of worker for this job's type*/) {
				if (v.Jobs.Count < leastJobs) {
					leastJobs = v.Jobs.Count;
					curLeast = v;
				}
			}
		}

		if (curLeast != null) {
			AddJob (priority, j, curLeast);
		} else {
			Debug.Log ("No villager can complete the job at: " + j.Tile.GetPosition());
		}
	}

	public void CancelJob(Tile t){
		Job j;
		currentJobs.TryGetValue (t, out j);
		Debug.Log (t.GetPosition () + "  Remove Job");

		if (j != null) {

			if (j.VillagerAssociated != null) {
				j.VillagerAssociated.CancelJob (j);
			}

			currentJobs.Remove (t);
		}
	}

	public void RemoveJob(Tile t){
		Job j;
		currentJobs.TryGetValue (t, out j);
		if (j != null) {
			currentJobs.Remove (t);
		}
	}

}
