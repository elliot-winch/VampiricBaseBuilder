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
	public GameObject villagerPanel;

	public GameObject villagerInfoPanel;
	Text[] villagerInfoTextFields;
	public GameObject villagerInvPanel;
	Text villagerInvText; //Might become array
	public GameObject villagerJobPanel;
	Text[] villagerJobInfoFields;

	public GameObject resourceInfoPanel;
	Text[] resourceTextFields;
	string[] defaultResourceStrings;

	public Villager UIActiveVillager {
		get {
			return v;
		}
	}

	void Start(){
		if (_instance != null) {
			Debug.LogError ("Should not be more than one UIControllerBuildMode");
		}

		_instance = this;

		canvas = transform.GetChild (0).GetComponent<Canvas> ();

		villagerPanel.SetActive (false);

		villagerInfoTextFields = new Text[villagerInfoPanel.transform.childCount];

		for (int i = 0; i < villagerInfoPanel.transform.childCount; i++) {
			villagerInfoTextFields[i] = villagerInfoPanel.transform.GetChild(i).GetComponent<Text> ();
		}

		//Inv
		villagerInvText = villagerInvPanel.transform.GetChild(0).GetComponent<Text>();

		//Jobs
		villagerJobInfoFields = new Text[villagerJobPanel.transform.childCount];

		for (int i = 0; i < villagerJobPanel.transform.childCount; i++) {
			villagerJobInfoFields[i] = villagerJobPanel.transform.GetChild(i).GetComponent<Text> ();
		}

		//Resource panel
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
		if (v != null) {
			this.v = v;
			villagerPanel.SetActive (true);
			UpdateAllUI (v);

			VillagerManager.Instance.VillagersWithObjects [v].GetComponent<SpriteOutline> ().enabled = true;
		}
	}

	public void CloseVillagerPanel(){
		villagerPanel.SetActive (false);

		if (v != null) {
			VillagerManager.Instance.VillagersWithObjects [v].GetComponent<SpriteOutline> ().enabled = false;

			this.v = null;
		}
	}

	public void EditResourceValue(int index, int value){
		resourceTextFields [index].text = defaultResourceStrings [index] + value.ToString();
	}

	public void Drop(){
		if (v.CurrentTile.Loose == null) {
			this.v.Inventory.Drop (v.CurrentTile);
		}
	}

	public void UpdateAllUI(Villager v){
		this.v = v;
		UpdateInvUI ();
		UpdateInfoUI ();
		UpdateJobUI ();
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

	public void UpdateJobUI(){
		if (v != null) {
			villagerJobInfoFields [0].text = v.CurrentJob /*FIXME Job names*/ + " at " + v.CurrentJob.Tile.GetPosition ();
		}
	}

	/// ////////////

	public void EmptyInvIntoStockpile(){
		if (v != null) {
			Tile t = InventoryManager.Instance.ClosestAvailableTile (v.CurrentTile);
			Debug.Log (t);
			if (t != null) {
				v.AddJob (1f, new Job (t, JobList.JobFunctions [(int)JobList.Jobs.PlaceLoose], 0.1f));
			}
		}
	}
}
