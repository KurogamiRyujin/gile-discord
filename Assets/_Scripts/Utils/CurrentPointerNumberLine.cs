using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPointerNumberLine : MonoBehaviour {

	public float Y_OFFSET = 0.0f; //0.4f;//0.2f;
	public GameObject lineSegmentPrefab;
	public GameObject initialPointer;
	public GameObject smallerFraction;

	private List<GameObject> segments;
	private LineRenderer lineRenderer;

	[SerializeField] private float lineLength;
	private int multiplier = 1;
	private bool isPositiveMovement = true;

	void OnEnable() {
		this.transform.position = new Vector3 (initialPointer.transform.position.x, initialPointer.transform.position.y + Y_OFFSET, 0);
//		this.HideSmallerFraction ();
//		this.ShowSmallerFraction();
	}

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer> ();
//		this.HideSmallerFraction ();

//		this.gameObject.SetActive (false);
	}


	public void HideSmallerFraction() {
		Debug.Log ("<color=white>HideSmallerFraction</color>");
		this.transform.position = new Vector3 (initialPointer.transform.position.x, initialPointer.transform.position.y + Y_OFFSET, 0);
		this.smallerFraction.SetActive (false);
	}

	public void ShowSmallerFraction() {
		Debug.Log ("<color=white>ShowSmallerFraction</color>");
		this.transform.position = new Vector3 (initialPointer.transform.position.x, initialPointer.transform.position.y + Y_OFFSET, 0);
		this.smallerFraction.SetActive (true);
	}
	public void SetLineLength(float length) {
		this.lineLength = length;
		//		float halfLength = length / 2.0f;
		Vector3[] listPositions = new Vector3[2];

		//		listPositions [0] = new Vector3 (-halfLength, 0f, 0f);
		//		listPositions [1] = new Vector3 (halfLength, 0f, 0f);
		listPositions [0] = new Vector3 (0f, 0f, 0f);
		listPositions [1] = new Vector3 (multiplier * length, 0f, 0f);
		lineRenderer.SetPositions (listPositions);

	}

	public List<GameObject> GetSegments() {
		if (this.segments == null) {
			this.segments = new List<GameObject>();
		}
		return this.segments;
	}

	public void PurgeSegments() {
		foreach (GameObject temp in segments) {
			Destroy (temp);
		}
		this.segments = null;
	}

	public void ExtendLine() {
		segments = GetSegments ();

		GameObject temp;
		Vector2 pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		float prevPieceX = Vector3.Distance (segments [0].transform.localPosition, segments [1].transform.localPosition);

		temp = Instantiate (lineSegmentPrefab, pos, Quaternion.identity);
		temp.GetComponent<LineRenderer> ().sortingLayerName = "Clone Partition";
		SegmentCollider segmentVal = temp.GetComponent<SegmentCollider> ();
		SegmentCollider prevSegment = segments [segments.Count - 1].GetComponent<SegmentCollider> ();
	
		temp.SetActive (true);
		temp.name = "segment_"+(segments.Count - 1);
		temp.tag = "Number Line Segment";
		segmentVal.SetValue ((prevSegment.GetNumerator()+1) * multiplier, prevSegment.GetDenominator());
		temp.transform.SetParent (gameObject.transform);

//		Vector2 locPos = new Vector2(numberLineBounds.center.x - (numberLineBounds.extents.x - (numberLineBounds.extents.x/partitionCount)) + prevPieceX, numberLineBounds.center.y);
		Vector2 locPos = new Vector2(segments [segments.Count - 1].transform.localPosition.x + (prevPieceX * multiplier), segments [segments.Count - 1].transform.localPosition.y);

		temp.transform.localPosition = locPos;
		lineRenderer.SetPosition (1, locPos);
	
		segments.Add (temp);
	}

	public void Partition(int partitionCount) {
		segments = GetSegments ();

		GameObject temp;
		Vector2 pos;

		float prevPieceX = 0f;

		for(int x = 0; x < partitionCount; x++) {
			pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

			temp = Instantiate (lineSegmentPrefab, pos, Quaternion.identity);
			temp.GetComponent<LineRenderer> ().sortingLayerName = "Clone Partition";
			SegmentCollider segmentVal = temp.GetComponent<SegmentCollider> ();

			temp.SetActive (true);
			temp.name = "segment_"+x;
			temp.tag = "Number Line Segment";
			segmentVal.SetValue (multiplier * x, partitionCount-1);
			temp.transform.SetParent (gameObject.transform);

			Bounds numberLineBounds = gameObject.GetComponent<LineRenderer> ().bounds;

//			Vector2 locPos = new Vector2(numberLineBounds.center.x - (numberLineBounds.extents.x - (numberLineBounds.extents.x/partitionCount)) + prevPieceX, numberLineBounds.center.y);
//			Vector2 locPos = new Vector2(this.gameObject.transform.position.x + prevPieceX, numberLineBounds.center.y);
			double currPos = this.gameObject.transform.position.x + (prevPieceX * multiplier);
			//			Vector2 locPos = new Vector2(Mathf.Round(currPos*10f)/10f, numberLineBounds.center.y);\
			Vector2 locPos = new Vector2((float)System.Math.Round(currPos, 2), numberLineBounds.center.y);

			temp.transform.position = locPos;
			prevPieceX += (this.lineLength/(partitionCount-1));

			segments.Add (temp);
		}
	}

	public void IsPositiveMovement(bool flag) {
		this.isPositiveMovement = flag;

		if (isPositiveMovement)
			this.multiplier = 1;
		else
			multiplier = -1;
	}
}
