using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenClone : MonoBehaviour {
	public const float CLONE_SPACE = 0.5f;

	// Inspector controls
	[SerializeField] private bool isIncrease;
	[SerializeField] private bool isDecrease;
	[SerializeField] private bool isCraft;

	[SerializeField] private GameObject pieceReference;
	[SerializeField] private GameObject craftedPieceReference;

	[SerializeField] private BrokenOriginal original;

	[SerializeField] private List<BrokenPiece> pieces;
	[SerializeField] private CraftedPiece craftedPiece;
	[SerializeField] private int pieceIndex;

	[SerializeField] private float numerator;
	[SerializeField] private float denominator;

	[SerializeField] private float widthWhole; // Width of a 1 whole object
//	[SerializeField] private float widthPiece; // Width of a piece with size num/den
	[SerializeField] private float widthSingle; // Width of a piece with size 1/den

	[SerializeField] private float height; // Height of object
	private bool isActivated;
	private bool isCorrect;
	private bool isCrafting;

	private float animationSpeed = 2.0f;

	void Awake() {
		this.numerator = 0;
		this.denominator = 1;
		this.pieces = new List<BrokenPiece> ();
		this.pieceIndex = -1;

		this.isCorrect = false;
		this.isCrafting = false;
	}

	void Update() {
		if (this.isActivated) {
			this.InputUpdates ();


//			if (isIncrease) {
//				this.isIncrease = false;
//				this.IncreaseFill ();
//			} else if (isDecrease) {
//				this.isDecrease = false;
//				this.DecreaseFill ();
//			} else if (isCraft && !isCrafting) {
//				this.isCraft = false;
//				StartCoroutine (CraftRoutine ());
//			}
		}
	}

	public void InputUpdates() {
		#if UNITY_STANDALONE
		if (Input.GetButtonDown("Fire1")) { 
			this.ConfirmAnswer();
		}
		else if (Input.GetButtonDown("Fire2")) { 
			this.CancelCrafting();
		}

		else if (Input.GetKeyDown (KeyCode.D)) {
			this.IncreaseFill ();
		} else if (Input.GetKeyDown (KeyCode.A)) {
			this.DecreaseFill();
		}


		#elif UNITY_ANDROID
		// Confirm answer
		if(GameController_v7.Instance.GetMobileUIManager().IsInteracting()) {
			this.ConfirmAnswer();
		}
		// Cancel answer
		else if (GameController_v7.Instance.GetMobileUIManager().IsCharging()) {
			this.CancelCrafting();
		}
		else if(GameController_v7.Instance.GetMobileUIManager().IsRightPressed()) {
			this.IncreaseFill ();
			GameController_v7.Instance.GetMobileUIManager().SetRightPressed(false);
		}
		else if(GameController_v7.Instance.GetMobileUIManager().IsLeftPressed()) {
			this.DecreaseFill ();
			GameController_v7.Instance.GetMobileUIManager().SetLeftPressed(false);
		}
		#endif
	}
	public void ConfirmAnswer() {
		if(this.numerator > 0 && !isCrafting) {
			StartCoroutine (CraftRoutine ());
		}
		else {
			SoundManager.Instance.Play(AudibleNames.Results.MISTAKE, false);
		}
	}

	public void Activate(float threadDenominator) {
		if (!isActivated) {
			this.gameObject.SetActive (true);
			this.isActivated = true;
			this.Partition (threadDenominator);
		}
	}

	// Continues the pause called by BrokenOriginal HitEvents()
	public void Deactivate() {
		if (this.isActivated) {
			this.UncraftPiece ();
			this.isActivated = false;
			this.gameObject.SetActive (false);

			this.DeactivateEvents ();
		}
	}

	public void DeactivateEvents() {
		GameController_v7.Instance.GetPauseController().Continue ();

		#if UNITY_ANDROID
////		GameController_v7.Instance.GetEventManager().DisableInteractButton();
//		EventManager.ToggleSwitchButton (true);
//		GameController_v7.Instance.GetEventManager().ToggleLeftRightButtons(true);

//		EventBroadcaster.Instance.PostEvent (EventNames.DISABLE_INTERACT);
		Parameters parameters = new Parameters ();
		parameters.PutExtra ("FLAG", true);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_SWITCH_WEAPON_BUTTON, parameters);
		EventBroadcaster.Instance.PostEvent (EventNames.TOGGLE_LEFT_RIGHT_BUTTONS, parameters);
		#endif
	}
	public void DestroyPieces() {
		foreach(BrokenPiece piece in this.pieces) {
			if(piece != null)
				Destroy (piece.gameObject);
		}
		this.GetPieces ().Clear ();
	}

	// Used to create a num/den piece for animation
	public void CraftPiece() {
		this.DestroyPieces ();

		float craftWidth = this.numerator*this.widthSingle;
		float craftHeight = this.height;

		this.craftedPiece = SpawnCraftedPiece (this.GetOriginal().gameObject, craftWidth, craftHeight);
		this.craftedPiece.AlignTo (gameObject.transform.position);
	}

	public bool IsCorrect() {
		return this.GetOriginal ().IsCorrect (this.numerator, this.denominator);
	}

	public IEnumerator CraftRoutine() {
		this.isCrafting = true;

		// Craft piece
		this.CraftPiece ();

		// Align piece to the left
		this.craftedPiece.AlignToLocal(
			new Vector3(
				craftedPiece.gameObject.transform.localPosition.x-((this.widthWhole-this.craftedPiece.GetWidth())/2),
				craftedPiece.gameObject.transform.localPosition.y,
				craftedPiece.gameObject.transform.localPosition.z));
	
		SoundManager.Instance.Play (AudibleNames.LCDInterface.INCREASE, false);
		yield return new WaitForSecondsRealtime (0.6f);

		// Move piece towards original
		Vector3 targetPosition = this.original.GetBrokenPiece ().transform.position;
		yield return StartCoroutine (MoveCraftedPieceTowards(targetPosition));

		if (this.IsCorrect ()) {
			this.GetOriginal ().Solved ();
			SoundManager.Instance.Play (AudibleNames.Results.SUCCESS, false);
		} else {
			SoundManager.Instance.Play (AudibleNames.Results.MISTAKE, false);
		}
		yield return new WaitForSecondsRealtime (1.0f);


		this.Deactivate ();
		this.isCrafting = false;
	}

	public IEnumerator MoveCraftedPieceTowards(Vector3 targetPosition) {

		Vector3 targetX = new Vector3 (targetPosition.x, 
			craftedPiece.gameObject.transform.position.y,
			craftedPiece.gameObject.transform.position.z);

	
		// Move Horizontally
		while (this.craftedPiece.gameObject.transform.position != targetX) {
			this.craftedPiece.gameObject.transform.position =
				Vector3.MoveTowards (craftedPiece.gameObject.transform.position,
										targetX,
										animationSpeed*Time.unscaledDeltaTime);
			yield return null;
		}

		yield return new WaitForSecondsRealtime (0.2f);

		Vector3 targetY = new Vector3 (craftedPiece.gameObject.transform.position.x, 
			targetPosition.y,
			craftedPiece.gameObject.transform.position.z);
		

		// Move Vertically
		while (this.craftedPiece.gameObject.transform.position != targetY) {
			this.craftedPiece.gameObject.transform.position =
				Vector3.MoveTowards (craftedPiece.gameObject.transform.position,
					targetY,
					animationSpeed*Time.unscaledDeltaTime);

			yield return null;
		}

	}

	public void UncraftPiece() {
		if (this.craftedPiece != null) {
			Destroy (this.craftedPiece.gameObject);
		}
	}	
		
	public void SetPieceIndex(int index) {
		if (index < -1) {
			index = -1;
		}
		else if(index >= this.GetPieces().Count) {
			index = this.GetPieces ().Count - 1;
		}
		this.pieceIndex = index;
	}

	public void IncreaseFill() {
		this.SetPieceIndex (this.pieceIndex+1);
		this.SelectPiece (this.pieceIndex);
		this.numerator = this.pieceIndex + 1;
	}

	public void DecreaseFill() {
		this.DeselectPiece (this.pieceIndex);
		this.SetPieceIndex (this.pieceIndex-1);
		this.numerator = this.pieceIndex + 1;
	}

	public void SelectPiece(int index) {
		// Select piece if GetPieces is not empty, index is greater than 0 and less than GetPieces.Count
		if (this.GetPieces ().Count > 0 && index >= 0 && index < this.GetPieces ().Count) {
			this.GetPieces () [index].Select ();
		}
	}

	public void DeselectPiece(int index) {
		if (this.GetPieces ().Count > 0 && index >= 0 && index < this.GetPieces ().Count) {
			this.GetPieces () [index].Deselect ();
		}
	}

	public void SetToInitialValues() {
		this.SetNumerator (0f);
		this.SetPieceIndex(-1);

		this.isCorrect = false;
		this.isCrafting = false;
	}

	public void CancelCrafting() {
		if (!isCrafting) {
			this.DestroyPieces ();
			this.Deactivate ();
		}
	}

	// Divide the box to N pieces
	public void Partition(float threadDenominator) {
		this.SetToInitialValues ();
		this.SetDenominator (threadDenominator);

		// Compute the width of a single CLONE piece (different from the size of the originla)
		this.widthWhole = this.GetOriginal ().GetWidthWhole ();
		this.widthSingle = this.widthWhole / this.denominator;
		this.height = this.GetOriginal ().GetHeight ();

		for(int i = 0; i < this.denominator; i++) {
			this.GetPieces().Add (this.SpawnBrokenPiece(gameObject, this.widthSingle, this.height));
			// Set the line renderers so that the previous are on top
			this.GetPieces () [i].GetComponent<LineRenderer> ().sortingOrder = this.GetPieces () [i].GetComponent<LineRenderer> ().sortingOrder + ((int)this.denominator-i);
		}

		this.AlignClonePosition ();
	}


	public BrokenPiece SpawnBrokenPiece(GameObject parent, float pieceWidth, float pieceHeight) {
		GameObject holder = GameObject.Instantiate (pieceReference, Vector3.zero, Quaternion.identity);
		holder.transform.SetParent (parent.transform);

		BrokenPiece brokenPiece = holder.GetComponent<BrokenPiece> ();
		brokenPiece.SetSize (pieceWidth, pieceHeight);

		holder.SetActive(true);

		return brokenPiece;
	}

	public CraftedPiece SpawnCraftedPiece(GameObject parent, float pieceWidth, float pieceHeight) {
		GameObject holder = GameObject.Instantiate (craftedPieceReference, Vector3.zero, Quaternion.identity);
		holder.transform.SetParent (parent.transform);

		CraftedPiece craftedPiece = holder.GetComponent<CraftedPiece> ();
		craftedPiece.SetSize (pieceWidth, pieceHeight);

		holder.SetActive(true);

		return craftedPiece;
	}


	public void AlignClonePosition() {
		this.gameObject.transform.localPosition = new Vector2 (0f, this.GetHeight()+CLONE_SPACE);
	}

	public BrokenOriginal GetOriginal() {
		if (this.original == null) {
			this.original = this.GetComponentInParent<BrokenOriginal> ();
		}
		return this.original;
	}

	public float GetHeight() {
		return this.height;
	}


	public void SetNumerator(float newNumerator) {
		this.numerator = newNumerator;
	}

	public void SetDenominator(float newDenominator) {
		this.denominator = newDenominator;
	}

	public List<BrokenPiece> GetPieces() {
		if (this.pieces == null) {
			this.pieces = new List<BrokenPiece> ();
		}
		return this.pieces;
	}
}
