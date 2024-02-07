using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds the actual sky block attached to the sky block game object.
/// </summary>
public class AttachedManager : MonoBehaviour {

    /// <summary>
    /// Sky block game object the manager is associated with.
    /// </summary>
	[SerializeField] private SkyBlock skyBlockParent;
    /// <summary>
    /// The unbroken/returned sky fragment. Does not have partitions.
    /// </summary>
	[SerializeField] private SkyFragmentBlock attachedFragmentBlock;

    /// <summary>
    /// Reference to the rect transform.
    /// </summary>
	[SerializeField] protected RectTransform rectTransform;
    /// <summary>
    /// Reference to the sprite renderer.
    /// </summary>
	[SerializeField] protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// Reference to a line renderer.
    /// </summary>
	[SerializeField] protected LineRenderer lineRenderer;
    /// <summary>
    /// Flag if the fragments cannot be broken.
    /// </summary>
	[SerializeField] protected bool piecesNeverBreak;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		if(this.skyBlockParent == null) {
			this.skyBlockParent = GetComponentInParent<SkyBlock> ();
		}
	}

    /// <summary>
    /// Crafts the sky block according to numerator and denominator but sets it to 1/1.
    /// </summary>
    /// <param name="blockParent">Sky Block Game Object</param>
    /// <param name="numerator">Numerator</param>
    /// <param name="denominator">Denominator</param>
    public void InitialCraft(SkyBlock blockParent, float numerator, float denominator) {
		this.skyBlockParent = blockParent;
		// TODO: Fixed since this overrides prefilled block settings, check if this holds.
		this.GetAttachedFragmentBlock().Initialize (this.skyBlockParent, numerator, denominator);
		this.GetAttachedFragmentBlock ().ChangeColor (this.skyBlockParent.GetPieceColor (), this.skyBlockParent.GetPieceOutlineColor ());
		this.GetAttachedFragmentBlock ().SetBlockValues (this.GetAttachedFragmentBlock ().GetNumerator(), this.GetAttachedFragmentBlock ().GetDenominator());

	}

//	public void Craft(SkyBlock blockParent, float numerator, float denominator) {
//		this.skyBlockParent = blockParent;
//		// Generate a block sppecified by the parent size
//		this.GetAttachedFragmentBlock().Initialize (this.skyBlockParent, numerator, denominator);
//	}

    /// <summary>
    /// Make the sky block visible.
    /// </summary>
	public void Show() {
		this.attachedFragmentBlock.Show ();
	}

    /// <summary>
    /// Make the sky block invisible.
    /// </summary>
	public void Hide() {
		this.attachedFragmentBlock.Hide ();
	}

    /// <summary>
    /// Drop the block as fragments depending on the needle's charm value.
    /// </summary>
	public void Drop() {
		this.GetAttachedFragmentBlock ().Drop ();
	}

    /// <summary>
    /// Drop the block as fragments into a target ghost block for prefilling.
    /// </summary>
    /// <param name="targetBlock"></param>
	public void Drop(HollowBlock targetBlock) {
		this.GetAttachedFragmentBlock ().Drop (targetBlock);
	}

    /// <summary>
    /// Return detached sky fragments to the sky block.
    /// </summary>
    /// <param name="detachedPiece">Sky Fragments</param>
	public void Absorb(SkyFragmentPiece detachedPiece) {
		this.attachedFragmentBlock.Absorb (detachedPiece);
	}

    /// <summary>
    /// Set if the fragments will be unbreakable.
    /// </summary>
    /// <param name="willNeverBreak"></param>
	public void SetPiecesNeverBreak(bool willNeverBreak) {
		this.piecesNeverBreak = willNeverBreak;
		this.GetAttachedFragmentBlock ().SetPiecesNeverBreak (this.piecesNeverBreak);
	}

    /// <summary>
    /// Returns the sky fragment block.
    /// </summary>
    /// <returns></returns>
	public SkyFragmentBlock GetAttachedFragmentBlock() {
		if (this.attachedFragmentBlock == null) {
			Debug.Log ("<color=cyan> GetAttachedFragmentBlock entered null. Finding in child </color>");
			this.attachedFragmentBlock = GetComponentInChildren<SkyFragmentBlock> ();
		}
		return this.attachedFragmentBlock;
	}
}
