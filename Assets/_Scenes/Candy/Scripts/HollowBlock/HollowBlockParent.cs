using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent game object for the ghost block game object.
/// </summary>
public class HollowBlockParent : MonoBehaviour {
    /// <summary>
    /// Reference to the ghost block behaviour.
    /// </summary>
	[SerializeField] private HollowBlock hollowBlock;

    /// <summary>
    /// make the ghost block game object visible.
    /// </summary>
	public void Show() {
		this.GetHollowBlock ().gameObject.SetActive (true);

	}

    /// <summary>
    /// Make the ghost block game object invisible.
    /// </summary>
	public void Hide() {
		this.GetHollowBlock ().gameObject.SetActive (false);
		
	}

    /// <summary>
    /// Returns the ghost block behaviour.
    /// </summary>
    /// <returns>Hollow Block Behaviour</returns>
	public HollowBlock GetHollowBlock() {
		if (this.hollowBlock == null) {
			this.hollowBlock = GetComponentInChildren<HollowBlock> ();
		}
		return this.hollowBlock;
	}
}
