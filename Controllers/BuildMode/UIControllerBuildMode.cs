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

	Villager v;
	GameObject villagerPanel;

	GameObject villagerInfoPanel;
	Text[] villagerInfoTextFields;
	GameObject villagerInvPanel;
	Text villagerInvText; //Might become array

	GameObject resourceInfoPanel;
	Text[] resourceTextFields;
	string[] defaultResourceStrings;

	void Start(){
		if (_instance != null) {
			Debug.LogError ("Should not be more than one UIControllerBuildMode");
		}

		_instance = this;

		canvas = transform.GetChild (0).GetComponent<Canvas> ();

		villagerPanel = canvas.transform.GetChild(8).gameObject;

		villagerInfoPanel = villagerPanel.transform.GetChild (2).gameObject;
		villagerInvPanel = villagerPanel.transform.GetChild (3).gameObject;

		//Info
		villagerInfoPanel.GetComponent<Display> ().RegisterOnDisplay (
			() =>
			{
				UpdateInfoUI();
			}
		);

		villagerInfoTextFields = new Text[villagerInfoPanel.transform.childCount];

		for (int i = 0; i < villagerInfoPanel.transform.childCount; i++) {
			villagerInfoTextFields[i] = villagerInfoPanel.transform.GetChild(i).GetComponent<Text> ();
		}

		//Inv
		villagerInvText = villagerInvPanel.transform.GetChild(0).GetComponent<Text>();

		villagerInvPanel.GetComponent<Display> ().RegisterOnDisplay (
			() =>
			{
				UpdateInvUI();
			}
		);
			
		villagerPanel.SetActive (false);

		//////

		resourceInfoPanel = canvas.transform.GetChild(0).gameObject;

		resourceTextFields = new Text[resourceInfoPanel.transform.childCount];

		for (int i = 0; i < resourceInfoPanel.transform.childCount; i++) {
			resourceTextFields[i] = resourceInfoPanel.transform.GetChild(i).GetComponent<Text> ();
		}

		resourceInfoPanel.SetActive (false);

		//FIXME Read from file: Or better still use images not text
		defaultResourceStrings = new string[resourceInfoPanel.transform.childCount];

		defaultResourceStrings[0] = "Mud: ";
		defaultResourceStrings[1] = "Wood: ";
		defaultResourceStrings[2] = "Stone: ";
		defaultResourceStrings[3] = "Bricks: ";
		defaultResourceStrings[4] = "Wheat: ";

	}

	public void OpenCanvas(){
		canvas.gameObject.SetActive (true);
	}

	public void CloseCanvas(){
		canvas.gameObject.SetActive (false);
	}

	public void OpenVillagerPanel(Villager v){
		this.v = v;
		villagerPanel.SetActive (true);
	}

	public void CloseVillagerPanel(){
		villagerPanel.SetActive (false);
	}

	public void EditResourceValue(int index, int value){
		resourceTextFields [index].text = defaultResourceStrings [index] + value.ToString();
	}

	public void UpdateAllUI(){
		UpdateInvUI ();
		UpdateInfoUI ();
	}

	public void UpdateInvUI(){
		if (v != null) {
			if (v.Inventory.Carrying != null) {
				villagerInvText.text = v.Inventory.Carrying.Name;
			} else {
				villagerInvText.text = "Empty";
			}
		}
	}

	public void UpdateInfoUI(){
		if(v != null){
			villagerInfoTextFields [0].text = "Name: " + v.Info.Name;
			villagerInfoTextFields [1].text = "Age: " + v.Info.Age;
		}
	}
}
