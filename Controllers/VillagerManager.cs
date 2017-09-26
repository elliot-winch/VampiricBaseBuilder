using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour {


	List<Villager> villagers;
	//do we need to link villager to it graphical representation in a dictionary?

	void Start(){
		villagers = new List<Villager> ();

		//FIXME: testing only

		Villager v = new Villager (MapController.Instance.Map.GetTileAt (0, 0), "Steve", 10f);
		Villager v1 = new Villager (MapController.Instance.Map.GetTileAt (1, 1), "Rachel", 20f);

		villagers.Add (v);
		villagers.Add (v1);


		foreach (Villager vil in villagers) {
			vil.Start();
			CreateVillagerObject(vil);
		}
	}

	void Update(){
		foreach (Villager vil in villagers) {
			vil.Update (Time.deltaTime);
		}
	}

	GameObject CreateVillagerObject(Villager v){

		GameObject vil_go = new GameObject ();
		vil_go.name = v.Info.Name;
		vil_go.transform.position = v.CurrentTile.GetPosition ();

		vil_go.transform.SetParent (this.transform, true);

		SpriteRenderer vil_go_sr = vil_go.AddComponent<SpriteRenderer> ();
		vil_go_sr.sprite = Resources.LoadAll<Sprite>("Sprites/CitizenSheet")[0];
		vil_go_sr.sortingLayerName = "Creatures";

		v.RegisterPositionChangedCallback ( (villager) => { ChangePosition(villager, vil_go); } );

		return vil_go;
	}

	void ChangePosition(Villager v, GameObject vil_go){
		if (v.HasPath) {
			vil_go.transform.position = v.Position;
		}
	}
}
