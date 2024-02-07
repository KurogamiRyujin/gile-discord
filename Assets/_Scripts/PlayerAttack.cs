using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles the player attack and weapons.
/// </summary>
public class PlayerAttack : MonoBehaviour {
    /// <summary>
    /// Reference to the needle prefab that is used to instantiate the needle.
    /// </summary>
	public GameObject needlePrefab;
    /// <summary>
    /// Reference to the hammer prefab that is used to instantiate the hammer.
    /// </summary>
	public GameObject hammerPrefab;
    /// <summary>
    /// Reference to the hammer's animator to be used in relation with player animations.
    /// </summary>
	public Animator hammerAnimator;

    /// <summary>
    /// Reference to the game object of the needle weapon.
    /// </summary>
	private GameObject needle;
    /// <summary>
    /// Reference to the game object of the hammer weapon.
    /// </summary>
	private GameObject hammer;
    /// <summary>
    /// Reference to the needle throwing controller.
    /// </summary>
	private NeedleThrowing needleThrowing;
    /// <summary>
    /// Reference to the needle controller.
    /// </summary>
	private NeedleController needleController;
    /// <summary>
    /// Reference to the hammer object.
    /// </summary>
	private HammerObject hammerAttack;
    /// <summary>
    /// Reference indicating whether the player is using the needle.
    /// </summary>
	[SerializeField] bool usingNeedle = true;
    /// <summary>
    /// Reference indicating whether the player is using the hammer.
    /// </summary>
	[SerializeField] bool usingHammer = false;
    /// <summary>
    /// Reference to the player animator.
    /// </summary>
    private Animator playerAnimator;
    /// <summary>
    /// Reference indicating whether the player can attack.
    /// </summary>
	[SerializeField] bool couldAttack = true;
    /// <summary>
    /// Reference indicating whether the player has the needle.
    /// </summary>
	[SerializeField] bool hasNeedle = false;
    /// <summary>
    /// Reference indicating whether the player has the hammer.
    /// </summary>
	[SerializeField] bool hasHammer = false;
    /// <summary>
    /// Reference indicating whether the player has thread. 
    /// </summary>
	[SerializeField] bool hasThread;
    /// <summary>
    /// Reference indicating whether the player is in dialogue.
    /// </summary>
	private bool inDialogue = false;
    /// <summary>
    /// Reference to the player controller.
    /// </summary>
	PlayerMovement playerController;

	private bool calledLCDHit = false;

    /// <summary>
    /// Reference to the hammer child collider.
    /// </summary>
	private BoxCollider2D hammerChildCollider;
    /// <summary>
    /// Reference to the numerator label text mesh.
    /// </summary>
	[SerializeField] private TextMesh numeratorLabel;
    /// <summary>
    /// Reference to the numerator label object.
    /// </summary>
	[SerializeField] private GameObject numeratorLabelObject;
    /// <summary>
    /// Reference to the fraction label game object.
    /// </summary>
	[SerializeField] private GameObject fractionLabelObject;
    /// <summary>
    /// Reference to the fraction numerator label game object.
    /// </summary>
	[SerializeField] private GameObject fractionNumeratorLabelObject;
    /// <summary>
    /// Reference to the fraction numerator label.
    /// </summary>
	[SerializeField] private TextMesh fractionNumeratorLabel;
    /// <summary>
    /// Reference to the fraction denominator label.
    /// </summary>
	[SerializeField] private TextMesh fractionDenominatorLabel;
    /// <summary>
    /// Reference to the hammer effects.
    /// </summary>
	[SerializeField] private ParticleSystem effectHammerCharge;
	List<TextMesh> listTextMesh;

	[SerializeField] private TutorialScreenCanvas tutorialCanvas;
	[SerializeField] private PickupUIParent pickupUIParent;
	[SerializeField] private CheckpointController checkpointController;

    /// <summary>
    /// Reference to the equipped denominator.
    /// </summary>
	[SerializeField] private int equippedDenominator = 0;
    /// <summary>
    /// Reference to the player.
    /// </summary>
    private PlayerYuni player;

    /// <summary>
    /// Awakes this instance.
    /// </summary>
    void Awake() {
		tag = "Player";
		hammer = Instantiate (hammerPrefab, transform.position, Quaternion.identity);
		hammer.transform.SetParent(this.gameObject.transform);
		hammerAttack = hammer.GetComponent<HammerObject> ();
		hammerAttack.SetEffectHammerCharge(this.effectHammerCharge);
		hammerAnimator = hammerAttack.GetComponentInChildren<Animator> ();
		this.hammerChildCollider = hammerAttack.getHammerChildCollider ();
		hammerChildCollider.enabled = false;
		GameController_v7.Instance.GetPauseController ().onPauseGame += this.DisableAttackOnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += this.EnableAttackOnContinue;
	}


    /// <summary>
    /// Disables the attack on pause.
    /// </summary>
    private void DisableAttackOnPause() {
		if (this.usingHammer) {
			this.canAttack (false);
		}
	}

    /// <summary>
    /// Enables the attack on continue.
    /// </summary>
    private void EnableAttackOnContinue() {
		this.canAttack (true);
	}


    /// <summary>
    /// Starts this instance.
    /// </summary>
    void Start () {
		needle = Instantiate(needlePrefab, transform.position, Quaternion.identity);
		needleThrowing = needle.GetComponent<NeedleThrowing> ();
		needleController = needle.GetComponent<NeedleController> ();


//		if (PlayerPrefs.HasKey ("Player_" + GameController_v7.Instance.GetObjectStateManager().currentPlayerName)) {
//			string playerJSON = PlayerPrefs.GetString ("Player_" + GameController_v7.Instance.GetObjectStateManager().currentPlayerName);
//			PlayerJSONParams playerStats = JsonUtility.FromJson<PlayerJSONParams> (playerJSON);
//			this.hasNeedle = playerStats.hasNeedle;
//			this.hasHammer = playerStats.hasHammer;
//			this.hasThread = playerStats.hasThread;
//			this.equippedDenominator = playerStats.equppedDenominator;
//		}

		this.player = GameObject.FindObjectOfType<PlayerYuni>().GetComponent<PlayerYuni>();


		playerAnimator = player.GetPlayerAnimator ();
		playerController = player.GetPlayerMovement ();

		this.fractionNumeratorLabelObject = this.fractionNumeratorLabel.gameObject;
		this.fractionLabelObject.SetActive (false);
		this.numeratorLabelObject.SetActive (false);


		this.listTextMesh = new List<TextMesh> ();
		listTextMesh.Add (numeratorLabel);
		listTextMesh.Add (fractionNumeratorLabel);
		listTextMesh.Add (fractionDenominatorLabel);

		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_UNPAUSE, this.IsLCDHit);

		this.hammerAttack.SyncWithEquippedDenominator (this.equippedDenominator);
		Debug.Log ("<color=green>Equipped is "+equippedDenominator+"</color>");
		this.needleController.SyncWireSliceCountWithEquippedDenominator (this.equippedDenominator);
		this.hammerAttack.DisableBreaker ();


		if (!this.HasHammer ()) {
			#if UNITY_ANDROID
			GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
			GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
			#endif
		}
		if (this.HasNeedle ()) {
			EventBroadcaster.Instance.PostEvent (EventNames.YUNI_ACQUIRED_NEEDLE);
		}

//		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_DONE, this.IsLCDDonePlaying);
	}


    /// <summary>
    /// Called when the object is destroyed. Removes observers.
    /// </summary>
    void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_LCD_UNPAUSE);
		GameController_v7.Instance.GetPauseController ().onPauseGame -= this.DisableAttackOnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= this.EnableAttackOnContinue;
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_LCD_DONE);
	}

    /// <summary>
    /// Called when the object is enabled.
    /// </summary>
    void OnEnable () {
		if (this.usingHammer)
			this.hammerAnimator.SetTrigger ("open");
	}

	/// <summary>
	/// Updates the fraction label.
	/// </summary>
	/// <param name="numerator"></param>
	/// <param name="denominator"></param>
	/// <returns></returns>
	public void UpdateFractionLabelObject(int numerator, int denominator) {
		if(this.fractionNumeratorLabel != null)
			this.fractionNumeratorLabel.text = "" + numerator;
		if(this.fractionDenominatorLabel != null)
			this.fractionDenominatorLabel.text = "" + denominator;



	}
    

	/// <summary>
	/// Unity function. Called once per frame.
	/// </summary>
	/// <returns></returns>
	void Update () {
		
		this.player.CheckOverlap ();
		if (!UsingHammer ()) {

			this.hammerChildCollider.enabled = false;
		}

		#if UNITY_ANDROID
		if (this.hasNeedle && this.hasHammer &&
			GameController_v7.Instance.GetMobileUIManager().IsSwitchPressed()) {// TODO Change in Player Settings
			//			mobile.switchPressed = false;
			GameController_v7.Instance.GetMobileUIManager().SetSwitch(false);
			ChangeWeapons ();
		}
		else if(GameController_v7.Instance.GetMobileUIManager().IsCharging()) {
			GameController_v7.Instance.GetMobileUIManager ().ReleaseChargeButton ();
			player.CarryItem();
		}
		else if (Input.GetButtonDown ("Fire1")) {
			Attack ();
		}
//		else if(this.usingHammer && mobile.chargePressed) {
//			ChargeHammer ();
//		}
//		else if (this.hasNeedle && this.hasHammer &&
//			GameController_v7.Instance.GetMobileUIManager().IsSwitchPressed()) {// TODO Change in Player Settings
////			mobile.switchPressed = false;
//			GameController_v7.Instance.GetMobileUIManager().SetSwitch(false);
//			ChangeWeapons ();
//		}
		#elif UNITY_STANDALONE
//		if (Input.GetButtonDown ("Fire1")) {
//			Attack ();
////		} else if(this.usingHammer && Input.GetButtonDown ("Fire2")) {
////			ChargeHammer ();
//		}
//		else if(Input.GetButtonDown ("Fire2")) {
////			EventBroadcaster.Instance.PostEvent (EventNames.ON_CHARGING);
//
//			player.CarryItem();
//		}
//		else if (this.hasNeedle && this.hasHammer && Input.GetKeyDown (KeyCode.Q)) { // TODO Change in Player Settings
//			ChangeWeapons ();
//		}
		if (this.hasNeedle && this.hasHammer && Input.GetKeyDown (KeyCode.Q)) {// TODO Change in Player Settings
//			mobile.switchPressed = false;
//			GameController_v7.Instance.GetMobileUIManager().SetSwitch(false);
			ChangeWeapons ();
		}
		else if(Input.GetButtonDown ("Fire2")) {
//			GameController_v7.Instance.GetMobileUIManager ().ReleaseChargeButton ();
			player.CarryItem();
		}
		else if (Input.GetButtonDown ("Fire1")) {
			Attack ();
		}

		#endif

	}

	public void IsLCDDonePlaying() {
		Debug.Log ("LCD Done is called");
		playerAnimator.SetTrigger ("isLCDDone");
	}


	/// <summary>
	/// Checks if a mobile button is clicked.
	/// </summary>
	/// <returns></returns>
	bool isMobileButtonClicked() {

		// Check if a UI Menu is open.
		if (this.GetTutorialCanvas () != null && this.GetTutorialCanvas ().IsOpen ()) {
			return true;
		}
		// Check if a Pickup screen is open. 
		if (this.GetPickupUIParent () != null && this.GetPickupUIParent ().IsOpen ()) {
			return true;
		}

		// Check if a Pickup screen is open. 
		if (this.GetCheckpointController () != null && this.GetCheckpointController ().IsOpen ()) {
			return true;
		}

		if (GameController_v7.Instance.GetMobileUIManager ().IsCharging () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsLeftPressed () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsRightPressed () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsInteracting () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsJumping () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsMenu () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsMenuSubButton () ||
		    GameController_v7.Instance.GetMobileUIManager ().IsHelping ()) {
			return true;
		}
		else {
			return false;
		}
	}

	public PickupUIParent GetPickupUIParent() {
		if (this.pickupUIParent == null) {
			this.pickupUIParent = GameObject.FindObjectOfType<PickupUIParent> ();
		}
		return this.pickupUIParent;
	}

	public CheckpointController GetCheckpointController() {
		if (this.checkpointController == null) {
			this.checkpointController = GameObject.FindObjectOfType<CheckpointController> ();
		}
		return this.checkpointController;
	}

	public TutorialScreenCanvas GetTutorialCanvas() {
		if (this.tutorialCanvas == null) {
			this.tutorialCanvas = GameObject.FindObjectOfType<TutorialScreenCanvas> ();
		}
		return this.tutorialCanvas;
	}

	/// <summary>
	/// Handles how the player attacks.
	/// </summary>
	/// <returns></returns>
	public virtual void Attack() {
		if (couldAttack && !GameController_v7.Instance.GetDialogueManager ().IsDialogueBoxPlaying ()) {
			#if UNITY_STANDALONE
			if (this.hasNeedle && this.usingNeedle) {
				needleThrowing.Throw (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			} else if (this.hasHammer && this.usingHammer
				/*&& this.hammerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hammer Stay")*/) {
				hammerAttack.HammerSmash ();
			}
			#elif UNITY_ANDROID
			if (this.hasNeedle && this.usingNeedle) {
				if(!isMobileButtonClicked()) {
					needleThrowing.Throw (Camera.main.ScreenToWorldPoint (Input.mousePosition));
				}
			} else if (this.hasHammer && this.usingHammer) {
				if(!isMobileButtonClicked()) {
					hammerAttack.HammerSmash ();
				}
			}
			#endif
		}
	}

	private void ChargeHammer() {
		if (couldAttack) {
			Debug.Log ("Hammer charging");

			hammerAttack.HammerCharge ();
		}
	}

	/// <summary>
	/// Handles the changing of weapons.
	/// </summary>
	/// <returns></returns>
	public void ChangeWeapons() {
		this.hammerChildCollider.enabled = false;

		if (this.usingHammer) {
			Debug.Log ("Changed to Needle.");

			this.hammerAttack.DisableBreaker ();
			this.usingHammer = false;
			this.usingNeedle = true;

			this.numeratorLabelObject.SetActive (false);
			this.fractionLabelObject.SetActive (false);
//			this.hammerChildCollider.enabled = false;

			this.hammerAnimator.SetTrigger ("close");
			playerAnimator.SetTrigger ("changed to needle");
			EventBroadcaster.Instance.PostEvent (EventNames.ON_SWITCH_NEEDLE);
			EventBroadcaster.Instance.PostEvent(EventNames.YUNI_ACQUIRED_NEEDLE);

		} else if (this.usingNeedle) {
			Debug.Log ("Changed to Hammer.");
		
			this.usingNeedle = false;
			this.usingHammer = true;
//			this.hammerChildCollider.enabled = false;

//			this.hammerAttack.gameObject.GetComponentInChildren<BoxCollider2D> ().enabled = true;
			this.hammerAttack.updateLabel ();

			this.UpdateFractionLabelObject (this.hammerAttack.GetNumerator(), this.hammerAttack.GetDenominator());

			this.numeratorLabelObject.SetActive (false);
			this.fractionLabelObject.SetActive (true);

			this.hammerAnimator.SetTrigger ("open");
			playerAnimator.SetTrigger ("changed to hammer");
//			this.hammerAttack.DelayedEnableBreaker ();
			this.hammerAttack.DisableBreaker();
			EventBroadcaster.Instance.PostEvent (EventNames.ON_SWITCH_HAMMER);
			EventBroadcaster.Instance.PostEvent(EventNames.YUNI_THREW_NEEDLE);

		}
	}

	public void NotifyDonePropelling() {
		Debug.Log ("Yuni Propel End event called");
		EventBroadcaster.Instance.PostEvent (EventNames.ON_YUNI_PROPEL_END);
	}

	public void IsLCDHit() {
		Debug.Log ("PlayerAttack's IsLCDHit function called");
		this.calledLCDHit = true;
		playerAnimator.ResetTrigger ("hammer attack");
		playerAnimator.SetTrigger ("isLCDHit");
	}

	/// <summary>
	/// Sets the equipped denominator from a yarnball/charm value.
	/// </summary>
	/// <param name="yarnball"></param>
	/// <returns></returns>
	public void SetEquippedDenominatorFromYarnball (Yarnball yarnball) {
		this.equippedDenominator = yarnball.GetDenominator ();
		PostDenominatorEvent (this.equippedDenominator);
		this.hammerAttack.prevDenominator = yarnball.GetDenominator ();
		this.hammerAttack.SyncWithEquippedDenominator (this.equippedDenominator);
		this.UpdateFractionLabelObject (this.hammerAttack.GetNumerator (), this.hammerAttack.GetDenominator ());

		this.needleController.SyncWireSliceCountWithEquippedDenominator (this.equippedDenominator);
	}

	/// <summary>
	/// Posts an event indicating that the denominator has changed.
	/// </summary>
	/// <param name="denominator"></param>
	/// <returns></returns>
	public void PostDenominatorEvent(float denominator) {
		Parameters parameters = new Parameters ();
		parameters.PutExtra (YarnballUI.YARNBALL_VALUE, denominator);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_DENOMINATOR_CHANGE, parameters);
	}

	/// <summary>
	/// Getter function that returns the fraction label game object.
	/// </summary>
	/// <returns></returns>
	public GameObject getFractionLabelObject() {
		return this.fractionLabelObject;
	}

	/// <summary>
	/// Getter function that returns the numerator label game object.
	/// </summary>
	/// <returns></returns>
	public GameObject getNumeratorLabelObject() {
		return this.numeratorLabelObject;
	}

	/// <summary>
	/// Throws the needle.
	/// </summary>
	/// <returns></returns>
	public virtual void ThrowNeedle() {
		needleThrowing.ThrowNeedle ();
	}

	public virtual void Attack1(Vector3 pos) {
		needleThrowing.Throw (pos);
	}

	/// <summary>
	/// Getter function to get the needle throwing controller.
	/// </summary>
	/// <returns></returns>
	public NeedleThrowing getNeedleThrowing() {
		return this.needleThrowing;
	}

	/// <summary>
	/// Setter function to set if the player can attack.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public void canAttack(bool value) {
		this.couldAttack = value;
	}

	/// <summary>
	/// Allows the player to acquire the needle.
	/// </summary>
	/// <returns></returns>
	public void AcquireNeedle() {
//		objectStateManager.UpdatePlayerHasNeedle (true);
		EventBroadcaster.Instance.PostEvent(EventNames.YUNI_ACQUIRED_NEEDLE);
		this.hasNeedle = true;
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
	}

	/// <summary>
	/// Allows the player to acquire the hammer.
	/// </summary>
	/// <returns></returns>
	public void AcquireHammer() {
//		objectStateManager.UpdatePlayerHasHammer (true);
		this.hasHammer = true;
		GameController_v7.Instance.GetMobileUIManager().ToggleMobileControls(false);
		GameController_v7.Instance.GetMobileUIManager().ToggleBaseWithPickupControls(true);
        EventBroadcaster.Instance.PostEvent(EventNames.HAMMER_FORCE_ACQUIRE);
    }


	/// <summary>
	/// Allows the player to acquire the thread.
	/// </summary>
	/// <returns></returns>
	public void AcquireThread() {
//		objectStateManager.UpdatePlayerHasThread (true);
		this.hasThread = true;
	}


	/// <summary>
	/// Getter function to check if the player can attack.
	/// </summary>
	/// <returns></returns>
	public bool IsAttackEnabled() {
		return this.couldAttack;
	}


	/// <summary>
	/// Setter function that sets if the player is in dialogue.
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public void InDialogue(bool flag) {
		this.inDialogue = flag;
	}

	/// <summary>
	/// Setter function to set that the player has needle.
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public void SetHasNeedle(bool flag) {
		this.hasNeedle = flag;
	}

	/// <summary>
	/// Setter function to set that the player has hammer.
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public void SetHasHammer(bool flag) {
		this.hasHammer = flag;
	}

	/// <summary>
	/// Setter function to set that the player has thread.
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public void SetHasThread(bool flag) {
		this.hasThread = flag;
	}

	/// <summary>
	/// Setter function to set that the player is using the needle.
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public void SetUsingNeedle(bool flag) {
		this.usingNeedle = flag;
	}

	/// <summary>
	/// Setter function to set that the player is using the hammer.
	/// </summary>
	/// <param name="flag"></param>
	/// <returns></returns>
	public void SetUsingHammer(bool flag) {
		this.usingHammer = flag;
	}

	/// <summary>
	/// Getter function to check if player has needle.
	/// </summary>
	/// <returns></returns>
	public bool HasNeedle() {
		return this.hasNeedle;
	}

	/// <summary>
	/// Getter function to check if player has hammer.
	/// </summary>
	/// <returns></returns>
	public bool HasHammer() {
		return this.hasHammer;
	}


	/// <summary>
	/// Getter function to check if player has thread.
	/// </summary>
	/// <returns></returns>
	public bool HasThread() {
		return this.hasThread;
	}

    /// <summary>
    /// Getter function to indicate if the player is using the needle.
    /// </summary>
    /// <returns></returns>
	public bool UsingNeedle() {
		return this.usingNeedle;
	}

    /// <summary>
    /// Getter function to indicate if the player is using the hammer.
    /// </summary>
    /// <returns></returns>
	public bool UsingHammer() {
		return this.usingHammer;
	}

    /// <summary>
    /// Getter function that returns the Numerator Label.
    /// </summary>
    /// <returns></returns>
	public TextMesh getNumeratorLabel() {
		return this.numeratorLabel;
	}

    /// <summary>
    /// Getter function that returns the Fraction Numerator Label game object.
    /// </summary>
    /// <returns></returns>
	public GameObject getFractionNumeratorLabelObject() {
		return this.fractionNumeratorLabelObject;
	}

    /// <summary>
    /// Handles text flipping. Negates the text scale based on the parent's scale so that the text won't appear mirrored whenever the parent object flips.
    /// </summary>
	public void HandleTextMeshFlip() { 
		if (listTextMesh != null) {
			// Switch the way the  labels are facing. This is called every time PlayerMovement's Flip() is called to negate its effect
			foreach (TextMesh textMesh in listTextMesh) {
				Vector3 theScale = textMesh.transform.localScale;
				theScale.x *= -1;
				textMesh.transform.localScale = theScale;
			}	
		}	
	}
    /// <summary>
    /// Getter function for Hammer Object.
    /// </summary>
    /// <returns></returns>
	public HammerObject getHammerObject() {
		return this.hammerAttack;
	}
    /// <summary>
    /// Getter function for Hammer Child Collider.
    /// </summary>
    /// <returns></returns>
	public BoxCollider2D getHammerChildCollider() {
		return this.hammerChildCollider;
	}


    /// <summary>
    /// Getter function for Needle.
    /// </summary>
    /// <returns></returns>
	public NeedleController getNeedle() {
		return this.needleController;
	}

	/// <summary>
	/// Sets the player's equipped denominator.
	/// </summary>
	/// <param name="denominator"></param>
	/// <returns></returns>
	public void SetEquippedDenominator(int denominator) {
		this.equippedDenominator = denominator;
		PostDenominatorEvent (this.equippedDenominator);
		this.hammerAttack.prevDenominator = this.equippedDenominator;
		this.hammerAttack.SyncWithEquippedDenominator (this.equippedDenominator);
		this.UpdateFractionLabelObject (this.hammerAttack.GetNumerator (), this.hammerAttack.GetDenominator ());

		this.needleController.SyncWireSliceCountWithEquippedDenominator (this.equippedDenominator);
	}

    /// <summary>
    /// Getter function for the Equipped Denominator.
    /// </summary>
    /// <returns></returns>
	public int getEquippedDenominator() {
		return this.equippedDenominator;
	}

}
