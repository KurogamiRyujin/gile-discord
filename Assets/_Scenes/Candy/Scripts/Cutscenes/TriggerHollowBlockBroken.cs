using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single action observer. Observes a specified hollow block has been broken.
/// </summary>
public class TriggerHollowBlockBroken : SingleActionObserver {

	/// <summary>
	/// The hollow block to be broken.
	/// </summary>
	[SerializeField] private HollowBlock holllowBlock; // Assign Via Inspector
	/// <summary>
	/// Flag if a specified sky block has been sliced.
	/// </summary>
	[SerializeField] private bool isSkyBlockReleased;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();

		EventBroadcaster.Instance.AddObserver (EventNames.SKY_FRAGMENT_PIECE_RELEASED, FragmentReleased);

	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.SKY_FRAGMENT_PIECE_RELEASED);
	}

	/// <summary>
	/// Call to confirm the sky block has been sliced.
	/// </summary>
	public void FragmentReleased () {
		this.isSkyBlockReleased = true;
	}

	// Will continuously do an action if is observing
	/// <summary>
	/// Standard Unity Function. Called every frame.
	/// </summary>
	void Update() {
		if (this.isObserving && !this.IsTriggered ()) {
			this.Action ();
		}
//		else if (!isObserving) {
//			this.isSkyBlockReleased = false;
//		}
	}

	// Do specific conditions here
	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		if (this.isSkyBlockReleased) {
			this.isTriggered = true;
		}
	}

	/// <summary>
	/// Gets the sky hollow block.
	/// </summary>
	/// <returns>The sky hollow block.</returns>
	public HollowBlock GetSkyHollowBlock() {
		// Must not enter this.
		if (this.holllowBlock == null) {
			this.holllowBlock = GameObject.FindObjectOfType<HollowBlock> ();
		}
		return this.holllowBlock;
	}
}
