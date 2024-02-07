using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySightedTrigger : MonoBehaviour {

	private bool hasSighted = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			hasSighted = true;
		}
	}

	public bool HasSighted() {
		return this.hasSighted;
	}
}
