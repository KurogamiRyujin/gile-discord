using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerObject : MonoBehaviour {

	public const float HAMMER_SMASH_COLLIDER_DELAY = 0.25f;
	public const float HAMMER_SMASH_ACTIVE_DELAY = 0.5f;

	public float hammerTime = 3.0f;
	public float numerator = 0.0f;
	public float denominator = 5.0f;

	public float prevDenominator = 5.0f;
	public bool isLCD = false;

	public float chargeRate = 0.5f;

	private Transform playerTransform;
	private Animator playerAnimator;
	private Animator hammerAnimator;
	private PlayerAttack playerAttack;
	private Collider2D hazard;
	private bool isAttacking = false;
	private Transform hammerTransform;
	private bool isCharging = false;
	private ParticleSystem effectHammerCharge;
	private float usingNumerator = 0.0f;
	MobileUI mobile;


	[SerializeField] private BoxCollider2D hammerChildCollider;
	[SerializeField] TextMesh numeratorLabel;
	[SerializeField] private ParticleSystem effectHammerSmash;

	[SerializeField] private HammerEffect hammerEffect;
	// Use this for initialization
	void Start () {
		hazard = GetComponentInChildren<Collider2D> ();
		hazard.enabled = false;
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		playerAnimator = player.GetComponent<Animator> ();
		hammerAnimator = gameObject.GetComponentInChildren<Animator> ();

		playerTransform = player.transform;
		hammerTransform = GameObject.FindGameObjectWithTag ("Hammer Child").transform;

		this.hammerEffect.SetHammerEffect (this.effectHammerSmash);

		this.playerAttack = player.GetComponent<PlayerAttack> ();
		this.numeratorLabel = playerAttack.getNumeratorLabel ();
		#if UNITY_ANDROID
		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif

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

	public void HammerCharge() {
		if (!isCharging) {
			StartCoroutine (Charging ());
		}
	}

	private IEnumerator Charging() {
		isCharging = true;
		playerAttack.getNumeratorLabelObject().SetActive (true);
//		playerAttack.getFractionLabelObject().SetActive (false);
		playerAttack.getFractionNumeratorLabelObject().SetActive(false);

		#if UNITY_ANDROID
		while (mobile.chargePressed) {
			effectHammerCharge.Play ();
			numerator += Mathf.Ceil (chargeRate * Time.deltaTime);
			DisplayCharge ();
			yield return new WaitForSeconds (1.0f);
		}
		#elif UNITY_STANDALONE
		#endif
		// TODO: Mobile
		while (Input.GetButton ("Fire2")) {
			effectHammerCharge.Play ();
			numerator += Mathf.Ceil (chargeRate * Time.deltaTime);
			DisplayCharge ();
			yield return new WaitForSeconds (1.0f);
		}
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, (int)this.denominator);
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

	public void HammerSmash() {
		if (!this.isAttacking) {
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
		this.DisplayCharge ();
	}

	public void UpdateFromLCD(LCDInterface lcdInterface) {
		this.denominator = lcdInterface.GetTargetDenominator ();
		this.DisplayCharge ();
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, (int)this.denominator);
	}

	public void updateLabel() {
		this.numeratorLabel.text = "" + this.numerator;
	}

	private IEnumerator HazardUp() {
		// TODO Face direction (flip)
		effectHammerCharge.Stop ();


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
		yield return new WaitForSeconds (HAMMER_SMASH_ACTIVE_DELAY);


		playerAttack.getHammerChildCollider().enabled = false;
		for (int i = 0; i < hammerTime; i++) {
			yield return new WaitForSeconds (1.0f);
		}

		isAttacking = false;
		hazard.enabled = false;
		playerAnimator.SetBool ("hammer isAttacking", this.isAttacking);
		numerator = 0.0f;

		playerAttack.UpdateFractionLabelObject ((int)this.numerator, (int)this.denominator);
		this.updateLabel ();

//		playerAttack.getHammerChildCollider().enabled = false;
	}
	public void IsAttacking(bool attacking){
		this.isAttacking = attacking;
	}
	void OnDisable() {
		isAttacking = false;
		hazard.enabled = false;
		numerator = 0.0f;
//		playerAttack.getHammerChildCollider ().enabled = false;
		playerAttack.UpdateFractionLabelObject ((int)this.numerator, (int)this.denominator);
		playerAttack.getNumeratorLabelObject().SetActive (false);
		playerAttack.getFractionNumeratorLabelObject().SetActive(true);
		effectHammerCharge.Stop ();
		isCharging = false;
	}

	public void TriggerCollision(Collider2D other) {

		other.gameObject.GetComponent<EnemyHealth> ().Smash (usingNumerator, denominator);
		this.isLCD = false;
		this.denominator = this.prevDenominator;
		usingNumerator = 0.0f;
	}

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
		return (int)this.denominator;
	}
	public void SetEffectHammerCharge(ParticleSystem effect) {
		this.effectHammerCharge = effect;
	}
}
