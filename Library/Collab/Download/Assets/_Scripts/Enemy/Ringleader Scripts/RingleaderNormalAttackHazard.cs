using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingleaderNormalAttackHazard : MonoBehaviour {

	private PlayerHealth playerHealth = null;

	public void TriggerDamage(int damage) {
		if (playerHealth != null)
			playerHealth.Damage (damage);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerHealth = other.gameObject.GetComponent<PlayerHealth> ();
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerHealth = null;
		}
	}
}
