using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionableObject : MonoBehaviour {

	public GameObject partitionPiece;
	public bool enableClone = true;
	[SerializeField] bool isTangible;
	//Will have to set these two to private later
	//for now, leave as public for testing purposes
	public float partitionCount = 5;
	public float filledCount = 3;
	public Color highlightColor = new Color(0, 1, 1, 1);
	public Color disabledColor = new Color(1, 1, 1, 1);
	public Sprite tangibleSprite;
	public float scaleReduction = 0.04f;

	private PauseController pauseController;
	private Vector2 clonePosition;
	private GameObject cloneObject;
	private PartitionController partitionController;
	private PartitionableObjectMarker fractionLabel;
	private int partitionPointer;

	private List<GameObject> partitions;
	private List<GameObject> partitionsReversed;

	private Collider2D playerCollider2D;
	private bool isFilling;
	// Use this for initialization
	void Start () {
		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
		partitionController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PartitionController> ();
		clonePosition = new Vector2 (
			this.gameObject.transform.position.x,
			this.gameObject.transform.position.y + gameObject.transform.localScale.y);

		fractionLabel = gameObject.GetComponent<PartitionableObjectMarker> ();
		fractionLabel.Disable ();

	}
	
	// Update is called once per frame
	void Update () {
		clonePosition = new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + gameObject.transform.localScale.y);
	}

	public void clone(int sliceCount) {
		pauseController.PauseGame();
		cloneObject = CloneBuilder.GetInstance().SpawnClone(this.gameObject, clonePosition, highlightColor, disabledColor, this.scaleReduction);
//		cloneObject.GetComponent<PartitionedClone> ().highlightColor = highlightColor;
//		cloneObject.GetComponent<PartitionedClone> ().disabledColor = disabledColor;
		cloneObject.GetComponent<BoxCollider2D> ().enabled = false;
		partitionController.Partition (this, cloneObject, sliceCount);

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
	private void increasePartition() {
		int partitionIndex = this.partitionPointer + 1;
		this.setPartitionPointer (partitionIndex);
		highlightPartition (this.partitionPointer, true);
	}

	// Decrement partitionPointer by 1
	private void decreasePartition() {

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

	void OnEnable() {
		partitions = new List<GameObject> ();
	}

	void OnDestroy() {
		foreach (GameObject temp in partitions) {
			Destroy (temp);
		}
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

	public void Fill(float fillCount, float totalFill) {
		Debug.Log ("Filled Count: " + filledCount);
		Debug.Log ("Partition Count: " + partitionCount);
		Debug.Log ("Fill Count: " + fillCount);
		Debug.Log ("Total Fill: " + totalFill);
		Debug.Log (((filledCount / partitionCount) + (fillCount / totalFill)));
		Debug.Log ("Epsilon: " + Mathf.Epsilon);

		float numerator1 = filledCount;
		float denominator1 = partitionCount;
		float numerator2 = fillCount;
		float denominator2 = totalFill;

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

			// Enable collider once filled
			playerCollider2D = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D> ();
			Physics2D.IgnoreCollision (playerCollider2D, GetComponent<Collider2D> (), false);

			this.isTangible = true;
			gameObject.GetComponent<SpriteRenderer> ().sprite = tangibleSprite;
			gameObject.GetComponent<Collider2D> ().isTrigger = false;
		} else {
			// Will we implement the animation that causes individual pieces to go into the object individually and melt when not whole?
			Debug.Log("Failed to fill...");
		}

		//Will have to visualize the changes overtime
		//for now, comment out
		//fractionLabel.UpdateValue (filledCount + fillCount, partitionCount);
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
			NeedleController needle = other.gameObject.GetComponent<NeedleController> ();
			needle.hasHit = needle.onlyHitOnce;
			this.clone (needle.GetSliceCount ());
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.gameObject.CompareTag ("Needle")) {
			ClearPartitions ();
		}
			
			
		DisableFractionLabel ();
	}

	void OnTriggerExit2D(Collider2D other) {
//		if (other.gameObject.CompareTag ("Needle")) {
		//			ClearPartitions (); // NOTE: Done in PartitionedClone's animateFill() function;
//		}


		DisableFractionLabel ();
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
}
