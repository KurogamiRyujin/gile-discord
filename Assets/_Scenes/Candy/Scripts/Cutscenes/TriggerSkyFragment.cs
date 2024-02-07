using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Checks if the player avatar has picked up the specified sky fragment and performs an action.
/// </summary>
public class TriggerSkyFragment : SingleTriggerObserver {
	/// <summary>
	/// Sky Block the sky fragment comes from.
	/// </summary>
	[SerializeField] private SkyBlock skyBlock;
	/// <summary>
	/// Player avatar status.
	/// </summary>
	[SerializeField] private PlayerYuni player;
	/// <summary>
	/// Flag if the player has carried the sky fragment.
	/// </summary>
	[SerializeField] private bool hasCarriedPiece;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		base.OnAwake();
		this.hasCarriedPiece = false;
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_GEM_CARRY, CarryGem);
	}

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_GEM_CARRY);
	}

	/// <summary>
	/// Hides the fragment.
	/// </summary>
	public void HideFragment() {
		this.GetSkyBlock ().HideDetachedPieces ();
	}
	/// <summary>
	/// Shows the fragment.
	/// </summary>
	public void ShowFragment() {
		this.GetSkyBlock ().ShowDetachedPieces ();
	}

	/// <summary>
	/// Gets the sky block.
	/// </summary>
	/// <returns>The sky block.</returns>
	public SkyBlock GetSkyBlock() {
		if(this.skyBlock == null) {
			this.skyBlock = gameObject.GetComponentInChildren<SkyBlock> ();
		}
		return this.skyBlock;
	}

	// Override, don't trigger.
	/// <summary>
	/// Action done when an object enters the observer's collider.
	/// </summary>
	/// <param name="other">Other object's collider.</param>
	protected override void TriggerEnterCondition(Collider2D other) {
		Debug.Log ("<color=green>TRIGGER ENTER</color>");
	}

	/// <summary>
	/// Action to do when this has been triggered.
	/// </summary>
	public override void Action() {
		Debug.Log ("No Action Taken");
	}

	/// <summary>
	/// Called to notify the sky fragment has been carried.
	/// </summary>
	/// <param name="parameters">Parameter object containin data to be used in the function.</param>
	public void CarryGem(Parameters parameters) {
		this.hasCarriedPiece = true;
	}
	/// <summary>
	/// Checker if the sky fragment has been carried.
	/// </summary>
	/// <returns><c>true</c> if sky fragment has been carried; otherwise, <c>false</c>.</returns>
	public bool HasCarriedPiece() {
		return this.hasCarriedPiece;
	}

	/// <summary>
	/// Gets the player avatar status.
	/// </summary>
	/// <returns>The player avatar status.</returns>
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}
}
