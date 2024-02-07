using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerLabelClone : MonoBehaviour {

	[SerializeField] GameObject fraction;
	[SerializeField] TextMesh numeratorLabel, denominatorLabel;
	[SerializeField] int numerator, denominator;

	[SerializeField] List<GameObject> numerLineSegments;

	// Use this for initialization
	void Awake () {
		fraction = GetComponentInChildren<Transform> ().gameObject;

		TextMesh[] txt = GetComponentsInChildren<TextMesh> ();

		foreach (TextMesh tm in txt) {
			if (tm.CompareTag ("FractionLabelNumerator"))
				numeratorLabel = tm;
			else if (tm.CompareTag ("FractionLabelDenominator"))
				denominatorLabel = tm;
		}
	}

	void Start() {
		this.numerLineSegments = transform.parent.GetComponent<NumberLineSwitchClone> ().GetSegments ();
	}

	void Update() {
		foreach (GameObject segment in this.numerLineSegments) {
			if (segment.transform.position == this.gameObject.transform.position) {
				this.SetValue (this.numerLineSegments.IndexOf(segment), this.numerLineSegments.Count - 1);
			}
		}
	}
	
	public void SetValue(int numerator, int denominator) {
		this.numerator = numerator;
		this.denominator = denominator;

		numeratorLabel.text = this.numerator.ToString ();
		denominatorLabel.text = this.denominator.ToString ();
	}
}
