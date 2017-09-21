using System;
using System.Collections.Generic;
using UnityEngine;

public class InstalledObject {
	
	string name;
	int id;

	Sprite sprite;
	string sortingLayer;
	int sortingOrder;

	int width;
	int height;
	float moveCost;

	Tile baseTile;
	Func<Tile, bool> placementValidation;


	public string Name {
		get {
			return name;
		}
	}

	public int ID {
		get {
			return id;
		}
	}

	public Sprite Sprite {
		get {
			return sprite;
		}
	}

	public string SortingLayer {
		get {
			return sortingLayer;
		}
	}

	public int SortingOrder {
		get {
			return sortingOrder;
		}
	}

	public Tile BaseTile {
		get {
			return baseTile;
		}
	}

	public float MoveCost {
		get {
			return moveCost;
		}
	}

	public InstalledObject (int ID, string name, Sprite sprite, Func<Tile, bool> validation, int sortingOrder = 50, string sortingLayer = "InstalledObject", float moveCost=Mathf.Infinity, int width=1, int height=1){
		this.id = ID;
		this.name = name;

		this.sprite = sprite;
		this.sortingLayer = sortingLayer;
		this.sortingOrder = sortingOrder;

		this.placementValidation = validation;
		this.moveCost = moveCost;
		this.width = width;
		this.height = height;
	}

	public void PlaceObjectPlan(Tile t){
		if (t != null && placementValidation(t)) {
			this.baseTile = t;

			t.Planned = this;
		}
	}

	public void PlaceObjectRaw(Tile t){
		if (placementValidation(t)) {
			this.baseTile = t;

			t.MoveCost = this.moveCost;
			t.Installed = this;
		}
	}

	public bool StandardValidation(Tile t){
		return true;
	}
}
