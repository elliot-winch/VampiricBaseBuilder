﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public float buttonWidth;
	public float buttonHeight;
	public float scalingFactor;
	public Vector3 offset;

	static UIController _instance;

	public GameObject jobListPanelPrefab;
	public GameObject buttonPrefab;

	public Text tileInfo;

	GameObject jobListPanel;

	public static UIController Instance {
		get {
			return _instance;
		}
	}

	void Start(){
		if (_instance != null) {
			Debug.LogError ("Should not be more than one UIController");
		}

		_instance = this;

		buttonWidth *= scalingFactor;
		buttonHeight *= scalingFactor;
	}

	public void DisplayJobPanel(Tile t){
		CloseJobPanel ();

		jobListPanel = Instantiate (jobListPanelPrefab, transform.GetChild(2));

		RectTransform rt = jobListPanel.GetComponent<RectTransform> ();
		rt.position = t.GetPosition () + offset;
		//Position is bottom right corner of tile

		List<PossibleJob> tilePosJobs = null;

		//More extensive checks are done before calling tis function
		if (t.Loose != null && t.Loose.PossibleJobs != null) {
			tilePosJobs = t.Loose.PossibleJobs.ListJobs;
		} else if (t.Installed != null && t.Installed.PossibleJobs != null){
			tilePosJobs = t.Installed.PossibleJobs.ListJobs;
		}

		if (tilePosJobs == null) {
			return;
		} else {

			int activeCount = 0;
			for (int counter = 0; counter < tilePosJobs.Count; counter++) {
				if (tilePosJobs [counter].active) {

					GameObject button = Instantiate (buttonPrefab, jobListPanel.transform);

					button.GetComponentInChildren<Text> ().text = ((JobList.Jobs)tilePosJobs [counter].possibleJobID).ToString ();
					int value = tilePosJobs [counter].possibleJobID;

					button.GetComponent<Button> ().onClick.AddListener (
						() => {				
							JobController.Instance.AddJob (Time.realtimeSinceStartup, new Job (t, JobList.JobFunctions [value]));
							CloseJobPanel ();
						}
					);


					activeCount++;
				}
			}

			rt.sizeDelta = new Vector2 (buttonWidth, buttonHeight * activeCount);
			jobListPanel.SetActive (true);
		}
	}

	public void CloseJobPanel(){
		if (jobListPanel != null) {
			Destroy (jobListPanel);
		}
	}

	void Update(){
		Tile t = MapController.Instance.GetTileAtWorldPos (Camera.main.ScreenToWorldPoint(Input.mousePosition));

		if (t != null) {
			tileInfo.text = t.inside.ToString ();
		}
	}
}
