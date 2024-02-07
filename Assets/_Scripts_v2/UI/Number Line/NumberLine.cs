using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles basic functionality for a number line.
/// </summary>
public class NumberLine : MonoBehaviour {

    /// <summary>
    /// Reference to the line segment prefab.
    /// </summary>
	[SerializeField] private GameObject lineSegmentPrefab;
    /// <summary>
    /// Reference to the fraction label prefab.
    /// </summary>
	[SerializeField] private Fraction fractionPrefab;//Used for labeling the markers
    /// <summary>
    /// Parent object for the line segment.
    /// </summary>
	[SerializeField] private Transform segmentParent;
    /// <summary>
    /// Parent object for the fraction markers.
    /// </summary>
	[SerializeField] private Transform markerParent;

    /// <summary>
    /// Reference to a line renderer.
    /// </summary>
	protected LineRenderer lineRenderer;
    /// <summary>
    /// Line segments on the number line.
    /// </summary>
	protected List<GameObject> segments;
    /// <summary>
    /// Markers used to label the number line's segments.
    /// </summary>
	protected List<Fraction> markers;
    /// <summary>
    /// Number line's length.
    /// </summary>
	protected float lineLength;
    /// <summary>
    /// Max value the number line represents. It is the value of the right end of the number line.
    /// </summary>
	protected int maximum;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	protected void Awake() {
		this.segments = new List<GameObject> ();
		this.markers = new List<Fraction> ();
		this.lineRenderer = GetComponent<LineRenderer> ();
	}

    /// <summary>
    /// Unity Function. Called when the component is enabled.
    /// 
    /// Sets the line length.
    /// </summary>
	void OnEnable() {
		this.lineLength = lineRenderer.GetPosition(lineRenderer.positionCount-1).x - lineRenderer.GetPosition(0).x;
	}

    /// <summary>
    /// Destroy all line segments.
    /// </summary>
	public void PurgeSegments() {
		foreach (GameObject temp in segments) {
			Destroy (temp);
		}
		this.segments.Clear ();
	}

    /// <summary>
    /// Returns the max value of the number line.
    /// </summary>
    /// <returns>Max value</returns>
	public int GetMaxPoint() {
        if (this.maximum <= 0) {
            this.maximum = 1;
        }
		return this.maximum;
	}

    /// <summary>
    /// Sets the maximum value of the number line.
    /// </summary>
    /// <param name="maxPoint">Max Point</param>
	public void SetMaxPoint(int maxPoint) {
        // ADDED MAX
        if (maxPoint < 1)
            maxPoint = 1;
		this.maximum = maxPoint;
		Parameters parameters = new Parameters ();
		parameters.PutExtra (StabilityMaxPointLabelListener.NEW_MAX, this.maximum);
		EventBroadcaster.Instance.PostEvent (EventNames.STABILITY_MAX_CHANGE, parameters);
	}

    /// <summary>
    /// Adds a marker on the number line with a fraction value.
    /// </summary>
    /// <param name="numerator">Numerator</param>
    /// <param name="denominator">Denominator</param>
    /// <param name="notation">Fraction notation for how the marker is displayed.</param>
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

    /// <summary>
    /// Partitions the number line into equal segments.
    /// </summary>
    /// <param name="denominator">Denominator; the number of segments.</param>
	public void Segment(int denominator) {
        this.Partition(denominator * GetMaxPoint() + 1);
        // ADDED but before was also working
        //this.Partition(denominator * (GetMaxPoint() + 1));
    }

    //	public void Segment(int denominator) {
    //		this.Partition (denominator + 1);
    //	}
    
    public LineRenderer GetLineRenderer() {
		if (this.lineRenderer == null) {
			this.lineRenderer = GetComponent<LineRenderer> ();
		}
		return this.lineRenderer;
	}

	public float GetLineLength() {
		return this.lineLength;
	}
    
    /// <summary>
    /// Partitioning implementation.
    /// Partitions the number line into equal partitionCount.
    /// </summary>
    /// <param name="partitionCount">Number of Partitions</param>
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
