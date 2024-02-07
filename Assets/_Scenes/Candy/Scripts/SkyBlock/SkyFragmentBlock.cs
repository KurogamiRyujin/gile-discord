using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This extends the SkyFragment.
/// It is intangible to the player but tangible to the needle.
/// It has a Split function which allows it to generate N number of SkyFragmentPiece(s) which will be the tangible blocks.
/// 
/// It has an Absorb function that allows it to destroy the generated SkyFragmentPieces.
/// </summary>
public class SkyFragmentBlock : SkyFragment {
    /// <summary>
    /// Limits how small the fragments can be. If the fragments will be smaller than the limit, they will not be sliced.
    /// </summary>
	public const float MINIMUM_PIECE_WIDTH = 0.08f;
    /// <summary>
    /// Reference to the sky fragment prefab asset.
    /// </summary>
	[SerializeField] private SkyFragmentPiece pieceReference;
    /// <summary>
    /// List of all sky fragment pieces.
    /// </summary>
	[SerializeField] private List<SkyFragmentPiece> skyFragmentPieces;
	[SerializeField] private int lcd;

    /// <summary>
    /// Flag if the sky block was hit by the needle.
    /// </summary>
	[SerializeField] private bool isHit;
    /// <summary>
    /// Flag if the sky block has been sliced into fragments.
    /// </summary>
	[SerializeField] private bool isBroken;
    /// <summary>
    /// Flag if the sky block cannot be broken.
    /// </summary>
	[SerializeField] private bool piecesNeverBreak;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake () {
		gameObject.layer = LayerMask.NameToLayer ("SkyBlock");
		base.Awake ();
		if (this.skyBlockParent == null) {
			this.skyBlockParent = GetComponentInParent<SkyBlock> ();
		}
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		foreach (SkyFragmentPiece piece in this.skyFragmentPieces) {
			if (piece != null)
				Destroy (piece.gameObject);
		}
		this.skyFragmentPieces.Clear ();
	}

    /// <summary>
    /// Makes the sky block visible.
    /// </summary>
	public void Show() {
		this.GetSpriteRenderer ().enabled = true;
		this.GetLineRenderer ().enabled = true;
		this.boxCollider.enabled = true;
	}

    /// <summary>
    /// Makes the sky block invisible.
    /// </summary>
	public void Hide() {
		this.GetSpriteRenderer ().enabled = false;
		this.GetLineRenderer ().enabled = false;
		this.boxCollider.enabled = false;
	}

    /// <summary>
    /// Drops sky fragments when the sky block is sliced by the needle.
    /// Fragment count depends on the equipped charm.
    /// 
    /// Default drop pieces if not specified is 4.
    /// </summary>
    public void Drop() {
		this.Drop (4f);
	}

    /// <summary>
    /// Used for splitting and dropping each piece (mainly for the backstage cutscene).
    /// </summary>
    /// <param name="pieceCount">Number of Fragments</param>
    public void Drop(float pieceCount) {

		if (this.GetNumerator () > 0) {
			if (pieceCount > 0 && this.NotTooSmall (pieceCount)) {
				Debug.Log ("Entered DROP");

				float newDenominator = this.GetDenominator () * pieceCount;
				//			float newNumerator = this.GetNumerator () * pieceCount;

				// Create a SkyFragmentPiece for each pieceCount
//				float speed = 500f;
				int originalCount = skyFragmentPieces.Count;
				for (int i = 0; i < pieceCount; i++) {
					this.skyFragmentPieces.Add (CreateSkyFragmentPiece (this.skyBlockParent.GetDetachedManager ().gameObject, (int)this.GetNumerator (), (int)newDenominator));
					this.skyFragmentPieces [originalCount + i].SetDenominator (pieceCount);
					this.skyFragmentPieces [originalCount + i].SetNumerator (1f);

					//					this.skyFragmentPieces [originalCount + i].transform.localPosition = new Vector3 (this.skyFragmentPieces [i].GetWidth () * i, 0f, 0f);
					this.skyFragmentPieces [originalCount + i].transform.localPosition = new Vector3 ((-GetWidthWhole()/2)+(this.skyFragmentPieces [i].GetWidth () * i)+(this.skyFragmentPieces [i].GetWidth ()/2), 0f, 0f);
					this.skyFragmentPieces [originalCount + i].Drop ();
					this.skyFragmentPieces [originalCount + i].SetPiecesNeverBreak (this.piecesNeverBreak);
//					this.skyFragmentPieces [originalCount + i].GetRigidBody2D ().AddRelativeForce (Random.onUnitSphere * speed);
				}

				this.SetNumerator (0);
				this.SetDenominator (newDenominator);
			}
			else {
				// Too small. Yuni prompt.
				// TODO: Yuni Hints
			}
		}
	}

    /// <summary>
    /// Set the sky fragments to be unbreakable.
    /// </summary>
    /// <param name="isNeverBreak"></param>
	public void SetPiecesNeverBreak(bool isNeverBreak) {
		this.piecesNeverBreak = isNeverBreak;
	}

    /// <summary>
    /// This drop is used for the Hollow Block prefilling ONLY (since it disables platform locking if skyblock is hidden).
    /// </summary>
    /// <param name="targetBlock">Target Ghost Block</param>
    public void Drop(HollowBlock targetBlock) {
		float pieceCount = (float) targetBlock.GetSliceCount ();
		int fillCount = targetBlock.GetFillCount ();
		int detachCount = targetBlock.GetDetachCount ();

		// NOTE: This is for hidden sky blocks only. The Sky Block reference is always forced to 1
//		if (targetBlock.IsHiddenSkyBlock ()) {
//			this.SetBlockValues (1, 1);
//		}

		if (this.GetNumerator () > 0) {
			if (pieceCount > 0 && this.NotTooSmall (pieceCount)) {
				Debug.Log ("Entered DROP");

				float newDenominator = this.GetDenominator () * pieceCount;
				int originalCount = skyFragmentPieces.Count;

				for (int i = 0; i < pieceCount; i++) {
					// For pieces that will fill the target block

					if (i < fillCount) {

						targetBlock.SolvedFromPrefill ();
						this.GetSkyFragmentPieces ().Add (CreateSkyFragmentPiece (this.GetSkyBlockParent().GetDetachedManager ().gameObject, (int)this.GetNumerator (), (int)newDenominator));
						if (targetBlock.IsHiddenSkyBlock ()) {
							this.skyFragmentPieces [originalCount + i].DisablePlatformLocking(); // Important for Hidden SkyBlocks
						}
						this.skyFragmentPieces [originalCount + i].SetDenominator (pieceCount);
						this.skyFragmentPieces [originalCount + i].SetNumerator (1f);
						this.skyFragmentPieces [originalCount + i].transform.localPosition = new Vector3 ((-GetWidthWhole()/2)+(this.skyFragmentPieces [i].GetWidth () * i)+(this.skyFragmentPieces [i].GetWidth ()/2), 0f, 0f);
						this.skyFragmentPieces [originalCount + i].FillDrop (targetBlock);

						this.skyFragmentPieces [originalCount + i].SetPiecesNeverBreak (this.piecesNeverBreak);
					}
					// For detached pieces
					else if(i < fillCount+detachCount) {
						this.skyFragmentPieces.Add (CreateSkyFragmentPiece (this.skyBlockParent.GetDetachedManager ().gameObject, (int)this.GetNumerator (), (int)newDenominator));
						if (targetBlock.IsHiddenSkyBlock ()) {
							this.skyFragmentPieces [originalCount + i].DisablePlatformLocking(); // Important for Hidden SkyBlocks
						}
						this.skyFragmentPieces [originalCount + i].SetDenominator (pieceCount);
						this.skyFragmentPieces [originalCount + i].SetNumerator (1f);
						this.skyFragmentPieces [originalCount + i].gameObject.transform.position = targetBlock.GetDetachedPosition ();

						this.skyFragmentPieces [originalCount + i].SetPiecesNeverBreak (this.piecesNeverBreak);
					}
				}
				Debug.Log ("<color=green>Piece, Fill, Detach count are = "+pieceCount+"  "+fillCount+"  "+detachCount+"</color>");
				this.SetNumerator (pieceCount-fillCount-detachCount);
				this.SetDenominator (newDenominator);
				this.PrefillNumerator ();

				Debug.Log ("<color=green>New NUM DEN = "+this.GetNumerator()+"  "+this.GetDenominator()+"</color>");
			}
			else {
				// Too small. Yuni prompt.
				// TODO: Yuni Hints
			}
		}
	}

    /// <summary>
    /// Returns the list of sky fragments of this sky block.
    /// </summary>
    /// <returns>Sky Fragments</returns>
	public List<SkyFragmentPiece> GetSkyFragmentPieces() {
		if (this.skyFragmentPieces == null) {
			this.skyFragmentPieces = new List<SkyFragmentPiece> ();
		}
		return this.skyFragmentPieces;
	}

    /// <summary>
    /// Slices the sky block into fragments. The fragments are then dropped to be picked up by the player avatar.
    /// </summary>
    /// <param name="pieceCount">Number of pieces</param>
    /// <param name="needle">Needle that sliced the sky block.</param>
    /// <param name="needleThrowing">Needle Throwing behaviour</param>
	public void Split(float pieceCount, NeedleController needle, NeedleThrowing needleThrowing) {
		// You can only split if the numerator is greater than 0
		// TODO: Add a constraint for splitting a piece too small (since players can cascadingly split)
		// i.e. Split to 4, return 2/4, Split to 8, and so on. Maybe check if the decimal value is past a certain threshold
		// and if its too small, prompt the user that he/she can't do that via Yuni/Hints


		//		Debug.Log ("num is "+this.GetNumerator());
		//		Debug.Log ("den is "+this.GetDenominator());
		//		Debug.Log ("divided by "+pieceCount);

		if (this.GetNumerator () > 0) {
			if (pieceCount <= 0 && needle != null) {
				// Deflect needle if pieceCount is 0
				Debug.Log ("NEEDLE HAS HIT");
				needleThrowing.setPullTowards (false);
				needleThrowing.setHookPullTowards (false);
				needle.hasHit = true;
			}
			else if (this.NotTooSmall (pieceCount)) {
				Debug.Log ("Entered Split");
				float newDenominator = this.GetDenominator () * pieceCount;
				//			float newNumerator = this.GetNumerator () * pieceCount;

				// Create a SkyFragmentPiece for each pieceCount
				float speed = 500f;
				int originalCount = skyFragmentPieces.Count;
				for (int i = 0; i < pieceCount; i++) {
					this.skyFragmentPieces.Add (CreateSkyFragmentPiece (this.skyBlockParent.GetDetachedManager ().gameObject, (int)this.GetNumerator (), (int)newDenominator));
					this.skyFragmentPieces [originalCount + i].transform.localPosition = new Vector3 (this.skyFragmentPieces [i].GetWidth () * i, 0f, 0f);
					this.skyFragmentPieces [originalCount + i].GetRigidBody2D ().AddRelativeForce (Random.onUnitSphere * speed);

					this.skyFragmentPieces [originalCount + i].SetPiecesNeverBreak (this.piecesNeverBreak);
				}

				this.SetNumerator (0);
				this.SetDenominator (newDenominator);
				this.HasBeenBroken ();
			} else {
				// Too small. Yuni prompt.
				// TODO: Yuni Hints
			}
		}
	}

    /// <summary>
    /// Generate N number of pieces.
    /// </summary>
    /// <param name="pieceCount">Number of fragments</param>
    public void Split(float pieceCount) {
		this.Split (pieceCount, null, null);
	}

    /// <summary>
    /// Checks if the fragments will not be smaller than the minimum size limit.
    /// </summary>
    /// <param name="pieceCount">Fragments that would be dropped.</param>
    /// <returns>If the fragments are valid. Otherwise, false.</returns>
	public bool NotTooSmall(float pieceCount) {
		float splitWidth = this.widthSingle / pieceCount;
		if (splitWidth > MINIMUM_PIECE_WIDTH) {
			return true;
		}
		else {
			Debug.Log ("<color=red>TOO SMALL. Split width was: "+ splitWidth +"</color>");
			return false;
		}
	}

    /// <summary>
    /// Instantiate a sky fragment from the pieceReference prefab. The sky fragment is then parented to the parent game object and assigned a numerator and denominator for its fraction value.
    /// </summary>
    /// <param name="parent">Parent (which is usually the sky block game object)</param>
    /// <param name="numValue">Numerator</param>
    /// <param name="denValue">Denominator</param>
    /// <returns></returns>
	public SkyFragmentPiece CreateSkyFragmentPiece(GameObject parent, int numValue, int denValue) {
		SkyFragmentPiece holder = SkyFragmentPiece.Instantiate (pieceReference, Vector3.zero, Quaternion.identity); //pieceReference is the sky piece prefab
		holder.gameObject.transform.SetParent (parent.transform); //parent.transform = detachedmanager of skyblock parent
		holder.AlignToLocal (Vector3.zero);
		holder.Initialize (this.skyBlockParent, numValue, denValue); //on load, this.skyBlockParent will be set to the recorded parent skyblock
		holder.ChangeColor (this.skyBlockParent.GetPieceColor (), this.skyBlockParent.GetPieceOutlineColor ());
//		brokenPiece.SetSize (pieceWidth, pieceHeight);

		holder.gameObject.SetActive(true);

		return holder;
	}

    // TODO: Validate results
    /// <summary>
    /// Absorbs a SkyFragmentPiece and adds its value back to it.
    /// Also destroy the piece here.
    /// </summary>
    /// <param name="piece">Sky Fragment</param>
    public void Absorb(SkyFragmentPiece piece) {
		// If empty, set numerator and denominator as the absorbed piece's
//		if (this.GetNumerator() == 0) {
//			this.SetNumerator (piece.GetNumerator());
//			this.SetDenominator (piece.GetDenominator());
//		}
//		// Else get the LCD and update the value by adding the current value
//		// with the absorbed piece's value
//		else {
			int lcd = General.LCD ((int) this.GetDenominator(), (int) piece.GetDenominator());

//			float lcdPieceNumVal = piece.GetDenominator () / lcd * piece.GetNumerator ();
//			float lcdBlockNumeratorVal = this.GetDenominator () / lcd * this.GetNumerator ();
			float lcdPieceNumVal = lcd / piece.GetDenominator () * piece.GetNumerator ();
			float lcdBlockNumeratorVal = lcd / this.GetDenominator () * this.GetNumerator ();

			float newNum = lcdPieceNumVal + lcdBlockNumeratorVal;
			this.SetNumerator ((int) newNum);
			this.SetDenominator (lcd);

			float[] results = General.SimplifyFraction (this.GetNumerator (), this.GetDenominator ());

			this.SetNumerator (results[0]);
			this.SetDenominator (results[1]);
//		}
		this.skyFragmentPieces.Remove (piece);
		Destroy (piece.gameObject);
	}

    /// <summary>
    /// Assign the fraction value for the sky block.
    /// </summary>
    /// <param name="newNumerator">Numerator</param>
    /// <param name="newDenominator">Denominator</param>
	public void SetBlockValues(float newNumerator, float newDenominator) {
//		base.SetNumerator (newNumerator);
//		base.SetDenominator (newDenominator);
		this.SetNumerator (newNumerator);
		this.SetDenominator (newDenominator);
	}
		
    /// <summary>
    /// Assigns the numerator and adjusts the sky block's size.
    /// </summary>
    /// <param name="value"></param>
	public override void SetNumerator(float value) {
		Debug.Log ("<color=red>NUMERATOR WAS SET to "+value+"</color>");
		base.SetNumerator (value);
		this.UpdateSize ();
	}

    /// <summary>
    /// Assignes the denominator and adjusts the sky block's size.
    /// Does not allow zero denominator.
    /// </summary>
    /// <param name="value"></param>
    public override void SetDenominator(float value) {
		base.SetDenominator (value);
		this.UpdateSize ();
	}

    /// <summary>
    /// Adjusts the size of the sky block whenever the numerator/denominator is changed.
    /// </summary>
    public void UpdateSize() {
		this.widthSingle = (this.widthWhole / this.GetDenominator ())*this.GetNumerator(); // This is the width of this object.
		this.SetSize (this.widthSingle, this.height);
	}

    /// <summary>
    /// Used for tutorial triggers. When a sky block is hit, it sets isHit to true.
    /// When a tutorial calls this, it "gets" the isHit value by setting it to false
    /// </summary>
    /// <returns>If this was hit. Otherwise, false.</returns>
    public bool TakeIfHit() {
		if (this.isHit) {
			this.isHit = false;
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// When a tutorial calls this, it "gets" the isBroken value by setting it to false.
    /// </summary>
    /// <returns>If broken. Otherwise, false.</returns>
    public bool TakeIfBroken() {
		if (this.isBroken) {
			this.isBroken = false;
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Confirms the sky block is broken.
    /// </summary>
	public void HasBeenBroken() {
		this.isBroken = true;
	}

    /// <summary>
    /// Confirms the sky block is hit.
    /// </summary>
	public void HasBeenHit() {
		this.isHit = true;
	}

    /// <summary>
    /// Unity Function. Check if another game object's collider entered the sky block's collider.
    /// 
    /// If it is the needle, break the sky block.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other) {
		if (other.GetComponent<NeedleController> () != null) {
			NeedleController needle = other.GetComponent<NeedleController> ();
			NeedleThrowing needleThrowing = other.gameObject.GetComponent<NeedleThrowing> ();
			this.Split (needle.GetSliceCount(), needle, needleThrowing);
			this.HasBeenHit ();
		}
	}
}
