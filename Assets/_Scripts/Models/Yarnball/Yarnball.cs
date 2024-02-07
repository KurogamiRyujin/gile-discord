using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Power charm behaviour. Picked up by the player to equip a denominator.
/// 
/// Allows the player avatar to slice sky blocks into fragments where the fragment count depends on the value.
/// </summary>
public class Yarnball : Interactable {
	
	[SerializeField] private YarnballProjectile yarnBallProjectile;

	[SerializeField] private TextMesh textValue;
	private const int COOLDOWN_SECONDS = 0;
//	private PlayerYuni player;
	private bool isStruck = false;
	private bool isHidden = false;
//	private PauseController pauseController;
	private SpriteRenderer sprite;
	private Collider2D triggerCollider;
//	private List<int> validDenominators;
//	private FractionsReference fractionReference;
    /// <summary>
    /// Denominator Value of the Charm
    /// </summary>
	private int denominator;


	private bool hammerMidSmash;
	private bool YuniIsPropelling;
    /// <summary>
    /// Reference to the pedestal holding it.
    /// </summary>
	private YarnballPedestal pedestal;
	private HammerObject hammer;
    /// <summary>
    /// Flag if the player avatar is near it.
    /// </summary>
	private bool isNearPlayer;

	void Awake() {
		UnityEngine.Random.InitState (9876);
		this.isNearPlayer = false;
//		validDenominators = new List<int> ();
//		fractionReference = FractionsReference.Instance ();
//		fractionReference.AddObserver (this.RequestValidDenominators);
		//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);
//		this.Show();
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_HAMMER_SMASH_END, this.HammerDoneSmashing);
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_CHARGING, this.Pickup);
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_YUNI_PROPEL_END, this.YuniDonePropelling);
//		if (gameObject.GetComponent<Yarnball> () == null)
//			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();

		EventBroadcaster.Instance.AddObserver (EventNames.ON_HAMMER_SMASH_END, this.HammerDoneSmashing);
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_CHARGING, this.Pickup);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_YUNI_PROPEL_END, this.YuniDonePropelling);
		this.Show();
		sprite = GetComponent<SpriteRenderer> ();
		triggerCollider = GetComponent<Collider2D> ();
	}

    /// <summary>
    /// Set the pedestal holding it.
    /// </summary>
    /// <param name="pedestal">Charm Pedestal</param>
	public void SetPedestal(YarnballPedestal pedestal) {
		this.pedestal = pedestal;
	}
	void OnDestroy() {
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_CHARGING);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HAMMER_SMASH_END);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_YUNI_PROPEL_END);
	}
	public void HammerDoneSmashing() {
		Debug.Log ("CALLED HAMMER DONE SMASHING");
		this.hammerMidSmash = false;
	}
	public void YuniDonePropelling() {
		this.YuniIsPropelling = false;
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

		Debug.Log ("<color=red>NOT NEAR</color>");
		if (isNearPlayer) {
			Interact ();
			Debug.Log ("<color=green>IS NEAR</color>");
//			GameController_v7.Instance.GetMobileUIManager ().ReleaseChargeButton ();
		}
	}

	public bool IsHidden() {
		return this.isHidden;
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.GetComponent<PlayerYuni>() != null) {
			this.isNearPlayer = true;
//			Debug.Log ("NEAR PLAYER");
			if (!this.IsHidden ()) {
				this.GetPlayer ().OverlapYarnball (this);
			}

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
			this.GetPlayer ().LeaveYarnball (this);
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


    /// <summary>
    /// Switches the denominator if not 0, else equips the denominator and disables the pedestal completely.
    /// </summary>
    public void SwitchYarnball() {
		int playerOldDenominator = this.GetPlayer ().GetPlayerAttack ().getEquippedDenominator ();
		int thisOldDenominator = this.denominator;

		this.GetPlayer ().GetPlayerAttack ().SetEquippedDenominatorFromYarnball (this);
		this.SetDenominator (playerOldDenominator);
		if (this.denominator == 0) {
			// If player gave a 0 denominator, meaning it's the first time to get a yarnball, show charm graphic
			EventBroadcaster.Instance.PostEvent (EventNames.CHARM_PICKUP);
			EventBroadcaster.Instance.PostEvent (EventNames.CHARM_PICKUP_SWITCH);
			this.Hide ();
		} else {
			EventBroadcaster.Instance.PostEvent (EventNames.CHARM_PICKUP);
			EventBroadcaster.Instance.PostEvent (EventNames.CHARM_PICKUP_SWITCH);
			this.Show ();
		}
//		if (playerOldDenominator != 0) {
//			this.denominator = playerOldDenominator;
//			this.Show ();
//		} else {
//			this.denominator = playerOldDenominator;
//			this.Hide ();
//		}
	}

    /// <summary>
    /// Makes the charm visible.
    /// </summary>
	public void Show() {

		this.isHidden = false;
		EnableYarnballRenderers ();
//		this.triggerCollider.enabled = true;
		if (this.pedestal != null) {
			pedestal.EnableEffects ();
		}
	}

    /// <summary>
    /// Makes the charm invisible.
    /// </summary>
	public void Hide() {
		this.isHidden = true;
		this.GetPlayer ().LeaveYarnball (this);
		DisableYarnballRenderers ();
//		this.triggerCollider.enabled = false;
		if (this.pedestal != null) {
			pedestal.DisableEffects ();
		}
	}

    /// <summary>
    /// Allows the player to pickup the charm.
    /// </summary>
	public override void Interact() {

		Debug.Log ("Interact");
		SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false);

		//				player.GetPlayerAttack ().getHammerObject ().PickupDenominatorFromYarn (this);
	
		this.SwitchYarnball();
//		this.GetPlayer().GetPlayerAttack ().SetEquippedDenominatorFromYarnball (this);



//		StartCoroutine (Cooldown ());
	
//		EventManager.RemoveHintOnTrigger ();
//		EventBroadcaster.Instance.PostEvent(EventNames.REMOVE_HINT);
	}

    /// <summary>
    /// Sets the charm's value.
    /// </summary>
    /// <param name="denominator"></param>
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

    /// <summary>
    /// Returns the charm's value.
    /// </summary>
    /// <returns>Charm Value</returns>
	public int GetDenominator() {
		return this.denominator;
	}
}
