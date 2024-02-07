using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functionality to the Ringleader's hazard. Deals the Ringleader's damage.
/// </summary>
public class RingleaderNormalAttackHazard : MonoBehaviour {
    
    /// <summary>
    /// Reference to the player avatar's health.
    /// </summary>
	private PlayerHealth playerHealth = null;

    /// <summary>
    /// Apply damage to the player's health, if the reference is there.
    /// </summary>
    /// <param name="damage">Damage dealt</param>
	public void TriggerDamage(int damage) {
		if (playerHealth != null)
			playerHealth.Damage (damage);
	}

    /// <summary>
    /// Unity Function. Checks if another game object's collider enters this game object's collider.
    /// 
    /// If it is the player avatar, takes the reference to its health component.
    /// </summary>
    /// <param name="other">Other Game Object's Collider</param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerHealth = other.gameObject.GetComponent<PlayerHealth> ();
		}
	}

    /// <summary>
    /// Unity Function. Checks if another game object's collider leaves this game object's collider.
    /// 
    /// If it is the player avatar, remove the reference to its health component.
    /// </summary>
    /// <param name="other">Other Game Object's Collider</param>
	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			playerHealth = null;
		}
	}
}
