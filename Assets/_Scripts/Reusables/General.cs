using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General {

	public static void ChangeSpriteTransparency(List<SpriteRenderer> images, float transparency) {
		foreach (SpriteRenderer sprite in images) {
			sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, transparency);
		}
	}

	public static float Round(float value, int decimalPlaces) {
		float decimalModifier = 10 * decimalPlaces;
		float roundedValue = (value * decimalModifier) / decimalModifier;
		return roundedValue;
	}

	// Subtract two fractions (pair1 - pair2) and return the numerator-denominator pair
	public static float[] SubtractFractions(float num1, float den1, float num2, float den2) {
		float[] results = new float[2];
		int lcd = LCD ((int)den1, (int)den2);
		float lcdNum1 = lcd / den1 * num1;
		float lcdNum2 = lcd / den2 * num2;

		float newNum = lcdNum1 - lcdNum2;

		results [0] = newNum;
		results [1] = lcd;
		return results;
	}

	// Add two fractions and return the numerator-denominator pair
	public static float[] AddFractions(float num1, float den1, float num2, float den2) {
		float[] results = new float[2];
		int lcd = LCD ((int)den1, (int)den2);
		float lcdNum1 = lcd / den1 * num1;
		float lcdNum2 = lcd / den2 * num2;

		float newNum = lcdNum1 + lcdNum2;

		results [0] = newNum;
		results [1] = lcd;
		return results;
	}

	public static int LCD(int x, int y) {
		int initialX, initialY;

		initialX = x;
		initialY = y;

		while (x != y) {
			if (x > y) {
				x = x - y;
			}
			else {
				y = y - x;
			}
		}

		return (initialX * initialY) / y;
	}

	// Reduce the fraction to its lowest form. Returns a float[] where the first element
	// is the reduced numerator and the second element is the reduced denominator
	public static float[] SimplifyFraction(float numerator, float denominator) {
		float[] results = new float[2];
		if (numerator != 0 && denominator != 0) {
			float lcd = General.LCD ((int) numerator, (int) denominator);
			float oldNumerator = numerator;
			float oldDenominator = denominator;

			// SetNumerator with lcd/Denominator is not a typo
			results[0] = (lcd/oldDenominator);
			results[1] = (lcd/oldNumerator);
		}

		return results;
	}

	public static LineRenderer GenerateBoxOutline(LineRenderer lineRenderer, float width, float height) {
		Vector3[] positionsArray = new Vector3[4];
		lineRenderer.positionCount = positionsArray.Length;

		float lineX = width / 2;
		float lineY = height / 2;

		Vector3 upperleft = new Vector3 (-lineX, lineY, 0f);
		Vector3 upperRight = new Vector3 (lineX, lineY, 0f);
		Vector3 lowerRight = new Vector3 (lineX, -lineY, 0f);
		Vector3 lowerLeft = new Vector3 (-lineX, -lineY, 0f);

		Vector3 closeRight = new Vector3 (lineX, -lineY, 0f);
		positionsArray [0] = lowerRight;
		positionsArray [1] = upperRight;
		positionsArray [2] = upperleft;
		positionsArray [3] = lowerLeft;
		lineRenderer.SetPositions (positionsArray);

		return lineRenderer;
	}

	// Scale an object based on a given length
	public static Vector3 LengthToScale(float currentWidth, Vector3 currentScale, float desiredLength) {
		Vector3 scale = currentScale;
		scale.x = desiredLength * scale.x / currentWidth;

		return scale;
	}

	// Don't destroy on load for a child element
	public static void DontDestroyChildOnLoad(GameObject child) {
		Transform parentTransform = child.transform;

		// If this object doesn't have a parent then its the root transform.
		while ( parentTransform.parent != null ) {
			// Keep going up the chain.
			parentTransform = parentTransform.parent;
		}
		GameObject.DontDestroyOnLoad(parentTransform.gameObject);
	}

	public static void CheckIfNull(object behaviour, string name, string scriptName) {
		if(behaviour == null)
			Debug.Log(name+" is NULL : in "+scriptName);
	}


}
