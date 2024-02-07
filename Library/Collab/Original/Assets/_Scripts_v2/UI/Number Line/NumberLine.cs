using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLine : MonoBehaviour {

	[SerializeField] private GameObject lineSegmentPrefab;
	[SerializeField] private Fraction fractionPrefab;//Used for labeling the markers
	[SerializeField] private Transform segmentParent;
	[SerializeField] private Transform markerParent;

	protected LineRenderer lineRenderer;
	protected List<GameObject> segments;
	protected List<Fraction> markers;
	protected float lineLength;
	protected int maximum;

	void Awake() {
		this.segments = new List<GameObject> ();
		this.markers = new List<Fraction> ();
		this.lineRenderer = GetComponent<LineRenderer> ();
	}

	void OnEnable() {
		this.lineLength = lineRenderer.GetPosition(lineRenderer.positionCount-1).x - lineRenderer.GetPosition(0).x;
	}

	public void PurgeSegments() {
		foreach (GameObject temp in segments) {
			Destroy (temp);
		}
		this.segments.Clear ();
	}

	public void SetMaxPoint(int maxPoint) {
		this.maximum = maxPoint;
	}

	public void AddMarker(int numerator, int denominator, Fraction.Notation notation) {
		Fraction temp = Instantiate<Fraction> (fractionPrefab, markerParent);
		float fNumerator = numerator;
		float fDenominator = denominator;
		float fValue = fNumerator / fDenominator;
		Debug.Log ("fvalue: " + fValue);
		float xPosition = (fValue / maximum) * this.lineLength;

		Vector2 pos = new Vector2 (xPosition, temp.transform.localPosition.y);

		temp.transform.localPosition = pos;
		temp.SetNotation (notation);
		temp.SetValue (numerator, denominator);
	}

	public void Segment(int denominator) {
		this.Partition (denominator + 1);
	}

	public void Partition(int partitionCount) {
		PurgeSegments ();

		GameObject temp;
		Vector2 pos;

		float prevPieceX = 0f;

		for(int x = 0; x < partitionCount; x++) {
			temp = Instantiate (lineSegmentPrefab, segmentParent);
			temp.GetComponent<LineRenderer> ().sortingLayerName = "Clone Partition";

			temp.SetActive (true);
			temp.name = "segment_"+x;
			temp.tag = "Number Line Segment";

			Bounds numberLineBounds = gameObject.GetComponent<LineRenderer> ().bounds;

			double currPos = this.gameObject.transform.position.x + prevPieceX;
//			Vector2 locPos = new Vector2((float)System.Math.Round(currPos, 2), numberLineBounds.center.y);

			Vector2 locPos = new Vector2 (0.0f + prevPieceX, 0.0f);

			temp.transform.localPosition = locPos;
			prevPieceX += (this.lineLength/(partitionCount-1));

			segments.Add (temp);
		}
	}
}
