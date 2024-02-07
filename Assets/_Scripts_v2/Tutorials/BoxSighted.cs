using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSighted : MonoBehaviour {

	private bool isSighted = false;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag (Tags.PLAYER))
			this.isSighted = true;
	}

	public bool IsSighted() {
		return this.isSighted;
	}
}
