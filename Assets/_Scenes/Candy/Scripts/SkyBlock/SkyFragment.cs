using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the tangible fragment that is generated when the player hits
 * the attachedFragment (SkyFragmentBlock)
**/
public class SkyFragment : MonoBehaviour {
	[SerializeField] protected SkyBlock skyBlockParent;

	[SerializeField] protected RectTransform rectTransform;
	[SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected LineRenderer lineRenderer;


	[SerializeField] protected Rigidbody2D rigidBody2D;
	[SerializeField] protected BoxCollider2D boxCollider;

	[SerializeField] protected float widthWhole; // Width of a 1 whole object
	[SerializeField] protected float widthSingle; // Width of a piece with size 1/den

	[SerializeField] protected float height; // Height of object


	[SerializeField] protected float blockSize;
	[SerializeField] protected float numerator;
	[SerializeField] protected float denominator;
	[SerializeField] protected bool isPrefilled;


	[SerializeField] protected Color pieceColor = new Color(0, 1, 1, 1);
	[SerializeField] protected Color pieceOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);


	protected virtual void Awake () {
		this.rectTransform = GetComponent<RectTransform> ();
		this.rigidBody2D = GetComponent<Rigidbody2D> ();
		this.boxCollider = GetComponent<BoxCollider2D> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.lineRenderer = GetComponent<LineRenderer> ();

		this.widthWhole = this.rectTransform.rect.width;
		this.widthSingle = this.widthWhole/this.GetDenominator();

		this.height = this.rectTransform.rect.height;

		this.numerator = 1;
		this.denominator = 1;

		this.ChangeColor (this.pieceColor, this.pieceOutlineColor);
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
		this.GetSpriteRenderer ().enabled = true;
		this.GetLineRenderer ().enabled = true;
	}

	public void Hide() {
		this.GetSpriteRenderer ().enabled = false;
		this.GetLineRenderer ().enabled = false;

	}

	public Color GetFillColor() {
		return this.spriteRenderer.color;
	}

	public Color GetOutlineColor() {
		return this.lineRenderer.startColor;
	}


	public virtual void Initialize(SkyBlock parent, float numValue, float denValue) {
		this.skyBlockParent = parent;
		this.SetBlockSize (skyBlockParent.GetBlockSize());
		this.widthWhole = parent.GetWidth (); // Reference the 1 whole size
		this.widthSingle = (this.widthWhole / denValue)*numValue; // This is the width of this object.

		//		this.widthSingle = (this.widthWhole / this.GetDenominator ())*this.GetNumerator(); // This is the width of this object.
		this.SetSize (this.widthSingle, this.height);


		Debug.Log ("<color=red>PREV NUM DEN = "+this.GetNumerator()+"  "+this.GetDenominator()+"</color>");
		if (!isPrefilled) {
			this.SetNumerator (numValue);
			this.SetDenominator (denValue);
		}
		Debug.Log ("<color=red>New NUM DEN = "+this.GetNumerator()+"  "+this.GetDenominator()+"</color>");
	}

	public void PrefillNumerator() {
		this.isPrefilled = true;
	}

//	public virtual void Initialize(SkyBlock parent, float numValue, float denValue) {
//		this.skyBlockParent = parent;
//
//		this.SetBlockSize (skyBlockParent.GetBlockSize());
//
//		// TODO: Fixed since this overrides prefilled block settings, check if this holds.
//
//		this.SetNumerator (numValue);
//		this.SetDenominator (denValue);
//
//		this.widthWhole = parent.GetWidth (); // Reference the 1 whole size
//
//		this.widthSingle = (this.widthWhole / this.GetDenominator ())*this.GetNumerator(); // This is the width of this object.
//		this.SetSize (this.widthSingle, this.height);
//	}

	public virtual float GetBlockSize() {
		return this.blockSize;
	}

	public virtual void SetBlockSize(float value) {
		this.blockSize = value;
	}

	public virtual void SetNumerator(float value) {
		this.numerator = (int) value;
	}

	// Do not allow zero denominator
	public virtual void SetDenominator(float value) {
		this.denominator = (int) value;
		if (this.denominator == 0)
			this.denominator = 1;
	}

	public float GetNumerator() {
		return this.numerator;
	}

	public float GetDenominator() {
		if (this.denominator == 0) {
			this.denominator = 1;
		}
		return this.denominator;
	}

	public Rigidbody2D GetRigidBody2D() {
		return this.rigidBody2D;
	}

	// Automatically called when size is set. Generates a rectangular outline.
	public void OutlinePiece() {
		this.lineRenderer = General.GenerateBoxOutline (this.lineRenderer, this.GetWidth(), this.GetHeight());
	}

	public void ChangeColor(Color fill, Color outline) {
		this.pieceColor = fill;
		this.pieceOutlineColor = outline;
		this.SoftChangeColor (fill, outline);
	}

	public void SoftChangeColor(Color fill, Color outline) {
		this.spriteRenderer.color = fill;
		this.lineRenderer.startColor = outline;
		this.lineRenderer.endColor = outline;
	}

	public float GetWidth() {
		return this.rectTransform.rect.width;
	}

	public float GetHeight() {
		return this.rectTransform.rect.height;
	}

	public void SetWidth(float newWidth) {
		this.SetSize (newWidth, this.rectTransform.rect.height);
	}

	public void SetHeight(float newHeight) {
		this.SetSize (this.rectTransform.rect.width, newHeight);
	}

	public void SetSize(float newWidth, float newHeight) {
		this.rectTransform.sizeDelta = new Vector2 (newWidth, newHeight);
		this.spriteRenderer.size = new Vector2 (newWidth, newHeight);
		this.boxCollider.size = new Vector2 (newWidth, newHeight);
		this.gameObject.transform.localScale = Vector3.one;

		this.OutlinePiece ();
	}

	public float GetWidthWhole() {
//		this.widthWhole = this.rectTransform.rect.width;
		return this.widthWhole;
	}
	public float GetWidthSingle() {
		this.widthSingle = (this.widthWhole / this.GetDenominator ())*this.GetNumerator();
//		this.widthSingle = this.rectTransform.sizeDelta.x;
		return this.widthSingle;
	}

	public void AlignToLocal(Vector3 position) {
		this.rectTransform.localPosition = position;
	}

	public SkyBlock GetSkyBlockParent() {
		return this.skyBlockParent;
	}
}
