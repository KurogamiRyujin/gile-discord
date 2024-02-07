using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisplacedPieces : MonoBehaviour {

	[SerializeField] private SkyFragmentBlock skyFragmentBlock;
	[SerializeField] private SkyBlock skyBlock;
	[SerializeField] private OneTimeFragmentCatcher fragmentCatcher;

	//Targets where the pieces will be distributed
	[SerializeField] private HollowBlock correctHollowBlock;
	[SerializeField] private HollowBlock wrongHollowBlock;
	[SerializeField] private Transform placement;

	[SerializeField] private int sliceCount = 4;
	[SerializeField] private int piecesForCorrectBlock = 0;
	[SerializeField] private int piecesForWrongBlock = 0;
	[SerializeField] private int piecesUnused = 0;
	[SerializeField] private int piecesReturned = 0;

	private bool initialized = false;
	
	// Update is called once per frame
	void Update () {
		if (this.skyFragmentBlock.GetNumerator () == 0 ||
			this.skyFragmentBlock.GetDenominator () == 0 ||
			this.skyFragmentBlock.GetWidthWhole () == 0 ||
		    this.skyFragmentBlock.GetWidthSingle () == 0)
			Debug.Log ("Initializing");
		else {
			if (!initialized) {
				initialized = true;
				this.skyFragmentBlock.Split (sliceCount);
			}
		}

		if (fragmentCatcher.pieces.Count == fragmentCatcher.expectedPieces) {
			CorrectBlockAbsorb ();
			WrongBlockAbsorb ();
			UnusedBlockPlace ();
			ReturnPieces ();
		}
	}

	private void CorrectBlockAbsorb() {
		for (int i = 0; i < piecesForCorrectBlock; i++) {
			SkyFragmentPiece piece = fragmentCatcher.pieces.Pop ();
			if (piece != null && !piece.IsCarried()) { // TODO
				piece.gameObject.SetActive (true);

				correctHollowBlock.Absorb (piece);
			}
		}
	}

	private void WrongBlockAbsorb() {
		for (int i = 0; i < piecesForWrongBlock; i++) {
			SkyFragmentPiece piece = fragmentCatcher.pieces.Pop ();
			if (piece != null && !piece.IsCarried()) { // TODO
				piece.gameObject.SetActive (true);

				wrongHollowBlock.Absorb (piece);
			}
		}
	}

	private void UnusedBlockPlace() {
		for (int i = 0; i < piecesUnused; i++) {
			SkyFragmentPiece piece = fragmentCatcher.pieces.Pop ();
			if (piece != null) {
				piece.gameObject.SetActive (true);
				piece.transform.position = placement.transform.position;
			}
		}
	}

	private void ReturnPieces() {
		for (int i = 0; i < piecesReturned; i++) {
			SkyFragmentPiece piece = fragmentCatcher.pieces.Pop ();
			if (piece != null) {
				piece.gameObject.SetActive (true);
//				piece.Break ();
				skyBlock.Absorb (piece);
			}
		}
	}
}
