using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Implementation of a single trigger observer. Destabilizes a room when triggered.
/// </summary>
public class TriggerBreakRoom : SingleTriggerObserver {
	/// <summary>
	/// The hollow block to be broken.
	/// </summary>
	[SerializeField] private HollowBlock hollowBlock;

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
		Debug.Log ("<color=green>TriggerBreakRoom Action Taken</color>");
		this.GetHollowBlock ().SetPiecesReturnToSkyBlock(false);
		this.GetHollowBlock ().Break ();
	}

	/// <summary>
	/// Gets the hollow block.
	/// </summary>
	/// <returns>The hollow block.</returns>
	public HollowBlock GetHollowBlock() {
		if (this.hollowBlock == null) {
			this.hollowBlock = GameObject.FindObjectOfType<HollowBlock> ();
		}
		return this.hollowBlock;
	}
}
