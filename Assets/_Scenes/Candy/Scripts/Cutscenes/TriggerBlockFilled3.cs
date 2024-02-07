using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single action observer. Observes if a block has been filled.
/// </summary>
public class TriggerBlockFilled3 : SingleActionObserver {

	/// <summary>
	/// Hollow block to be filled.
	/// </summary>
	[SerializeField] private HollowBlock hollowBlock; // Assign Via Inspector

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
	}

	// Will continuously do an action if is observing
	/// <summary>
	/// Standard Unity Funciton. Called every frame.
	/// </summary>
	void Update() {
		if (this.isObserving && !this.IsTriggered ()) {
			this.Action ();
		}
	}

	// Check if hollow block has 3 sky fragments
	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		if (this.GetHollowBlock ().GetSkyPieceContainer ().GetSkyPieces().Count == 3) {
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
