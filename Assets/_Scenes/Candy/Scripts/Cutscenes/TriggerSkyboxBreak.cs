using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Checks if the specified ghost block has been broken and performs an action.
/// </summary>
public class TriggerSkyboxBreak : SingleTriggerObserver {
	/// <summary>
	/// Specified hollow block to be broken.
	/// </summary>
	[SerializeField] private HollowBlockParent hollowBlockParent;

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
		SoundManager.Instance.Play (AudibleNames.Results.BREAK, false);
		this.ShowBlock ();
	}

	/// <summary>
	/// Hides the block.
	/// </summary>
	public void HideBlock() {
		this.GetHollowBlockParent ().Hide ();
	}
	/// <summary>
	/// Shows the block.
	/// </summary>
	public void ShowBlock() {
		this.GetHollowBlockParent ().Show ();
	}

	/// <summary>
	/// Gets the hollow block.
	/// </summary>
	/// <returns>The hollow block.</returns>
	public HollowBlockParent GetHollowBlockParent() {
		if(this.hollowBlockParent == null) {
			GameObject.FindObjectsOfType<HollowBlockParent> ();
		}
		return this.hollowBlockParent;
	}
}
