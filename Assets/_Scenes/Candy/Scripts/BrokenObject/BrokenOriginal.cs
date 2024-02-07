using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenOriginal : MonoBehaviour {

	[SerializeField] private BrokenClone clone;
	[SerializeField] private BrokenFractionLabel fractionLabel;

	[SerializeField] private bool isPurelyKinematic; // Main checker if interactable by needle

	[SerializeField] private float numerator;
	[SerializeField] private float denominator;


	[SerializeField] private float cloneDenominator;


	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private Rigidbody2D rigidBody2D;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private BoxCollider2D boxCollider;
	[SerializeField] private BrokenPiece brokenPiece; // The tangible piece

	[SerializeField] private float widthWhole; // Width of a 1 whole object
	[SerializeField] private float widthPiece; // Width of a piece with size num/den
	[SerializeField] private float widthSingle; // Width of a piece with size 1/den

	[SerializeField] private float height; // Height of object

	[SerializeField] private bool isIntangible; // Main checker if interactable by needle

	[SerializeField] private ResultsUI resultsUI;
	[SerializeField] private HintBubbleManager hintBubble;


	private Color tangibleColor = new Color(0, 1, 1, 1);
	private Color tangibleOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);

	private Color intangibleColor = new Color(1, 1, 1, 0);
	private Color intangibleOutlineColor = new Color (1, 1, 1, 1);


	void Awake() {
		this.clone = GetComponentInChildren<BrokenClone> ();
		this.fractionLabel = GetComponentInChildren<BrokenFractionLabel> ();

		this.rectTransform = GetComponent<RectTransform> ();
		this.rigidBody2D = GetComponent<Rigidbody2D> ();
		this.boxCollider = GetComponent<BoxCollider2D> ();
			
		this.brokenPiece = GetComponentInChildren<BrokenPiece> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.lineRenderer = GetComponent<LineRenderer> ();

		this.widthWhole = this.rectTransform.rect.width;
		this.widthSingle = this.widthWhole/this.GetDenominator();
		this.widthPiece = this.widthSingle * this.GetNumerator();
		this.height = this.rectTransform.rect.height;

		this.fractionLabel.SetFraction ((int)this.GetNumerator(), (int)this.GetDenominator());
	}

	void Start () {
//		this.lineRenderer = General.GenerateBoxOutline (this.lineRenderer, this.widthWhole, this.GetHeight());
		this.CreatePiece ();
		this.MakeIntangible ();
	}


	// Create the original piece and make it intangible)
	public void CreatePiece() {
		this.brokenPiece.SetSize (this.widthPiece, this.height);
		this.spriteRenderer.size = new Vector2 (this.widthPiece, this.height);
		this.boxCollider.size = new Vector2 (this.widthPiece, this.height);
	}

	public void MakeIntangible() {
		this.isIntangible = true;

		this.rigidBody2D.bodyType = RigidbodyType2D.Kinematic;
		this.boxCollider.enabled = true;
		this.brokenPiece.ChangeColor (this.intangibleColor, this.intangibleOutlineColor);

		this.GetHintBubble ().Activate ();

		this.clone.Deactivate ();
	}



	public void MakeTangible() {
		this.isIntangible = false;
		this.boxCollider.isTrigger = false;
		this.brokenPiece.ChangeColor (this.tangibleColor, this.tangibleOutlineColor);

		this.GetHintBubble ().Deactivate ();

		if (!isPurelyKinematic) {
			this.rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
		}
	}

	public float GetHeight() {
		return this.height;
	}

	void OnTriggerEnter2D(Collider2D other) {

		// Only needle can interact on trigger
		if (other.GetComponent<NeedleController> () != null) {
			NeedleController needle = other.GetComponent<NeedleController> ();
			this.HitEvents (needle);
			this.Interact (needle.GetSliceCount());
		}
	}

	public void HitEvents(NeedleController needle) {
		// Continued by BrokenClone Deactivat()
		GameController_v7.Instance.GetPauseController().Pause ();

		needle.hasHit = needle.onlyHitOnce;

		#if UNITY_ANDROID
//		EventManager.DisableJumpButton ();
//		EventManager.ToggleSwitchButton (false);
//		GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(false);

		Parameters parameters = new Parameters();
		parameters.PutExtra("FLAG", false);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_JUMP, parameters);

		parameters = new Parameters();
		parameters.PutExtra("FLAG", true);
		EventBroadcaster.Instance.PostEvent(EventNames.TOGGLE_INTERACT, parameters);

		parameters = new Parameters ();
		parameters.PutExtra ("FLAG", false);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
		#endif
		// TODO Record Data?
	}
		
	// Needle interaction. Can only interact if intangible
	public void Interact(int sliceCount) {
		if (this.isIntangible) {
			this.cloneDenominator = sliceCount;
			this.clone.Activate (this.cloneDenominator);
		}
	}

	// Correct if the clone is equal to the value of this object
	public bool IsCorrect(float cloneNumerator, float cloneDenominator) {
		if (this.numerator / this.denominator == cloneNumerator / cloneDenominator) {
			return true;
		} else {
			return false;
		}
	}

	public ResultsUI GetResultsUI() {
		if (this.resultsUI == null) {
			this.resultsUI = GetComponentInChildren<ResultsUI> ();
		}
		return this.resultsUI;
	}

	public HintBubbleManager GetHintBubble() {
		if (this.hintBubble == null) {
			this.hintBubble = GetComponentInChildren<HintBubbleManager> ();
		}
		return this.hintBubble;
	}

	// Called when the crafted clone is correct
	public void Solved() {
		this.GetResultsUI ().PlaySuccess ();
		this.MakeTangible ();
	}

	public BrokenPiece GetBrokenPiece() {
		return this.brokenPiece;
	}

	public float GetNumerator() {
		if (this.numerator<= 0)
			this.numerator = 1;
		return this.numerator;
	}

	public float GetDenominator() {
		if (this.denominator <= 0)
			this.denominator = 1;
		return this.denominator;
	}

	public float GetWidthWhole() {
		return this.widthWhole;	
	}
}
