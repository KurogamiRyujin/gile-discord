using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeFragmentCatcher : MonoBehaviour {
	
	public Stack<SkyFragmentPiece> pieces;
	public int expectedPieces = 4;

	private Collider2D fragmentChecker;

	void Awake() {
		pieces = new Stack<SkyFragmentPiece> ();
		fragmentChecker = GetComponent<Collider2D> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag (Tags.SKY_FRAGMENT_PIECE)) {
			SkyFragmentPiece piece = other.gameObject.GetComponent<SkyFragmentPiece> ();

			if (!pieces.Contains (piece))
				pieces.Push (piece);

			piece.gameObject.SetActive (false);

			if (pieces.Count == expectedPieces)
				fragmentChecker.enabled = false;
		}
	}
}
