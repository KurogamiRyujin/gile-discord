using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collision checker if a hollow block has been hit.
/// </summary>
public class BreakerCollider : MonoBehaviour {
    /// <summary>
    /// Flag if a hollow block was hit.
    /// </summary>
	[SerializeField] private bool hasHitHollowBlock;

    /// <summary>
    /// Function call to signal a hollow block was hit.
    /// </summary>
	public void HitHollowBlock() {
		this.hasHitHollowBlock = true;
	}
    /// <summary>
    /// Resets the collision status.
    /// </summary>
	public void ResetBreaker() {
		this.hasHitHollowBlock = false;
	}

    /// <summary>
    /// Checker if a hollow block has been hit.
    /// </summary>
    /// <returns>Has hit Hollow Block</returns>
	public bool HasHitHollowBlock() {
		return this.hasHitHollowBlock;
	}
}
