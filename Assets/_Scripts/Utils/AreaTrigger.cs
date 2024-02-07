using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaTrigger : MonoBehaviour {

	protected abstract void Effect ();
	protected string targetTag = "";

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag (targetTag)) {
			Effect ();
		}
	}
}
