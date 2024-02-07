using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
	
//	private const float MAX_ZOOM_OUT = 4.92f;
	public const float MAX_ZOOM_OUT = 5.5f;
//	public const float MAX_ZOOM_IN = 3.72f;
	public const float MAX_ZOOM_IN = 4.5f;
	private const float INCREMENT = 0.75f;
	private const float TIME_LERP = 1f;

	private float camSize;

	public bool shouldZoomIn;
	public bool shouldZoomOut;

	private Vector3 defaultCenter;

	// Use this for initialization
	void Start () {
		shouldZoomIn = false;
		shouldZoomOut = false;
		defaultCenter = Camera.main.transform.position;
		camSize = Camera.main.orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (shouldZoomOut) {
			ZoomOut ();
		} else if (shouldZoomIn) {
			ZoomIn ();
		}

		camSize = Camera.main.orthographicSize;
	}

	public void ZoomIn() {
		if (camSize > MAX_ZOOM_IN) {
//			Camera.main.orthographicSize = Mathf.Lerp (camSize, camSize - INCREMENT, TIME_LERP * Time.deltaTime);
			Camera.main.orthographicSize = Mathf.MoveTowards (camSize, camSize - INCREMENT, TIME_LERP * Time.deltaTime);
		} else if (camSize < MAX_ZOOM_IN) {
			shouldZoomIn = false;
		}
	}

	public void ZoomOut() {
		if (camSize < MAX_ZOOM_OUT) {
//			Camera.main.orthographicSize = Mathf.Lerp (camSize, camSize + INCREMENT, TIME_LERP * Time.deltaTime);
			Camera.main.orthographicSize = Mathf.MoveTowards (camSize, camSize + INCREMENT, TIME_LERP * Time.deltaTime);

		} else if (camSize > MAX_ZOOM_OUT) {
			shouldZoomOut = false;
		}
	}

	public void ZoomTowards(Vector3 point) {
		Debug.Log ("Zoom towards: " + point);

		Camera.main.orthographicSize -= 3f;
		if (Camera.main.orthographicSize < MAX_ZOOM_IN)
			Camera.main.orthographicSize = MAX_ZOOM_IN;
		Camera.main.gameObject.transform.position = new Vector3(point.x, point.y, defaultCenter.z);
	}

//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.CompareTag ("Player")) {
//			shouldZoomIn = true;
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D other) {
//		if (other.gameObject.CompareTag ("Player")) {
//			shouldZoomOut = true;
//		}
//	}
}
