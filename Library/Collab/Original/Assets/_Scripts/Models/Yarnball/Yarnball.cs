using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yarnball : Interactable {
	
	[SerializeField] private YarnballProjectile yarnBallProjectile;

	[SerializeField] private TextMesh textValue;
	private const int COOLDOWN_SECONDS = 0;
	private PlayerYuni player;
	private bool isStruck = false;
//	private PauseController pauseController;
	private SpriteRenderer sprite;
	private Collider2D triggerCollider;
//	private List<int> validDenominators;
//	private FractionsReference fractionReference;
	private int denominator;


	private bool hammerMidSmash;
	private bool YuniIsPropelling;
	private YarnballPedestal pedestal;
	private HammerObject hammer;
	private bool isNearPlayer;

	void Awake() {
		UnityEngine.Random.InitState (9876);
		this.isNearPlayer = false;
//		validDenominators = new List<int> ();
//		fractionReference = FractionsReference.Instance ();
//		fractionReference.AddObserver (this.RequestValidDenominators);
		//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_HAMMER_SMASH_END, this.HammerDoneSmashing);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_CHARGING, this.Pickup);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_YUNI_PROPEL_END, this.YuniDonePropelling);
//		if (gameObject.GetComponent<Yarnball> () == null)
//			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
		sprite = GetComponent<SpriteRenderer> ();
		triggerCollider = GetComponent<Collider2D> ();
	}

	public void SetPedestal(YarnballPedestal pedestal) {
		this.pedestal = pedestal;
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_CHARGING);

		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HAMMER_SMASH_END);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_YUNI_PROPEL_END);
	}
//	private void InitiateDenominator() {
//		this.denominator = this.validDenominators [(int)Random.Range (0, this.validDenominators.Count)];
//		this.textValue.text = "" + this.denominator;
//	}

//	private void RequestValidDenominators(List<int> validDenominators) {
//		this.validDenominators = validDenominators;
//	}

	public void HammerDoneSmashing() {
		Debug.Log ("CALLED HAMMER DONE SMASHING");
		this.hammerMidSmash = false;
	}
	public void YuniDonePropelling() {
		this.YuniIsPropelling = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
//		if (other.gameObject.CompareTag ("Hammer Child")) {
//		if(other.GetComponent<HammerChildCollisionDetection>() != null) {
//			if (!isStruck && IsAllowed()) {
//
//				other.GetComponent<HammerChildCollisionDetection>().GetCollider().enabled = false;
//				StartCoroutine (this.ChoosingYarnballDirection (other.gameObject.GetComponentInParent<HammerObject> ()));
//			}
//		}
	}

	public bool IsAllowed() {
		Debug.Log ("<color=blue>NUM IS " + 	this.GetPlayer().GetPlayerAttack().getHammerObject().GetUsingNumerator() + "</color>");
		if(this.GetPlayer().IsLCDEnabled() &&
			this.GetPlayer().GetPlayerAttack().getHammerObject().GetUsingNumerator() == 0) {
			return true;
		}
		else {
			return false;
		}
	}
	public void SetHammer(HammerObject hammerObject) {
		this.hammer = hammerObject;
	}
	public HammerObject GetHammerObject() {
		if (this.hammer == null) {
			this.hammer = GetPlayer ().GetPlayerAttack ().getHammerObject ();
		}
		return this.hammer;
	}

//	void Update() {
//		if(GameController_v7.Instance.GetMobileUIManager().IsCharging()) {
//			this.Pickup ();
//		}
//	}

	public void Pickup() {
		if (isNearPlayer) {
			Interact ();
//			GameController_v7.Instance.GetMobileUIManager ().ReleaseChargeButton ();
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni>() != null) {
			this.isNearPlayer = true;
			#if UNITY_STANDALONE
			if (Input.GetButtonDown ("Fire2")) { // TODO: Mobile alternative
				Interact();
			}
//			#elif UNITY_ANDROID
//			if(GameController_v7.Instance.GetMobileUIManager().IsCharging()) {
////				(GameController_v7.Instance.GetMobileUIManager().IsInteracting()
////				|| GameController_v7.Instance.GetMobileUIManager().IsJumping() ||
////				GameController_v7.Instance.GetMobileUIManager().IsCharging())) {
////				GameController_v7.Instance.GetMobileUIManager().SetInteracting(false);
////				GameController_v7.Instance.GetMobileUIManager().SetJumping(false);
////				GameController_v7.Instance.GetMobileUIManager().ReleaseChargeButton();
//				Interact ();
//			}
			#endif
		}
	}
	void OnTriggerExit2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni> () != null) {
			this.isNearPlayer = false;
		}
	}
	public void SetPlayer(PlayerYuni playerInstance) {
		this.player = playerInstance;
	}
	public PlayerYuni GetPlayer() {
		if (this.player == null) {
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}

	public override void Interact() {

		Debug.Log ("Interact");
		SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false);

		//				player.GetPlayerAttack ().getHammerObject ().PickupDenominatorFromYarn (this);
		this.GetPlayer().GetPlayerAttack ().SetEquippedDenominatorFromYarnball (this);

		if (this.pedestal != null) {
			pedestal.DisableEffects ();
		}

		StartCoroutine (Cooldown ());
	
//		EventManager.RemoveHintOnTrigger ();
//		EventBroadcaster.Instance.PostEvent(EventNames.REMOVE_HINT);
	}

	public void SetDenominator(int denominator) {
		this.denominator = denominator;
		this.textValue.text = "" + this.denominator;
	}

	private IEnumerator ChoosingYarnballDirection(HammerObject hammer) {
		Vector3 mouseDirection;
		isStruck = true;
		this.hammerMidSmash = true;
		this.YuniIsPropelling = true;
		int cnt = 0;

//		while(this.hammerMidSmash) {
//		yield return new WaitForSeconds (0.5f);
//			yield return null;
//		}
		Debug.Log ("Exited Yield");


		EventBroadcaster.Instance.PostEvent (EventNames.ON_LCD_PAUSE);
		GameController_v7.Instance.GetPauseController ().Pause ();
//		pauseController.PauseGame ();
		while (isStruck) {
			hammer.IsAttacking (true);
			yield return null;

			//will have to add checker for android or windows // TODO Mobile compatibility
			if (Input.GetButtonDown ("Fire1")) {
				mouseDirection = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				EventBroadcaster.Instance.PostEvent (EventNames.ON_LCD_UNPAUSE);


				if (pedestal != null) {
					pedestal.DisableEffects ();
				}
				GameController_v7.Instance.GetPauseController ().Continue ();
//				pauseController.ContinueGame ();
//				while(this.YuniIsPropelling) {
//					Debug.Log ("Yuni is propelling");
//					yield return null;
//				}


				PropelYarnball (mouseDirection, hammer);
				isStruck = false;
				hammer.IsAttacking (false);
			}
		}
		StartCoroutine (Cooldown ());
		Debug.Log ("Exited Choosing Yarnball");

	}

	private void PropelYarnball(Vector3 direction, HammerObject hammer) {
		Debug.Log ("Entered Propel");
		YarnballProjectile yarnProjectile = Instantiate<YarnballProjectile> (yarnBallProjectile, this.transform.position, Quaternion.identity);
		yarnProjectile.SetTarget (direction);
		yarnProjectile.SetDenominator (this.denominator);
		yarnProjectile.SetSource (this);
		yarnProjectile.SetHammer (hammer);
		yarnProjectile.gameObject.SetActive (true);
		yarnProjectile.Propel ();
		Debug.Log ("Exited Propel");
	}

	void DisableYarnballRenderers() {
		ToggleYarnballRenderers (false);
	}

	void EnableYarnballRenderers() {
		ToggleYarnballRenderers (true);
	}

	void ToggleYarnballRenderers(bool value) {
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			renderer.enabled = value;
		}
		GetComponent<SpriteRenderer> ().enabled = value;
	}

	private IEnumerator Cooldown() {
		Debug.Log ("Entered Cooldown");

		DisableYarnballRenderers ();
//		if(sprite != null)
//			this.sprite.enabled = false;
//		this.textValue.gameObject.SetActive (false);
		this.triggerCollider.enabled = false;
		for (int i = 0; i < COOLDOWN_SECONDS; i++) {
			yield return new WaitForSeconds (1.0f);
		}

		EnableYarnballRenderers ();
//		if(sprite != null)
//			this.sprite.enabled = true;
//		this.textValue.gameObject.SetActive (true);

		this.triggerCollider.enabled = true;
		if (this.pedestal != null) {
			pedestal.EnableEffects ();
		}
		Debug.Log ("Exited Cooldown");

		yield return null;
	}

	public void UpdateLCD(LCDInterface lcdInterface) {
		this.denominator = lcdInterface.GetTargetDenominator ();
	}

	public int GetDenominator() {
		return this.denominator;
	}
}
