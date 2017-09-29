using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour {

	static TimeController _instance;

	public static TimeController Instance {
		get {
			return _instance;
		}
	}

	//Updating controllers
	float timeScaler;
	List<IUpdateableWithTime> managersToUpdate;

	//Time of day managment
	float secondsSinceSunChange;
	bool night;

	public enum Month
	{
		//FIXME: Change to real names
		ONE,
		TWO,
		THREE,
		FOUR,
		FIVE,
		SIX
	}

	int day;
	Month month;
	int year;

	public float secondsInDay; //The length of a day or night
	public int daysInMonth;
	public float lengthOfSunChange; //In frames

	[Range(0f,1f)]
	public float minAmbientIntensity;
	[Range(0f,1f)]
	public float maxAmbientIntensity;

	//Time aspects of AI
	Action onSunset;
	Action onSunrise;

	void Start(){
		if (_instance != null) {
			Debug.LogError ("There should not be more than one TimeController");
		}

		timeScaler = 1f;

		_instance = this;

		//Updating controllers
		managersToUpdate = new List<IUpdateableWithTime> ();
		managersToUpdate.Add(VampireController.Instance);
		managersToUpdate.Add(VillagerManager.Instance);

		//Time of Day
		year = 1776; //FIXME
		month = Month.ONE;
		day = 1;
		secondsSinceSunChange = 0f;
		night = false;

		RegisterOnSunset (SetTheSun);
		RegisterOnSunset (ToggleNight);
		RegisterOnSunset (ResetCounter);
		RegisterOnSunrise (RiseTheSun);
		RegisterOnSunrise(ToggleNight);
		RegisterOnSunrise (ResetCounter);
	}

	/*
	 * This Update is the Main game update. That is, it calls all update functions that 
	 * require a time argument.
	 */ 
	void Update(){
		float timePast = Time.deltaTime * timeScaler;

		HandleInput ();
		UpdateControllers (timePast);
		UpdateTimeOfDay (timePast);
	}

	public void UpdateControllers(float time){
		foreach (IUpdateableWithTime c in managersToUpdate) {
			if (c.IsActive()) {
				c.UpdateWithTime (time);
			}
		}
	}

	void HandleInput(){
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			timeScaler = 1f;
		} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
			timeScaler = 2f;
		} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
			timeScaler = 3f;
		} else if (Input.GetKeyDown (KeyCode.Space)) {
			if (timeScaler == 0f) {
				timeScaler = 1f;
			} else {
				timeScaler = 0f;
			}
		} 
	}

	void ChangePace(int pace){
		if (pace >= 0 && pace <= 3) {
			timeScaler = pace;
		}
	}

	void UpdateTimeOfDay (float time){
		secondsSinceSunChange += time;

		if(secondsSinceSunChange > secondsInDay) {
			if (night) {
				onSunrise ();

				day++;

				if (day > daysInMonth) {
					day = 1;

					if (month.Equals (Month.SIX)) {
						year++;
						month = Month.ONE;
					} else {
						month++;
					}
				}
			} else {
				onSunset ();
			}
		}
	}

	public void RegisterOnSunrise(Action callback){
		onSunrise += callback;
	}

	public void UnregisterOnSunrise(Action callback){
		onSunrise -= callback;
	}

	public void RegisterOnSunset(Action callback){
		onSunset += callback;
	}

	public void UnregisterOnSunset(Action callback){
		onSunset -= callback;
	}

	void ToggleNight(){
		night = !night;
	}

	void ResetCounter(){
		secondsSinceSunChange = 0f;
	}

	void SetTheSun(){
		StartCoroutine ("ChangeLight", minAmbientIntensity);
	}

	void RiseTheSun(){
		StartCoroutine ("ChangeLight", maxAmbientIntensity);
	}
		
	//Pos should be -1 or 1
	IEnumerator ChangeLight(float targetLightLevel){
		Vector4 newColor = RenderSettings.ambientLight;
		float timeRemaining = lengthOfSunChange;
		float ambientDistance = targetLightLevel - newColor.x;
		float timePast;
		float percentageChange;

		while(timeRemaining > 0){
			timePast = Time.deltaTime * timeScaler;
			percentageChange = timePast / timeRemaining;

			//Debug.Log (RenderSettings.ambientLight.r + " " + timeRemaining);
			newColor.x += percentageChange * ambientDistance;
			newColor.y += percentageChange * ambientDistance;
			newColor.z += percentageChange * ambientDistance;
			RenderSettings.ambientLight = newColor;

			ambientDistance -= percentageChange * ambientDistance;
			timeRemaining -= timePast;
			yield return 1f;
		}
	}
}


public interface IUpdateableWithTime {

	bool IsActive ();
	void UpdateWithTime(float time);
}

