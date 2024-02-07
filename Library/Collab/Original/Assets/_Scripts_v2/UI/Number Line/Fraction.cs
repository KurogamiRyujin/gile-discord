﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fraction : MonoBehaviour {

	public enum Notation {
		FRACTION,
		DECIMAL
	}

	[SerializeField] private Notation notation = Notation.FRACTION;
	[SerializeField] private Text numerator, denominator;//numerator is also used when its a whole number or decimal notation
	[SerializeField] private LineRenderer bar;

	protected int numeratorVal = 0, denominatorVal = 0;

	public void SetValue(int numerator, int denominator) {
		this.numeratorVal = numerator;
		this.denominatorVal = denominator;
		int value = numerator / denominator;

		float tempN = this.numeratorVal;
		float tempD = this.denominatorVal;
		float fValue = tempN / tempD;

		switch (this.notation) {
		case Notation.FRACTION:
			this.numerator.text = numerator.ToString ();
			this.denominator.text = denominator.ToString ();

			if (numerator == 0 && denominator == 0) {
				this.denominator.enabled = false;
				this.bar.enabled = false;
			}
			break;
		case Notation.DECIMAL:
			this.denominator.enabled = false;
			this.bar.enabled = false;

			if (denominator != 0) {
				if (numerator % denominator == 0)
					this.numerator.text = value.ToString ();
				else
					this.numerator.text = fValue.ToString ();
			}

			break;
		}
	}

	public void SetNotation(Notation notation) {
		this.notation = notation;
	}

	public int GetNumerator() {
		return this.numeratorVal;
	}

	public int GetDenominator() {
		return this.denominatorVal;
	}
}
