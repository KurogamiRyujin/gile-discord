using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Marker for the current stability on the stability number line.
/// </summary>
public class StabilityPointer : MonoBehaviour {

	public const string TARGET_CHANGE = "targetChange";
	public const string NEW_NUMERATOR = "newNumerator";
	public const string NEW_DENOMINATOR = "newDenominator";
    /// <summary>
    /// Numerator value of the fraction value.
    /// </summary>
	[SerializeField] float numerator;
    /// <summary>
    /// Denominator value of the fraction value.
    /// </summary>
	[SerializeField] float denominator;
    /// <summary>
    /// Length reference used as a relative placement for the marker.
    /// </summary>
	[SerializeField] float length;

    /// <summary>
    /// Fraction label for this marker.
    /// </summary>
	[SerializeField] Fraction fractionLabel;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.SetNumerator (0);
		this.SetDenominator (1);
	}

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start() {
		this.GetFractionLabel ().SetValue ((int)this.numerator, (int)this.denominator);
	}

	public float GetNumerator() {
		return this.numerator;
	}

	public float GetDenominator() {
		if (this.denominator == 0)
			this.denominator = 1;
		return this.denominator;
	}

    /// <summary>
    /// Points to the location of the given numerator-denominator pair and sets the numerator and denominator values accordingly.
    /// </summary>
    /// <param name="numerator">Target Fraction's Numerator</param>
    /// <param name="denominator">Target Fraction's Denominator</param>
    /// <param name="maxPoint">Stability Number Line's Max Point</param>
    public void PointTo(float numerator, float denominator, int maxPoint) {
        this.ChangeLocation((this.length/maxPoint) * numerator / denominator);
        //this.ChangeLocation (this.length * numerator / denominator / maxPoint);
		this.SetNumerator (numerator);
		this.SetDenominator (denominator);
		this.UpdateLabel ((int)numerator, (int)denominator);
		this.PostChangeEvent ();
	}

    /// <summary>
    /// Broadcast that the target marker has changed.
    /// </summary>
	public void PostChangeEvent() {
		Parameters parameters = new Parameters ();
		parameters.PutExtra (NEW_NUMERATOR, this.GetNumerator ());
		parameters.PutExtra (NEW_DENOMINATOR, this.GetDenominator ());
		EventBroadcaster.Instance.PostEvent (EventNames.STABILITY_MARKER_CHANGED, parameters);
	}
    //	public void PointTo(float numerator, float denominator) {
    //		this.ChangeLocation (this.length * numerator / denominator);
    //		this.SetNumerator (numerator);
    //		this.SetDenominator (denominator);
    //		this.UpdateLabel ((int)numerator, (int)denominator);
    //	}

    /// <summary>
    /// Changes the x coordinate value of the pointer.
    /// </summary>
    /// <param name="x">New X Coordinate</param>
    public void ChangeLocation(float x) {
		gameObject.transform.localPosition = new Vector3 (
			x,
			gameObject.transform.localPosition.y,
			gameObject.transform.localPosition.z);
	}

    /// <summary>
    /// Returns the local x coordinate of this pointer's current position.
    /// </summary>
    /// <returns>X Coordinate</returns>
	public float GetLocalX() {
		return this.gameObject.transform.localPosition.x;
	}

    /// <summary>
    /// Update the fraction displayed by the fraction label.
    /// </summary>
	public void UpdateLabel() {
		this.GetFractionLabel ().SetValue ((int)this.GetNumerator (), (int)this.GetDenominator ());
	}

    /// <summary>
    /// Update the fraction displayed by the fraction label.
    /// </summary>
    /// <param name="numerator">New Fraction Numerator</param>
    /// <param name="denominator">New Fraction Denominator</param>
	public void UpdateLabel(int numerator, int denominator) {
		this.GetFractionLabel ().SetValue (numerator, denominator);
	}

	public Fraction GetFractionLabel() {
		if (this.fractionLabel == null) {
			this.fractionLabel = GetComponentInChildren<Fraction> ();
		}
		return this.fractionLabel;
	}

    /// <summary>
    /// Sets the reference length.
    /// </summary>
    /// <param name="value">New Value</param>
	public void SetLength(float value) {
		this.length = value;
	}

	public float GetLength() {
		return this.length;
	}

	public void SetNumerator(float value) {
		this.numerator = value;
	}

	public void SetDenominator(float value) {
		if (value == 0) { // Don't allow 0 denominator
			value = 1;
		}
		this.denominator = value;
	}
}
