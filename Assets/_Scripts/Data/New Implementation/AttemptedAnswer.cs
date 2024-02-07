using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttemptedAnswer {
	float numerator;
	float denominator;

	public AttemptedAnswer() {
		this.numerator = 0;
		this.denominator = 0;
	}

	public void SetNumerator(float numerator) {
		this.numerator = numerator;
	}

	public void SetDenominator(float denominator) {
		this.denominator = denominator;
	}

	public float GetNumerator () {
		return numerator;
	}

	public float GetDenominator() {
		return denominator;
	}
}
