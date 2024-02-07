using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFragmentPieceUI : MonoBehaviour {
	public const string SKY_PIECE = "skyPiece";
	public const string HOLLOW_BLOCK = "hollowBlock";
	[SerializeField] protected SkyFragmentPiece skyPiece;
	[SerializeField] protected HollowBlock hollowBlock;
	[SerializeField] protected Fraction fractioLabel;

	[SerializeField] protected RectTransform rectTransform;
	[SerializeField] protected SpriteRenderer spriteRenderer;
	[SerializeField] protected LineRenderer lineRenderer;

	[SerializeField] protected float widthSingle; // Width of a piece with size 1/den
	[SerializeField] protected float height; // Height of object

	[SerializeField] protected float blockSize;
	[SerializeField] protected float numerator;
	[SerializeField] protected float denominator;

	[SerializeField] protected Color pieceColor = new Color(0, 1, 1, 1);
	[SerializeField] protected Color pieceOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);


	void Awake () {
		this.rectTransform = GetComponent<RectTransform> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.lineRenderer = GetComponent<LineRenderer> ();

		this.widthSingle = 0f;
		this.height = this.rectTransform.rect.height;

		this.numerator = 1;
		this.denominator = 1;

		this.ChangeColor (this.pieceColor, this.pieceOutlineColor);

		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_GEM_CARRY, UpdateCarriedGem);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_LIFT_CARRY, UpdateCarriedLift);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_GEM_DROP, DropGem);
		EventBroadcaster.Instance.AddObserver (EventNames.ON_PLAYER_LIFT_DROP, DropLift);
		this.DropGem ();
	}
	void OnDestroy() {

		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_GEM_CARRY);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_LIFT_CARRY);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_GEM_DROP);
		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_PLAYER_LIFT_DROP);
	}

	public void ChangeColor(Color fill, Color outline) {
		this.spriteRenderer.color = fill;
		this.lineRenderer.startColor = outline;
		this.lineRenderer.endColor = outline;
	}

	public Fraction GetFractionLabel() {
		if (this.fractioLabel == null) {
			this.fractioLabel = GetComponentInChildren<Fraction> ();
		}
		return this.fractioLabel;
	}

	public void UpdateCarriedGem(Parameters parameters) {
		SkyFragmentPiece piece = parameters.GetObjectExtra (SKY_PIECE) as SkyFragmentPiece;
		this.skyPiece = piece;

		// Safety measure. Should never enter this.
		if(this.skyPiece == null) {
			DropGem();
		}
		else {
			this.SetSize (this.skyPiece.GetWidth (), this.skyPiece.GetHeight ());
			this.ChangeColor (this.skyPiece.GetFillColor (), this.skyPiece.GetOutlineColor ());
			this.spriteRenderer.material = skyPiece.GetSpriteRenderer ().material;
			this.GetFractionLabel ().Show ();
			this.SetFractionLabel ((int)this.skyPiece.GetNumerator (), (int)this.skyPiece.GetDenominator ());
		}
	}

	// TODO
	public void UpdateCarriedLift(Parameters parameters) {
		HollowBlock piece = parameters.GetObjectExtra (HOLLOW_BLOCK) as HollowBlock;
//		this.skyPiece = piece;
		this.hollowBlock = piece;
		// Safety measure. Should never enter this.
		if(this.hollowBlock == null) {
			DropLift();
		}
		else {
			this.SetSize (this.hollowBlock.GetWidthPiece (), this.hollowBlock.GetHeight ());
			this.ChangeColor (this.hollowBlock.GetTangibleColor (), this.hollowBlock.GetOutlineStartColor ());
			this.spriteRenderer.material = hollowBlock.GetSpriteRenderer ().material;
			this.GetFractionLabel ().Show ();
			this.SetFractionLabel ((int)this.hollowBlock.GetNumerator (), (int)this.hollowBlock.GetDenominator ());
		}
	}

	public void SetFractionLabel(int newNum, int newDen) {
		this.numerator = newNum;
		this.denominator = newDen;
		this.GetFractionLabel ().SetValue ((int)this.numerator, (int)this.denominator);
	}

	public void DropLift() {
		this.hollowBlock = null;
		this.SetSize (0f, 0f);

		this.GetFractionLabel ().Hide ();
	}

	public void DropGem() {
		this.skyPiece = null;
		this.SetSize (0f, 0f);

		this.GetFractionLabel ().Hide ();
	}

	public void SetSize(float newWidth, float newHeight) {
		this.rectTransform.sizeDelta = new Vector2 (newWidth, newHeight);
		this.spriteRenderer.size = new Vector2 (newWidth, newHeight);
		this.gameObject.transform.localScale = Vector3.one;

		this.OutlinePiece ();
	}
	// Automatically called when size is set. Generates a rectangular outline.
	public void OutlinePiece() {
		this.lineRenderer = General.GenerateBoxOutline (this.lineRenderer, this.GetWidth (), this.GetHeight ());
	}

	public float GetWidth() {
		return this.rectTransform.rect.width;
	}

	public float GetHeight() {
		return this.rectTransform.rect.height;
	}
}
