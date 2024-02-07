using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus {

	private static CameraFocus sharedInstance;

	public static CameraFocus Instance {
		get {
			if (sharedInstance == null)
				sharedInstance = new CameraFocus ();

			return sharedInstance;
		}
	}

	public void FocusCameraAt(GameObject target) {
		Camera.main.GetComponent<CameraController> ().SetTarget (target);
	}

}
