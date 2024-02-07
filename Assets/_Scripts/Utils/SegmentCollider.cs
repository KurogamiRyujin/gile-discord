using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentCollider : MonoBehaviour {

	protected int numerator = 0;
	protected int denominator = 0;

	public void SetValue(int numerator, int denominator) {
		this.numerator = numerator;
		this.denominator = denominator;

		Debug.Log ("Segment labeled: " + numerator + "/" + denominator);
	}

	public int GetNumerator() {
		return this.numerator;
	}

	public int GetDenominator() {
		return this.denominator;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Current Pointer")) {
			PointerLabel fraction = other.gameObject.GetComponent<PointerLabel> ();
			fraction.SetValue (this.numerator, this.denominator);
		}
	}
}
