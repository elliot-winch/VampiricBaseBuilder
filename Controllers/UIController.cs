using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	static UIController _instance;

	public GameObject jobListPanel;

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

		jobListPanel = Instantiate (jobListPanel, transform.GetChild(2));
		CloseJobPanel ();
	}

	public void DisplayJobPanel(Tile t, List<PossibleJob> jobs){

		RectTransform rt = jobListPanel.GetComponent<RectTransform> ();
		rt.position = t.GetPosition (); 
		/*
		for (int i = 0; i < max; i++) {
			
		}
*/
		jobListPanel.SetActive (true);

	}

	public void CloseJobPanel(){
		jobListPanel.SetActive (false);
	}
}
