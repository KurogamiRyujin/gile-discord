using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single action observer. Observes if the specified block has been fixed.
/// </summary>
public class TriggerBoxFix : SingleActionObserver {

	/// <summary>
	/// The hollow block to be fixed.
	/// </summary>
	[SerializeField] private HollowBlock hollowBlock; // Assign Via Inspector

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
	}

	// Will continuously do an action if is observing
	// Will continuously do an action if is observing
	/// <summary>
	/// Standard Unity Funciton. Called every frame.
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
		if (this.GetHollowBlock ().IsSolved ()) {
			this.isTriggered = true;
		}
	}

	/// <summary>
	/// Gets the hollow block.
	/// </summary>
	/// <returns>The hollow block.</returns>
	public HollowBlock GetHollowBlock() {
		// Must not enter this.
		if (this.hollowBlock == null) {
			this.hollowBlock = GameObject.FindObjectOfType<HollowBlock> ();
		}
		return this.hollowBlock;
	}
}
