using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartitionableObject : MonoBehaviour {
	[SerializeField] protected const float CLONE_DISTANCE_Y = 0.5f;
	public GameObject partitionPiece;
	public bool enableClone = true;
	[SerializeField] protected bool isTangible;
	public Color highlightColor = new Color(0, 1, 1, 1);
	public Color disabledColor = new Color(1, 1, 1, 1);
	public Sprite tangibleSprite;
	public float scaleReduction = 0.04f;
	public int blockWidth = 1;

//	protected PauseController pauseController;
	protected Vector2 clonePosition;
	protected GameObject cloneObject;
	protected PartitionController partitionController;
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
	protected Animator animator;
	protected SpriteRenderer spriteRenderer;
	protected SpriteRenderer[] childrenSpriteRenderers;
	protected Rigidbody2D rigidBody2d;

	[SerializeField] protected float disabledTransparencyValue = 0.3f;

	//	PlayerData playerData;
	protected Tuple<Entry.Type, int> poBoxKey;
	protected Tuple<Entry.Type, int> currentKey;
	protected POEntry entry;

	void Awake() {
		UnityEngine.Random.InitState (3216);
		validDenominators = new List<int> ();
		fractionReference = FractionsReference.Instance ();
		fractionReference.AddObserver (this.RequestValidDenominators);
//		fractionReference.UpdateFractionRange (this.RequestValidDenominators);

		animator = gameObject.GetComponent<Animator> ();
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		childrenSpriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		this.objectCollider = gameObject.GetComponent<Collider2D> ();
		rigidBody2d = gameObject.GetComponent<Rigidbody2D> ();
		fractionLabel = gameObject.GetComponent<PartitionableObjectMarker> ();
	}

	// Use this for initialization
	void Start () {
		poBoxKey = null;
		partitionController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PartitionController> ();
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
		clonePosition = new Vector2 (
			this.gameObject.transform.position.x,
			this.gameObject.transform.position.y + CLONE_DISTANCE_Y);


		fractionLabel.Disable ();

//		playerData = GameObject.FindGameObjectWithTag ("PlayerData").GetComponent<PlayerData> ();
//		FractionsReference.Instance ().UpdateValidDenominators ();

		if (validDenominators == null)
			Debug.Log ("<color=red>Start Error</color>");
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
		clonePosition = new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + gameObject.transform.localScale.y);
		if (cloneObject != null) {
			if (enableClone) {
				cloneObject.GetComponent<PartitionedClone> ().Enabled (true);
			} else {
				cloneObject.GetComponent<PartitionedClone> ().Enabled (false);
			}
		}
	}

	public void clone(int sliceCount) {
//		pauseController.PauseGame();
		GameController_v7.Instance.GetPauseController().Pause();

		cloneObject = CloneBuilder.GetInstance().SpawnClone(this.gameObject, clonePosition, highlightColor, disabledColor, this.scaleReduction);
//		cloneObject.GetComponent<PartitionedClone> ().highlightColor = highlightColor;
//		cloneObject.GetComponent<PartitionedClone> ().disabledColor = disabledColor;
		Destroy(cloneObject.GetComponent<Rigidbody2D> ());
		cloneObject.transform.position = new Vector3 (0, gameObject.transform.localScale.y, cloneObject.transform.position.z);
		cloneObject.transform.SetPositionAndRotation (new Vector3 (0, gameObject.transform.localScale.y, cloneObject.transform.position.z), Quaternion.identity);
		cloneObject.transform.localPosition = new Vector3 (0, gameObject.transform.localScale.y, cloneObject.transform.position.z);

		partitionController.Partition (this, cloneObject, sliceCount);

		this.MatchScale (cloneObject);

	}

	public void MatchScale(GameObject clone) {
		clone.transform.SetParent (gameObject.GetComponentsInParent<Transform>()[1]);
		clone.transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, transform.localScale.z);

		PartitionedClone partitionedClone = clone.GetComponent<PartitionedClone> ();
		List<GameObject> clonedPartitions = partitionedClone.getPartitions ();

		for (int i = 0; i < clonedPartitions.Count; i++) {
			clonedPartitions [i].transform.localScale = new Vector3 (this.partitions[i].transform.localScale.x,
																		this.partitions[i].transform.localScale.y,
																		this.partitions[i].transform.localScale.z);
		}
		partitionedClone.getPartitions ().Reverse ();

		clone.transform.SetParent (gameObject.transform);
	}


	//Changes the highlight of the index depending on the value of isHighlight (true = highlighted)
	void highlightPartition(int index, bool isHighlight) {
		if (index == -1) {
			partitions [0].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [0].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
		}
		else if (isHighlight) {
			partitions [index].GetComponent<SpriteRenderer> ().color = this.highlightColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "HighlightedPartition";
		}
		else {
			partitions [index].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
		}

		this.fractionLabel.UpdateValue (index + 1, partitionCount);
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
			temp.transform.localScale = new Vector2 (temp.transform.localScale.x-scaleReduction, temp.transform.localScale.y-scaleReduction);

			temp.GetComponent<SpriteRenderer> ().sortingLayerName = "Clone Partition";

			temp.SetActive (true);
			temp.name = "partition_"+x;
			temp.tag = "Partition Instance";
			temp.transform.SetParent (gameObject.transform);

			Bounds cloneBounds = gameObject.GetComponent<SpriteRenderer> ().bounds;

			Vector2 locPos = new Vector2(cloneBounds.center.x + (cloneBounds.extents.x - (cloneBounds.extents.x/partitionCount)) - prevPieceX, cloneBounds.center.y);

			temp.transform.position = locPos;
			prevPieceX += (cloneBounds.size.x/partitionCount);

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

		this.enabledTransparency ();
		//Will have to visualize the changes overtime
		//for now, comment out
		//fractionLabel.UpdateValue (filledCount + fillCount, partitionCount);
	}

	IEnumerator animateFill() {
		this.isAnimatingFill = true;
		animator.SetBool ("isTangible", true);

		Debug.Log (this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + " :: " + this.animator.GetCurrentAnimatorStateInfo(0).length);
		float animationTime = Time.time + this.animator.GetCurrentAnimatorStateInfo (0).length;
		while(Time.time < animationTime) {
			this.isAnimatingFill = true;
			yield return null;
		}

		// Enable collider once filled
//		playerCollider2D = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D> ();
//		Physics2D.IgnoreCollision (playerCollider2D, GetComponent<Collider2D> (), false);

		Solved ();

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

	protected virtual void Solved() {
		this.isTangible = true;
		this.isAnimatingFill = false;
		gameObject.GetComponent<SpriteRenderer> ().sprite = tangibleSprite;
		this.objectCollider.isTrigger = false;
		this.rigidBody2d.bodyType = RigidbodyType2D.Dynamic;

		this.rigidBody2d.drag = 5;
		this.rigidBody2d.useAutoMass = true;

		entry.timeSolved = System.DateTime.Now;
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
		if(!isTangible && !isFilling && other.gameObject.GetComponent<NeedleThrowing>() != null
			&& !other.gameObject.GetComponent<NeedleController>().hasHit) {
			Debug.Log ("Enter Trigger");
			this.disabledTransparency ();
			NeedleController needle = other.gameObject.GetComponent<NeedleController> ();
			needle.hasHit = needle.onlyHitOnce;
			#if UNITY_ANDROID
//				EventManager.DisableJumpButton ();
//				EventManager.ToggleSwitchButton (false);
////				EventManager.Instance.ToggleLeftRightButtons(false);
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
			//TODO:; CHANGE TO needle.GetSliceCount()
			this.clone ((int)this.partitionCount);

			RecordData ();
		}
		if(other.gameObject.CompareTag("Player")) {
			Debug.Log ("PLAYER COLLIDING");
			animator.SetBool("isCollidingPlayer", true);
		}
		if(other.gameObject.CompareTag("Hammer")) {
			Debug.Log ("Hammer is colliding");
		}
	}

	public void disabledTransparency() {
		foreach (SpriteRenderer renderer in this.childrenSpriteRenderers) {
			renderer.color = new Color (renderer.color.r, renderer.color.g, renderer.color.b, this.disabledTransparencyValue);
		}
	}

	public void enabledTransparency() {
		foreach (SpriteRenderer renderer in this.childrenSpriteRenderers) {
			renderer.color = new Color (renderer.color.r, renderer.color.g, renderer.color.b, 1.0f);
		}
	}


	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Needle")) {
			ClearPartitions ();
		}
			
		if(other.gameObject.CompareTag("Player")) {
			Debug.Log ("PLAYER NOT COLLIDING");
			animator.SetBool("isCollidingPlayer", false);
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
			animator.SetBool("isCollidingPlayer", true);
		} else if(other.gameObject.CompareTag("Needle")) {
			#if UNITY_ANDROID

//				GameController_v7.Instance.GetEventManager().DisableInteractButton();
//				EventManager.DisableInteractButton ();
//				EventManager.ToggleSwitchButton (true);
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
