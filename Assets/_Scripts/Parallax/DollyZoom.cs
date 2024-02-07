using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyZoom : MonoBehaviour {
	[SerializeField] GameObject targetGameObject;
	public float zoomSpeed = 1;
	public float targetOrtho;
	public float smoothSpeed = 2.0f;
	public float minOrtho = 1.0f;
	public float maxOrtho = 20.0f;

	void Start() {
		targetOrtho = Camera.main.orthographicSize;
//		targetGameObject.transform.position = Camera.main.ScreenToWorldPoint (Vector3 (Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
			
	}



	void Update () {
//		float scroll = Input.GetAxis ("Mouse ScrollWheel");
//		if (scroll != 0.0f) {
			targetOrtho -= zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
//		}

		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize,
			targetOrtho, smoothSpeed * Time.deltaTime);



//		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, zoomAmount, lerpPercent);
	}
}
