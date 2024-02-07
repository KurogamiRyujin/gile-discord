using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sky block's general behaviour and status.
/// 
/// Sky blocks are sliced by the needle to use its pieces to fill ghost blocks.
/// </summary>
public class SkyBlock : MonoBehaviour {

//	[SerializeField] private SkyFragment attachedFragment;

    /// <summary>
    /// Sky block's color.
    /// </summary>
	[SerializeField] protected Color pieceColor = new Color(0, 1, 1, 1);
    /// <summary>
    /// Sky block's outline color.
    /// </summary>
	[SerializeField] protected Color pieceOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);
    
    /// <summary>
    /// This is the size of one whole blocks.
    /// </summary>
    [SerializeField] private float blockSize = 1;
    /// <summary>
    /// The numerator value. The number of units of the original block it is.
    /// </summary>
	[SerializeField] private float numerator;
    /// <summary>
    /// The denominator value. The number of partitions of the whole.
    /// </summary>
	[SerializeField] private float denominator;

    /// <summary>
    /// Reference to the rectangle the sprite is rendered.
    /// </summary>
	[SerializeField] protected RectTransform rectTransform;
    /// <summary>
    /// Reference to a line renderer.
    /// </summary>
	[SerializeField] protected LineRenderer lineRenderer;
    /// <summary>
    /// Reference to the sky block's sprite renderer.
    /// </summary>
	[SerializeField] protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// Flag if the sky block should be invisible when the roomis stabilized.
    /// </summary>
	[SerializeField] private bool hideOnStable;
    /// <summary>
    /// Flag if the sky block's pieces cannot be broken.
    /// </summary>
	[SerializeField] private bool piecesNeverBreak;

    /// <summary>
    /// List of all fragments.
    /// </summary>
    List<SkyFragment> detachedFragments;

    /// <summary>
    /// Reference to the attached manager. Handles the unbroken/returned sky fragments.
    /// </summary>
	private AttachedManager attachedManager;
    /// <summary>
    /// Reference to the detached manager. Handles the broken sky fragments.
    /// </summary>
	private DetachedManager detachedManager;

	private int sceneObjectId;

    /// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		gameObject.layer = LayerMask.NameToLayer ("SkyBlock");
		this.rectTransform = GetComponent<RectTransform> ();
		this.lineRenderer = GetComponent<LineRenderer> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.attachedManager = GetComponentInChildren<AttachedManager> ();
		this.detachedManager = GetComponentInChildren<DetachedManager> ();
		this.attachedManager.SetPiecesNeverBreak (this.piecesNeverBreak);

		this.SetSize (rectTransform.sizeDelta.x*blockSize, rectTransform.sizeDelta.y);
		Debug.Log ("Awake Width: " + rectTransform.sizeDelta.x * blockSize);

		EventBroadcaster.Instance.AddObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.AddObserver (EventNames.UNSTABLE_AREA, Destabilize);
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.STABLE_AREA, Stabilize);
		EventBroadcaster.Instance.RemoveActionAtObserver (EventNames.UNSTABLE_AREA, Destabilize);
	}

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start() {
		this.attachedManager.InitialCraft (this, this.GetNumerator(), this.GetDenominator()); // Craft an unbroken sky fragment based on the numerator and denominator specified

		Debug.Log ("Width: " + this.attachedManager.GetAttachedFragmentBlock ().GetWidthWhole ());
		this.SetSize (this.attachedManager.GetAttachedFragmentBlock().GetWidthWhole(),
			this.attachedManager.GetAttachedFragmentBlock().GetHeight());

		this.attachedManager.GetAttachedFragmentBlock ().UpdateSize ();
	}

    /// <summary>
    /// Prompt to the sky block that the room has stabilized.
    /// 
    /// If the flag for it to be invisble on stability is true, it will be invisible.
    /// </summary>
	public void Stabilize() {
		if (this.hideOnStable) {
			this.Hide ();
		}
	}

    /// <summary>
    /// Prompt to the sky block that the room has destabilized.
    /// 
    /// If the flag for it to be invisble on stability is true, it will be visible.
    /// </summary>
	public void Destabilize() {
		if (this.hideOnStable) {
			this.Show ();
		}
	}

    /// <summary>
    /// Returns the sprite renderer.
    /// </summary>
    /// <returns>Sprite Renderer</returns>
	public SpriteRenderer GetSpriteRenderer() {
		if (this.spriteRenderer == null) {
			this.spriteRenderer = GetComponent<SpriteRenderer> ();
		}
		return this.spriteRenderer;
	}

    /// <summary>
    /// Returns the line renderer.
    /// </summary>
    /// <returns>Line Renderer</returns>
	public LineRenderer GetLineRenderer() {
		if (this.lineRenderer == null) {
			this.lineRenderer = GetComponent<LineRenderer> ();
		}
		return this.lineRenderer;
	}

    /// <summary>
    /// Makes the sky block visible.
    /// </summary>
	public void Show() {
		Debug.Log ("<color=blue>Called SHOW</color>");
		this.GetSpriteRenderer ().enabled = true;
		this.GetLineRenderer ().enabled = true;
		this.attachedManager.Show ();
	}

    /// <summary>
    /// Makes the sky block invisible.
    /// </summary>
	public void Hide() {
		Debug.Log ("<color=blue>Called HIDE</color>");
		this.GetSpriteRenderer ().enabled = false;
		this.GetLineRenderer ().enabled = false;
		this.attachedManager.Hide ();
	}

	/// <summary>
    /// Drops sky fragments when hit by the needle.
    /// 
    /// Fragments dropped depend on the charm equipped on the needle.
    /// </summary>
	public void Drop() {
		this.attachedManager.Drop ();
	}

    //This is called every Prefill so make sure Sky Block is whole
    //everytime it is called.
    /// <summary>
    /// Drops pieces directly to a ghost block to fill it. Used when ghost blocks need to be prefilled for a subtractino puzzle.
    /// </summary>
    /// <param name="targetBlock"></param>
    public void Drop(HollowBlock targetBlock) {
		this.attachedManager.Drop (targetBlock);
	}

    /// <summary>
    /// Returns the sky block's color.
    /// </summary>
    /// <returns>Sky Block Color</returns>
	public Color GetPieceColor() {
		return this.pieceColor;
	}
    /// <summary>
    /// Returns the outline color.
    /// </summary>
    /// <returns>Outline Color</returns>
	public Color GetPieceOutlineColor() {
		return this.pieceOutlineColor;
	}

    /// <summary>
    /// Adjusts the height and width of the sky block.
    /// </summary>
    /// <param name="newWidth">Width</param>
    /// <param name="newHeight">Height</param>
	public void SetSize(float newWidth, float newHeight) {
		this.rectTransform.sizeDelta = new Vector2 (newWidth, newHeight);
		this.spriteRenderer.size = new Vector2 (newWidth, newHeight);
		this.gameObject.transform.localScale = Vector3.one;

		this.OutlinePiece ();
	}

    /// <summary>
    /// Returns the sky block's whole size.
    /// </summary>
    /// <returns></returns>
	public float GetBlockSize() {
		return this.blockSize;
	}

    /// <summary>
    /// Outline the sky block.
    /// </summary>
	public void OutlinePiece() {
		this.lineRenderer = General.GenerateBoxOutline (this.lineRenderer, this.GetWidth(), this.GetHeight());
	}

    /// <summary>
    /// Absorbs sky fragments back to this main block.
    /// Return the value of the detachedFragment to the attachedFragment.
    /// </summary>
    /// <param name="detachedFragment"></param>
    public void Absorb(SkyFragmentPiece detachedFragment) {
		this.attachedManager.Absorb (detachedFragment);
	}

    /// <summary>
    /// Returns the sky block's width (Rect Transform).
    /// </summary>
    /// <returns></returns>
	public float GetWidth() {
		return this.rectTransform.rect.width;
	}

    /// <summary>
    /// Returns the sky block's height (Rect Transform).
    /// </summary>
    /// <returns></returns>
	public float GetHeight() {
		return this.rectTransform.rect.height;
	}

    /// <summary>
    /// Returns the numerator.
    /// </summary>
    /// <returns>Numerator</returns>
	public float GetNumerator() {
		if (this.numerator<= 0)
			this.numerator = 1;
		return this.numerator;
	}

    /// <summary>
    /// Returns the denominator.
    /// </summary>
    /// <returns>Denominator</returns>
	public float GetDenominator() {
		if (this.denominator <= 0)
			this.denominator = 1;
		return this.denominator;
	}

    /// <summary>
    /// Specifically used in tutorial 1.
    /// 
    /// Makes detached pieces invisible.
    /// </summary>
    public void HideDetachedPieces() {
		this.GetDetachedManager ().gameObject.SetActive (false);
	}

    /// <summary>
    /// Makes detached pieces visible.
    /// </summary>
	public void ShowDetachedPieces() {
		this.GetDetachedManager ().gameObject.SetActive (true);
	}

    /// <summary>
    /// Returns the detached manager. Makes sure the reference is not null.
    /// </summary>
    /// <returns></returns>
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
