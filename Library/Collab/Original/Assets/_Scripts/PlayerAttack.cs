using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour {

	public GameObject needlePrefab;
	public GameObject hammerPrefab;
	public Animator hammerAnimator;

	private GameObject needle;
	private GameObject hammer;
	private NeedleThrowing needleThrowing;
	private NeedleController needleController;
	private HammerObject hammerAttack;
	[SerializeField] bool usingNeedle = true;
	[SerializeField] bool usingHammer = false;
	private Animator playerAnimator;
	[SerializeField] bool couldAttack = true;
	[SerializeField] bool hasNeedle = false;
	[SerializeField] bool hasHammer = false;
	[SerializeField] bool hasThread;
	private bool inDialogue = false;
	PlayerMovement playerController;
	MobileUI mobile;

	private bool calledLCDHit = false;

	private BoxCollider2D hammerChildCollider;
	[SerializeField] private TextMesh numeratorLabel;
	[SerializeField] private GameObject numeratorLabelObject;

	[SerializeField] private GameObject fractionLabelObject;
	[SerializeField] private GameObject fractionNumeratorLabelObject;
	[SerializeField] private TextMesh fractionNumeratorLabel;
	[SerializeField] private TextMesh fractionDenominatorLabel;
	[SerializeField] private ParticleSystem effectHammerCharge;
	List<TextMesh> listTextMesh;
	// Use this for initialization

	private int equippedDenominator = 4;
	private PlayerYuni player;

	void Awake() {
		tag = "Player";
		hammer = Instantiate (hammerPrefab, transform.position, Quaternion.identity);
//		hammer.transform.parent = this.transform;
		hammer.transform.SetParent(this.gameObject.transform);
		hammerAttack = hammer.GetComponent<HammerObject> ();
		hammerAttack.SetEffectHammerCharge(this.effectHammerCharge);
		hammerAnimator = hammerAttack.GetComponentInChildren<Animator> ();
		this.hammerChildCollider = hammerAttack.getHammerChildCollider ();
		hammerChildCollider.enabled = false;
		GameController_v7.Instance.GetPauseController ().onPauseGame += this.DisableAttackOnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += this.EnableAttackOnContinue;
	}

	private void DisableAttackOnPause() {
		if (this.usingHammer) {
			this.canAttack (false);
		}
	}

	private void EnableAttackOnContinue() {
		this.canAttack (true);
	}

	void Start () {
		needle = Instantiate(needlePrefab, transform.position, Quaternion.identity);
		needleThrowing = needle.GetComponent<NeedleThrowing> ();
		needleController = needle.GetComponent<NeedleController> ();
		Debug.Log ("<color='blue'>NEEDLE INSTANTIATED </color>");
		if (PlayerPrefs.HasKey ("Player_" + GameController_v7.Instance.GetObjectStateManager().currentPlayerName)) {
			string playerJSON = PlayerPrefs.GetString ("Player_" + GameController_v7.Instance.GetObjectStateManager().currentPlayerName);
			PlayerJSONParams playerStats = JsonUtility.FromJson<PlayerJSONParams> (playerJSON);
			this.hasNeedle = playerStats.hasNeedle;
			this.hasHammer = playerStats.hasHammer;
			this.hasThread = playerStats.hasThread;
		}

//		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		this.player = GameObject.FindObjectOfType<PlayerYuni>().GetComponent<PlayerYuni>();


		playerAnimator = player.GetPlayerAnimator ();
		playerController = player.GetPlayerMovement ();

		this.fractionNumeratorLabelObject = this.fractionNumeratorLabel.gameObject;
		this.fractionLabelObject.SetActive (false);
		this.numeratorLabelObject.SetActive (false);

		#if UNITY_ANDROID
		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif

		this.listTextMesh = new List<TextMesh> ();
		listTextMesh.Add (numeratorLabel);
		listTextMesh.Add (fractionNumeratorLabel);
		listTextMesh.Add (fractionDenominatorLabel);

		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_UNPAUSE, this.IsLCDHit);

//		this.numeratorLabel = GetComponentInChildren<NumeratorLabel> ().GetComponent<TextMesh>();
		this.hammerAttack.SyncWithEquippedDenominator (this.equippedDenominator);
		Debug.Log ("<color=green>Equipped is "+equippedDenominator+"</color>");
		this.needleController.SyncWireSliceCountWithEquippedDenominator (this.equippedDenominator);
		this.hammerAttack.DisableBreaker ();

//		EventBroadcaster.Instance.AddObserver (EventNames.ON_LCD_DONE, this.IsLCDDonePlaying);
	}

	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_LCD_UNPAUSE);
		GameController_v7.Instance.GetPauseController ().onPauseGame -= this.DisableAttackOnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= this.EnableAttackOnContinue;
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_LCD_DONE);
	}

	void OnEnable () {
		if (this.usingHammer)
			this.hammerAnimator.SetTrigger ("open");
	}

	public void UpdateFractionLabelObject(int numerator, int denominator) {
		this.fractionNumeratorLabel.text = "" + numerator;
		this.fractionDenominatorLabel.text = "" + denominator;



	}
	// Update is called once per frame
	void Update () {
		//		if (inDialogue)
		//			couldAttack = false;
		//		else
		//			couldAttack = true;
		//		Debug.Log ("Could Attack: " + couldAttack);

		//			bool clickedOnScene = true;
		//			RaycastHit hit;
		//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		//
		//			if (EventSystem.current.IsPointerOverGameObject ()) {
		//				GameObject objectHit = hit.transform.gameObject;
		//				Debug.Log ("Dialogue Tag: " + objectHit.tag);
		//				if (objectHit.CompareTag ("Dialogue Box"))
		//					clickedOnScene = false;
		//			}
		//
		//			if(clickedOnScene)
		//				Attack ();

		if (!UsingHammer ()) {

			this.hammerChildCollider.enabled = false;
		}

		#if UNITY_ANDROID
		if (Input.GetButtonDown ("Fire1")) {
			Attack ();
		}
//		else if(this.usingHammer && mobile.chargePressed) {
//			ChargeHammer ();
//		}
		else if (this.hasNeedle && this.hasHammer && mobile.switchPressed) { // TODO Change in Player Settings
			mobile.switchPressed = false;
			ChangeWeapons ();
		}
		#elif UNITY_STANDALONE
		if (Input.GetButtonDown ("Fire1")) {
			Attack ();
//		} else if(this.usingHammer && Input.GetButtonDown ("Fire2")) {
//			ChargeHammer ();
		}
		else if (this.hasNeedle && this.hasHammer && Input.GetKeyDown (KeyCode.Q)) { // TODO Change in Player Settings
			ChangeWeapons ();
		}
		#endif

	}

	public void IsLCDDonePlaying() {
		Debug.Log ("LCD Done is called");
		playerAnimator.SetTrigger ("isLCDDone");
	}

	bool isMobileButtonClicked() {
//		if (mobile.leftPressed || mobile.rightPressed || mobile.interactPressed || 
//			mobile.jumpPressed || mobile.switchPressed || mobile.chargePressed)

//		if(mobile.leftPressed || mobile.rightPressed) 
//			return false;

//			return false;
		if (mobile.leftPressed || mobile.rightPressed || mobile.interactPressed || 
			mobile.jumpPressed || mobile.switchPressed || mobile.chargePressed)
			return true;
		else
			return false;
	}
	public virtual void Attack() {
		if (couldAttack) {
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
			this.hammerAttack.DelayedEnableBreaker ();
			EventBroadcaster.Instance.PostEvent (EventNames.ON_SWITCH_HAMMER);
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

	public void SetEquippedDenominatorFromYarnball (Yarnball yarnball) {
		this.equippedDenominator = yarnball.GetDenominator ();
		this.hammerAttack.prevDenominator = yarnball.GetDenominator ();
		this.hammerAttack.SyncWithEquippedDenominator (this.equippedDenominator);
		this.UpdateFractionLabelObject (this.hammerAttack.GetNumerator (), this.hammerAttack.GetDenominator ());

		this.needleController.SyncWireSliceCountWithEquippedDenominator (this.equippedDenominator);
	}

	public GameObject getFractionLabelObject() {
		return this.fractionLabelObject;
	}

	public GameObject getNumeratorLabelObject() {
		return this.numeratorLabelObject;
	}

	public virtual void ThrowNeedle() {
		needleThrowing.ThrowNeedle ();
	}

	public virtual void Attack1(Vector3 pos) {
		needleThrowing.Throw (pos);
	}

	public NeedleThrowing getNeedleThrowing() {
		return this.needleThrowing;
	}

	public void canAttack(bool value) {
		this.couldAttack = value;
	}

	public void AcquireNeedle() {
//		objectStateManager.UpdatePlayerHasNeedle (true);
		this.hasNeedle = true;
	}

	public void AcquireHammer() {
//		objectStateManager.UpdatePlayerHasHammer (true);
		this.hasHammer = true;
	}
	public void AcquireThread() {
//		objectStateManager.UpdatePlayerHasThread (true);
		this.hasThread = true;
	}
	public bool IsAttackEnabled() {
		return this.couldAttack;
	}

	public void InDialogue(bool flag) {
		this.inDialogue = flag;
	}

	public void SetHasNeedle(bool flag) {
		this.hasNeedle = flag;
	}

	public void SetHasHammer(bool flag) {
		this.hasHammer = flag;
	}

	public void SetHasThread(bool flag) {
		this.hasThread = flag;
	}

	public void SetUsingNeedle(bool flag) {
		this.usingNeedle = flag;
	}

	public void SetUsingHammer(bool flag) {
		this.usingHammer = flag;
	}

	public bool HasNeedle() {
		return this.hasNeedle;
	}

	public bool HasHammer() {
		return this.hasHammer;
	}

	public bool HasThread() {
		return this.hasThread;
	}

	public bool UsingNeedle() {
		return this.usingNeedle;
	}

	public bool UsingHammer() {
		return this.usingHammer;
	}

	public TextMesh getNumeratorLabel() {
		return this.numeratorLabel;
	}

	public GameObject getFractionNumeratorLabelObject() {
		return this.fractionNumeratorLabelObject;
	}

	public void HandleTextMeshFlip() { 
		// Switch the way the  labels are facing. This is called everytime PlayerMovement's Flip() is called to negate its effect
		foreach (TextMesh textMesh in listTextMesh) {
			Vector3 theScale = textMesh.transform.localScale;
			theScale.x *= -1;
			textMesh.transform.localScale = theScale;
		}			
	}
	public HammerObject getHammerObject() {
		return this.hammerAttack;
	}
	public BoxCollider2D getHammerChildCollider() {
		return this.hammerChildCollider;
	}

	public NeedleController getNeedle() {
		return this.needleController;
	}

	public int getEquippedDenominator() {
		return this.equippedDenominator;
	}

}
