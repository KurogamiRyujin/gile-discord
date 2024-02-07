using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightObject {
	private float numerator;
	private float denominator;
	private Color color;
	private bool isAdd;
    private HollowBlock hollowBlock;

    public HighlightObject (float newNumerator, float newDenominator, Color newColor, bool newIsAdd, HollowBlock block) {
		this.numerator = newNumerator;
		this.denominator = newDenominator;
		this.color = newColor;
		this.isAdd = newIsAdd;
        this.hollowBlock = block;
	}
	public float GetNumerator() {
		return this.numerator;
	}
	public float GetDenominator() {
		return this.denominator;
	}
	public Color GetColor() {
		return this.color;
	}
	public bool IsAdd() {
		return this.isAdd;
	}
}
