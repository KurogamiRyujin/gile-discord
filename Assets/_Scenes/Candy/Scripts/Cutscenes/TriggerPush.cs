using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Checks if a specified object has been pushed then performs an action.
/// </summary>
public class TriggerPush : SingleTriggerObserver {

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
	}

	/// <summary>
	/// Action done when an object enters the observer's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	protected override void TriggerEnterCondition(Collider2D other) {
		if (other.GetComponent<HollowBlock> () != null) {
			this.Trigger ();
		}
	}

	/// <summary>
	/// Unity Function. Action done when another object with a collider enters the game object's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	void OnTriggerEnter2D(Collider2D other) {
		this.TriggerEnterCondition (other);
	}

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		Debug.Log ("No Action Taken");
	}
}
