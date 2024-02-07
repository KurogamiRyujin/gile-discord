using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NumberLineSwitch : MonoBehaviour {
	private const float CORRECT_DELAY = 1.0f;
	private const float WRONG_DELAY = 1.0f;
	private const float POINTER_MOVEMENT_SPEED = 1.0f;

	public GameObject segmentParent;
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
	[SerializeField] private Transform highlightTransform;
	[SerializeField] private LineRenderer lineRenderer;

	[SerializeField] private float lineLength;
	[SerializeField] private bool isPlaying = false;

	[SerializeField] private GameObject healthBackMid;
	[SerializeField] private GameObject healthBackLeft;
	[SerializeField] private GameObject healthBackRight;
	private bool isPositive = true;
    
//    Tuple<Entry.Type, int> currentKey;
//    EnemyEntry entry;

//	private PauseController pauseController;
	private CameraZoom cameraZoom;
 

	[SerializeField] private ResultsUI resultsUI;
	private ResultsPosition resultsPosition;

    // Use this for initialization
    void Start () {
//		this.Segment (testTargetDenominator);
//		this.TargetSetter (testTargetNumerator, testTargetDenominator);
//		this.PointerPointTo (testCurrentPointerNumerator, testCurrentPointerDenominator);

		this.resultsUI = GetComponentInChildren<ResultsUI> ();
		enemyHealth = GetComponentInParent<EnemyHealth> ();
		currentPointerNumberLine = currentPointerNumberLineObject.GetComponent<CurrentPointerNumberLine> ();
//		highlightRenderer = pointer.GetComponentInChildren<LineRenderer> ();
		highlightRenderer.enabled = false;

//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();

		cameraZoom = Camera.main.GetComponentInChildren<CameraZoom> ();
		currentPointerNumberLine.HideSmallerFraction ();
//		lineRenderer = GetComponent<LineRenderer> ();
	}

	public ResultsUI GetResultsUI() {
		if (this.resultsUI == null) {
			this.resultsUI = GetComponentInChildren<ResultsUI> ();
		}
		return this.resultsUI;
	}

	public void SetLineLength(float length) {
		this.lineLength = length; 
//		float halfLength = length / 2.0f;

		Debug.Log ("LENGTH IS "+length);
		Vector3[] listPositions = new Vector3[2];

//		listPositions [0] = new Vector3 (-halfLength, 0f, 0f);
//		listPositions [1] = new Vector3 (halfLength, 0f, 0f);
		listPositions [0] = new Vector3 (0f, 0f, 0f);
		listPositions [1] = new Vector3 (length, 0f, 0f);
		lineRenderer.SetPositions (listPositions);
		lineRenderer.numCapVertices = 90;
		lineRenderer.numCornerVertices = 90;


		this.healthBackRight.transform.localPosition = new Vector3(length, healthBackRight.transform.localPosition.y, healthBackRight.transform.localPosition.z);


		// Scaling according to length, consider making a function
		float currentSize = healthBackMid.GetComponent<SpriteRenderer> ().bounds.size.x;
//		Vector3 scale = healthBackMid.transform.localScale;
//		Debug.Log (length);
//		Debug.Log (currentSize);
//		Debug.Log (scale);
//		scale.x = length * scale.x / currentSize;
	
		healthBackMid.transform.localScale = General.LengthToScale(currentSize, healthBackMid.transform.localScale, length);
		Vector3 lineCenter = this.GetResultsPosition ().gameObject.transform.position;
		this.GetResultsUI ().gameObject.transform.position = new Vector3 (lineCenter.x, lineCenter.y, this.GetResultsUI ().gameObject.transform.position.z);
	}
	
	public ResultsPosition GetResultsPosition() {
		if (this.resultsPosition == null) {
			this.resultsPosition = GetComponentInChildren<ResultsPosition> ();
		}
		return this.resultsPosition;
	}

	public void SetHealthBackgroundImages(float length) {
		this.healthBackRight.transform.position = new Vector3(length, healthBackRight.transform.position.y, healthBackRight.transform.position.z);
		this.healthBackMid.transform.localScale = new Vector3(length, healthBackMid.transform.localScale.y);
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
//            currentKey = new Tuple<Entry.Type, int>(Entry.Type.Enemy, PlayerData.enemyIndex);
//            if (!DataManager.DoesKeyExist(currentKey))
			if(enemyHealth.GetEnemyKey() == null)
            {
//                entry = new EnemyEntry();
//                Debug.Log("Created new empty entry");
//                entry.name = "Enemy " + PlayerData.enemyIndex;
//                entry.timeWeaponInteracted = new List<DateTime>();
//                entry.timeWeaponInteracted.Add(System.DateTime.Now);
//                entry.interactionCount++;

//                Parameters parameters = new Parameters();
//                parameters.PutExtra("TYPE", Entry.Type.Enemy.ToString());
//                parameters.PutExtra("NUMBER", PlayerData.enemyIndex);
//                DataManager.CreateEmptyEntry(parameters);
//                enemyHealth.SetEnemyKey(currentKey);
//                PlayerData.Instance.IncrementEnemyIndex();
            }
            else
            {
                // TODO: commented this out 
//                entry = DataManager.GetEnemyEntry(enemyHealth.GetEnemyKey());
//                entry.timeWeaponInteracted.Add(System.DateTime.Now);
//                entry.interactionCount++;
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

//		pauseController.PauseGame ();
		GameController_v7.Instance.GetPauseController ().Pause ();
//		cameraZoom.ZoomTowards (gameObject.transform.position);
		this.AdjustCamera();
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
	
		this.currentPointerNumberLine.ShowSmallerFraction ();

		this.currentPointerNumberLine.Partition ((int) denominatorDestination + 1);
		float xCoords = this.currentPointerNumberLine.GetSegments()[0].transform.localPosition.x + multiplier * (numeratorDestination*Vector2.Distance(this.currentPointerNumberLine.GetSegments()[0].transform.position, this.currentPointerNumberLine.GetSegments()[1].transform.position));
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

		Physics2D.autoSimulation = false;
		while (pos.x != this.pointer.transform.localPosition.x) {
			Physics2D.Simulate (POINTER_MOVEMENT_SPEED * Time.unscaledDeltaTime);
			this.pointer.transform.localPosition = Vector2.MoveTowards(this.pointer.transform.localPosition, pos, POINTER_MOVEMENT_SPEED * Time.unscaledDeltaTime);

			listPositions [0] = new Vector3 (currentPointerNumberLine.GetComponent<LineRenderer>().GetPosition(0).x, highlightTransform.localPosition.y, -1.0f);

			listPositions [1] = new Vector3 (pointer.transform.localPosition.x, highlightTransform.localPosition.y, -1.0f);//pointer.transform.position.y, (float)-0.1);
			highlightRenderer.SetPositions (listPositions);



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

		Physics2D.autoSimulation = true;
//		GameController_v7.Instance.GetPauseController ().Continue ();
//		cameraZoom.shouldZoomIn = false;
//		cameraZoom.shouldZoomOut = true;
	
//		if (Mathf.Approximately (this.pointer.transform.position.x, this.targetPointer.transform.position.x)) {
		if (isCorrect) {

//			StartCoroutine (this.UpdateHighlight ());
//			yield return new WaitForSeconds (CORRECT_DELAY); // If CORRECT
			yield return new WaitForSecondsRealtime(CORRECT_DELAY);
			this.resultsUI.PlaySuccess();
//			GameController_v7.Instance.GetPauseController ().Continue ();
//			StopCoroutine (this.UpdateHighlight ());

			// TODO: uncomment on tally fix :(
//            if (entry.isSimilar == null)
//                entry.isSimilar = ((int)targetValDenominator == (int)denominatorDestination) ? true : false;
//            if (entry.isProper == null)
//                entry.isProper = ((int)numeratorDestination < (int)denominatorDestination) ? true : false;
//            if (entry.initialValue == null)
//                entry.initialValue = new Tuple<int, int>((int)initialCurrentPointerNumerator, (int)initialCurrentPointerDenominator);
//            if (entry.targetValue == null)
//                entry.targetValue = new Tuple<int, int>((int)targetValNumerator, (int)targetValDenominator);
//            if (entry.actualAnswer == null)
//                entry.actualAnswer = new Tuple<int, int>(Math.Abs((int)targetValNumerator - (int)initialCurrentPointerNumerator), (int)targetValDenominator);
//            if (entry.attemptedAnswers == null)
//                entry.attemptedAnswers = new List<Tuple<int, int>>();
//
//            entry.isDeadThroughLCD = isNumberLineUpdated;
//            entry.attemptedAnswers.Add(new Tuple<int, int>((int)numeratorDestination, (int)denominatorDestination));
//            entry.numberOfAttempts++;
//            entry.timeSolved = System.DateTime.Now;
//			if (initialCurrentPointerDenominator == targetValDenominator)
//				entry.topic = EnemyEntry.Topic.Similar;
//			else
//				entry.topic = EnemyEntry.Topic.Dissimilar;

            enemyHealth.Death ();
			yield return new WaitForSecondsRealtime(1.0f);
			this.ReturnCamera ();
			GameController_v7.Instance.GetPauseController ().Continue ();

        } else {
			// TODO: temporarily commented out, conflict when hitting 2 enemies :'(
            isNumberLineUpdated = false;
//            if (entry.isSimilar == null)
//                entry.isSimilar = ((int)targetValDenominator == (int)denominatorDestination) ? true : false;
//            if (entry.isProper == null)
//                entry.isProper = ((int)numeratorDestination < (int)denominatorDestination) ? true : false;
//            if (entry.initialValue == null)
//                entry.initialValue = new Tuple<int, int>((int)initialCurrentPointerNumerator, (int)initialCurrentPointerDenominator);
//            if (entry.targetValue == null)
//                entry.targetValue = new Tuple<int, int>((int)targetValNumerator, (int)targetValDenominator);
//            if (entry.actualAnswer == null)
//                entry.actualAnswer = new Tuple<int, int>(Mathf.Abs((int)targetValNumerator - (int)initialCurrentPointerNumerator), (int)targetValDenominator);
//            if (entry.attemptedAnswers == null)
//                entry.attemptedAnswers = new List<Tuple<int, int>>();
//            entry.isDeadThroughLCD = isNumberLineUpdated;
//            entry.attemptedAnswers.Add(new Tuple<int, int>((int)numeratorDestination, (int)denominatorDestination));
//            entry.numberOfAttempts++;


//            StartCoroutine (this.UpdateHighlight ());
			SoundManager.Instance.Play(AudibleNames.Results.MISTAKE, false);
			yield return new WaitForSecondsRealtime (WRONG_DELAY); // If WRONG
			this.ReturnCamera ();
			GameController_v7.Instance.GetPauseController ().Continue ();
//			cameraZoom.shouldZoomIn = false;
//			cameraZoom.shouldZoomOut = true;
//			StopCoroutine (this.UpdateHighlight ());

			startTime = Time.time;
			pos = new Vector2 (pointerOriginalPosX, this.pointer.transform.localPosition.y);
			journeyLength = Vector2.Distance (this.transform.localPosition, pos);
			listPositions = new Vector3[2];
			while (pos.x != this.pointer.transform.localPosition.x) {
				this.pointer.transform.localPosition = Vector2.MoveTowards (this.pointer.transform.localPosition, pos, 2*POINTER_MOVEMENT_SPEED * Time.deltaTime);
				// Pointer movement speed of returning

				listPositions [0] = new Vector3 (currentPointerNumberLine.GetComponent<LineRenderer>().GetPosition(0).x, highlightTransform.localPosition.y, -1.0f);
				listPositions [1] = new Vector3 (pointer.transform.localPosition.x, highlightTransform.localPosition.y, highlightRenderer.transform.position.z);//pointer.transform.position.y, (float)-0.1);
				highlightRenderer.SetPositions (listPositions);

				yield return new WaitForSeconds (0.0f);
			}
		}

		this.currentPointerNumberLine.PurgeSegments ();
//		this.currentPointerNumberLine.gameObject.SetActive (false);
		this.currentPointerNumberLine.HideSmallerFraction ();
//		this.pointer.SetActive (false);
		highlightRenderer.enabled = false;
		this.isPlaying = false;
//        if (entry.timeWeaponRemoved == null)
//            entry.timeWeaponRemoved = new List<DateTime>();
//        entry.timeWeaponRemoved.Add(System.DateTime.Now);

//		DataManager.UpdateEnemyEntry(DataManager.GetEnemyLastKey(), entry);
//		Debug.Log ("<b>NUMBER OF ATTEMPTS: " + String.Join(",",DataManager.GetIntColumn (StringConstants.TableNames.ENEMY, StringConstants.ColumnNames.NUMBER_OF_ATTEMPTS)) + "</b>");



    }
	public void AdjustCamera() {
		Vector3 zoomPosition = gameObject.transform.position;
		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_CAMERA);
		Parameters data = new Parameters ();
		data.PutExtra ("x", zoomPosition.x);
		data.PutExtra ("y", zoomPosition.y);
		EventBroadcaster.Instance.PostEvent (EventNames.ZOOM_CAMERA_TOWARDS, data);
	}
	public void ReturnCamera() {
		Parameters data = new Parameters ();
		data.PutExtra ("shouldZoomIn", false);

		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);
		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);
	}
//	private IEnumerator UpdateHighlight() {
//		while (true) {
//			highlightRenderer.SetPosition (0, new Vector3 (currentPointerNumberLine.transform.position.x, 
////				0.0f, //currentPointerNumberLineObject.GetComponent<LineRenderer>().GetPosition(0).y,//
////				pointer.transform.position.y, 
//				highlightTransform.position.y,
//				(float)-0.1));
//			highlightRenderer.SetPosition (1, new Vector3 (pointer.transform.position.x,
//				//0.0f, //currentPointerNumberLineObject.GetComponent<LineRenderer>().GetPosition(0).y, // 
//				//pointer.transform.position.y,
//				highlightTransform.position.y,
//				(float)-0.1));
//
//			yield return null;
//		}
//	}

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
	void OnDestroy() {
		this.ReturnCamera ();
		GameController_v7.Instance.GetPauseController ().Continue ();
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
//			SegmentCollider segmentVal = temp.GetComponent<SegmentCollider> ();

			temp.SetActive (true);
			temp.name = "segment_"+x;
			temp.tag = "Number Line Segment";
//			segmentVal.SetValue (x, partitionCount-1);
			temp.transform.SetParent (segmentParent.transform);

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
