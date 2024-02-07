using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrokenFractionLabel : MonoBehaviour {
	private int numerator;
	private int denominator;
	private TextMeshPro textMesh;

	void Awake() {
		this.textMesh = GetComponent<TextMeshPro> ();
	}

	public void SetFraction(int newNumerator, int newDenominator) {
		this.SetNumerator (newNumerator);
		this.SetDenominator (newDenominator);
	}

	public void SetNumerator(int newNumerator) {
		this.numerator = newNumerator;
		this.UpdateLabel ();
	}
	public void SetDenominator(int newDenominator) {
		this.denominator = newDenominator;
		this.UpdateLabel ();
	}
	public TextMeshPro GetTextMesh () {
		if (this.textMesh == null) {
			this.textMesh = GetComponent<TextMeshPro> ();
		}
		return this.textMesh;
	}
	public void UpdateLabel() {
		this.GetTextMesh().text = this.numerator + "\n" + this.denominator;
	}

}
