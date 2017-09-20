using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerVampireMode : MonoBehaviour {

	public float lerpTime = 2f;
	public float cameraScroll = 1f;
	public float cameraMinZoom = 3f;
	public float cameraMaxZoom = 6f;

	Vampire target;
	Vector3 offset;

	//Must execute after VampireController
	public void Begin(Vampire target){
		this.target = target;
		offset = new Vector3 (0.5f, 0.5f, -10f);
		target.AssignMoveCallback( (vampire) => { MoveCameraToTarget(vampire); } );

	}

	public void MoveToPlayer(){
		StartCoroutine ("LerpToTarget");

		StartCoroutine("LerpZoom");
	}

	void Update(){
		Zoom ();

	}

	IEnumerator LerpToTarget(){
		float startTime = Time.time;
		float timeSinceStarted;
		float percentageComplete;
		Vector3 startPos = Camera.main.transform.position;

		do {
			timeSinceStarted = Time.time - startTime;
			percentageComplete = timeSinceStarted / lerpTime;
			MoveCamera (Vector2.Lerp (startPos, target.Position, percentageComplete));
			yield return null;
		} while(MyMath.EqualsRoughly2D (Camera.main.transform.position, target.Position + offset) == false);


	}

	IEnumerator LerpZoom(){

		float startTime = Time.time;
		float timeSinceStarted;
		float percentageComplete;
		float startZoom = Camera.main.orthographicSize;

		while(Camera.main.orthographicSize > cameraMaxZoom){
			timeSinceStarted = Time.time - startTime;
			percentageComplete = (timeSinceStarted / lerpTime);
			Camera.main.orthographicSize = Mathf.Lerp (startZoom, cameraMaxZoom, percentageComplete);
			yield return null;
		}

		while(Camera.main.orthographicSize < cameraMinZoom){
			timeSinceStarted = Time.time - startTime;
			percentageComplete = timeSinceStarted / lerpTime;
			Camera.main.orthographicSize = Mathf.Lerp (startZoom, cameraMinZoom, percentageComplete);
			yield return null;
		}

	}

	void MoveCamera(Vector3 pos){
		Camera.main.transform.position = pos + offset;
	}
		
	void MoveCameraToTarget(Vampire /*FIXME*/ target){
		Camera.main.transform.position = target.Position + offset;
	}

	void Zoom(){

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis ("Mouse ScrollWheel") * cameraScroll;

		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, cameraMinZoom, cameraMaxZoom);
	}
}
