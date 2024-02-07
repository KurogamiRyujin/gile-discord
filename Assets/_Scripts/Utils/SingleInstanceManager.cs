using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleInstanceManager : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DisableComponents ();

		DontDestroyOnLoad (this.gameObject);

		if (FindObjectsOfType (GetType ()).Length > 1)
			Destroy (this.gameObject);
		else
			EnableComponents ();
	}

	void DisableComponents() {
		foreach (MonoBehaviour component in this.gameObject.GetComponents<MonoBehaviour>()) {
			component.enabled = false;
		}
	}

	void EnableComponents() {
		foreach (MonoBehaviour component in this.gameObject.GetComponents<MonoBehaviour>()) {
			component.enabled = true;
		}
	}
}
