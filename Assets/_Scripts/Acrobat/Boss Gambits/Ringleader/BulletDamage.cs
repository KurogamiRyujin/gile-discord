using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet Damage functionality. Implements the way the Bullet object damages a target.
/// </summary>
public class BulletDamage : MonoBehaviour {

    /// <summary>
    /// Bullet damage
    /// </summary>
	private int damage;

    /// <summary>
    /// Damage setter
    /// </summary>
    /// <param name="damage"></param>
	public void SetDamage(int damage) {
		this.damage = damage;
	}

    /// <summary>
    /// Standard Unity Trigger Collision function. Called when the Bullet enters another collider.
    /// If conditions are satisfied, Bullet Damage applies the damage to the other collider's game object through some component
    /// </summary>
    /// <param name="other"></param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.GetComponent<PlayerHealth> ().Damage (this.damage);
			Expire ();
		} else if (other.gameObject.CompareTag ("PartitionableObject")) {
			if (other.gameObject.GetComponent<PartitionableObject_v2> () != null) {
				if (other.gameObject.GetComponent<PartitionableObject_v2> ().IsTangible ())
					Expire ();
			} else {
				Expire ();
			}
		}
	}

    /// <summary>
    /// Expiration functionality when the Bullet Object collides with another object.
    /// </summary>
	private void Expire() {
		Destroy (this.gameObject);
	}

}
