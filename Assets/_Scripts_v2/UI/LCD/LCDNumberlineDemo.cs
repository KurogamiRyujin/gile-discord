using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LCDNumberlineDemo : MonoBehaviour {

	[SerializeField] private GameObject segmentParent;
	[SerializeField] private GameObject lineSegmentPrefab;
	[SerializeField] private PointerLabel targetPointer;
	[SerializeField] private PointerLabel initialCurrentPointer;
	[SerializeField] private float overallPresentationTime = 3.0f;
	[SerializeField] private float correctDelay = 2.0f;
	[SerializeField] private GameObject healthBackMid;
	[SerializeField] private GameObject healthBackLeft;
	[SerializeField] private GameObject healthBackRight;
	[SerializeField] private LCDYarnball lcdYarnball;
	private float segmentHeight = 0.16f;
	private LineRenderer lineRenderer;
	private int targetValNumerator, targetValDenominator;
	private int initialCurrentPointerNumerator, initialCurrentPointerDenominator;
	private float lineLength;

	private List<GameObject> segments;
	private PartitioningParent parent;
	// Use this for initialization
	void Awake () {
		this.segments = new List<GameObject> ();
		this.lineRenderer = GetComponent<LineRenderer> ();
	}

	public void Deactivate () {
		PurgeSegments ();
//		this.gameObject.GetComponentInParent<Transform>().gameObject.SetActive (false);
		if(this.GetPartitioningParent() != null)
			this.GetPartitioningParent().gameObject.SetActive(false);
//		this.gameObject.SetActive (false);
//		this.lcdYarnball.gameObject.SetActive (false);
	}
		
	public void Activate() {
//		this.gameObject.GetComponentInParent<Transform>().gameObject.SetActive (true);
		this.GetPartitioningParent().gameObject.SetActive(true);
//		this.gameObject.SetActive (true);
//		this.lcdYarnball.gameObject.SetActive (true);
	}

	public PartitioningParent GetPartitioningParent() {
		if (this.parent == null) {
			this.parent = GetComponentInParent<PartitioningParent>();
		}
		return this.parent;
	}

	public LineRenderer GetLineRenderer() {
		if (this.lineRenderer == null) {
			this.lineRenderer = GetComponent<LineRenderer> ();
		}
		return this.lineRenderer;
	}

	public void SetLineLength(float length) {
		this.lineLength = length;
//		this.lineLength = length;
//		this.lineLength = 4f; // TODO
//		Vector3[] listPositions = new Vector3[2];

//		listPositions [0] = new Vector3 (0f, 0f, 0f);
//		listPositions [1] = new Vector3 (this.lineLength, 0f, 0f);
//		this.GetLineRenderer().SetPositions (listPositions);

//		this.healthBackRight.transform.localPosition = new Vector3(this.lineLength, healthBackRight.transform.localPosition.y, healthBackRight.transform.localPosition.z);

		// Scaling according to length, consider making a function
//		float currentSize = healthBackMid.GetComponent<SpriteRenderer> ().bounds.size.x-healthBackRight.GetComponent<SpriteRenderer>().bounds.size.x;
//		healthBackMid.transform.localScale = General.LengthToScale(currentSize, healthBackMid.transform.localScale, this.lineLength);

//		healthBackMid.transform.localScale = General.LengthToScale(currentSize, healthBackMid.transform.localScale, this.lineLength);
	}

	public void TargetSetter(int numerator, int denominator) {
		this.targetValNumerator = numerator;
		this.targetValDenominator = denominator;
		this.targetPointer.SetValue (numerator, denominator);

		this.targetPointer.gameObject.transform.position = this.segments [numerator].transform.position;
	}

	public void InitialPointerPointTo(int numerator, int denominator) {
		this.initialCurrentPointer.SetValue (numerator, denominator);

		this.initialCurrentPointerNumerator = numerator;
		this.initialCurrentPointerDenominator = denominator;

		this.initialCurrentPointer.gameObject.transform.position = this.segments [numerator].transform.position;
	}

	public void Segment(int denominator) {
		this.Partition (denominator + 1);
	}

	private void Partition(int partitionCount) {
		GameObject temp;
		Vector2 pos;

		float prevPieceX = 0f;
		LineRenderer lineRenderer;
		Vector3 position;
		for(int x = 0; x < partitionCount; x++) {
			pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

			temp = Instantiate (lineSegmentPrefab, pos, Quaternion.identity);
			lineRenderer = temp.GetComponent<LineRenderer> ();
//			lineRenderer.sortingLayerName = "Clone Partition";
		
			position = lineRenderer.GetPosition (0);
			lineRenderer.SetPosition (0, new Vector3 (position.x, segmentHeight, position.z));

			position = lineRenderer.GetPosition (1);
			lineRenderer.SetPosition (1, new Vector3 (position.x, -segmentHeight, position.z));

			temp.SetActive (true);
			temp.name = "segment_"+x;
			temp.tag = "Number Line Segment";
			temp.transform.SetParent (segmentParent.transform);
			temp.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			Vector2 locPos = new Vector2 (0.0f + prevPieceX, 0.0f);

			temp.transform.localPosition = locPos;
			prevPieceX += (this.lineLength/(partitionCount-1));

			segments.Add (temp);
		}
	}

	public void ApplyLCD(int lcd) {
		this.lcdYarnball.SetLCD (lcd);
		StartCoroutine (LCDApplication (lcd + 1));
	}

	private IEnumerator LCDApplication(int partitions) {
		GameObject temp;
		Vector2 pos;

		float prevPieceX = 0f;

		float timePerPiece = overallPresentationTime / partitions;

		LineRenderer tempLineRenderer;
		Vector3 position;
	
//		float rotation;
		for(int x = 0; x < partitions; x++) {
			pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

			temp = Instantiate (lineSegmentPrefab, pos, Quaternion.identity);
//			temp.GetComponent<LineRenderer> ().sortingLayerName = "Clone Partition";

			temp.SetActive (true);
			temp.name = "new_segment_"+x;
			temp.tag = "Number Line Segment";
			temp.transform.SetParent (segmentParent.transform);

			tempLineRenderer = temp.GetComponent<LineRenderer> ();

			position = tempLineRenderer.GetPosition (0);
			tempLineRenderer.SetPosition (0, new Vector3 (position.x, segmentHeight, position.z));

			position = tempLineRenderer.GetPosition (1);
			tempLineRenderer.SetPosition (1, new Vector3 (position.x, -segmentHeight, position.z));


			temp.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

			Vector2 locPos = new Vector2 (0.0f + prevPieceX, 0.0f);

			temp.transform.localPosition = locPos;
			prevPieceX += (this.lineLength/(partitions-1));

//			rotation = x/partitions;
//			this.lcdYarnball.ThreadTowards (temp.transform.position);
			if (temp.transform.position == this.initialCurrentPointer.gameObject.transform.position)
				this.initialCurrentPointer.SetValue (x, partitions - 1);
			if (temp.transform.position == this.targetPointer.gameObject.transform.position)
				this.targetPointer.SetValue (x, partitions - 1);

			segments.Add (temp);
			yield return StartCoroutine (lcdYarnball.ThreadEffects(temp.transform.position));
//			yield return new WaitForSecondsRealtime (timePerPiece);
//			yield return new WaitForSecondsRealtime (3f);
		}
		SoundManager.Instance.Play (AudibleNames.Results.SUCCESS, false);
		yield return new WaitForSecondsRealtime (correctDelay);
		Deactivate ();
		GameController_v7.Instance.GetPauseController ().Continue ();
	}
	private List<GameObject> GetSegments() {
		if (this.segments == null) {
			this.segments = new List<GameObject> ();
		}
		return this.segments;
	}

	private void PurgeSegments() {
		for (int i = 0; i < this.GetSegments().Count; i++) {
			Destroy (this.segments [i].gameObject);
		}

		this.segments.Clear ();
	}
}
