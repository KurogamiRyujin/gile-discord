using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player") || other.gameObject.CompareTag ("Enemy")) {
			if (other.gameObject.CompareTag ("Player")) {
				other.gameObject.GetComponent<PlayerHealth> ().isAlive = false;
			} else {
				Destroy (other.gameObject);
			}
			Destroy (gameObject.GetComponent<Collider2D>());
		}
	}
}
