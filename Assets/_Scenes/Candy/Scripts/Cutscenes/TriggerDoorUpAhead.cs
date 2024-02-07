using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Can be used to extend functionality of the base class.
/// </summary>
public class TriggerDoorUpAhead : SingleTriggerObserver {
	/// <summary>
	/// Door ahead of the player avatar.
	/// </summary>
	[SerializeField] private DoorStateManager targetDoor;

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
		Debug.Log ("No Action Taken");
		this.GetTargetDoor ().OpenAfar ();
	}

	/// <summary>
	/// Gets the target door.
	/// </summary>
	/// <returns>The target door.</returns>
	public DoorStateManager GetTargetDoor() {
		// Must not enter this. Not ensured
		if (this.targetDoor == null) {
			this.targetDoor = GameObject.FindObjectsOfType<DoorStateManager> ()[1];
		}
		return this.targetDoor;
	}
}
