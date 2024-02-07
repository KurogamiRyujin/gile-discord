using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour {

	GameObject camera;

	// Use this for initialization
	void Start () {
//		camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
//			camera.GetComponent<CameraZoom>().shouldZoomOut = false;
//			camera.GetComponent<CameraZoom>().shouldZoomIn = true;
//		}
	}

	void OnTriggerExit2D(Collider2D other) {
//		if (other.gameObject.GetComponent<PlayerYuni>() != null) {
//			camera.GetComponent<CameraZoom>().shouldZoomIn = false;
//			camera.GetComponent<CameraZoom>().shouldZoomOut = true;
//		}
	}
}
