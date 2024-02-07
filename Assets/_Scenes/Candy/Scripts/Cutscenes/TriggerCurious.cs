using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Can be used to extend functionality of the base class.
/// </summary>
public class TriggerCurious : SingleTriggerObserver {
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
	}
}
