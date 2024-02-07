using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller to handle camera behaviour.
/// </summary>
public class CameraController_v2 : MonoBehaviour {
    /// <summary>
    /// Reference to the player avatar.
    /// </summary>
	private GameObject player;
    /// <summary>
    /// Reference to the target to be focused.
    /// </summary>
	private GameObject target;
    /// <summary>
    /// Reference to the camera zoom functionality.
    /// </summary>
	private CameraZoom cameraZoom;
    /// <summary>
    /// Maintained difference of the camera y and the player y
    /// </summary>
    public float yOffset;

    /// <summary>
    /// Unity Function. Called once when the MonoBehaviour's game object is instantiated.
    /// </summary>
	void Awake() {
		cameraZoom = GetComponent<CameraZoom> ();

		EventBroadcaster.Instance.AddObserver (EventNames.ZOOM_CAMERA_TOWARDS, ZoomTowards);
		EventBroadcaster.Instance.AddObserver (EventNames.SHOULD_CAMERA_ZOOM_IN, ShouldZoomIn);
		EventBroadcaster.Instance.AddObserver (EventNames.ENABLE_CAMERA, EnableCamera);
		EventBroadcaster.Instance.AddObserver (EventNames.DISABLE_CAMERA, DisableCamera);
	}

    /// <summary>
    /// Unity Function called when MonoBehaviour is destroyed.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ZOOM_CAMERA_TOWARDS);
		EventBroadcaster.Instance.RemoveObserver (EventNames.SHOULD_CAMERA_ZOOM_IN);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ENABLE_CAMERA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.DISABLE_CAMERA);
	}

    /// <summary>
    /// Standard Unity Function. Starts up the MonoBehaviour. Called once throughout its life.
    /// </summary>
	void Start () {
		Debug.Log ("Camera");
		SearchPlayer ();
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
	void Update () {
		if (target == null) {
			if (player == null)
				SearchPlayer ();
			target = player;
		}


		if(target != null) {
			transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + yOffset, transform.position.z);
		}
	}

    /// <summary>
    /// Function call to make the camera zoom towards a certain point in the scene.
    /// </summary>
    /// <param name="data">Target</param>
	private void ZoomTowards(Parameters data) {
		float x = data.GetFloatExtra ("x", 0.0f);
		float y = data.GetFloatExtra ("y", 0.0f);
		Vector2 target = new Vector2 (x, y);
		Debug.Log ("TARGET: "+target);

		cameraZoom.ZoomTowards (target);
	}

    /// <summary>
    /// Function call to toggle if the camera should zoom in or not.
    /// </summary>
    /// <param name="data">Flag</param>
	private void ShouldZoomIn(Parameters data) {
		bool flag = data.GetBoolExtra ("shouldZoomIn", false);
		if (flag) {
			cameraZoom.shouldZoomIn = true;
			cameraZoom.shouldZoomOut = false;
		} else {
			cameraZoom.shouldZoomIn = false;
			cameraZoom.shouldZoomOut = true;
		}
	}

    /// <summary>
    /// Target Focus setter.
    /// </summary>
    /// <param name="target">Target</param>
	public void SetTarget(GameObject target) {
		this.target = target;
	}

    /// <summary>
    /// Function call to find the player avatar.
    /// </summary>
	public void SearchPlayer() {
		this.player = GameObject.FindGameObjectWithTag ("Player");
	}

    /// <summary>
    /// Function call to enable camera behaviour.
    /// </summary>
	private void EnableCamera() {
		this.enabled = true;
	}

    /// <summary>
    /// Function call to disable camera behaviour.
    /// </summary>
	private void DisableCamera() {
		this.enabled = false;
	}
}
