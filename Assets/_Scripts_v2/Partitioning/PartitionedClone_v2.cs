using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartitionedClone_v2 : MonoBehaviour {

	public int maxPartition = 7;
	public Color highlightColor;
	public Color disabledColor;
	public Color highlightOutlineColor;
	public Color disabledOutlineColor;

	public Color filledColor;
	public Color filledOutlineColor;


	public float scaleReduction = 0.04f;

	private List<GameObject> partitions;
	private PartitionableObjectMarker fractionLabel;
	private PlayerAttack playerNeedle;
	private int partitionPointer;
	private PartitionableObject_v2 original;
	public bool enabled = true;

	MobileUI mobile;
	PlayerMovement playerController;

	[SerializeField] float completionPause = 0.15f;
	private float animationSpeed = 2.0f;


	[SerializeField] protected PartitionableSprite[] spriteRenderers;

	protected Vector3 underflowPosition;
	protected Vector3 overflowPosition;
	protected Vector3 originalPosition;
	protected Vector2 zoomPosition;

	protected ResultsUI resultsUI;

	private bool isCorrect;
	[SerializeField] protected List<LineRenderer> outlineRenderers;

	protected const float INTANGIBLE_OPACITY = 0.25f;
	void Awake () {
		this.outlineRenderers = new List<LineRenderer> ();
//		playerNeedle = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerAttack> ();
		playerNeedle = GameObject.FindObjectOfType<PlayerYuni> ().GetPlayerAttack ();
		fractionLabel = gameObject.GetComponent<PartitionableObjectMarker> ();
		#if UNITY_ANDROID
		mobile = GameObject.Find("Mobile UI").GetComponent<MobileUI> ();
		#endif
		playerController = GameObject.FindObjectOfType<PlayerYuni> ().GetPlayerMovement();

		partitionPointer = -1;
		//		gameObject.transform.position = new Vector3 (0, 0, gameObject.transform.position.z);
		//		gameObject.transform.SetPositionAndRotation (new Vector3 (0, 0, gameObject.transform.position.z), Quaternion.identity);
		//		gameObject.transform.localPosition = new Vector3 (0, 0, gameObject.transform.position.z);
		GameController_v7.Instance.GetPauseController().Pause();

//		zoomPosition = original.GetComponentInChildren<ZoomReference> ().gameObject.transform.position;
//		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_CAMERA);
//		Parameters data = new Parameters ();
//		Debug.Log ("X Y ZOOM IS "+zoomPosition.x+" "+zoomPosition.y);
//		data.PutExtra ("x", zoomPosition.x);
//		data.PutExtra ("y", zoomPosition.y);
//		EventBroadcaster.Instance.PostEvent (EventNames.ZOOM_CAMERA_TOWARDS, data);
		spriteRenderers = gameObject.GetComponentsInChildren<PartitionableSprite>();
		this.ChangeSpriteOpacity (INTANGIBLE_OPACITY);
	
	}

	public void DisableFractionLabel() {
		this.fractionLabel.Disable ();
	}

	public void SetResultsUI(ResultsUI results) {
		this.resultsUI = results;
	}

	public Vector3 GetPositionReference(float cloneValue, float originalValue) {
		float totalFill = cloneValue + originalValue;
		this.isCorrect = false;

		if (totalFill < 1) {
			Debug.Log ("<color=red>UNDERFLOW "+cloneValue+" "+originalValue+"</color>");
			return this.underflowPosition;
		}
		else if (totalFill > 1) {
			Debug.Log ("<color=red>OVERFLOW "+cloneValue+" "+originalValue+"</color>");
			return this.overflowPosition;
		}
		else {
			Debug.Log ("<color=red>ORIGINAL "+cloneValue+" "+originalValue+"</color>");
			this.isCorrect = true;
			return this.originalPosition;
		}
	}

	public void SetUnderflowPosition(Vector3 position) {
		this.underflowPosition = position;
	}
	public void SetOverflowPosition(Vector3 position) {
		this.overflowPosition = position;
	}
	public void SetOriginalPosition(Vector3 position) {
		this.originalPosition = position;
	}

	IEnumerator animateFill() {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		List<Vector3> newPositions = new List<Vector3> ();
		Vector3 originalPosition;
		int partitionSize = this.partitions.Count; // TODO: Changed from .Capacity
		int originalPartitionSize = (int) original.GetPartitionCount();

		Vector3 reference =  GetPositionReference(this.fractionLabel.GetValue(), original.GetValue());
		RectTransform rectTransform = original.GetComponent<RectTransform> ();
		//		RectTransform rectTransform = original.GetComponentInChildren<PartitionableObjectOutline> ().gameObject.GetComponent<RectTransform>();
//		int smallerPartition;
//		if (partitionSize < originalPartitionSize) {
//			smallerPartition = partitionSize;
//		} else {
//			smallerPartition = originalPartitionSize;
//		}
//		int computedSize;
//		if (partitionSize < originalPartitionSize) {
////			computedSize = partitionSize / originalPartitionSize;
//			computedSize = originalPartitionSize / partitionSize;
//		}
//		else {
//			computedSize = partitionSize / originalPartitionSize;
////			computedSize = originalPartitionSize / partitionSize;
//		}

		// If correct
		if (original.isFilled (this.partitionPointer + 1, this.partitions.Count)) {
			for (int i = 0; i < partitionSize; i++) {
				// If partition is highlighted, go to reference bottom
				if (partitions [i].gameObject.GetComponent<SpriteRenderer> ().color == highlightColor) {
					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
						reference.y, partitions [i].gameObject.transform.position.z);
				} else {
					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
						reference.y, partitions [i].gameObject.transform.position.z);
				}
				newPositions.Add (originalPosition);
			}		
		}
		else {
			for (int i = 0; i < partitionSize; i++) {
				// If partition is highlighted, go to reference bottom
				if (partitions [i].gameObject.GetComponent<SpriteRenderer> ().color == highlightColor) {
					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
						reference.y, partitions [i].gameObject.transform.position.z);
				}
				else {
					originalPosition = partitions [i].transform.position;
					partitions [i].gameObject.SetActive (false);
				}
				newPositions.Add (originalPosition);
			}	
		}

//		for (int i = 0; i < partitionSize; i++) {
//
//			// If Correct (original is filled)
//			if (original.isFilled (this.partitionPointer + 1, this.partitions.Count)) {
//
//				// If not highlighted, stay (Note: Original has partitions reversed)
//				if (original.GetPartitions()[i].gameObject.GetComponent<SpriteRenderer> ().color == original.GetHighlightColor()) {
//
//					originalPosition = partitions [i].transform.position;
//					partitions [i].gameObject.SetActive (false);
////					this.outlineRenderers [i].startColor = highlightColor;
////					this.outlineRenderers [i].endColor = highlightColor;
//				}
//
//				// If highlighted, animate to a certain position
//				else {
////					originalPosition = new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
////						reference.y, partitions [i].gameObject.transform.position.z);
//					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
//						reference.y, partitions [i].gameObject.transform.position.z);
//
//
////					partitions[i].gameObject.GetComponent<SpriteRenderer> ().enabled = false;
//					// new Vector3 (original.GetPartitions () [i].gameObject.transform.position.x,
//					//	original.gameObject.transform.position.y, partitions [i].gameObject.transform.position.z);
//				}
//			}
//			// If wrong
//			else { 
//				// If highlighted
//				if (partitions [i].gameObject.GetComponent<SpriteRenderer> ().color == highlightColor) {
////					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
////						original.gameObject.transform.position.y + rectTransform.rect.height, partitions [i].gameObject.transform.position.z);
//
////					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
////						reference.y, partitions [i].gameObject.transform.position.z);
//
//					originalPosition = new Vector3 (partitions [i].gameObject.transform.position.x,
//						reference.y, partitions [i].gameObject.transform.position.z);
//					
////					partitions [i].gameObject.GetComponent<SpriteRenderer> ().color = filledColor;
////					this.ChangeColorOutline (i, filledOutlineColor);
//				}
//				// If not highlighted
//				else {
//					originalPosition = partitions [i].transform.position;
//					partitions [i].gameObject.SetActive (false);
//				}
//			}
//			newPositions.Add (originalPosition);
//		}

		Vector3 newPosition = newPositions [0]; // new Vector3 (original.GetPartitions()[0].gameObject.transform.position.x, original.gameObject.transform.position.y, partitions [0].gameObject.transform.position.z);
	
		SoundManager.Instance.Play (AudibleNames.LCDInterface.INCREASE, false);
		this.ChangeSpriteOpacity (0.0f);
//		this.spriteRenderer.color = Color.clear; // Hide sprite renderer
		yield return new WaitForSecondsRealtime (0.4f);
		this.ColorActivePartitions(); // Change the selected partitions to gold
		yield return new WaitForSecondsRealtime (0.9f); // Wait for a while to show the clone fill

		while (partitions [0].gameObject.transform.position != newPosition) {

			for (int i = 0; i < partitionSize; i++) {
				if(partitions[i].gameObject.activeInHierarchy)
					partitions [i].gameObject.transform.position = Vector3.MoveTowards (partitions[i].gameObject.transform.position, newPositions[i], animationSpeed*Time.unscaledDeltaTime);
//				partitions [i].gameObject.transform.position = Vector3.MoveTowards (partitions[i].gameObject.transform.position, 
//					new Vector3(partitions[i].gameObject.transform.position.x, partitions[i].gameObject.transform.position.y-10, partitions[i].gameObject.transform.position.z),
//					animationSpeed*Time.unscaledDeltaTime);

			}

			yield return null;
		}

//		SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false); // TODO Sound
//		yield return new WaitForSecondsRealtime (0.5f);

		if (this.isCorrect) {
			resultsUI.PlaySuccess ();
			Debug.Log ("<color=red>ENTER RESULTS</color>");
			while (resultsUI.IsPlaying ()) {
				Debug.Log ("<color=red>ISPLAYING</color>");
				yield return null;
			}
			this.original.GetHintBubbleManager ().Deactivate();
			this.original.GetHintBubbleManager ().HideHint();
			yield return new WaitForSecondsRealtime(0.5f);
		}
		else {
			// Show what's wrong
//			yield return new WaitForSecondsRealtime(1.0f);
			SoundManager.Instance.Play (AudibleNames.LCDInterface.DECREASE, false); // TODO Sound
//			SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false);
		}

		yield return new WaitForSecondsRealtime (0.5f);
		if (!isCorrect) {
			SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false);
		}
		Debug.Log ("<color=red>EXIT RESULTS</color>");

		//		yield return new WaitForSeconds (completionPause);

//		pauseController.HideOverlay ();
		this.transform.parent.gameObject.GetComponent<PartitionableObject_v2> ().Fill (this.partitionPointer + 1, this.partitions.Count);

		original.ClearPartitions ();

		original.SetFilling (false);
		Destroy (gameObject);

		GameController_v7.Instance.GetPauseController().Continue ();
	}
	public void ChangeSpriteOpacity(float opacity) {
		if (this.spriteRenderers == null) {
			spriteRenderers = gameObject.GetComponentsInChildren<PartitionableSprite>();
		}
		SpriteRenderer spriteRenderer;
		foreach (PartitionableSprite sprite in this.spriteRenderers) {
			Debug.Log ("SPRITE " + sprite.name);
			spriteRenderer = sprite.gameObject.GetComponent<SpriteRenderer> ();
			spriteRenderer.color = new Color (spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
		}
		Debug.Log ("SPRITE SIZE "+spriteRenderers.Length+" opacity "+opacity);
	}
	void ColorActivePartitions() {
		for (int i = 0; i < this.partitions.Count; i++) {
			if (partitions [i].gameObject.GetComponent<SpriteRenderer> ().color == highlightColor) {
				partitions [i].gameObject.GetComponent<SpriteRenderer> ().color = filledColor;
				ChangeColorOutline (i, this.filledOutlineColor, "HighlightedPartition");
			}
			else {
				partitions [i].gameObject.SetActive (false);
			}
		}
	}

	void fill() {
		original.SetFilling (true);
		this.DisableFractionLabel ();
		//		pauseController.ContinueGame (true);
		StartCoroutine (this.animateFill ());
	}

	void Update () {
		if (enabled && !original.IsFilling()) {
			//			playerController.canMove (false);
			#if UNITY_STANDALONE
			if (Input.GetButtonDown("Fire1") && playerNeedle.IsAttackEnabled ()) {
				if(this.partitionPointer > -1) {
					playerNeedle.getNeedleThrowing ().setPullTowards (false);
					fill ();
				}
				else {
					SoundManager.Instance.Play(AudibleNames.Results.MISTAKE, false);
				}
			} else if (Input.GetButtonDown("Fire2") && playerNeedle.IsAttackEnabled ()) {
				playerNeedle.getNeedleThrowing ().setPullTowards (true);
				fill ();
			} else if (Input.GetKeyDown (KeyCode.D)) { // TODO: Check if has mobile input
				increasePartition ();
			} else if (Input.GetKeyDown (KeyCode.A)) { // TODO: Check if has mobile input
				decreasePartition ();
			}
			#elif UNITY_ANDROID
			if (mobile.interactPressed && playerNeedle.IsAttackEnabled ()) {
			playerNeedle.getNeedleThrowing ().setPullTowards (false);
			fill ();
			} else if (mobile.chargePressed && playerNeedle.IsAttackEnabled ()) {
				playerNeedle.getNeedleThrowing ().setPullTowards (false); // TODO remove
			fill ();
			} else if (mobile.rightPressed) {
			increasePartition ();
			mobile.rightPressed = false;
			} else if (mobile.leftPressed) {
			decreasePartition ();
			mobile.leftPressed = false;
			}
			#endif

		} 
	}

	//Changes the highlight of the index depending on the value of isHighlight (true = highlighted)
	void highlightPartition(int index, bool isHighlight) {
		if (index == -1) {
			this.ChangeColorOutline (index, this.disabledOutlineColor, "DisabledPartition");
			partitions [0].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [0].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
			partitions [0].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		else if (isHighlight) {
			this.ChangeColorOutline (index, this.highlightOutlineColor, "HighlightedPartition");
			partitions [index].GetComponent<SpriteRenderer> ().color = this.highlightColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "HighlightedPartition";
			partitions [index].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
		else {
			this.ChangeColorOutline (index, this.disabledOutlineColor, "DisabledPartition");
			partitions [index].GetComponent<SpriteRenderer> ().color = this.disabledColor;
			partitions [index].GetComponent<SpriteRenderer> ().sortingLayerName = "DisabledPartition";
			partitions [index].GetComponent<SpriteRenderer> ().sortingOrder = 10;
		}
	}
	void ChangeColorOutline(int index, Color newColor, string sortingLayer) {
		this.outlineRenderers [index].startColor = newColor;
		this.outlineRenderers [index].endColor = newColor;
		this.outlineRenderers[index].sortingLayerName = sortingLayer;
		this.outlineRenderers [index].sortingOrder = 11;
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
		this.ReturnCamera ();
//		Parameters data = new Parameters ();
//		data.PutExtra ("shouldZoomIn", false);
//		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);
//		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);
	}
	public void ReturnCamera() {
		Parameters data = new Parameters ();
		data.PutExtra ("shouldZoomIn", false);

		EventBroadcaster.Instance.PostEvent (EventNames.SHOULD_CAMERA_ZOOM_IN, data);
		EventBroadcaster.Instance.PostEvent (EventNames.ENABLE_CAMERA);
	}

	public void AdjustCamera() {
		zoomPosition = original.GetComponentInChildren<ZoomReference> ().gameObject.transform.position;
		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_CAMERA);
		Parameters data = new Parameters ();
		Debug.Log ("X Y ZOOM IS "+zoomPosition.x+" "+zoomPosition.y);
		data.PutExtra ("x", zoomPosition.x);
		data.PutExtra ("y", zoomPosition.y);
		EventBroadcaster.Instance.PostEvent (EventNames.ZOOM_CAMERA_TOWARDS, data);
	}
	public void Partition(int partitionCount, GameObject pieceReference, GameObject original) {
		
		this.outlineRenderers.Clear ();
		partitions = getPartitions ();

		GameObject temp;
		SpriteRenderer sprite = original.GetComponent<SpriteRenderer> ();
		Vector2 pos;
		this.original = original.GetComponent<PartitionableObject_v2> ();
		this.AdjustCamera ();

		this.partitionPointer = -1;
		float prevPieceX = 0f;
		Debug.Log ("<color=yellow>PartitionCount clone is "+partitionCount+"</color>");
		for(int x = 0; x < partitionCount; x++) {
			pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

			temp = Instantiate (original.GetComponent<PartitionableObject_v2>().partitionPiece, pos, Quaternion.identity);
			//			temp.transform.localScale = new Vector2 (original.transform.localScale.x / partitionCount, original.transform.localScale.y);
			//			temp.transform.localScale = new Vector2 (temp.transform.localScale.x-scaleReduction, temp.transform.localScale.y-scaleReduction);
			temp.transform.localScale = new Vector2 (original.transform.localScale.x / partitionCount+.5f, gameObject.transform.localScale.y);




			temp.GetComponent<SpriteRenderer> ().sortingLayerName = "Clone Partition";

			temp.SetActive (true);
			temp.name = "partition_"+x;
			temp.tag = "Partition Instance";
			temp.transform.SetParent (gameObject.transform);

			Bounds cloneBounds = gameObject.GetComponent<SpriteRenderer> ().bounds;

			Vector2 locPos = new Vector2(cloneBounds.center.x - (cloneBounds.extents.x - (cloneBounds.extents.x/partitionCount)) + prevPieceX, cloneBounds.center.y);

			temp.transform.position = locPos;
			temp.AddComponent<RectTransform> ();
			this.GenerateOutline (temp);

			prevPieceX += (cloneBounds.size.x/partitionCount);

			partitions.Add (temp);
			this.highlightPartition (x, false);
		}

//		if (!this.fractionLabel.gameObject.activeInHierarchy)
//			Debug.LogError ("Fraction Label is DISABLED!!!");
//		Debug.Log ("Partition Count: " + partitions.Count);

		this.fractionLabel.Enable ();
		this.fractionLabel.UpdateValue (0, partitionCount);

	}

	public void GenerateOutline(GameObject parent) {
		LineRenderer lineRenderer = parent.AddComponent<LineRenderer>();
		Vector3[] positionsArray = new Vector3[5];

		lineRenderer.material = this.original.GetLineMaterial();
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

		this.outlineRenderers.Add (lineRenderer);
	}

	private void OnDrawGizmos() {
		Bounds b = gameObject.GetComponent<SpriteRenderer> ().bounds;
		Gizmos.DrawWireCube (b.center, b.size);
	}

	//	public List<GameObject> GetPartitions() {
	//		return this.partitions;
	//	}

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
