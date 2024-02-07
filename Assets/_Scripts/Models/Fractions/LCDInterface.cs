using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LCDInterface : MonoBehaviour {

	private const int UNKNOWN_VALUE = 9999;

	[SerializeField] private TextMesh initialNumeratorLeft;
	[SerializeField] private TextMesh initialDenominatorLeft;
	[SerializeField] private TextMesh numeratorLeft;
	[SerializeField] private TextMesh denominatorLeft;
	[SerializeField] private TextMesh initialNumeratorRight;
	[SerializeField] private TextMesh initialDenominatorRight;
	[SerializeField] private TextMesh numeratorRight;
	[SerializeField] private TextMesh denominatorRight;

//	private PauseController pauseController;
	private EnemyHealth enemyHealth;
	private LCDNumberLine lcdNumberLine;
	private Yarnball yarnball;
	private HammerObject hammer;

	private int originalTargetNumerator = 0;
	private int originalTargetDenominator = 0;

	private int originalInitialNumerator = 0;
	private int originalInitialDenominator = 0;

	private int correctTargetNumerator = 0;
	private int targetNumerator = 0;
	private int targetDenominator = 0;

	private int initialNumerator = 0;
	private int initialDenominator = 0;

	private bool initiated = false;
	[SerializeField] private bool isInitialFractionLeft;
//	private CameraZoom cameraZoom;

//    Tuple<Entry.Type, int> currentKey;
//    LCDEntry entry;
//	EnemyEntry enemyEntry;

    // Use this for initialization
    void Start () {
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
//		pauseController.PauseGame ();

		GameController_v7.Instance.GetPauseController().Pause();

		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_CAMERA);
		Parameters data = new Parameters ();
		data.PutExtra ("x", gameObject.transform.position.x);
		data.PutExtra ("y", gameObject.transform.position.y);
		EventBroadcaster.Instance.PostEvent (EventNames.ZOOM_CAMERA_TOWARDS, data);

//		Camera.main.GetComponent<CameraController>().enabled = false;
//		cameraZoom = Camera.main.GetComponentInChildren<CameraZoom> ();
//		cameraZoom.ZoomTowards (gameObject.transform.position);
//		enemyEntry = DataManager.GetEnemyEntry(enemyHealth.GetEnemyKey());
//		enemyEntry.hammerValue = (int)hammer.denominator;
	}

	void OnDestroy() {
//		CameraFocus.Instance.FocusCameraAt (null);

		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);

//		Camera.main.GetComponent<CameraController>().enabled = true;
//		pauseController.ContinueGame ();
//		cameraZoom.ZoomOut ();
	}
	
	// Update is called once per frame
	void Update () {
		if (initiated) {
//			CameraFocus.Instance.FocusCameraAt (this.gameObject);
			ControlNumerator ();
		}
	}

	private void ControlNumerator() {

		// TODO Mobile compatibility
		if (Input.GetKeyDown (KeyCode.A)) {
			this.targetNumerator--;
		} else if (Input.GetKeyDown (KeyCode.D)) {
			this.targetNumerator++;
		}

		if (isInitialFractionLeft)
			this.numeratorRight.text = this.targetNumerator.ToString ();
		else
			this.numeratorLeft.text = this.targetNumerator.ToString ();

		if (Input.GetButtonDown ("Fire1")) {
			Answer ();
		}
	}

	private void Answer() {
//        entry.attemptedNumerators.Add(targetNumerator);

        if (this.targetNumerator == this.correctTargetNumerator) {
			//do something
			Debug.Log ("Correct!");

			//NOTE: commented out due to error after EnemyHealth was updated
//			this.enemyHealth.LCDUpdate (this);
            //this.yarnball.UpdateLCD(this);

            this.hammer.prevDenominator = this.hammer.denominator;
			this.hammer.isLCD = true;
			this.hammer.UpdateFromLCD (this);

        } else {
			Debug.Log ("Wrong!");
		}


//        DataManager.UpdateLCDEntry(DataManager.GetLCDLastKey(), entry);

        this.lcdNumberLine.Segment (this.targetDenominator);
		this.lcdNumberLine.AnimateLCDResult (this.targetNumerator, this.targetDenominator);

		ReturnCamera ();
//		Parameters data = new Parameters ();
//		data.PutExtra ("shouldZoomIn", false);
//		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);

//		cameraZoom.shouldZoomIn = false;
//		cameraZoom.shouldZoomOut = true;

		#if UNITY_ANDROID
//		EventManager.DisableInteractButton ();
//		GameController_v7.Instance.GetEventManager().DisableInteractButton();
//
//		EventManager.ToggleSwitchButton (true);
////		EventManager.Instance.ToggleLeftRightButtons(true);
//
//		GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(true);

		Parameters parameters = new Parameters();
		parameters.PutExtra("FLAG", true);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);

		parameters = new Parameters();
		parameters.PutExtra("FLAG", false);
		EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_INTERACT, parameters);

		parameters = new Parameters ();
		parameters.PutExtra ("FLAG", true);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
		#endif

		Destroy (this.gameObject);
	}
	public void ReturnCamera() {
		Parameters data = new Parameters ();
		data.PutExtra ("shouldZoomIn", false);

		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);
		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);
	}
	private void SyncText() {
		isInitialFractionLeft = ((float) this.originalInitialNumerator / (float) this.originalInitialDenominator) < ((float) this.originalTargetNumerator / (float) this.originalTargetDenominator);

		if (isInitialFractionLeft) {
			SetLeftFractions (this.originalInitialNumerator, this.originalInitialDenominator, this.initialNumerator, this.initialDenominator);

			SetRightFractions (this.originalTargetNumerator, this.originalTargetDenominator, UNKNOWN_VALUE, this.targetDenominator);
		} else {
			SetLeftFractions (this.originalTargetNumerator, this.originalTargetDenominator, UNKNOWN_VALUE, this.targetDenominator);

			SetRightFractions (this.originalInitialNumerator, this.originalInitialDenominator, this.initialNumerator, this.initialDenominator);
		}
	}

	private void SetLeftFractions(int originalNumerator, int originalDenominator, int numerator, int denominator) {
		this.initialNumeratorLeft.text = originalNumerator.ToString ();
		this.initialDenominatorLeft.text = originalDenominator.ToString ();

		if (numerator == UNKNOWN_VALUE)
			this.numeratorLeft.text = "?";
		else
			this.numeratorLeft.text = numerator.ToString ();
		
		this.denominatorLeft.text = denominator.ToString ();
	}

	private void SetRightFractions(int originalNumerator, int originalDenominator, int numerator, int denominator) {
		this.initialNumeratorRight.text = originalNumerator.ToString ();
		this.initialDenominatorRight.text = originalDenominator.ToString ();

		if (numerator == UNKNOWN_VALUE)
			this.numeratorRight.text = "?";
		else
			this.numeratorRight.text = numerator.ToString ();
		
		this.denominatorRight.text = denominator.ToString ();
	}

	public void InitiateCDProblem(int denominator, int targetValNumerator, int targetValDenominator, int initialNumerator, int initialDenominator) {
		int commonDenominator = targetDenominator * denominator;

		this.originalTargetNumerator = targetValNumerator;
		this.originalTargetDenominator = targetValDenominator;

		this.originalInitialNumerator = initialNumerator;
		this.originalInitialDenominator = initialDenominator;

		this.initialDenominator = commonDenominator;
		this.initialNumerator = initialNumerator * (commonDenominator / initialDenominator);

		this.targetDenominator = commonDenominator;
		this.correctTargetNumerator = targetValNumerator * (commonDenominator / targetValDenominator);
		this.targetNumerator = 0;

		initiated = true;

		SyncText ();
	}

	public void InitiateLCDProblem(int denominator, int targetValNumerator, int targetValDenominator, int initialNumerator, int initialDenominator) {

		int x = denominator, y = targetValDenominator, lcm, initialX, initialY;

		initialX = x;
		initialY = y;

		while (x != y) {
			if (x > y) {
				x = x - y;
			}
			else {
				y = y - x;
			}
		}

		lcm = (initialX * initialY) / y;

		this.originalTargetNumerator = targetValNumerator;
		this.originalTargetDenominator = targetValDenominator;

		this.originalInitialNumerator = initialNumerator;
		this.originalInitialDenominator = initialDenominator;

		this.correctTargetNumerator = targetValNumerator * (lcm / targetValDenominator);
		this.targetNumerator = 0;
		this.targetDenominator = lcm;

		this.initialNumerator = initialNumerator * (lcm / initialDenominator);
		this.initialDenominator = lcm;

//        currentKey = new Tuple<Entry.Type, int>(Entry.Type.LCD, PlayerData.LCDIndex);
        //if (!DataManager.DoesKeyExist(currentKey))
        if(enemyHealth.GetLCDKey() == null)
        {
//            entry = new LCDEntry();
//            entry.name = "LCD " + PlayerData.LCDIndex;
//            entry.initialNumerator = initialNumerator;
//            entry.initialDenominator = initialDenominator;
//            if(entry.convertedDenominator == null)
//                entry.convertedDenominator = targetDenominator;
//            entry.actualNumerator = correctTargetNumerator;
//            entry.yarnballValue = denominator;
//            entry.attemptedNumerators = new List<int>();
//
//            Parameters parameters = new Parameters();
//            parameters.PutExtra("TYPE", Entry.Type.LCD.ToString());
//            parameters.PutExtra("NUMBER", PlayerData.LCDIndex);
//            DataManager.CreateEmptyEntry(parameters);
//            enemyHealth.SetLCDKey(currentKey);
//            PlayerData.Instance.IncrementLCDIndex();
        }
        else
        {
//            entry = DataManager.GetLCDEntry(enemyHealth.GetLCDKey());
        }

        initiated = true;

		SyncText ();
	}

	public void SetSourceHealth(EnemyHealth enemyHealth) {
		this.enemyHealth = enemyHealth;
	}

	public void SetLCDNumberLine(LCDNumberLine lcdNumberLine) {
		this.lcdNumberLine = lcdNumberLine;
	}

	public void SetYarnball (Yarnball yarnball) {
		this.yarnball = yarnball;
	}

	public void SetHammer(HammerObject hammer) {
		this.hammer = hammer;
	}

	public int GetTargetNumerator() {
		return this.targetNumerator;
	}

	public int GetTargetDenominator() {
		return this.targetDenominator;
	}

	public int GetInitialNumerator() {
		return this.initialNumerator;
	}

	public int GetInitialDenominator() {
		return this.initialDenominator;
	}
}
