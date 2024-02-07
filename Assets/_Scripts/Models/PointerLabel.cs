using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerLabel : MonoBehaviour {
	
	[SerializeField] TextMesh numeratorLabel, denominatorLabel;
	[SerializeField] int numerator, denominator;

	// Use this for initialization
	void Awake () {
		TextMesh[] txt = GetComponentsInChildren<TextMesh> ();

		foreach (TextMesh tm in txt) {
			if (tm.CompareTag ("FractionLabelNumerator"))
				numeratorLabel = tm;
			else if (tm.CompareTag ("FractionLabelDenominator"))
				denominatorLabel = tm;
		}
	}

	public void SetValue(int numerator, int denominator) {
		this.numerator = numerator;
		this.denominator = denominator;

		numeratorLabel.text = this.numerator.ToString ();
		denominatorLabel.text = this.denominator.ToString ();
	}

	public int GetNumerator() {
		return this.numerator;
	}

	public int GetDenominator() {
		return this.denominator;
	}
}
