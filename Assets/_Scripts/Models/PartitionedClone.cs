using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionedClone : MonoBehaviour {


	public int maxPartition = 7;
	public Color highlightColor;
	public Color disabledColor;
	public float scaleReduction = 0.04f;

	private List<GameObject> partitions;
//	private PauseController pauseController;
	private FillController fillController;
	private PartitionableObjectMarker fractionLabel;
//	private PlayerAttack playerNeedle;
	private int partitionPointer;
	private PartitionableObject original;
	public bool enabled = true;

	MobileUI mobile;
//	PlayerMovement playerController;
	PlayerYuni player;

	[SerializeField] float completionPause = 0.15f;
	private float animationSpeed = 2.0f;
	void Awake () {
		player = GameObject.FindObjectOfType<PlayerYuni> ();
//		playerNeedle = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();
//		pauseController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<PauseController> ();
		fillController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<FillController> ();
		fractionLabel = gameObject.GetComponent<PartitionableObjectMarker> ();
		#if UNITY_ANDROID
			mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif
//		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerMovement> ();

		partitionPointer = -1;
//		gameObject.transform.position = new Vector3 (0, 0, gameObject.transform.position.z);
//		gameObject.transform.SetPositionAndRotation (new Vector3 (0, 0, gameObject.transform.position.z), Quaternion.identity);
//		gameObject.transform.localPosition = new Vector3 (0, 0, gameObject.transform.position.z);

	}

	public void DisableFractionLabel() {
		this.fractionLabel.Disable ();
	}

	IEnumerator animateFill() {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		List<Vector3> newPositions = new List<Vector3> ();
		Vector3 originalPosition;
		int partitionSize = this.partitions.Count; // TODO: Changed from .Capacity

		RectTransform rectTransform = original.GetComponent<RectTransform> ();
//		RectTransform rectTransform = original.GetComponentInChildren<PartitionableObjectOutline> ().gameObject.GetComponent<RectTransform>();

		for (int i = 0; i < partitionSize; i++) {

			if (original.isFilled (this.partitionPointer + 1, this.partitions.Count)) {
				if (original.GetPartitions()[i].gameObject.GetComponent<SpriteRenderer> ().color == original.highlightColor) {

					originalPosition = partitions [i].transform.position;
				}
				else {
					originalPosition = new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
						original.gameObject.transform.position.y, partitions [i].gameObject.transform.position.z);
					
					// new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
					//	original.gameObject.transform.position.y, partitions [i].gameObject.transform.position.z);
				}
			}
			else {
				if (partitions [i].gameObject.GetComponent<SpriteRenderer> ().color == highlightColor) {
					originalPosition = new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
						original.gameObject.transform.position.y + rectTransform.rect.height, partitions [i].gameObject.transform.position.z);
					
				}
				else {
					originalPosition = partitions [i].transform.position;
				}
			}
//				if (partitions [i].gameObject.GetComponent<SpriteRenderer> ().color == disabledColor) {
//			if (original.GetPartitions()[i].gameObject.GetComponent<SpriteRenderer> ().color == original.highlightColor) {
//				originalPosition = new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
//					original.gameObject.transform.position.y + rectTransform.rect.height, partitions [i].gameObject.transform.position.z);
//			}
//			else {
//				originalPosition = new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
//					original.gameObject.transform.position.y, partitions [i].gameObject.transform.position.z);			
//			}
		
			newPositions.Add (originalPosition);
		}

		Vector3 newPosition = newPositions [0]; // new Vector3 (original.GetPartitions()[0].gameObject.transform.position.x, original.gameObject.transform.position.y, partitions [0].gameObject.transform.position.z);

		while (partitions [0].gameObject.transform.position != newPosition) {

			for (int i = 0; i < partitionSize; i++) {
				partitions [i].gameObject.transform.position = Vector3.MoveTowards (partitions[i].gameObject.transform.position, newPositions[i], animationSpeed*Time.unscaledDeltaTime);
			}

			yield return null;
		}
//		yield return new WaitForSeconds (completionPause);

//		pauseController.HideOverlay (); // TODO Pause Hide Overlay
		fillController.FillTarget (this.partitionPointer + 1, this.partitions.Count, this.transform.parent.gameObject.GetComponent<PartitionableObject> ());

	
		original.ClearPartitions ();

		original.SetFilling (false);
		Destroy (gameObject);

		GameController_v7.Instance.GetPauseController().Continue();
//		pauseController.ContinueGame (true);
	}

	void fill() {
		original.SetFilling (true);
		this.DisableFractionLabel ();
//		pauseController.ContinueGame (true);
		StartCoroutine (this.animateFill ());
	}
	void Update () {
//		Debug.Log ("Partitions: " + partitions.Count);
		if (enabled && !original.IsFilling()) {
//			playerController.canMove (false);
			#if UNITY_STANDALONE
				if (Input.GetButtonDown("Fire1") && player.GetPlayerAttack().IsAttackEnabled ()) {
					player.GetPlayerAttack().getNeedleThrowing ().setPullTowards (false);
					fill ();
				} else if (Input.GetButtonDown("Fire2") && player.GetPlayerAttack().IsAttackEnabled ()) {
				//					player.GetPlayerAttack().getNeedleThrowing ().setPullTowards (true);
					player.GetPlayerAttack().getNeedleThrowing ().setPullTowards (false);
					fill ();
				} else if (Input.GetKeyDown (KeyCode.D)) {
					increasePartition ();
				} else if (Input.GetKeyDown (KeyCode.A)) {
					decreasePartition ();
				}
			#elif UNITY_ANDROID
			#endif

		} 
	}

	//Changes the highlight of the index depending on the value of isHighlight (true = highlighted)
	void highlightPartition(int index, bool isHighlight) {
		if (index == -1) {
			partitions [0].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [0].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
			partitions [0].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		else if (isHighlight) {
			partitions [index].GetComponent<SpriteRenderer> ().color = this.highlightColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "HighlightedPartition";
			partitions [index].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		else {
			partitions [index].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
			partitions [index].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
	}

	// Increment partitionPointer by 1
	void increasePartition() {
		int partitionIndex = this.partitionPointer + 1;
		this.setPartitionPointer (partitionIndex);
		highlightPartition (this.partitionPointer, true);
	}

	// Decrement partitionPointer by 1
	void decreasePartition() {
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
		this.fractionLabel.UpdateValue (partitionPointer + 1, partitions.Count);
	}


	// Returns the partitions array. If null, it instantiates it.
	public List<GameObject> getPartitions() {
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

	public void Partition(int partitionCount, GameObject pieceReference, GameObject original) {
		partitions = getPartitions ();

		GameObject temp;
		SpriteRenderer sprite = original.GetComponent<SpriteRenderer> ();
		Vector2 pos;
		this.original = original.GetComponent<PartitionableObject> ();

		this.partitionPointer = -1;
		float prevPieceX = 0f;

		for(int x = 0; x < partitionCount; x++) {
			pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

			temp = Instantiate (pieceReference, pos, Quaternion.identity);
			temp.transform.localScale = new Vector2 (original.transform.localScale.x / partitionCount, original.transform.localScale.y);
			temp.transform.localScale = new Vector2 (temp.transform.localScale.x-scaleReduction, temp.transform.localScale.y-scaleReduction);

			temp.GetComponent<SpriteRenderer> ().sortingLayerName = "Clone Partition";

			temp.SetActive (true);
			temp.name = "partition_"+x;
			temp.tag = "Partition Instance";
			temp.transform.SetParent (gameObject.transform);

//			Bounds cloneBounds = gameObject.GetComponent<SpriteRenderer> ().bounds;
//			float cloneX = cloneBounds.center.x - (cloneBounds.extents.x - (cloneBounds.extents.x / partitionCount)) + prevPieceX;
////			if (x == 0)
////				cloneX += 0.075f;
//			Vector2 locPos = new Vector2(cloneX, cloneBounds.center.y);

//			Bounds cloneBounds = gameObject.GetComponent<SpriteRenderer> ().bounds;
//			int cloneX = cloneBounds.center.x + (cloneBounds.extents.x - (cloneBounds.extents.x / partitionCount)) - prevPieceX - sc;
//			Vector2 locPos = new Vector2(, cloneBounds.center.y);
			Bounds cloneBounds = gameObject.GetComponent<SpriteRenderer> ().bounds;

			Vector2 locPos = new Vector2(cloneBounds.center.x + (cloneBounds.extents.x - (cloneBounds.extents.x/partitionCount)) - prevPieceX, cloneBounds.center.y);

			temp.transform.position = locPos;
			prevPieceX += (cloneBounds.size.x/partitionCount);

//			temp.transform.position = locPos;
//			prevPieceX += (cloneBounds.size.x/partitionCount)-(0.066f);

			partitions.Add (temp);
			this.highlightPartition (x, false);
		}

//		if (!this.fractionLabel.gameObject.activeInHierarchy)
//			Debug.LogError ("Fraction Label is DISABLED!!!");
//		Debug.Log ("Partition Count: " + partitions.Count);

		this.fractionLabel.Enable ();
		this.fractionLabel.UpdateValue (0, partitionCount);

	}

	private void OnDrawGizmos() {
		Bounds b = gameObject.GetComponent<SpriteRenderer> ().bounds;
		Gizmos.DrawWireCube (b.center, b.size);
	}
	public void Enabled(bool flag) {
		this.enabled = flag;
		if (this.enabled) {
//			this.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
//			foreach (GameObject partition in this.partitions) {
//				partition.GetComponent<SpriteRenderer> ().enabled = true;
//			}
//			fractionLabel.Enable ();

			foreach (Transform child in gameObject.transform.GetComponentsInChildren<Transform>(true)) {
				if (child.GetComponent<SpriteRenderer> () != null)
					child.GetComponent<SpriteRenderer> ().enabled = true;
				else
					child.gameObject.SetActive (true);
			}
		} else {
//			this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
//			foreach (GameObject partition in this.partitions) {
//				partition.GetComponent<SpriteRenderer> ().enabled = false;
//			}
//			fractionLabel.Disable ();

			foreach (Transform child in gameObject.transform.GetComponentsInChildren<Transform>()) {
				if (child.GetComponent<SpriteRenderer> () != null)
					child.GetComponent<SpriteRenderer> ().enabled = false;
				else
					child.gameObject.SetActive (false);
			}
		}

		this.GetComponent<SpriteRenderer> ().enabled = false;
	}
}
