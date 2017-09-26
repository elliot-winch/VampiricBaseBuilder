using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerBuildMode : MonoBehaviour {

	static UIControllerBuildMode _instance;

	public static UIControllerBuildMode Instance {
		get {
			return _instance;
		}
	}

	Canvas canvas;

	GameObject villagerInfoPanel;
	Text[] textFields;

	void Start(){
		if (_instance != null) {
			Debug.LogError ("Should not be more than one UIControllerBuildMode");
		}

		_instance = this;

		canvas = transform.GetChild (0).GetComponent<Canvas> ();

		villagerInfoPanel = canvas.transform.GetChild(4).gameObject;

		textFields = new Text[villagerInfoPanel.transform.childCount];
		Debug.Log (villagerInfoPanel.transform.childCount);

		for (int i = 0; i < villagerInfoPanel.transform.childCount; i++) {
			textFields[i] = villagerInfoPanel.transform.GetChild(i).GetComponent<Text> ();
		}


		villagerInfoPanel.SetActive (false);
	}

	public void OpenCanvas(){
		canvas.gameObject.SetActive (true);
	}

	public void CloseCanvas(){
		canvas.gameObject.SetActive (false);
	}

	public void OpenVillagerPanel(VillagerInfo v_info){
		villagerInfoPanel.SetActive (true);

		Debug.Log(textFields [1] +  "Name: " + v_info.Name);

		//Bc the title is at 0
		textFields [1].text = "Name: " + v_info.Name;
		textFields [2].text = "Age: " + v_info.Age; 
	}

	public void CloseVillagerPanel(){
		villagerInfoPanel.SetActive (false);
	}
}
