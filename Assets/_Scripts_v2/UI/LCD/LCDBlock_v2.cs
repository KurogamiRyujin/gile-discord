using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDBlock_v2 : MonoBehaviour {
	private int number;
	[SerializeField] private Text numberUI;
	[SerializeField] private RectTransform blockSprite;

	[SerializeField] private Color highlightColor;
	[SerializeField] private Color baseColor;

	[SerializeField] private Color highlightOutline;
	[SerializeField] private Color baseOutline;

	private bool isHighlight;

	void Awake() {
		this.number = 0;
	}

	void Update() {
		this.numberUI.text = this.number.ToString ();
	}

	public void SetSpriteWidth (float width) {
		this.blockSprite.sizeDelta = new Vector2 (width, this.blockSprite.sizeDelta.y);
//		this.GetCollider2D ().offset = new Vector2 (width, this.GetCollider2D ().offset.y);
	}

	public void HighlightSprite() {
		this.isHighlight = true;
		this.SetSpriteColor (this.highlightColor, this.highlightOutline);
	}

	public void UnhighlightSprite() {
		this.isHighlight = false;
		this.SetSpriteColor (this.baseColor, this.baseOutline);
	}

	public void ShowOutline() {
//		if (this.isHighlight) {
//			this.SetSpriteColor (this.highlightColor, this.highlightOutline);
//		} else {
//			this.SetSpriteColor (this.baseColor, this.baseOutline);
//		}

		this.GetCollider2D ().enabled = true;
		this.SetSpriteColor (this.baseColor, this.baseOutline);
		this.numberUI.color = Color.white;
	}

	public void HideOutline() {
//		if (this.isHighlight) {
//			this.SetSpriteColor (this.highlightColor, this.highlightColor);
//		} else {
//			this.SetSpriteColor (this.baseColor, this.baseColor);
//		}
		this.GetCollider2D().enabled = false;
		this.SetSpriteColor (this.highlightColor, this.highlightColor);
		this.numberUI.color = Color.clear;
	}

	public void SetSpriteColor (Color blockColor, Color outlineColor) {
		GetComponentInChildren<LCDBlockColor> ().GetComponent<Image>().color = blockColor;
		GetComponentInChildren<LCDBlockOutline> ().GetComponent<Image>().color = outlineColor;
	}

	public Collider2D GetCollider2D() {
		return GetComponentInChildren<Collider2D> ();
	}

	public void SetNumber(int number) {
		this.number = number;
	}

	public int GetNumber() {
		return this.number;
	}

	public float GetSpriteWidth () {
		return this.blockSprite.sizeDelta.x;
	}
}
