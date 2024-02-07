using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NumberLineSwitch : MonoBehaviour {
	private const float CORRECT_DELAY = 1.0f;
	private const float WRONG_DELAY = 1.0f;

	public GameObject lineSegmentPrefab;
	public GameObject targetPointer;
	public GameObject initialCurrentPointer;
	public GameObject pointer;
	public GameObject currentPointerNumberLineObject;//original number line

    public bool isNumberLineUpdated = false;

	private List<GameObject> segments;
	private float targetValNumerator, targetValDenominator;
	private float initialCurrentPointerNumerator, initialCurrentPointerDenominator;
	private float currentPointerNumerator, currentPointerDenominator;
	private EnemyHealth enemyHealth;
	private CurrentPointerNumberLine currentPointerNumberLine;
	[SerializeField] private LineRenderer highlightRenderer;
	[SerializeField] private LineRenderer lineRenderer;

	[SerializeField] private float lineLength;
	[SerializeField] private bool isPlaying = false;
	private bool isPositive = true;
    
    Tuple<Entry.Type, int> currentKey;
    EnemyEntry entry;
 

    // Use this for initialization
    void Start () {
//		this.Segment (testTargetDenominator);
//		this.TargetSetter (testTargetNumerator, testTargetDenominator);
//		this.PointerPointTo (testCurrentPointerNumerator, testCurrentPointerDenominator);

		enemyHealth = GetComponentInParent<EnemyHealth> ();
		currentPointerNumberLine = currentPointerNumberLineObject.GetComponent<CurrentPointerNumberLine> ();
//		highlightRenderer = pointer.GetComponentInChildren<LineRenderer> ();
		highlightRenderer.enabled = false;


//		lineRenderer = GetComponent<LineRenderer> ();
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
		int multiplier = 1;

		if (isPositive)
			multiplier = 1;
		else
			multiplier = -1;
		
		if (!this.isPlaying) {
			this.isPlaying = true;
            currentKey = new Tuple<Entry.Type, int>(Entry.Type.Enemy, PlayerData.enemyIndex);
            if (!DataManager.DoesKeyExist(currentKey))
            {
                entry = new EnemyEntry();
                Debug.Log("Created new empty entry");
                entry.name = "Enemy " + PlayerData.enemyIndex;
                entry.timeWeaponInteracted = new List<DateTime>();
                entry.timeWeaponInteracted.Add(System.DateTime.Now);
                entry.interactionCount++;

                Parameters parameters = new Parameters();
                parameters.PutExtra("TYPE", Entry.Type.Enemy.ToString());
                parameters.PutExtra("NUMBER", PlayerData.enemyIndex);
                DataManager.CreateEmptyEntry(parameters);
            }
            else
            {
                entry.timeWeaponInteracted.Add(System.DateTime.Now);
                entry.interactionCount++;
            }
            StartCoroutine (this.MovePointer (multiplier * numerator/* + this.currentPointerNumerator*/, denominator));
			Debug.Log ("Hammer Time!!!");
		}
		else {
			Debug.Log ("Hammer Time with Reservations!!!");
		}
//		}
	}

	private IEnumerator MovePointer(float numeratorDestination, float denominatorDestination) {
		bool isCorrect = false;

		highlightRenderer.enabled = true;
		highlightRenderer.sortingLayerName = "Needle";
		highlightRenderer.sortingOrder = (-1);
		highlightRenderer.SetPosition (0, pointer.transform.position);

		this.pointer.SetActive (true);
		this.pointer.GetComponent<PointerLabel> ().SetValue (0, (int) denominatorDestination);
		float pointerOriginalPosX = this.pointer.transform.localPosition.x;

		bool isPositiveMovement = (this.targetValNumerator > this.initialCurrentPointerNumerator);
		int multiplier = 1;
		if (isPositiveMovement)
			multiplier = 1;
		else
			multiplier = -1;

		this.currentPointerNumberLine.IsPositiveMovement (isPositiveMovement);
		this.currentPointerNumberLine.SetLineLength (this.lineLength);
//		this.currentPointerNumberLine.SetLineLength (lineRenderer.GetPosition (1).x);

		this.currentPointerNumberLine.gameObject.SetActive (true);
		this.currentPointerNumberLine.Partition ((int) denominatorDestination + 1);
//		float xCoords = this.segments[0].transform.localPosition.x + ((numeratorDestination * (Vector2.Distance(this.segments[0].transform.localPosition, this.segments[this.segments.Count-1].transform.localPosition)))/denominatorDestination);
//		float xCoords = this.currentPointerNumberLine.GetSegments()[0].transform.localPosition.x + ((numeratorDestination * (Vector2.Distance(this.currentPointerNumberLine.GetSegments()[0].transform.localPosition, this.currentPointerNumberLine.GetSegments()[this.currentPointerNumberLine.GetSegments().Count-1].transform.localPosition)))/denominatorDestination);
//		float xCoords = this.currentPointerNumberLine.GetSegments()[(int)numeratorDestination].transform.position.x;
//		float xCoords = this.currentPointerNumberLine.GetSegments()[0].transform.localPosition.x * numeratorDestination;
	
//		float xCoords = this.currentPointerNumberLine.GetSegments()[0].transform.localPosition.x + multiplier * (((numeratorDestination * (Vector2.Distance(this.currentPointerNumberLine.GetSegments()[0].transform.localPosition, this.currentPointerNumberLine.GetSegments()[this.segments.Count-1].transform.localPosition)))/denominatorDestination));
		float xCoords = this.currentPointerNumberLine.GetSegments()[0].transform.localPosition.x + multiplier * (numeratorDestination*Vector2.Distance(this.currentPointerNumberLine.GetSegments()[0].transform.position, this.currentPointerNumberLine.GetSegments()[1].transform.position));

//		Debug.Log ("xCoords: " + Vector2.Distance(this.segments[0].transform.localPosition, this.segments[this.segments.Count-1].transform.localPosition));

		float answer = (this.initialCurrentPointerNumerator / this.initialCurrentPointerDenominator) + ((numeratorDestination * multiplier) / denominatorDestination);
		if (Mathf.Approximately(answer, (this.targetValNumerator / this.targetValDenominator)))
			isCorrect = true;

		float startTime = Time.time;
		Vector2 pos = new Vector2 (xCoords, this.pointer.transform.localPosition.y);
		float journeyLength = Vector2.Distance (this.pointer.transform.localPosition, pos);
		PointerLabel currentPointerLabel = pointer.GetComponent<PointerLabel> ();
		int currentMaxNumerator = Mathf.Abs (currentPointerLabel.GetDenominator ());

		highlightRenderer.positionCount = 2;
		Vector3[] listPositions = new Vector3[2];

		while (pos.x != this.pointer.transform.localPosition.x) {
			float distCovered = (Time.time - startTime) * 0.25f;
			float fracJourney = distCovered / journeyLength;
//			this.pointer.transform.localPosition = Vector2.Lerp (this.pointer.transform.localPosition, pos, fracJourney);
			this.pointer.transform.localPosition = Vector2.MoveTowards(this.pointer.transform.localPosition, pos, fracJourney);



			//		listPositions [0] = new Vector3 (-halfLength, 0f, 0f);
			//		listPositions [1] = new Vector3 (halfLength, 0f, 0f);
			listPositions [0] = new Vector3 (currentPointerNumberLine.transform.position.x, 0f, -1.0f);
				//pointer.transform.position.y, (float)-0.1);
			listPositions [1] = new Vector3 (pointer.transform.position.x, 0f, -1.0f);//pointer.transform.position.y, (float)-0.1);
			highlightRenderer.SetPositions (listPositions);




//			highlightRenderer.SetPosition (0, new Vector3(currentPointerNumberLine.transform.position.x, 
//				pointer.transform.position.y, 
//				(float)-0.1));
//			highlightRenderer.SetPosition (1, new Vector3(pointer.transform.position.x, pointer.transform.position.y, (float)-0.1));




			Debug.Log ("Numerator: " + Mathf.Abs (currentPointerLabel.GetNumerator ()));
			Debug.Log ("Max Numerator: " + currentMaxNumerator);

			if (Mathf.Abs(currentPointerLabel.GetNumerator ()) >= currentMaxNumerator) {
				currentPointerNumberLine.ExtendLine ();
				currentMaxNumerator++;
			}

			yield return new WaitForSeconds (0.0f);
		}
		this.pointer.transform.localPosition = pos;
		this.pointer.GetComponent<PointerLabel> ().SetValue ((int) numeratorDestination, (int) denominatorDestination);

//		if (Mathf.Approximately (this.pointer.transform.position.x, this.targetPointer.transform.position.x)) {
		if (isCorrect) {

			StartCoroutine (this.UpdateHighlight ());
			yield return new WaitForSeconds (CORRECT_DELAY); // If CORRECT
			StopCoroutine (this.UpdateHighlight ());
            if (entry.isSimilar == null)
                entry.isSimilar = ((int)targetValDenominator == (int)denominatorDestination) ? true : false;
            if (entry.isProper == null)
                entry.isProper = ((int)numeratorDestination < (int)denominatorDestination) ? true : false;
            if (entry.initialValue == null)
                entry.initialValue = new Tuple<int, int>((int)initialCurrentPointerNumerator, (int)initialCurrentPointerDenominator);
            if (entry.targetValue == null)
                entry.targetValue = new Tuple<int, int>((int)targetValNumerator, (int)targetValDenominator);
            if (entry.actualAnswer == null)
                entry.actualAnswer = new Tuple<int, int>(Math.Abs((int)targetValNumerator - (int)initialCurrentPointerNumerator), (int)targetValDenominator);
            if (entry.attemptedAnswers == null)
                entry.attemptedAnswers = new List<Tuple<int, int>>();

            entry.isDeadThroughLCD = isNumberLineUpdated;
            entry.attemptedAnswers.Add(new Tuple<int, int>((int)numeratorDestination, (int)denominatorDestination));
            entry.numberOfAttempts++;
            entry.timeSolved = System.DateTime.Now;
            DataManager.UpdateEnemyEntry(DataManager.GetEnemyLastKey(), entry);
            PlayerData.Instance.IncrementEnemyIndex();
            enemyHealth.Death ();

        } else {
            isNumberLineUpdated = false;
            if (entry.isSimilar == null)
                entry.isSimilar = ((int)targetValDenominator == (int)denominatorDestination) ? true : false;
            if (entry.isProper == null)
                entry.isProper = ((int)numeratorDestination < (int)denominatorDestination) ? true : false;
            if (entry.initialValue == null)
                entry.initialValue = new Tuple<int, int>((int)initialCurrentPointerNumerator, (int)initialCurrentPointerDenominator);
            if (entry.targetValue == null)
                entry.targetValue = new Tuple<int, int>((int)targetValNumerator, (int)targetValDenominator);
            if (entry.actualAnswer == null)
                entry.actualAnswer = new Tuple<int, int>(Mathf.Abs((int)targetValNumerator - (int)initialCurrentPointerNumerator), (int)targetValDenominator);
            if (entry.attemptedAnswers == null)
                entry.attemptedAnswers = new List<Tuple<int, int>>();
            entry.isDeadThroughLCD = isNumberLineUpdated;
            entry.attemptedAnswers.Add(new Tuple<int, int>((int)numeratorDestination, (int)denominatorDestination));
            entry.numberOfAttempts++;
            StartCoroutine (this.UpdateHighlight ());
			yield return new WaitForSeconds (WRONG_DELAY); // If WRONG

			StopCoroutine (this.UpdateHighlight ());

			startTime = Time.time;
			pos = new Vector2 (pointerOriginalPosX, this.pointer.transform.localPosition.y);
			journeyLength = Vector2.Distance (this.transform.localPosition, pos);

			while (pos.x != this.pointer.transform.localPosition.x) {
				float distCovered = (Time.time - startTime) * 1.0f;
				float fracJourney = distCovered / journeyLength;
				this.pointer.transform.localPosition = Vector2.Lerp (this.pointer.transform.localPosition, pos, fracJourney);

//				highlightRenderer.positionCount = count+1;

				highlightRenderer.SetPosition (0, new Vector3(currentPointerNumberLine.transform.position.x, 
					pointer.transform.position.y, 
					(float)-0.1));
				highlightRenderer.SetPosition (1, pointer.transform.position);
//				highlightRenderer.SetPosition (0, new Vector3(pointer.transform.position.x, 
//					pointer.transform.position.y, 
//					(float)-0.1));
//				highlightRenderer.SetPosition (count, new Vector3(transform.position.x, transform.position.y, (float)-0.1));

				yield return new WaitForSeconds (0.0f);
			}
		}

		this.currentPointerNumberLine.PurgeSegments ();
		this.currentPointerNumberLine.gameObject.SetActive (false);
		this.pointer.SetActive (false);
		highlightRenderer.enabled = false;
		this.isPlaying = false;
        if (entry.timeWeaponRemoved == null)
            entry.timeWeaponRemoved = new List<DateTime>();
        entry.timeWeaponRemoved.Add(System.DateTime.Now);
    }

	private IEnumerator UpdateHighlight() {
		while (true) {
			highlightRenderer.SetPosition (0, new Vector3 (currentPointerNumberLine.transform.position.x, 
				pointer.transform.position.y, 
				(float)-0.1));
			highlightRenderer.SetPosition (1, new Vector3 (pointer.transform.position.x, pointer.transform.position.y, (float)-0.1));

			yield return null;
		}
	}

	public void Segment(int denominator) {
		this.Partition (denominator + 1);
	}

	public void TargetSetter(float numerator, float denominator) {
		this.targetValNumerator = numerator;
		this.targetValDenominator = denominator;
		this.targetPointer.GetComponent<PointerLabel> ().SetValue ((int) numerator,(int) denominator);

		this.targetPointer.transform.position = this.segments [(int) numerator].transform.position;
	}

	public void PointerPointTo(float numerator, float denominator) {
		this.pointer.GetComponent<PointerLabel> ().SetValue ((int) numerator,(int) denominator);

		this.currentPointerNumerator = numerator;
		this.currentPointerDenominator = denominator;

//		this.pointer.transform.localPosition = new Vector3 (this.currentPointerNumberLine.GetSegments () [(int)numerator].transform.localPosition.x, this.currentPointerNumberLine.GetSegments () [(int)numerator].transform.localPosition.y + 0.2f);
	}

	public void InitialPointerPointTo(float numerator, float denominator) {
		this.initialCurrentPointer.GetComponent<PointerLabel> ().SetValue ((int) numerator,(int) denominator);

		this.initialCurrentPointerNumerator = numerator;
		this.initialCurrentPointerDenominator = denominator;

		this.initialCurrentPointer.transform.position = this.segments [(int)numerator].transform.position;
	}

	public List<GameObject> GetSegments() {
		if (this.segments == null) {
			this.segments = new List<GameObject>();
		}
		return this.segments;
	}

	void OnEnable () {
		segments = new List<GameObject> ();
	}

	void OnDisable () {
		PurgeSegments ();
	}

	public void PurgeSegments() {
		foreach (GameObject temp in segments) {
			Destroy (temp);
		}
		this.segments.Clear ();
	}

	public void Partition(int partitionCount) {
//		segments = GetSegments ();

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

	public void IsPositive(bool flag) {
		this.isPositive = flag;
	}
}
