using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartitionableObject_v2 : MonoBehaviour {
	protected const float INTANGIBLE_OPACITY = 0.3f;
	protected const float TANGIBLE_OPACITY = 1.0f;

	[SerializeField] private bool willChangeBodyType = true;

	public GameObject partitionPiece;
	public bool enableClone = true;
	[SerializeField] protected bool isTangible;

	private Color highlightColor = new Color (0.898f, 0.851f, 0.267f, 1f);
	private Color disabledColor = new Color(1, 1, 1, 0);
	private Color highlightOutlineColor = new Color (0.455f, 0.345f, 0.031f, 1f);
	private Color disabledOutlineColor = new Color(1, 1, 1, 1);

	public Color cloneHighlightColor = new Color(0, 1, 1, 1);
	public Color cloneHighlightOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);


	public Sprite tangibleSprite;
	public float scaleReduction = 0.04f;
	public int blockWidth = 1;

	protected Vector3 clonePosition;
	protected GameObject cloneObject;
	protected PartitionableObjectMarker fractionLabel;
	protected int partitionPointer;

	[SerializeField] protected List<int> validDenominators;
	protected FractionsReference fractionReference;
	protected float partitionCount = 0;
	protected float filledCount = 0;
	protected List<GameObject> partitions;
	protected List<GameObject> partitionsReversed;

	protected Collider2D objectCollider;
	protected bool isFilling;
	protected bool isAnimatingFill = false;
//	protected Animator animator;
//	protected SpriteRenderer spriteRenderer;
	[SerializeField] protected PartitionableSprite[] spriteRenderers;
//	protected SpriteRenderer[] childrenSpriteRenderers;
	protected Rigidbody2D rigidBody2d;

	[SerializeField] protected float disabledTransparencyValue = 0.3f;
	[SerializeField] private bool isArbitraryValue = true;
	[SerializeField] private int arbitraryNumerator = 2;
	[SerializeField] private int arbitraryDenominator = 5;
	[SerializeField] protected List<LineRenderer> outlineRenderers;
	[SerializeField] protected Material lineMaterial;

	//	PlayerData playerData;
	protected Tuple<Entry.Type, int> poBoxKey;
	protected Tuple<Entry.Type, int> currentKey;
	protected POEntry entry;

	protected int sceneObjectId;
	protected HintBubbleManager hintBubble;
	void Awake() {
		UnityEngine.Random.InitState (3216);
		validDenominators = new List<int> ();
		fractionReference = FractionsReference.Instance ();
		fractionReference.AddObserver (this.RequestValidDenominators);
		//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);

//		animator = gameObject.GetComponent<Animator> ();

		spriteRenderers = gameObject.GetComponentsInChildren<PartitionableSprite>();

		this.hintBubble = GetComponentInChildren<HintBubbleManager> ();
//		spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, INTANGIBLE_OPACITY);
//		childrenSpriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		this.objectCollider = gameObject.GetComponent<Collider2D> ();
		rigidBody2d = gameObject.GetComponent<Rigidbody2D> ();
		fractionLabel = gameObject.GetComponent<PartitionableObjectMarker> ();

		this.disabledTransparency ();
//		ChangeSpriteOpacity (INTANGIBLE_OPACITY);
	}
	public HintBubbleManager GetHintBubbleManager() {
		if (this.hintBubble == null) {
			this.hintBubble = GetComponentInChildren<HintBubbleManager> ();
		}
		return this.hintBubble;
	}
	public Color GetDisabledColor() {
		return this.disabledColor;
	}
	public Color GetDisabledOutlineColor() {
		return this.disabledOutlineColor;
	}
	public Color GetHighlightColor() {
		return this.highlightColor;
	}
	public Color GetHighlightOutlineColor() {
		return this.highlightOutlineColor;
	}
	public void ChangeSpriteOpacity(float opacity) {
		if (this.spriteRenderers == null) {
			spriteRenderers = gameObject.GetComponentsInChildren<PartitionableSprite> ();
		}
		SpriteRenderer spriteRenderer;
		foreach (PartitionableSprite sprite in this.spriteRenderers) {
			Debug.Log ("SPRITE " + sprite.name);
			spriteRenderer = sprite.gameObject.GetComponent<SpriteRenderer> ();
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
		}
		Debug.Log ("SPRITE SIZE "+spriteRenderers.Length+" opacity "+opacity);
	}

	// Use this for initialization
	void Start () {
		poBoxKey = null;
		isTangible = false;
		clonePosition = GetComponentInChildren<ClonePosition>().transform.position;
		this.outlineRenderers = new List<LineRenderer> ();

		fractionLabel.Disable ();

		//		playerData = GameObject.FindGameObjectWithTag ("PlayerData").GetComponent<PlayerData> ();
		//		FractionsReference.Instance ().UpdateValidDenominators ();

		if (validDenominators == null)
			Debug.LogError ("Start Error");


		Debug.Log ("PARTITIONABLE start");
	}

	protected void RequestValidDenominators(List<int> validDenominators) {
		this.validDenominators = validDenominators;
		Debug.Log ("Denominators count on request: " + this.validDenominators.Count);
		if (validDenominators.Count < 1) {
			this.gameObject.SetActive (false);
		} else {
			this.gameObject.SetActive (true);
		}
	}

	void OnEnable() {
		//		FractionsReference.Instance ().AddObserver (this.RequestValidDenominators);
		partitions = new List<GameObject> ();
		//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);
		InitiateFraction ();
	}

	//	void OnDisable() {
	//		FractionsReference.Instance ().RemoveObserver (this.RequestValidDenominators);
	//	}

	void OnDestroy() {
		foreach (GameObject temp in partitions) {
			Destroy (temp);
		}
		fractionReference.RemoveObserver (this.RequestValidDenominators);
	}

	protected void InitiateFraction() {
		Debug.Log ("Initiating Fraction");
		if (this.isArbitraryValue) {
			this.ArbitraryValue ();
		}
		else {
			PedagogicalValue ();
		}
		this.GetHintBubbleManager ().Activate();
	}

	void ArbitraryValue() {

		//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);
//		this.validDenominators = fractionReference.RequestDenominators ();
//		if (validDenominators.Count < 1) {
//			this.gameObject.SetActive (false);
//			return;
//		} else {
//			this.gameObject.SetActive (true);
//		}

		this.partitionCount = arbitraryDenominator;
		this.filledCount = arbitraryNumerator;

	}

	void PedagogicalValue() {

		//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);
		this.validDenominators = fractionReference.RequestDenominators ();
		if (validDenominators.Count < 1) {
			this.gameObject.SetActive (false);
			return;
		} else {
			this.gameObject.SetActive (true);
		}

		this.partitionCount = validDenominators [UnityEngine.Random.Range (0, validDenominators.Count)];
		this.filledCount = (int) UnityEngine.Random.Range (1, partitionCount);

	}

	// Update is called once per frame
	void Update () {
//		clonePosition = new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + gameObject.transform.localScale.y);
		if (cloneObject != null) {
			if (enableClone) {
				cloneObject.GetComponent<PartitionedClone_v2> ().Enabled (true);
			} else {
				cloneObject.GetComponent<PartitionedClone_v2> ().Enabled (false);
			}
		}
	}

	public void clone(int sliceCount) {
		GameController_v7.Instance.GetPauseController().Pause ();
		cloneObject = CloneBuilder_v2.Instance.SpawnClone(this.gameObject, clonePosition, highlightColor, disabledColor, this.scaleReduction);
		Destroy(cloneObject.GetComponent<Rigidbody2D> ());
		cloneObject.transform.position = new Vector3 (0, gameObject.transform.localScale.y, cloneObject.transform.position.z);
		cloneObject.transform.SetPositionAndRotation (new Vector3 (0, gameObject.transform.localScale.y, cloneObject.transform.position.z), Quaternion.identity);
		cloneObject.transform.localPosition = new Vector3 (0, gameObject.transform.localScale.y, cloneObject.transform.position.z);

//		partitionController.Partition (this, cloneObject, sliceCount);
		cloneObject.GetComponent<PartitionedClone_v2> ().Partition (sliceCount, this.partitionPiece, this.gameObject);
		this.Partition ();

		this.MatchScale (cloneObject);
	}
	public void MatchScale(GameObject clone) {
//		Transform originalTransform = clone.transform;
//		originalTransform.position = new Vector3 (0, originalTransform.position.y, originalTransform.position.z);
		// To fix scaling, resize the clone outside the parent


		if (gameObject.GetComponentsInParent<Transform> ().Length > 1)
			clone.transform.SetParent (gameObject.GetComponentsInParent<Transform> () [1]);
		else
			clone.transform.SetParent (null);
		
		clone.transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);

		PartitionedClone_v2 partitionedClone = clone.GetComponent<PartitionedClone_v2> ();
		List<GameObject> clonedPartitions = partitionedClone.getPartitions ();


		float pieceSize = partitions[0].GetComponentInChildren<SpriteRenderer> ().bounds.size.x;
		float objectSize = pieceSize*partitionCount;

		float currentWidth = clonedPartitions[0].GetComponentInChildren<SpriteRenderer> ().bounds.size.x;
		Vector3 currentScale = clonedPartitions [0].transform.localScale;

		float desiredLength = objectSize/clonedPartitions.Count;


		for (int i = 0; i < clonedPartitions.Count; i++) {
			clonedPartitions [i].transform.localScale = new Vector3 (General.LengthToScale(currentWidth, currentScale, desiredLength).x,
				this.partitions[0].transform.localScale.y,
				this.partitions[0].transform.localScale.z);
			
//			clonedPartitions [i].transform.localScale = new Vector3 (this.partitions[i].transform.localScale.x,
//				this.partitions[i].transform.localScale.y,
//				this.partitions[i].transform.localScale.z);
		}
//		partitionedClone.getPartitions ().Reverse ();
		clone.transform.SetParent (gameObject.transform);
		clone.transform.position = clonePosition;
	}

	// When this is solved, allow needle pull/jump
	void OnCollisionEnter2D(Collision2D collision) {
		GameObject other = collision.collider.gameObject;
		if (this.isTangible && other.GetComponent<NeedleController> () != null
		    && !other.GetComponent<NeedleController> ().hasHit) {
			NeedleController needle = other.GetComponent<NeedleController> ();
			this.Manipulate (needle);
		}
	}
	void Manipulate(NeedleController needle) {
		needle.hasHit = true;
		this.rigidBody2d.velocity = Vector2.zero;
		this.rigidBody2d.AddRelativeForce (Vector2.up*500f);
	}

	public float GetValue() {
		return this.fractionLabel.GetValue ();
	}

	//Changes the highlight of the index depending on the value of isHighlight (true = highlighted)
	void highlightPartition(int index, bool isHighlight) {
		if (index == -1) {
			this.ChangeColorOutline (index, this.disabledOutlineColor, "DisabledPartition");
			partitions [0].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [0].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
		}
		else if (isHighlight) {
			this.ChangeColorOutline (index, this.highlightOutlineColor, "HighlightedPartition");
			partitions [index].GetComponent<SpriteRenderer> ().color = this.highlightColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "HighlightedPartition";
		}
		else {
			this.ChangeColorOutline (index, this.disabledOutlineColor, "DisabledPartition");
			partitions [index].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
		}

		this.fractionLabel.UpdateValue (index + 1, partitionCount);
	}

	void ChangeColorOutline(int index, Color newColor, string sortingLayer) {
		this.outlineRenderers [index].startColor = newColor;
		this.outlineRenderers [index].endColor = newColor;
		outlineRenderers[index].sortingLayerName = sortingLayer;
	}

	// Increment partitionPointer by 1
	protected void increasePartition() {
		int partitionIndex = this.partitionPointer + 1;
		this.setPartitionPointer (partitionIndex);
		highlightPartition (this.partitionPointer, true);
	}

	// Decrement partitionPointer by 1
	protected void decreasePartition() {

		highlightPartition (this.partitionPointer, false);
		int partitionIndex = this.partitionPointer - 1;
		this.setPartitionPointer (partitionIndex);
	}

	// Sets the partitionPointer. If greater than the number of partitions, it returns to 0
	void setPartitionPointer(int pointerValue) {
		if (pointerValue >= this.getPartitions().Count) {
			pointerValue = this.getPartitions().Count-1;
		}
		else if (pointerValue < -1) {
			pointerValue = -1;
		}

		this.partitionPointer = pointerValue;
	}


	// Returns the partitions array. If null, it instantiates it.
	List<GameObject> getPartitions() {
		if (this.partitions == null) {
			this.partitions = new List<GameObject>();
		}
		return this.partitions;
	}
	public void Partition() {
		this.outlineRenderers.Clear ();

		GameObject temp;
		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer> ();
		Vector2 pos;

		this.partitionPointer = -1;
		float prevPieceX = 0f;
		this.partitionsReversed = new List<GameObject> ();

		for(int x = 0; x < partitionCount; x++) {
			pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

			temp = Instantiate (partitionPiece, pos, Quaternion.identity);
			temp.transform.localScale = new Vector2 (gameObject.transform.localScale.x / partitionCount, gameObject.transform.localScale.y);
			//			temp.transform.localScale = new Vector2 (temp.transform.localScale.x-scaleReduction, temp.transform.localScale.y-scaleReduction);

			temp.GetComponent<SpriteRenderer> ().sortingLayerName = "Clone Partition";
			temp.AddComponent<RectTransform> ();
			temp.SetActive (true);
			temp.name = "partition_"+x;
			temp.tag = "Partition Instance";
			temp.transform.SetParent (gameObject.transform);

			Bounds cloneBounds = gameObject.GetComponent<SpriteRenderer> ().bounds;

			Vector2 locPos = new Vector2(cloneBounds.center.x + (cloneBounds.extents.x - (cloneBounds.extents.x/partitionCount)) - prevPieceX, cloneBounds.center.y);

			temp.transform.position = locPos;


			this.GenerateOutline (temp);
			prevPieceX += (cloneBounds.size.x / partitionCount);
			//			prevPieceX += (cloneBounds.size.x/partitionCount)-0.03f;

			partitions.Add (temp);
			partitionsReversed.Add (temp);
			this.highlightPartition (x, false);
		}

		this.partitionsReversed.Reverse ();

		Debug.Log (this.filledCount);
		for (int i = 0; i < this.filledCount; i++) {
			Debug.Log ("Fill");
			this.increasePartition ();
		}


		this.fractionLabel.Enable ();
		this.fractionLabel.UpdateValue (filledCount, partitionCount);
	}

	public void GenerateOutline(GameObject parent) {
		LineRenderer lineRenderer = parent.AddComponent<LineRenderer>();
		Vector3[] positionsArray = new Vector3[5];

		lineRenderer.material = this.lineMaterial;
		lineRenderer.useWorldSpace = false;
		lineRenderer.widthMultiplier = 0.05f;
		lineRenderer.sortingLayerName = "HighlightedPartition";
		lineRenderer.sortingOrder = 10;
		lineRenderer.positionCount = positionsArray.Length;


		//		AnimationCurve curve = new AnimationCurve ();
		//		curve.AddKey (0f, 0f);
		//		curve.AddKey (0f, 0f);
		//		curve.AddKey (1f, 1f);
		//		curve.AddKey (0f, 0f);
		//		lineRenderer.widthCurve = curve;

		Bounds tempBounds = parent.GetComponent<SpriteRenderer> ().bounds;
		RectTransform rectBounds = parent.GetComponent<RectTransform>();
		float bleedX = 0.04f;
		float bleedY = 0.06f;

		float lineX = (rectBounds.rect.width/2);
		float lineY = (rectBounds.rect.height/2);

		Vector3 upperleft = new Vector3 (-lineX, lineY, 0f);
		Vector3 upperRight = new Vector3 (lineX, lineY, 0f);

		Vector3 lowerRight = new Vector3 (lineX, -lineY, 0f);
		Vector3 lowerLeft = new Vector3 (-lineX, -lineY, 0f);

		//		Vector3 closeLeft = new Vector3 (-lineX, lineY+0.011f, 0f);
		Vector3 closeRight = new Vector3 (lineX+0.02f, -lineY, 0f);

		positionsArray [0] = lowerRight;
		positionsArray [1] = upperRight;
		positionsArray [2] = upperleft;
		positionsArray [3] = lowerLeft;
		positionsArray [4] = closeRight;
		lineRenderer.SetPositions (positionsArray);

		outlineRenderers.Add (lineRenderer);
	}

	public Material GetLineMaterial() {
		return this.lineMaterial;
	}

	public LineRenderer CreateLineRenderer(LineRenderer line) {
		line.material = this.lineMaterial;
		line.useWorldSpace = false;
		line.startWidth = 0.06f;
		line.endWidth = 0.06f;
		line.sortingLayerName = "HighlightedPartition";
		line.sortingOrder = 10;
		line.positionCount = 2;
		return line;
	}


	public virtual void Fill(float fillCount, float totalFill) {
		Debug.Log ("Filled Count: " + filledCount);
		Debug.Log ("Partition Count: " + partitionCount);
		Debug.Log ("Fill Count: " + fillCount);
		Debug.Log ("Total Fill: " + totalFill);
		Debug.Log (((filledCount / partitionCount) + (fillCount / totalFill)));
		Debug.Log ("Epsilon: " + Mathf.Epsilon);

		if(entry.objectGiven == null)
			entry.objectGiven = new Tuple<int, int>((int)filledCount, (int)partitionCount);
		if(entry.actualAnswer == null)
			entry.actualAnswer = new Tuple<int, int>((int)partitionCount - (int)filledCount, (int)partitionCount);
		if(entry.attemptedAnswers == null)
			entry.attemptedAnswers = new List<Tuple<int, int>>();
		entry.attemptedAnswers.Add(new Tuple<int, int>((int)fillCount, (int)totalFill));
		entry.numberOfAttempts++;

		float numerator1 = filledCount;
		float denominator1 = partitionCount;
		float numerator2 = fillCount;
		float denominator2 = totalFill;

		print (numerator1);
		print (denominator1);

		print (numerator2);
		print (denominator2);

		if (denominator1 != denominator2) {
			numerator1 *= denominator2;
			denominator1 *= denominator2;
			numerator2 *= denominator1;
			denominator2 *= denominator1;
		}

		if (Mathf.Abs (((numerator1/denominator1) + (numerator2/denominator2)) - 1)  == 0) {
			for (int i = 0; i < fillCount; i++) {
				this.increasePartition ();
			}
			StartCoroutine (this.animateFill ());
		} else {
			// Will we implement the animation that causes individual pieces to go into the object individually and melt when not whole?
			Debug.Log("Failed to fill...");
		}

		DataManager.UpdatePOEntry(DataManager.GetPOLastKey(), entry);

//		this.enabledTransparency ();
		//Will have to visualize the changes overtime
		//for now, comment out
		//fractionLabel.UpdateValue (filledCount + fillCount, partitionCount);
	}

	IEnumerator animateFill() {
		this.isAnimatingFill = true;
//		animator.SetBool ("isTangible", true);

//		Debug.Log (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " :: " + this.animator.GetCurrentAnimatorStateInfo(0).length);
//		float animationTime = Time.time + this.animator.GetCurrentAnimatorStateInfo (0).length;
//		while(Time.time < animationTime) {
//			this.isAnimatingFill = true;
//			yield return null;
//		}

		// Enable collider once filled
		//		playerCollider2D = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D> ();
		//		Physics2D.IgnoreCollision (playerCollider2D, GetComponent<Collider2D> (), false);

		Solved ();
		yield return null;
		//		this.isTangible = true;
		//		this.isAnimatingFill = false;
		//		gameObject.GetComponent<SpriteRenderer> ().sprite = tangibleSprite;
		//		gameObject.GetComponent<Collider2D> ().isTrigger = false;
		//		this.rigidBody2d.bodyType = RigidbodyType2D.Dynamic;
		//
		//		this.rigidBody2d.drag = 5;
		//		this.rigidBody2d.useAutoMass = true;
		//
		//		entry.timeSolved = System.DateTime.Now;

	}

	public virtual void Solved() {
		this.isTangible = true;
		this.isAnimatingFill = false;
		this.hintBubble.Deactivate ();

		enabledTransparency ();
		gameObject.layer = LayerMask.NameToLayer("Ground");

		this.objectCollider.isTrigger = false;
		if(willChangeBodyType)
			this.rigidBody2d.bodyType = RigidbodyType2D.Dynamic;

//		this.rigidBody2d.drag = 5;
//		this.rigidBody2d.useAutoMass = true;
		if (entry != null) {
			entry.timeSolved = System.DateTime.Now;
		}
	}

	public bool isFilled(float fillCount, float totalFill) {
		float numerator1 = filledCount;
		float denominator1 = partitionCount;
		float numerator2 = fillCount;
		float denominator2 = totalFill;

		if (Mathf.Abs (((numerator1 / denominator1) + (numerator2 / denominator2)) - 1) == 0) {
			return true;
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		// NOTE: !isFilling is to prevent the player from targeting the object WHILE the filling animation is playing
		if(!isTangible && !isFilling && other.gameObject.CompareTag ("Needle")
			&& !other.gameObject.GetComponent<NeedleController>().hasHit) {
			Debug.Log ("Enter Trigger");
//			this.disabledTransparency ();
			NeedleController needle = other.gameObject.GetComponent<NeedleController> ();
			needle.hasHit = needle.onlyHitOnce;
			#if UNITY_ANDROID
//			EventManager.DisableJumpButton ();
//			EventManager.ToggleSwitchButton (false);
////			EventManager.Instance.ToggleLeftRightButtons(false);
//			GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(false);

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

			this.clone (needle.GetSliceCount());

			RecordData ();
		}
		if(other.gameObject.CompareTag("Player")) {
			Debug.Log ("PLAYER COLLIDING");
//			animator.SetBool("isCollidingPlayer", true);
		}
		if(other.gameObject.CompareTag("Hammer")) {
			Debug.Log ("Hammer is colliding");
		}
	}

	public void disabledTransparency() {
		ChangeSpriteOpacity (INTANGIBLE_OPACITY);
		this.GetHintBubbleManager ().Activate ();
	}

	public void enabledTransparency() {
		ChangeSpriteOpacity (TANGIBLE_OPACITY);
		this.GetHintBubbleManager ().Deactivate ();
	}


	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Needle")) {
			ClearPartitions ();
		}

		if(other.gameObject.CompareTag("Player")) {
			Debug.Log ("PLAYER NOT COLLIDING");
//			animator.SetBool("isCollidingPlayer", false);
		}
		DisableFractionLabel ();
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Needle")) {
			//			ClearPartitions (); // NOTE: Done in PartitionedClone's animateFill() function
			if(entry.timeWeaponRemoved == null)
				entry.timeWeaponRemoved = new List<DateTime>();
			entry.timeWeaponRemoved.Add(System.DateTime.Now);
		}

		if(other.gameObject.CompareTag("Player")) {
//			animator.SetBool("isCollidingPlayer", true);
		} else if(other.gameObject.CompareTag("Needle")) {
			#if UNITY_ANDROID
//			EventManager.DisableInteractButton ();

//			GameController_v7.Instance.GetEventManager().DisableInteractButton();
//			EventManager.ToggleSwitchButton (true);
//			EventManager.Instance.ToggleLeftRightButtons(true);
//			GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(true);

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
		}
		DisableFractionLabel ();

	}

	void RecordData() {

		//		parameters.PutExtra ("TIME_NEEDLE_REMOVED", System.DateTime.Now.ToString());
		//		parameters.PutExtra ("OBJECT_GIVEN", "");
		//		parameters.PutExtra ("ACTUAL_ANSWER", "");
		//		parameters.PutExtra ("ATTEMPTED_ANSWERS", "");
		//		parameters.PutExtra ("TIME_SOLVED", System.DateTime.Now.ToString());
		//		parameters.PutExtra ("NEEDLE_INTERACTION_COUNT", "");
		//		parameters.PutExtra ("NUMBER_OF_ATTEMPTS", "");
		//		EventBroadcaster.Instance.PostEvent (EventNames.ON_NEEDLE_HIT, parameters);
		currentKey = new Tuple<Entry.Type, int>(Entry.Type.PartitionableObject, PlayerData.partitionableObjectIndex);
		//        if (!DataManager.DoesKeyExist(currentKey))
		Debug.Log("Current "+currentKey.Item1 + " " +currentKey.Item2);
		if(poBoxKey != null)
			Debug.Log ("PO Box "+poBoxKey.Item1 + " " + poBoxKey.Item2);
		if(poBoxKey == null)
		{
			entry = new POEntry ();
			entry.name = "Box " + PlayerData.partitionableObjectIndex;
			entry.timeWeaponInteracted = new List<DateTime> ();
			entry.timeWeaponInteracted.Add(System.DateTime.Now);
			entry.interactionCount++;
			Parameters parameters = new Parameters ();
			parameters.PutExtra ("TYPE", Entry.Type.PartitionableObject.ToString());
			parameters.PutExtra ("NUMBER", PlayerData.partitionableObjectIndex);
			DataManager.CreateEmptyEntry (parameters);
			poBoxKey = currentKey;
			PlayerData.IncrementPOIndex();
		} else {
			entry = DataManager.GetPOEntry (poBoxKey);
			entry.timeWeaponInteracted.Add (System.DateTime.Now);
			entry.interactionCount++;
		}
	}

	public void ClearPartitions() {
		foreach (GameObject temp in this.partitions) {
			Destroy (temp);
		}

		this.partitions = new List<GameObject> ();
	}

	public void DisableFractionLabel() {
		this.fractionLabel.Disable ();
	}

	public void RemoveComponents() {
		//		Destroy (this.GetComponent<Rigidbody2D> ());
		//		Destroy (this.GetComponent<BoxCollider2D> ());
		Destroy (this);
	}

	public float GetPartitionCount() {
		return this.partitionCount;
	}

	public float GetFilledCount() {
		return this.filledCount;
	}

	public int GetSceneObjectId() {
		return this.sceneObjectId;
	}

	public void SetSceneObjectId(int id) {
		this.sceneObjectId = id;
	}

	public bool IsTangible() {
		return this.isTangible;
	}

	public List<GameObject> GetPartitions() {
		return this.partitionsReversed;
	}

	public void SetFilling(bool isFilling) {
		this.isFilling = isFilling;
	}

	public bool IsFilling() {
		return this.isFilling;
	}

	public bool IsAnimatingFill() {
		return this.isAnimatingFill;
	}
}
