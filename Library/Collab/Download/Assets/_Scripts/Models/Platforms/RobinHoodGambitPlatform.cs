using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinHoodGambitPlatform : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		EventBroadcaster.Instance.AddObserver (EventNames.ROBIN_HOOD_PLATFORMS_ACTIVATE, this.ActivatePlatform);
		EventBroadcaster.Instance.AddObserver (EventNames.ROBIN_HOOD_PLATFORMS_DEACTIVATE, this.DeactivatePlatform);
	}

	void OnDestroy () {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ROBIN_HOOD_PLATFORMS_ACTIVATE);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ROBIN_HOOD_PLATFORMS_DEACTIVATE);
	}

	private void DeactivatePlatform() {
		this.gameObject.SetActive (false);
	}

	private void ActivatePlatform() {
		this.gameObject.SetActive (true);
	}
}
