using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Checks if the specified item has been picked up then performs an action.
/// </summary>
public class TriggerShowPickupItem : SingleTriggerObserver {

	/// <summary>
	/// Specified object to be picked up.
	/// </summary>
	[SerializeField] protected PickupObject pickupObject;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
	}

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		this.GetPickupObject ().gameObject.SetActive (true);
	}

	/// <summary>
	/// Stop observing.
	/// </summary>
	public override void Ignore() {
		base.Ignore ();
		this.GetPickupObject ().gameObject.SetActive (false);
	}

	/// <summary>
	/// Begin observing.
	/// </summary>
	public override void Observe() {
		base.Observe ();
		this.GetPickupObject ().gameObject.SetActive (true);
	}

	/// <summary>
	/// Gets the pickup object.
	/// </summary>
	/// <returns>The pickup object.</returns>
	public PickupObject GetPickupObject() {
		if (this.pickupObject == null) {
			this.pickupObject = GetComponentInChildren<PickupObject> ();
		}
		return this.pickupObject;

	}
}
