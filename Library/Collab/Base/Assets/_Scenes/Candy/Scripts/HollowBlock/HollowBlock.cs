using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowBlock : MonoBehaviour {
	//	[SerializeField] private BrokenClone clone;
	[SerializeField] private GameObject parent;
	[SerializeField] private BrokenFractionLabel fractionLabel;
	[SerializeField] private string DEFAULT_LAYER = "Default";
	[SerializeField] private string BREAKABLE_LAYER = "Breakable";

	[SerializeField] private bool isSolved;


	[SerializeField] private HintBubbleHollowStability stabilityLabel;
	[SerializeField] private bool isPurelyKinematic; // Main checker if interactable by needle

	[SerializeField] private float numerator;
	[SerializeField] private float denominator;

//	[SerializeField] private float cloneDenominator;

	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private Rigidbody2D rigidBody2D;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private LineRenderer lineRenderer;
	[SerializeField] private BoxCollider2D boxCollider;

//	[SerializeField] private BrokenPiece brokenPiece; // The tangible piece

	[SerializeField] private float widthWhole; // Width of a 1 whole object
	[SerializeField] private float widthPiece; // Width of a piece with size num/den
	[SerializeField] private float widthSingle; // Width of a piece with size 1/den

	[SerializeField] private float height; // Height of object

	[SerializeField] private bool isIntangible; // Main checker if interactable by needle

	[SerializeField] private ResultsUI resultsUI;
	[SerializeField] private HintBubbleManager hintBubble;

	[SerializeField] private HollowBlockSkyPieceContainer skyPieceContainer;

	[SerializeField] private Color tangibleColor = new Color(0.212f, 0.635f, 0.655f, 1f);
//	private Color tangibleOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);
	[SerializeField] private Color tangibleOutlineColor = new Color (1f, 1f, 1f, 1f);


	[SerializeField] private Color observedTangibleColor = new Color(0.74f, 0.62f, 0.25f, 1f);
	[SerializeField] private Color observedTangibleOutlineColor = new Color (0.46f, 0.35f, 0.05f, 1f);

	private Color intangibleColor = new Color(1, 1, 1, 0);
	private Color intangibleOutlineColor = new Color (1, 1, 1, 1);




	void Awake() {
//		this.clone = GetComponentInChildren<BrokenClone> ();
		this.fractionLabel = GetComponentInChildren<BrokenFractionLabel> ();

		this.rectTransform = GetComponent<RectTransform> ();
		this.rigidBody2D = GetComponent<Rigidbody2D> ();
		this.boxCollider = GetComponent<BoxCollider2D> ();

//		this.brokenPiece = GetComponentInChildren<BrokenPiece> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.lineRenderer = GetComponent<LineRenderer> ();

		this.widthWhole = this.rectTransform.rect.width;
		this.widthSingle = this.widthWhole/this.GetDenominator();
		this.widthPiece = this.widthSingle * this.GetNumerator();
		this.height = this.rectTransform.rect.height;

		this.fractionLabel.SetFraction ((int)this.GetNumerator(), (int)this.GetDenominator());
		this.GetStabilityLabel().UpdateLabel (0, (int)this.GetDenominator ());
		this.isSolved = false;
	}
	public Color GetTangibleColor() {
		return this.tangibleColor;
	}
	public Color GetTangibleOutlineColor() {
		return this.tangibleOutlineColor;
	}
	public SpriteRenderer GetSpriteRenderer() {
		return this.spriteRenderer;
	}
	public GameObject GetParent() {
		if (this.parent == null) {
			this.parent = GetComponentInParent<HollowBlockParent> ().gameObject;
			if (this.parent == null) {
				this.parent = GetComponentInParent<Transform> ().gameObject;
			}
		}
		return this.parent;
	}

	void Update() {
		this.UpdateStabilityLabel ();
	}

	public Rigidbody2D GetRigidBody2D() {
		if(this.rigidBody2D == null) {
			this.rigidBody2D = this.GetComponent<Rigidbody2D>();
		}
		return this.rigidBody2D;
	}

	public HintBubbleHollowStability GetStabilityLabel() {
		if (this.stabilityLabel == null) {
			this.stabilityLabel = GetComponentInChildren<HintBubbleHollowStability> ();
		}
		return this.stabilityLabel;
	}



	public HollowBlockSkyPieceContainer GetSkyPieceContainer() {
		if (this.skyPieceContainer == null) {
			this.skyPieceContainer = GetComponentInChildren<HollowBlockSkyPieceContainer> ();
		}
		return this.skyPieceContainer;
	}
	void Start () {
		this.CreatePiece ();
		this.MakeIntangible ();
	}

	public float GetStabilityNumerator() {
		return this.GetSkyPieceContainer ().GetStabilityNumerator ();
	}

	public float GetStabilityDenominator() {
		return this.GetSkyPieceContainer ().GetStabilityDenominator ();
	}

	public void Absorb(SkyFragmentPiece skyPiece) {
		if (!isSolved) {
			this.GetSkyPieceContainer ().Absorb (skyPiece);
			this.UpdateStabilityLabel ();
		}
//		this.CheckStability ();
	}
	public void CheckStability() {
		// Check only of not yet solved
		if (!isSolved) {
			float[] simplifiedTargetValue = General.SimplifyFraction (this.GetNumerator (), this.GetDenominator ());

			// Solved
			if (simplifiedTargetValue [0] == this.GetStabilityNumerator () &&
			    simplifiedTargetValue [1] == this.GetStabilityDenominator ()) {
				this.Solved ();
				this.PostHollowBlockEvent (true);
			}
		}
		// If Solved
		else {
			float[] simplifiedTargetValue = General.SimplifyFraction (this.GetNumerator (), this.GetDenominator ());

			if (simplifiedTargetValue [0] != this.GetStabilityNumerator () ||
				simplifiedTargetValue [1] != this.GetStabilityDenominator ()) {
				//SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false); TODO Play Sound (Breaking)
				this.isSolved = false;
				this.PostHollowBlockEvent (false);
			}
		}
	}

	public void PostHollowBlockEvent(bool isAdd) {
		Parameters parameters = new Parameters ();
		parameters.PutExtra (StabilityNumberLine.NUMERATOR, this.GetNumerator ());
		parameters.PutExtra (StabilityNumberLine.DENOMINATOR, this.GetDenominator ());
		parameters.PutObjectExtra (StabilityNumberLine.COLOR, this.tangibleColor);
		parameters.PutExtra (StabilityNumberLine.IS_ADD, isAdd);
		EventBroadcaster.Instance.PostEvent (EventNames.ON_HOLLOW_STABILITY_UPDATE, parameters);
	}

	public void UpdateStabilityLabel() {
		this.GetSkyPieceContainer ().UpdateSkyPieceList ();
		this.GetSkyPieceContainer().UpdateCurrentStability ();
		this.stabilityLabel.UpdateLabel (
			(int) GetSkyPieceContainer().GetStabilityNumerator(),
			(int) GetSkyPieceContainer().GetStabilityDenominator());

		this.CheckStability ();
	}

	// Create the original piece according to specified numerator and denominator
	// size and then make it intangible
	public void CreatePiece() {
//		this.brokenPiece.SetSize (this.widthPiece, this.height);
		this.spriteRenderer.size = new Vector2 (this.widthPiece, this.height);
		this.boxCollider.size = new Vector2 (this.widthPiece, this.height);
		General.GenerateBoxOutline (this.lineRenderer, this.widthPiece, this.height);
	}

	public void MakeIntangible() {
		Debug.Log ("<color=red>Make Intangible</color>");
		this.isIntangible = true;

		this.rigidBody2D.bodyType = RigidbodyType2D.Kinematic;
//		this.boxCollider.enabled = true;
		this.boxCollider.isTrigger = true;
		this.GetSkyPieceContainer ().Show ();
		this.ChangeColor (this.intangibleColor, this.intangibleOutlineColor);
		gameObject.layer = LayerMask.NameToLayer (DEFAULT_LAYER);
//		this.GetHintBubble ().Activate ();

//		this.clone.Deactivate ();
	}
	public void ChangeColor(Color fill, Color outline) {
		this.spriteRenderer.color = fill;
		this.lineRenderer.startColor = outline;
		this.lineRenderer.endColor = outline;
	}



	public void MakeTangible() {
		Debug.Log ("<color=red>Make Tangible</color>");
		this.isIntangible = false;
//		this.boxCollider.isTrigger = false;
		this.GetBoxCollider().isTrigger = false;
		this.GetSkyPieceContainer ().Hide ();
		this.ChangeColor (this.tangibleColor, this.tangibleOutlineColor);
		gameObject.layer = LayerMask.NameToLayer (BREAKABLE_LAYER);
//		this.GetHintBubble ().Deactivate ();

		if (!isPurelyKinematic) {
			this.SetDynamicRigidBody ();
		}
	}

	public void SetDynamicRigidBody() {
		this.rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
		this.rigidBody2D.angularDrag = 0.05f;
		this.rigidBody2D.drag = 80f;
		this.rigidBody2D.gravityScale = 75f;
		this.rigidBody2D.mass = 1f;
	}

	public void Carry(GameObject parent, Vector3 position) {
		if (this.IsSolved ()) {
			this.Unobserve ();
//			this.SetCarried (true);
//			HideLabel ();
//			this.hintBubble.HideHint ();
			this.ChangeColor(this.tangibleColor, this.tangibleOutlineColor);
			this.gameObject.layer = LayerMask.NameToLayer(DEFAULT_LAYER);
			this.GetStabilityLabel ().Sleep ();
			this.GetRigidBody2D ().bodyType = RigidbodyType2D.Kinematic;
			this.GetRigidBody2D ().velocity = Vector2.zero;
			this.GetRigidBody2D ().angularVelocity = 0f;
			gameObject.transform.SetParent (parent.transform);
			gameObject.transform.localPosition = position;
			gameObject.transform.eulerAngles = new Vector3 (0f, 0f, -45f);
			this.boxCollider.isTrigger = true;
		}
	}

	public void Drop() {
		//		this.SetCarried (false);
		this.gameObject.layer = LayerMask.NameToLayer(BREAKABLE_LAYER);
		this.GetRigidBody2D ().bodyType = RigidbodyType2D.Dynamic;
		gameObject.transform.SetParent (this.GetParent().transform);
		gameObject.transform.eulerAngles = new Vector3 (0f, 0f, 0f);
		gameObject.transform.localScale = 
			new Vector3 (Mathf.Abs(gameObject.transform.localScale.x),
				gameObject.transform.localScale.y,
				gameObject.transform.localScale.z);
		this.boxCollider.isTrigger = false;
//		StartCoroutine (DropRoutine());
	}

//	IEnumerator DropRoutine() {
//		Debug.Log ("<color=magenta>DROP ROUTINE ENTER</color>");
//		Collider2D[] colliders;
//		Vector3 prevPosition = Vector3.zero;
//		bool hasAbsorbed = false;
//		// While falling
//		while (!hasAbsorbed && gameObject.transform.position != prevPosition) {
//			//			ContactFilter2D contactFilter = new ContactFilter2D ();
//			//			Physics2D.OverlapBox (this.boxCollider.transform.position, this.boxCollider.size, this.boxCollider.transform.eulerAngles.z, contactFilter, colliders);
//			colliders = Physics2D.OverlapBoxAll (this.boxCollider.transform.position, this.boxCollider.size, this.boxCollider.transform.eulerAngles.z);
//
//			foreach(Collider2D collider in colliders) {
//				if (!hasAbsorbed && collider.gameObject.GetComponent<HollowBlock> () != null) {
//					hasAbsorbed = true;
//					Debug.Log ("<color=blue>HOLLOW BLOCK COLLISION</color>");
//					HollowBlock hollowBlock = collider.gameObject.GetComponent<HollowBlock> ();
//					hollowBlock.Absorb (this);
//				}
//			}
//
//			prevPosition = gameObject.transform.position;
//			yield return null;
//		}
//		Debug.Log ("<color=magenta>DROP ROUTINE EXIT</color>");
//	}

	public void Observe() {
		if (IsSolved()) {
			Debug.Log ("OBSERVE");
			this.ChangeColor (this.observedTangibleColor, this.observedTangibleOutlineColor);
//			EventBroadcaster.Instance.PostEvent (EventNames.SHOW_PLAYER_CARRY);
		}
	}

	public void Unobserve() {
		Debug.Log ("UNOBSERVE");
		if (this.IsSolved ()) {
			this.ChangeColor (this.tangibleColor, this.tangibleOutlineColor);
		} else {
			this.ChangeColor (this.intangibleColor, this.intangibleOutlineColor);
		}
	}

	public void Break() {
		Debug.Log ("Called Break");
		this.isSolved = true;
		this.GetSkyPieceContainer ().Break ();
		this.UpdateStabilityLabel ();
		this.MakeIntangible ();
	}

	public float GetHeight() {
		return this.height;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!this.isSolved) {
			// Only needle can interact on trigger
			if (other.GetComponent<NeedleController> () != null) {
				NeedleController needle = other.GetComponent<NeedleController> ();
				//			this.HitEvents (needle);
				//			this.Interact (needle.GetSliceCount());
			}

		}
	}
	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.gameObject.GetComponent<BreakerCollider> () != null) {
			BreakerCollider breaker = other.collider.gameObject.GetComponent<BreakerCollider> ();
			this.Break ();
			//			this.HitEvents (needle);
			//			this.Interact (needle.GetSliceCount());
		}
	}

	public BoxCollider2D GetBoxCollider() {
		if (this.boxCollider == null) {
			this.boxCollider = GetComponent<BoxCollider2D> ();
		}
		return this.boxCollider;
	}

//	public void HitEvents(NeedleController needle) {
//		// Continued by BrokenClone Deactivat()
//		GameController_v7.Instance.GetPauseController().Pause ();
//		needle.hasHit = needle.onlyHitOnce;
//
//		#if UNITY_ANDROID
//		//		EventManager.DisableJumpButton ();
//		//		EventManager.ToggleSwitchButton (false);
//		//		GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(false);
//
//		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_JUMP);
//		Parameters parameters = new Parameters ();
//		parameters.PutExtra ("FLAG", false);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
//		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
//		#endif
//		// TODO Record Data?
//	}

	// Needle interaction. Can only interact if intangible
//	public void Interact(SkyFragmentPiece skyPiece) {
//		if (this.isIntangible) {
//			// Add to current stability
//		}
//	}

	// Correct if the clone is equal to the value of this object
	public bool IsCorrect(float cloneNumerator, float cloneDenominator) {
//		if (this.numerator / this.denominator == cloneNumerator / cloneDenominator) {

//			return true;
//		} else {
			return false;
//		}
	}

	public ResultsUI GetResultsUI() {
		if (this.resultsUI == null) {
			this.resultsUI = GetComponentInChildren<ResultsUI> ();
		}
		return this.resultsUI;
	}

//	public HintBubbleManager GetHintBubble() {
//		if (this.hintBubble == null) {
//			this.hintBubble = GetComponentInChildren<HintBubbleManager> ();
//		}
//		return this.hintBubble;
//	}

	// Called when the crafted clone is correct
	public void Solved() {
		this.isSolved = true;
		this.GetResultsUI ().PlayCraft ();
		this.MakeTangible ();
	}

//	public BrokenPiece GetBrokenPiece() {
//		return this.brokenPiece;
//	}

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
	public float GetWidthPiece() {
		return this.widthPiece;	
	}
	public float GetWidthWhole() {
		return this.widthWhole;	
	}

	public bool IsSolved() {
		return this.isSolved;
	}
}
