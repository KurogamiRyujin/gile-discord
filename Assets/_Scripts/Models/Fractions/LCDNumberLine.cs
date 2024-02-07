using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDNumberLine : MonoBehaviour {

	[SerializeField] private GameObject lineSegmentPrefab;
	[SerializeField] private GameObject lcdAnswerPointer;
	[SerializeField] private GameObject fractionLabel;

	private List<GameObject> segments;
	private float lineLength;
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private LineRenderer highlightRenderer;
//	private PauseController pauseController;

	void OnEnable() {
		GetSegments ();
		fractionLabel.SetActive (false);
	}

	// Use this for initialization
	void Start () {
//		lineRenderer = GetComponent<LineRenderer> ();
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
	}

	public void AnimateLCDResult(float numerator, float denominator) {
		StartCoroutine (MovePointer (numerator, denominator));
	}

	private IEnumerator MovePointer(float numerator, float denominator) {
		fractionLabel.SetActive (true);
//		pauseController.PauseGame ();
		GameController_v7.Instance.GetPauseController().Pause();
		int i = (int)numerator;

		if (numerator > denominator) {
			for (int j = 0; j < (numerator - denominator); j++) {
				ExtendLine ();
			}
		}

		PointerLabel label = lcdAnswerPointer.GetComponent<PointerLabel> ();

		float startTime = Time.unscaledTime;
		float journeyLength = Vector2.Distance (this.lcdAnswerPointer.transform.position, segments[i].transform.position);
		float distCovered;
		float fracJourney;
		lineRenderer.positionCount = 2;
		Vector3[] listPositions = new Vector3[2];

		float distanceMultiplier = 0.25f;

		Physics2D.autoSimulation = false;
		while (this.lcdAnswerPointer.transform.position != segments[i].transform.position) {
			distCovered = (Time.unscaledTime - startTime) * distanceMultiplier;
			fracJourney = distCovered / journeyLength;
			Physics2D.Simulate (fracJourney);		
			this.lcdAnswerPointer.transform.position = Vector3.MoveTowards (this.lcdAnswerPointer.transform.position, segments[i].transform.position, fracJourney);

			listPositions [0] = new Vector3 (this.transform.position.x, this.transform.position.y, -1.0f);
			listPositions [1] = new Vector3 (lcdAnswerPointer.transform.position.x, this.transform.position.y, -1.0f);//pointer.transform.position.y, (float)-0.1);
			highlightRenderer.SetPositions (listPositions);

			yield return null;
		}

		label.SetValue (segments [i].GetComponent<SegmentCollider> ().GetNumerator (), segments [i].GetComponent<SegmentCollider> ().GetDenominator ());

		for(int j = 0; j < 2; j++) {
			yield return new WaitForSecondsRealtime (1.0f);
		}

		while (this.lcdAnswerPointer.transform.position != this.segments [0].transform.position) {
			distCovered = (Time.unscaledTime - startTime) * distanceMultiplier;
			fracJourney = distCovered / journeyLength;
			Physics2D.Simulate (fracJourney);
			this.lcdAnswerPointer.transform.position = Vector3.MoveTowards (this.lcdAnswerPointer.transform.position, this.segments [0].transform.position, fracJourney);

			listPositions [0] = new Vector3 (this.transform.position.x, this.transform.position.y, -1.0f);
			listPositions [1] = new Vector3 (lcdAnswerPointer.transform.position.x, this.transform.position.y, -1.0f);//pointer.transform.position.y, (float)-0.1);
			highlightRenderer.SetPositions (listPositions);

			yield return null;
		}
		Physics2D.autoSimulation = true;
//		pauseController.ContinueGame ();
		GameController_v7.Instance.GetPauseController().Continue();
		Destroy (this.gameObject);
	}

	public void Segment(int denominator) {
		this.Partition (denominator + 1);
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

	public void SetLineLength(float length) {
		this.lineLength = length;
		//		float halfLength = length / 2.0f;
		Vector3[] listPositions = new Vector3[2];

		//		listPositions [0] = new Vector3 (-halfLength, 0f, 0f);
		//		listPositions [1] = new Vector3 (halfLength, 0f, 0f);
		listPositions [0] = new Vector3 (0f, 0f, 0f);
		listPositions [1] = new Vector3 (length, 0f, 0f);
		lineRenderer.SetPositions (listPositions);

	}

	public void ExtendLine() {
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
		segmentVal.SetValue ((prevSegment.GetNumerator()+1), prevSegment.GetDenominator());
		temp.transform.SetParent (gameObject.transform);

//		Vector2 locPos = new Vector2(numberLineBounds.center.x - (numberLineBounds.extents.x - (numberLineBounds.extents.x/partitionCount)) + prevPieceX, numberLineBounds.center.y);
		Vector2 locPos = new Vector2(segments [segments.Count - 1].transform.localPosition.x + prevPieceX, segments [segments.Count - 1].transform.localPosition.y);

		temp.transform.localPosition = locPos;
		lineRenderer.SetPosition (1, locPos);

		segments.Add (temp);
	}

	private void Partition(int partitionCount) {
		PurgeSegments ();
		GetSegments ();

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
			segmentVal.SetValue (x, partitionCount-1);
			temp.transform.SetParent (gameObject.transform);

			Bounds numberLineBounds = gameObject.GetComponent<LineRenderer> ().bounds;

			//			Vector2 locPos = new Vector2(numberLineBounds.center.x - (numberLineBounds.extents.x - (numberLineBounds.extents.x/partitionCount)) + prevPieceX, numberLineBounds.center.y);
			double currPos = this.gameObject.transform.position.x + prevPieceX;
			//			Vector2 locPos = new Vector2(Mathf.Round(currPos*10f)/10f, numberLineBounds.center.y);
			Vector2 locPos = new Vector2((float)System.Math.Round(currPos, 2), numberLineBounds.center.y);

			temp.transform.position = locPos;
			prevPieceX += (this.lineLength/(partitionCount-1));

			segments.Add (temp);
		}
	}
}
