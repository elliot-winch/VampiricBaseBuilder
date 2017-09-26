using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerBuildMode : MonoBehaviour {

	public KeyCode cameraUp = KeyCode.W;
	public KeyCode cameraLeft = KeyCode.A;
	public KeyCode cameraRight = KeyCode.D;
	public KeyCode cameraDown = KeyCode.S;
	public float cameraMoveDrag = 0.1f;
	public float cameraMoveButtons = 0.3f;
	public float cameraScroll = 1f;
	public float cameraMinZoom = 3f;
	public float cameraMaxZoom = 25f;

	MouseManagerBuildMode mm;

	void Start(){
		mm = MouseManagerBuildMode.Instance;
	}

	void Update(){
		UpdateCamera ();
	}


	void UpdateCamera(){
		Zoom ();

		if (!mm.DragCamera(cameraMoveDrag)) {
			MoveCameraWithButtons ();
		} 
	}

	void MoveCameraWithButtons(){
		Camera.main.transform.Translate (MoveCameraLeftRight (), MoveCameraUpDown (), 0f);
	}

	float MoveCameraLeftRight(){
		if (Input.GetKey (cameraRight) && Input.GetKey (cameraLeft)) {
			return 0f;
		} else if (Input.GetKey (cameraLeft)) {
			return -cameraMoveButtons * Camera.main.orthographicSize;
		} else if (Input.GetKey (cameraRight)) {
			return cameraMoveButtons * Camera.main.orthographicSize;
		} else {
			return 0f;
		}
	}

	float MoveCameraUpDown(){
		if (Input.GetKey (cameraUp) && Input.GetKey (cameraDown)) {
			return 0f;
		} else if (Input.GetKey (cameraDown)) {
			return -cameraMoveButtons * Camera.main.orthographicSize;
		} else if (Input.GetKey (cameraUp)) {
			return cameraMoveButtons * Camera.main.orthographicSize;
		} else {
			return 0f;
		}
	}

	void Zoom(){

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis ("Mouse ScrollWheel") * cameraScroll;

		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, cameraMinZoom, cameraMaxZoom);
	}
}
