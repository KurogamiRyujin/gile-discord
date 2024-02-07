using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour {

	[SerializeField] protected RectTransform rectTransform;
	[SerializeField] protected SpriteRenderer spriteRenderer;

	[SerializeField] protected LineRenderer lineRenderer;

	[SerializeField] protected bool isSelected;


	private Color deselectColor = new Color(1, 1, 1, 0);
	private Color deselectOutlineColor = new Color(1, 1, 1, 1);


	private Color selectColor = new Color(0, 1, 1, 1);
	private Color selectOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);


	protected virtual void Awake () {
		this.rectTransform = GetComponent<RectTransform> ();
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
		this.lineRenderer = GetComponent<LineRenderer> ();

		this.Deselect ();
	}

	// Automatically called when size is set. Generates a rectangular outline.
	public void OutlinePiece() {
		this.lineRenderer = General.GenerateBoxOutline (this.lineRenderer, this.GetWidth(), this.GetHeight());
	}

	public void Select() {
		this.ChangeColor (this.selectColor, this.selectOutlineColor);
	}

	public void Deselect() {
		this.ChangeColor (this.deselectColor, this.deselectOutlineColor);
	}

	public void ChangeColor(Color fill, Color outline) {
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
		this.gameObject.transform.localScale = Vector3.one;

		this.OutlinePiece ();
	}
}
