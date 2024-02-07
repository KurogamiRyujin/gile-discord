using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single action observer. Checks if a sky block has been hit by the needle and then performs an action.
/// </summary>
public class TriggerSkyBlockHit : SingleActionObserver {

	/// <summary>
	/// Specified sky block to be sliced.
	/// </summary>
	[SerializeField] private SkyFragmentBlock skyFragmentBlock; // Assign Via Inspector

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
	}

	// Will continuously do an action if is observing
	/// <summary>
	/// Standard Unity Function. Called every frame.
	/// </summary>
	void Update() {
		if (this.isObserving && !this.IsTriggered ()) {
			this.Action ();
		}
	}

	// Do specific conditions here
	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		if (this.GetSkyFragmentBlock ().TakeIfHit ()) {
			this.isTriggered = true;
		}
	}

	/// <summary>
	/// Gets the sky fragment block.
	/// </summary>
	/// <returns>The sky fragment block.</returns>
	public SkyFragmentBlock GetSkyFragmentBlock() {
		// Must not enter this.
		if (this.skyFragmentBlock == null) {
			this.skyFragmentBlock = GameObject.FindObjectOfType<SkyFragmentBlock> ();
		}
		return this.skyFragmentBlock;
	}
}
