using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// UNUSED CLASS
/// 
/// Variant of a partitionable object (v2) which can be activated and deactivated, causing it to become hollowed or tangible at will.
/// </summary>
public class DetonatingTrampolinePlatformPiece : PartitionableObject_v2 {
    
    /// <summary>
    /// Flag if it is invisible.
    /// </summary>
	[SerializeField] private bool isHidden = false;
    /// <summary>
    /// Reference to its game object's collider.
    /// </summary>
	[SerializeField] private Collider2D collider;

    /// <summary>
    /// Unity Function. Called upon instantiation of this MonoBehaviour's game object.
    /// </summary>
	void Awake() {
		UnityEngine.Random.InitState (3216);
		validDenominators = new List<int> ();
		fractionReference = FractionsReference.Instance ();
		fractionReference.AddObserver (this.RequestValidDenominators);

		spriteRenderers = gameObject.GetComponentsInChildren<PartitionableSprite>();
        
		this.objectCollider = gameObject.GetComponent<Collider2D> ();
		rigidBody2d = gameObject.GetComponent<Rigidbody2D> ();
		fractionLabel = gameObject.GetComponent<PartitionableObjectMarker> ();
	}

    /// <summary>
    /// Standard Unity Function. Used to start up the MonoBehaviour. Called once throughout the game object's life.
    /// </summary>
	void Start () {
		poBoxKey = null;
		clonePosition = GetComponentInChildren<ClonePosition>().transform.position;
		this.outlineRenderers = new List<LineRenderer> ();

		fractionLabel.Disable ();

		if (validDenominators == null)
			Debug.Log ("Start Error");

	}

    /// <summary>
    /// Unity Function. Called every time the MonoBehaviour is enabled.
    /// </summary>
	void OnEnable() {
		if (!isHidden) {
			partitions = new List<GameObject> ();
			InitiateFraction ();

			this.GetHintBubbleManager ().Deactivate ();
			Init ();
		}
		this.isHidden = false;
		this.collider = GetComponent<BoxCollider2D> ();
	}

    /// <summary>
    /// Hides the game object's associated hint bubble.
    /// </summary>
	public void Hide() {
		this.collider.enabled = false;
		this.GetHintBubbleManager ().Activate ();
	}

    /// <summary>
    /// Shows the game object's associated hint bubble.
    /// </summary>
	public void Show() {
		this.collider.enabled = true;
		this.GetHintBubbleManager ().Deactivate ();
	}

    /// <summary>
    /// Function that fills the hollow block with the given fillCount (numerator) and totalFill (denominator).
    /// </summary>
    /// <param name="fillCount"></param>
    /// <param name="totalFill"></param>
	public override void Fill (float fillCount, float totalFill) {
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

			Solved ();
		} else {
			// Will we implement the animation that causes individual pieces to go into the object individually and melt when not whole?
			Debug.Log("Failed to fill...");
		}

		DataManager.UpdatePOEntry(DataManager.GetPOLastKey(), entry);

		this.collider.enabled = true;
		enabledTransparency ();
	}

    /// <summary>
    /// Initializes the platform as a tangible object.
    /// </summary>
	protected void Init() {
		this.isTangible = true;
		this.isAnimatingFill = false;

		gameObject.layer = LayerMask.NameToLayer("Ground");
		enabledTransparency ();
		this.objectCollider.isTrigger = false;
		this.rigidBody2d.bodyType = RigidbodyType2D.Static;
	}

    /// <summary>
    /// Called when the platform piece has been solved. Reinitializes the platform piece as a tangible block and records its completion time.
    /// </summary>
	public override void Solved () {
		Init ();

		entry.timeSolved = System.DateTime.Now;
	}

    /// <summary>
    /// Resets the platform to its tangible state.
    /// </summary>
	public void Replenish() {
		Init ();
	}

    /// <summary>
    /// Causes the block to be intangible.
    /// </summary>
	public void Detonate() {
		InitiateFraction ();
		Debug.Log ("Entered Detonate");
		this.isTangible = false;
		this.objectCollider.isTrigger = true;
		this.rigidBody2d.bodyType = RigidbodyType2D.Kinematic;
	}

    /// <summary>
    /// Destroys the platform piece.
    /// </summary>
	public void Purge() {
		Destroy (this.gameObject);
	}
}
