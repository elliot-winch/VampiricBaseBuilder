using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour, IUpdateableWithTime {

	static VillagerManager _instance;

	public static VillagerManager Instance {
		get {
			return _instance;
		}
	}

	Dictionary<Villager, GameObject> villagers;

	public Villager[] Villagers {
		get {
			Villager[] vils = new Villager[villagers.Count];
			villagers.Keys.CopyTo(vils, 0); 
			return vils;
		}
	}

	public Dictionary<Villager, GameObject> VillagersWithObjects{
		get { return villagers; }
	}


	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be more than one VillagerManager");
		}

		_instance = this;

		villagers = new Dictionary<Villager, GameObject>();

		//FIXME: testing only


		for(int i = 0; i < 2; i++) {
			Villager vil = new Villager (MapController.Instance.Map.GetTileAt (i, 2*i), "Steve_" + i, 10f);

			vil.Start();
			CreateVillagerObject(vil);

			vil.Inventory.RegisterOnCarryingChangedCallback (
				() => {
					if(vil.Equals(UIControllerBuildMode.Instance.UIActiveVillager)){
						UIControllerBuildMode.Instance.UpdateAllUI(vil);
					}
			});


		}
	}

	public void UpdateWithTime(float time){
		foreach (Villager vil in this.Villagers) {
			vil.Update (time);
		}
	}

	//For IUpdateableWithTime inteface
	public bool IsActive(){
		return this.enabled;
	}

	GameObject CreateVillagerObject(Villager v){

		GameObject vil_go = new GameObject ();
		vil_go.name = v.Info.Name;
		vil_go.transform.position = v.CurrentTile.GetPosition ();

		vil_go.transform.SetParent (this.transform, true);

		SpriteRenderer vil_go_sr = vil_go.AddComponent<SpriteRenderer> ();
		vil_go_sr.sprite = Resources.LoadAll<Sprite>("Sprites/spriteSheet1")[17];
		vil_go_sr.sortingLayerName = "Creatures";
		vil_go_sr.material = Resources.Load<Material> ("Materials/Lit2DMatOutline");

		vil_go.AddComponent<SpriteOutline> ().color = Color.red;
		vil_go.GetComponent<SpriteOutline> ().enabled = false;

		villagers.Add (v, vil_go);

		v.RegisterPositionChangedCallback ( (villager) => { ChangePosition(villager, vil_go); } );

		return vil_go;
	}

	void ChangePosition(Villager v, GameObject vil_go){
		vil_go.transform.position = v.Position;
	}
}
