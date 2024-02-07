using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyPieceAbyss : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag (Tags.SKY_FRAGMENT_PIECE)) {
			SkyFragmentPiece piece = other.gameObject.GetComponent<SkyFragmentPiece> ();
			piece.transform.position = piece.GetDetachedManagerParent ().transform.position;
		}
	}
}
