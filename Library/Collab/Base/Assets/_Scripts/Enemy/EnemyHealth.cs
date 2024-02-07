using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour {
	// NOTE: Animator should call death

	[SerializeField] private LCDNumberLine lcdNumberLinePrefab;
	[SerializeField] private LCDInterface lcdInterfacePrefab;
	public const string ANIMATOR_HIT = "hit";
	public const string ANIMATOR_DEATH = "death";

	public const float HEALTH_HEIGHT_OFFSET = 1.0f;
	public const float HEALTH_LENGTH_OFFSET = 0.5f;
	public GameObject numberLinePrefab;
//	public float testTargetNumerator = 4;
//	public float testTargetDenominator = 5;
//	public float testCurrentPointerNumerator = 0;
//	public float testCurrentPointerDenominator = 5;
//	public float testInitialCurrentPointerNumerator = 2;
//	public float testInitialCurrentPointerDenominator = 5;

	public bool isAlive = true;
	[SerializeField] private GameObject numberLineObject;
	[SerializeField] private bool useArbitraryDenominators;

	[SerializeField] private int arbitraryInitialNumerator = 1;
	[SerializeField] private int arbitraryTargetNumerator = 3;
	[SerializeField] private int arbitraryDenominator = 4;

	private NumberLineSwitch numberLineSwitch;
	private float numberLineLength;
	private float targetValNumerator, targetValDenominator;
//	private float currentPointerNumerator, currentPointerDenominator;
	private float initialCurrentPointerNumerator, initialCurrentPointerDenominator;
	private List<int> validDenominators;

	private FractionsReference fractionReference;
	[SerializeField] private Animator enemyAnimator;
	[SerializeField] private Animator phantomAnimator;

    Tuple<Entry.Type, int> enemyKey;
    Tuple<Entry.Type, int> lcdKey;

	Transform top;

    void Awake() {
		this.enemyAnimator = GetComponent<Animator> ();
		UnityEngine.Random.InitState (21853);
		fractionReference = FractionsReference.Instance ();
		fractionReference.AddObserver (this.RequestValidDenominators);
		validDenominators = new List<int> ();

		top = gameObject.transform.GetChild (0);
		enemyKey = null;
		lcdKey = null;

		//		numberLineObject = Instantiate (numberLinePrefab, transform.position, Quaternion.identity);
		this.numberLineObject = Instantiate (numberLinePrefab, top.position, Quaternion.identity);
		Debug.Log ("Assigned Numberline Object");
		//		numberLineObject.transform.position = Vector3.zero;//this.gameObject.transform.position;

		RectTransform rectTransform = (RectTransform)gameObject.transform;

		numberLineObject.transform.SetParent (this.gameObject.transform);

		//		float halfWidth = rectTransform.rect.width / 2.0f;
		//		float halfHeight = rectTransform.rect.height / 2.0f;
		//		numberLineObject.transform.localPosition = new Vector3 (-halfWidth, halfHeight+HEALTH_HEIGHT_OFFSET, 0.0f);

		numberLineObject.transform.position = new Vector3(top.position.x - 1, top.position.y, 0.0f);

		numberLineSwitch = numberLineObject.GetComponent<NumberLineSwitch> ();

		// SET LENGTH HERE
		this.numberLineLength = rectTransform.rect.width;
		numberLineSwitch.SetLineLength (this.numberLineLength);

		//		Debug.Log ("halfwidth " + halfWidth);

		UpdateNumberLineSwitch ();
	}
	public GameObject GetNumberLineObject() {
		return this.numberLineObject;
	}
	void Start() {
		
//		top = gameObject.transform.GetChild (0);
//        enemyKey = null;
//        lcdKey = null;
//
////		numberLineObject = Instantiate (numberLinePrefab, transform.position, Quaternion.identity);
//		this.numberLineObject = Instantiate (numberLinePrefab, top.position, Quaternion.identity);
//		Debug.Log ("Assigned Numberline Object");
////		numberLineObject.transform.position = Vector3.zero;//this.gameObject.transform.position;
//
//		RectTransform rectTransform = (RectTransform)gameObject.transform;
//
//		numberLineObject.transform.SetParent (this.gameObject.transform);
//
////		float halfWidth = rectTransform.rect.width / 2.0f;
////		float halfHeight = rectTransform.rect.height / 2.0f;
////		numberLineObject.transform.localPosition = new Vector3 (-halfWidth, halfHeight+HEALTH_HEIGHT_OFFSET, 0.0f);
//
//		numberLineObject.transform.position = new Vector3(top.position.x - 1, top.position.y, 0.0f);
//
//		numberLineSwitch = numberLineObject.GetComponent<NumberLineSwitch> ();
//
//		// SET LENGTH HERE
//		this.numberLineLength = rectTransform.rect.width;
//		numberLineSwitch.SetLineLength (this.numberLineLength);
//
////		Debug.Log ("halfwidth " + halfWidth);
//
	}

	private void RequestValidDenominators(List<int> validDenominators) {
		this.validDenominators = validDenominators;
		if (validDenominators.Count < 1) {
			this.gameObject.SetActive (false);
		} else {
			this.gameObject.SetActive (true);
		}
	}

	bool isAnimatorNotNull(Animator animator) {
		if (animator != null) {
			return true;
		}
		return false;
	}

	private void InitiateFraction() {
//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);

		if (this.useArbitraryDenominators) {
			this.ArbitraryValues ();
		}
		else {
			this.PedagogicalValues ();
		}
	
		if (numberLineSwitch != null) {
			UpdateNumberLineSwitch ();
		}
	}

	void ArbitraryValues() {
//		this.validDenominators = new List<int>();
//		this.validDenominators.Add (this.arbitraryInitialDenominator);

		this.targetValNumerator = this.arbitraryTargetNumerator;
		this.targetValDenominator = this.arbitraryDenominator;

		fractionReference.AddEnemyDenominator ((int) this.targetValDenominator);


		this.initialCurrentPointerNumerator = this.arbitraryInitialNumerator;
		this.initialCurrentPointerDenominator = this.targetValDenominator;

	}

	void PedagogicalValues() {
		this.validDenominators = fractionReference.RequestDenominators ();
		this.targetValDenominator = this.validDenominators [UnityEngine.Random.Range (0, validDenominators.Count)];

		fractionReference.AddEnemyDenominator ((int) this.targetValDenominator);

		this.targetValNumerator = (int) UnityEngine.Random.Range (1, this.targetValDenominator);

		this.initialCurrentPointerDenominator = this.targetValDenominator;
		this.initialCurrentPointerNumerator = (int) UnityEngine.Random.Range (0, this.initialCurrentPointerDenominator);

		while(this.initialCurrentPointerNumerator == this.targetValNumerator)
			this.initialCurrentPointerNumerator = (int) UnityEngine.Random.Range (0, this.initialCurrentPointerDenominator);
	}

	public void LCD(int denominator, HammerObject hammer) {
		Vector3 pos = new Vector3 (this.numberLineObject.transform.position.x, this.numberLineObject.transform.position.y + 0.75f, 0.0f);
		Parameters data = new Parameters ();
		data.PutExtra ("x", pos.x);
		data.PutExtra ("y", pos.y);
		EventBroadcaster.Instance.PostEvent (EventNames.ZOOM_CAMERA_TOWARDS, data);

		GameController_v7.Instance.GetLCDManager ().InitiateLCD (denominator, this, hammer);

//		LCDInterface lcd = Instantiate<LCDInterface> (this.lcdInterfacePrefab, pos, Quaternion.identity);
//		lcd.transform.SetParent (this.transform);
//		lcd.SetSourceHealth (this);
//		lcd.SetHammer (hammer);
//		lcd.transform.localPosition = pos;
//
//		LCDNumberLine lcdNumberLine = Instantiate<LCDNumberLine> (this.lcdNumberLinePrefab, this.numberLineSwitch.gameObject.transform.position, Quaternion.identity);
//		lcdNumberLine.transform.SetParent (this.gameObject.transform);
//		lcdNumberLine.SetLineLength (this.numberLineLength);
//		lcd.SetLCDNumberLine (lcdNumberLine);
//
//		print ("Denom: " + denominator);
//		lcd.InitiateLCDProblem (denominator, (int) this.targetValNumerator, (int) this.targetValDenominator, (int) this.initialCurrentPointerNumerator, (int) this.initialCurrentPointerDenominator);
	}

	public void LCDUpdate(LCDInterface_v3 lcdInterface) {
		int lcd = lcdInterface.GetLCD ();

		this.targetValNumerator = (lcd / this.targetValDenominator) * this.targetValNumerator;
		this.targetValDenominator = lcd;

		this.initialCurrentPointerNumerator = (lcd / this.initialCurrentPointerDenominator) * this.initialCurrentPointerNumerator;
		this.initialCurrentPointerDenominator = lcd;

		this.numberLineSwitch.isNumberLineUpdated = true;
		numberLineSwitch.PurgeSegments ();
		UpdateNumberLineSwitch ();
	}

	public void LCDUpdate(LCDInterface lcdInterface) {
		this.targetValNumerator = lcdInterface.GetTargetNumerator ();
		this.targetValDenominator = lcdInterface.GetTargetDenominator ();

		this.initialCurrentPointerNumerator = lcdInterface.GetInitialNumerator ();
		this.initialCurrentPointerDenominator = lcdInterface.GetInitialDenominator ();

        this.numberLineSwitch.isNumberLineUpdated = true;
		numberLineSwitch.PurgeSegments ();
        UpdateNumberLineSwitch ();
	}

	private void UpdateNumberLineSwitch() {
//		this.numberLineSwitch.PurgeSegments ();
		this.numberLineSwitch.Segment ((int) this.targetValDenominator);
		this.numberLineSwitch.TargetSetter (this.targetValNumerator, this.targetValDenominator);
		this.numberLineSwitch.InitialPointerPointTo (this.initialCurrentPointerNumerator, this.initialCurrentPointerDenominator);
    }

	void OnEnable() {
		fractionReference.UpdateFractionRange (this.RequestValidDenominators);
		InitiateFraction ();

		if (numberLineObject != null) {
			this.numberLineObject.SetActive (true);
		}
	}

	void OnDestroy() {
		fractionReference.RemoveObserver (this.RequestValidDenominators);
	}

	void OnDisable() {
		if (numberLineSwitch != null) {
			numberLineSwitch.PurgeSegments ();
		}
		this.numberLineObject.SetActive (false);
	}

	public void setAnimator(Animator animator, string keyword) {
		if (isAnimatorNotNull (animator)) {
			switch (keyword) {
			case ANIMATOR_HIT:
				animator.SetTrigger (ANIMATOR_HIT);	
				break;
			case ANIMATOR_DEATH:
				animator.SetTrigger (ANIMATOR_DEATH);	
				break;
			}
		}
	}
	public void Smash(float numerator, float denominator) {
		this.setAnimator (this.enemyAnimator, ANIMATOR_HIT);
		Debug.Log ("ENTERED ANIMATOR");
		numberLineSwitch.HammerDown (numerator, denominator);
		if (numerator != 0f) {
			
			Debug.Log ("<color=red>Zero numerator on hammer</color>");
			EventBroadcaster.Instance.PostEvent (EventNames.ON_HAMMER_DOWN_ZERO);
		}
	}

	public void Death() {
		isAlive = false;
		this.setAnimator (this.enemyAnimator, ANIMATOR_DEATH);
		SoundManager.Instance.Play (AudibleNames.Phantom.DEATH, false);
	}
	public void CallPhantomDeath() {
		this.phantomAnimator.gameObject.SetActive (true);
		this.setAnimator (this.phantomAnimator, ANIMATOR_DEATH);
	}
	public void CallDeath(float value) {
		Debug.Log ("DESTROYED");
		Destroy (this.gameObject);
	}

	public void HandleNumberLineFlip() {
		// Switch the way the  labels are facing. This is called everytime EnemyMovement's FlipFace() is called to negate its effect
		Vector3 theScale = numberLineSwitch.gameObject.transform.localScale;
		theScale.x *= -1;
		numberLineSwitch.gameObject.transform.localScale = theScale;

		Vector3 thePosition = numberLineSwitch.gameObject.transform.position;
		if (theScale.x > 0) {
			thePosition.x = -1;
		} else {
			thePosition.x = 1;
		}
	}

	public int GetDenominator() {
		return (int) this.targetValDenominator;
	}

    public void SetEnemyKey(Tuple<Entry.Type, int> key)
    {
        this.enemyKey = key;
        Debug.LogWarning("Key set to enemy " + String.Join(" ", key));
    }

    public Tuple<Entry.Type, int> GetEnemyKey()
    {
        return enemyKey;
    }

    public void SetLCDKey(Tuple<Entry.Type, int> key)
    {
        this.lcdKey = key;
        Debug.LogWarning("Key set to lcd " + String.Join(" ", key));
    }

    public Tuple<Entry.Type, int> GetLCDKey()
    {
        return lcdKey;
    }
}
