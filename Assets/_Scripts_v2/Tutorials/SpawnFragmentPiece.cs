using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFragmentPiece : MonoBehaviour {
	//NOTE: Specialized class for the tutorial. NOT MEANT TO BE USED IN OTHER SCENES.
	private SkyFragmentPiece skyFragmentPiece;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag (Tags.SKY_FRAGMENT_PIECE))
			this.skyFragmentPiece = other.gameObject.GetComponent<SkyFragmentPiece> ();
	}

	public bool IsFragmentCarried () {
		if (this.skyFragmentPiece != null) {
			return this.skyFragmentPiece.IsCarried ();
		} else
			return true;
	}
}
