using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBlock : MonoBehaviour {

//	[SerializeField] private SkyFragment attachedFragment;

	[SerializeField] protected Color pieceColor = new Color(0, 1, 1, 1);
	[SerializeField] protected Color pieceOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);

	// This is the size of one whole blocks it originally is
	[SerializeField] private float blockSize = 1;
	[SerializeField] private float numerator;
	[SerializeField] private float denominator;

	[SerializeField] protected RectTransform rectTransform;
	[SerializeField] protected LineRenderer lineRenderer;
	[SerializeField] protected SpriteRenderer spriteRenderer;

	[SerializeField] private bool hideOnStable;
	[SerializeField] private bool piecesNeverBreak;

	// List of all fragments
	List<SkyFragment> detachedFragments;

	private AttachedManager attachedManager; // Handles the unbroken/returned sky fragments
	private DetachedManager detachedManager; // Handles the broken sky fragments

	private int sceneObjectId;

	void Awake() {
		gameObject.layer = LayerMask.NameToLayer ("SkyBlock");
		this.rectTransform = GetComponent<RectTransform> ();
		this.lineRenderer = GetComponent<LineRenderer> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.attachedManager = GetComponentInChildren<AttachedManager> ();
		this.detachedManager = GetComponentInChildren<DetachedManager> ();
		this.attachedManager.SetPiecesNeverBreak (this.piecesNeverBreak);

		this.SetSize (rectTransform.sizeDelta.x*blockSize, rectTransform.sizeDelta.y);
		Debug.LogError ("Awake Width: " + rectTransform.sizeDelta.x * blockSize);

		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, Destabilize);
	}
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveObserver (EventNames.STABLE_AREA);
		EventBroadcaster.Instance.RemoveObserver (EventNames.UNSTABLE_AREA);
	}

	void Start() {
		this.attachedManager.InitialCraft (this, this.GetNumerator(), this.GetDenominator()); // Craft an unbroken sky fragment based on the numerator and denominator specified

		Debug.LogError ("Width: " + this.attachedManager.GetAttachedFragmentBlock ().GetWidthWhole ());
		this.SetSize (this.attachedManager.GetAttachedFragmentBlock().GetWidthWhole(),
			this.attachedManager.GetAttachedFragmentBlock().GetHeight());

		this.attachedManager.GetAttachedFragmentBlock ().UpdateSize ();
	}

	public void Stabilize() {
		if (this.hideOnStable) {
			this.Hide ();
		}
	}
	public void Destabilize() {
		if (this.hideOnStable) {
			this.Show ();
		}
	}

	public SpriteRenderer GetSpriteRenderer() {
		if (this.spriteRenderer == null) {
			this.spriteRenderer = GetComponent<SpriteRenderer> ();
		}
		return this.spriteRenderer;
	}

	public LineRenderer GetLineRenderer() {
		if (this.lineRenderer == null) {
			this.lineRenderer = GetComponent<LineRenderer> ();
		}
		return this.lineRenderer;
	}

	public void Show() {
		Debug.Log ("<color=blue>Called SHOW</color>");
		this.GetSpriteRenderer ().enabled = true;
		this.GetLineRenderer ().enabled = true;
		this.attachedManager.Show ();
	}

	public void Hide() {
		Debug.Log ("<color=blue>Called HIDE</color>");
		this.GetSpriteRenderer ().enabled = false;
		this.GetLineRenderer ().enabled = false;
		this.attachedManager.Hide ();
	}

	// For manual breaking
	public void Drop() {
		this.attachedManager.Drop ();
	}
	// This is called every Prefill so make sure Sky Block is whole
	// everytime it is called
	public void Drop(HollowBlock targetBlock) {
		this.attachedManager.Drop (targetBlock);
	}

	public Color GetPieceColor() {
		return this.pieceColor;
	}
	public Color GetPieceOutlineColor() {
		return this.pieceOutlineColor;
	}
	public void SetSize(float newWidth, float newHeight) {
		this.rectTransform.sizeDelta = new Vector2 (newWidth, newHeight);
		this.spriteRenderer.size = new Vector2 (newWidth, newHeight);
		this.gameObject.transform.localScale = Vector3.one;

		this.OutlinePiece ();
	}

	public float GetBlockSize() {
		return this.blockSize;
	}
	public void OutlinePiece() {
		this.lineRenderer = General.GenerateBoxOutline (this.lineRenderer, this.GetWidth(), this.GetHeight());
	}
	// Return the value of the detachedFragment to the attachedFragment
	public void Absorb(SkyFragmentPiece detachedFragment) {
		this.attachedManager.Absorb (detachedFragment);
	}

	public float GetWidth() {
		return this.rectTransform.rect.width;
	}

	public float GetHeight() {
		return this.rectTransform.rect.height;
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
	// For tutorial 1
	public void HideDetachedPieces() {
		this.GetDetachedManager ().gameObject.SetActive (false);
	}
	public void ShowDetachedPieces() {
		this.GetDetachedManager ().gameObject.SetActive (true);
	}

	// Safety null check
	public DetachedManager GetDetachedManager() {
		if (this.detachedManager == null) {
			this.detachedManager = GetComponentInChildren<DetachedManager> ();
		}

		return this.detachedManager;
	}

	public int GetSceneObjectId() {
		return this.sceneObjectId;
	}

	public void SetSceneObjectId(int id) {
		this.sceneObjectId = id;
	}
}
