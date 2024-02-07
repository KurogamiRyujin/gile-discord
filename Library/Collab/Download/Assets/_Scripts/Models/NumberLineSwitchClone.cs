using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberLineSwitchClone : MonoBehaviour {


	public GameObject lineSegmentPrefab;
	public GameObject targetPointer;
	public GameObject pointer;
	public GameObject currentPointerNumberLineObject;

	private List<GameObject> segments;
	private float targetValNumerator, targetValDenominator;
	private float currentPointerNumerator, currentPointerDenominator;
	private EnemyHealth enemyHealth;
	private CurrentPointerNumberLine currentPointerNumberLine;


	private GameObject numberLineObject;
	public float testTargetNumerator = 4;
	public float testTargetDenominator = 5;
	public float testCurrentPointerNumerator = 2;
	public float testCurrentPointerDenominator = 5;


	// Use this for initialization
	void Start () {
//		this.Segment ((int) testTargetDenominator);
		//		this.TargetSetter (testTargetNumerator, testTargetDenominator);
		//		this.PointerPointTo (testCurrentPointerNumerator, testCurrentPointerDenominator);
		enemyHealth = GetComponentInParent<EnemyHealth> ();
		currentPointerNumberLine = currentPointerNumberLineObject.GetComponent<CurrentPointerNumberLine> ();
	}

	// Update is called once per frame
	//	void Update () {
	//		if (Input.GetKeyDown (KeyCode.X)) {
	//			HammerDown (1, 5);
	//
	//		}
	//	}

	public void HammerDown(float numerator, float denominator) {
		//		if (this.currentPointerDenominator != denominator) {
		//
		//		} else {
		//			StartCoroutine (this.MovePointer (numerator + this.currentPointerNumerator, this.currentPointerDenominator));
//		StartCoroutine (this.HitNumberLine (numerator + this.currentPointerNumerator, this.currentPointerDenominator));

		Debug.Log ("Hammer Time Clone!!!");
		//		}
	}


	private IEnumerator HitNumberLine(float numeratorDestination, float denominatorDestination) {
		//		if (this.numberLineObject != null)
//		Destroy (this.numberLineObject);
//
//
//		this.numberLineObject = Instantiate (this.numberLineClonePrefab, this.gameObject.transform.position, Quaternion.identity);
//
//
//		//		this.numberLineSwitchClone = this.numberLineClonePrefab.GetComponent<NumberLineSwitchClone> ();
//		//		this.numberLineSwitchClone.testTargetDenominator = 20;
//
//		//		numberLineSwitchClone = numberLineObject.GetComponent<NumberLineSwitchClone> ();
//		//		numberLineSwitchClone.Segment ((int) numberLineSwitchClone.testTargetDenominator);
//		//		numberLineSwitchClone.TargetSetter (numberLineSwitchClone.testTargetNumerator, numberLineSwitchClone.testTargetDenominator);
//		//		numberLineSwitchClone.PointerPointTo (numberLineSwitchClone.testCurrentPointerNumerator, numberLineSwitchClone.testCurrentPointerDenominator);
//
//
//		numberLineObject.transform.position = this.gameObject.transform.position;
//		numberLineObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.numberLineCloneY, this.gameObject.transform.position.z);
//
//
//		numberLineObject.transform.SetParent (this.gameObject.transform);
//
//		numberLineSwitchClone = numberLineObject.GetComponent<NumberLineSwitchClone> ();
//		numberLineSwitchClone.Segment ((int) testTargetDenominator);
//		numberLineSwitchClone.TargetSetter (testTargetNumerator, testTargetDenominator);
//		numberLineSwitchClone.PointerPointTo (testCurrentPointerNumerator, testCurrentPointerDenominator);


		yield return null;
	}

	public void Segment(int denominator) {
		Debug.Log ("SEGMENT CLONE");
		this.Partition (denominator + 1);
	}

	public void TargetSetter(float numerator, float denominator) {
		this.targetPointer.GetComponent<PointerLabel> ().SetValue ((int) numerator,(int) denominator);

		this.targetPointer.transform.position = this.segments [(int) numerator].transform.position;
	}

	public void PointerPointTo(float numerator, float denominator) {
		this.pointer.GetComponent<PointerLabel> ().SetValue ((int) numerator,(int) denominator);

		this.currentPointerNumerator = numerator;
		this.currentPointerDenominator = denominator;

		this.pointer.transform.position = this.segments [(int) numerator].transform.position;
	}

	public List<GameObject> GetSegments() {
		if (this.segments == null) {
			this.segments = new List<GameObject>();
		}
		return this.segments;
	}

	void OnEnable() {
		segments = new List<GameObject> ();
	}

	void OnDestroy() {
		foreach (GameObject temp in segments) {
			Destroy (temp);
		}
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

			temp.SetActive (true);
			temp.name = "segment_"+x;
			temp.tag = "Number Line Segment";
			temp.transform.SetParent (gameObject.transform);

			Bounds numberLineBounds = gameObject.GetComponent<LineRenderer> ().bounds;

			Vector2 locPos = new Vector2(numberLineBounds.center.x - (numberLineBounds.extents.x - (numberLineBounds.extents.x/partitionCount)) + prevPieceX, numberLineBounds.center.y);

			temp.transform.position = locPos;
			prevPieceX += (numberLineBounds.size.x/partitionCount);

			segments.Add (temp);
		}
	}
}
