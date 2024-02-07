using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for an enemy attack behaviour.
/// </summary>
public class EnemyAttack : MonoBehaviour {

    /// <summary>
    /// Damage inflicted by the enemy to the player avatar.
    /// </summary>
	[SerializeField] protected int damage;

    /// <summary>
    /// Sets the damage.
    /// </summary>
    /// <param name="damage">Damage Value</param>
	public void SetDamage(int damage) {
		this.damage = damage;
	}

    /// <summary>
    /// Enemy attack function.
    /// </summary>
	public virtual void Attack() {
		Debug.Log ("Enemy Attacked");
	}
}
