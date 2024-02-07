using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerObject : MonoBehaviour {
	public const float HAMMER_SMASH_COLLIDER_DELAY = 0.25f;
	public const float HAMMER_SMASH_ACTIVE_DELAY = 0.5f;

	public float hammerTime = 3.0f;
	public float numerator = 0.0f;
	public int denominator = 4;

	public int prevDenominator = 4;
	public bool isLCD = false;

	public float chargeRate = 0.2f;

	private Transform playerTransform;
	private Animator playerAnimator;
	private Animator hammerAnimator;
	private PlayerAttack playerAttack;
	private Collider2D hazard;
	private bool isAttacking = false;
	private bool isPlaying = false;
	private Transform hammerTransform;
	private bool isCharging = false;
	private ParticleSystem effectHammerCharge;
	private float usingNumerator = 0.0f;
//	MobileUI mobile;

	private BreakerCollider breakerCollider;
//	private Collider2D breakerCollider;
	public bool isAllowed;
	private bool hasCharge; // to check if player has charge when HazardUp was called (for Yarnball compatibility)
	[SerializeField] private BoxCollider2D hammerChildCollider;
	[SerializeField] TextMesh numeratorLabel;
	[SerializeField] private ParticleSystem effectHammerSmash;

	[SerializeField] private HammerEffect hammerEffect;
	[SerializeField] private AudibleNames.Hammer soundID;

	void Awake() {
		GameController_v7.Instance.GetPauseController ().onPauseGame += this.SetNotAllowed;
		GameController_v7.Instance.GetPauseController ().onContinueGame += this.SetAllowed;

	}
	void Destroy() {
		GameController_v7.Instance.GetPauseController ().onPauseGame -= this.SetNotAllowed;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= this.SetAllowed;
	}

	// Use this for initialization
	void Start () {
		hazard = GetComponentInChildren<Collider2D> ();
		hazard.enabled = false;
		PlayerYuni player = GameObject.FindObjectOfType<PlayerYuni>();
		playerAnimator = player.GetComponent<Animator> ();
		hammerAnimator = gameObject.GetComponentInChildren<Animator> ();



		playerTransform = player.transform;
		hammerTransform = GameObject.FindGameObjectWithTag ("Hammer Child").transform;

		this.hammerEffect.SetHammerEffect (this.effectHammerSmash);

		this.playerAttack = player.GetComponent<PlayerAttack> ();
		this.numeratorLabel = playerAttack.getNumeratorLabel ();

		this.breakerCollider = GetComponentInChildren<BreakerCollider> ();
		this.isAllowed = true;
//		#if UNITY_ANDROID
//		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
//		#endif
//		this.breakerCollider ;
//		this.audioSmash = GetComponent<AudioSource> ();
	}
	public BoxCollider2D getHammerChildCollider() {
		return this.hammerChildCollider;
	}
//	void FixedUpdate() {
//		if (!PauseController.isPaused) {
//			playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
////			if (!isAttacking) {
////				playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
////				transform.position = playerTransform.position;
////			}
//		}
//	}


	public void EnableBreaker() {
		this.GetBreaker ().enabled = true;
	}
	public void DisableBreaker() {
		this.GetBreaker ().enabled = false;
	}
	public BreakerCollider GetBreakerCollider() {
		if (this.breakerCollider == null) {
			this.breakerCollider = GetComponentInChildren<BreakerCollider> ();
		}
		return this.breakerCollider;
	}
	public Collider2D GetBreaker() {
		if (this.breakerCollider == null) {
			this.breakerCollider = GetComponentInChildren<BreakerCollider> ();
		}
		return this.breakerCollider.GetComponent<Collider2D> ();
	}
	public void HammerCharge() {
		if (!isCharging) {
			StartCoroutine (Charging ());
		}
	}
	public void DelayedEnableBreaker() {
		StartCoroutine (DelayedEnable());
	}
	IEnumerator DelayedEnable() {
		yield return new WaitForSecondsRealtime (1.0f);
		this.EnableBreaker ();
	}

	private IEnumerator Charging() {
		isCharging = true;
		playerAttack.getNumeratorLabelObject().SetActive (true);
//		playerAttack.getFractionLabelObject().SetActive (false);
		playerAttack.getFractionNumeratorLabelObject().SetActive(false);

		// TODO: Mobile
		#if UNITY_ANDROID
		while (GameController_v7.Instance.GetMobileUIManager().IsCharging()) {
			effectHammerCharge.Play ();
			numerator += Mathf.Ceil (chargeRate * Time.deltaTime);
			DisplayCharge ();
			yield return new WaitForSeconds (1.0f);
		}
		#else
		while (Input.GetButton ("Fire2")) {
			effectHammerCharge.Play ();
			numerator += Mathf.Ceil (chargeRate * Time.deltaTime);
			DisplayCharge ();
			yield return new WaitForSeconds (1.0f);
		}
		#endif
	
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, this.denominator);
		playerAttack.getNumeratorLabelObject().SetActive (false);
		playerAttack.getFractionNumeratorLabelObject().SetActive(true);
//		playerAttack.getFractionLabelObject().SetActive (true);
		effectHammerCharge.Stop ();
		isCharging = false;
	}

	private void DisplayCharge() {
		this.updateLabel ();
		Debug.Log ("Numerator: " + numerator);
	}
	public bool IsAllowed() {
		return this.isAllowed;
	}
	public void SetNotAllowed() {
		this.isAllowed = false;
	}
	public void SetAllowed() {
		this.isAllowed = true;
	}
	public void HammerSmash() {
		if (!this.isAttacking && this.IsAllowed()) {
			StartCoroutine (HazardUp ());
		}
	}

//	public void PickupDenominatorFromYarn(Yarnball yarnball) {
//		this.denominator = yarnball.GetDenominator ();
//		this.DisplayCharge ();
//		playerAttack.UpdateFractionLabelObject ((int)this.numerator, (int)this.denominator);
//	}

	public void SyncWithEquippedDenominator(int denominator) {
		this.denominator = denominator;
		this.PostDenominatorEvent (this.denominator);
		this.DisplayCharge ();
	}
	public void PostDenominatorEvent(float denominator) {
		Parameters parameters = new Parameters ();
		parameters.PutExtra (YarnballUI.YARNBALL_VALUE, denominator);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_DENOMINATOR_CHANGE, parameters);
	}

	public void UpdateFromLCD(LCDInterface_v3 lcdInterface) {
		this.denominator = lcdInterface.GetLCD ();
		this.DisplayCharge ();
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, this.denominator);
	}

	public void UpdateFromLCD(LCDInterface lcdInterface) {
		this.denominator = lcdInterface.GetTargetDenominator ();
		this.DisplayCharge ();
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, this.denominator);
	}

	public void updateLabel() {
		if(numeratorLabel != null)
			this.numeratorLabel.text = "" + this.numerator;
	}

	private IEnumerator HazardUp() {
		this.GetBreakerCollider ().ResetBreaker ();
		// TODO Face direction (flip)
		effectHammerCharge.Stop ();
		this.SetSoundID (AudibleNames.Hammer.BASIC);

		DisableBreaker();

		usingNumerator = numerator;
		transform.position = new Vector3 (playerTransform.position.x, playerTransform.position.y, -4);
		hazard.enabled = true;
		isAttacking = true;
		playerAnimator.SetBool ("hammer isAttacking", this.isAttacking);
		playerAnimator.SetTrigger ("hammer attack");
		hammerAnimator.SetTrigger ("smash");


		playerAttack.getHammerChildCollider().enabled = false; // For animation
		yield return new WaitForSeconds (HAMMER_SMASH_COLLIDER_DELAY);
		playerAttack.getHammerChildCollider().enabled = true;
//		EnableBreaker ();
//		yield return new WaitForSeconds (HAMMER_SMASH_ACTIVE_DELAY);
		StartCoroutine(BreakerSmashRoutine());

		numerator = 0.0f;

		playerAttack.UpdateFractionLabelObject ((int)this.numerator, this.denominator);
		this.updateLabel ();

//		playerAttack.getHammerChildCollider().enabled = false;
//		isAttacking = false;

		this.isPlaying = true;
		while (isPlaying) {
			yield return null;
		}

//		DisableBreaker();
		SoundManager.Instance.Play(this.soundID, false);
		playerAnimator.SetBool ("hammer isAttacking", false);

//		for (int i = 0; i < hammerTime; i++) {
//			yield return new WaitForSeconds (1.0f);
//		}
		isAttacking = false;
		hazard.enabled = false;

//		playerAttack.getHammerChildCollider().enabled = false;
	}
	public IEnumerator BreakerSmashRoutine() {
		yield return new WaitForSeconds (0.1f);
		EnableBreaker ();
		yield return new WaitForSeconds (0.5f);
		DisableBreaker();
	}
	public void IsPlaying(bool playing){
		this.isPlaying = playing;
	}
	public void IsAttacking(bool attacking){
		this.isAttacking = attacking;
	}
	public bool IsAttacking(){
		return this.isAttacking;
	}
	void OnDisable() {
		isAttacking = false;
		hazard.enabled = false;
		numerator = 0.0f;
//		playerAttack.getHammerChildCollider ().enabled = false;
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, this.denominator);
		playerAttack.getNumeratorLabelObject().SetActive (false);
		playerAttack.getFractionNumeratorLabelObject().SetActive(true);
		effectHammerCharge.Stop ();
		isCharging = false;
	}

	public void TriggerCollision(Collider2D other) {
//		if (this.usingNumerator > 0.0f) {
//			other.gameObject.GetComponent<EnemyHealth> ().Smash (usingNumerator, denominator);

		EnemyHealth health = other.gameObject.GetComponent<RingleaderHealth> ();
		if (health != null) {
			health.Smash ();
		} else {
			health = other.gameObject.GetComponent<EnemyHealth> ();
			health.Smash ();
		}

			this.isLCD = false;
//			this.denominator = this.prevDenominator; TODO Commented out
			usingNumerator = 0.0f;
//		}
	}
	public float GetUsingNumerator() {
		return this.usingNumerator;
	}

//	public void TriggerEnemyCollision(Collider2D other) {
//
//		other.gameObject.GetComponent<EnemyHealth> ().Smash (usingNumerator, denominator);
//		this.isLCD = false;
//		this.denominator = this.prevDenominator;
//		usingNumerator = 0.0f;
//	}

//	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.CompareTag ("Enemy")) {
//			Debug.Log ("ENTERED Trigger Hammer");
//
//		}
//	}

	public int GetNumerator() {
		return (int)this.numerator;
	}

	public int GetDenominator() {
		return this.denominator;
	}
	public void SetEffectHammerCharge(ParticleSystem effect) {
		this.effectHammerCharge = effect;
	}

	public void SetSoundID(AudibleNames.Hammer id) {
		this.soundID = id;
		Debug.Log ("<color=red>ID SET "+id.ToString()+"</color>");
	}
}
