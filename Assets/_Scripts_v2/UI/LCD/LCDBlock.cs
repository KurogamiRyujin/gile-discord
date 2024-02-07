using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDBlock : MonoBehaviour {

	private int number;
	[SerializeField] private TextMesh numberUI;
	[SerializeField] private Transform blockSprite;
	private SpriteRenderer blockSpriteRenderer;

	void Awake() {
		this.number = 0;
		blockSpriteRenderer = blockSprite.GetComponent<SpriteRenderer> ();
	}

	void Update() {
		this.numberUI.text = this.number.ToString ();
	}

	public void SetSpriteXScale (float xScale) {
		Vector3 givenScale = new Vector3 (xScale, this.blockSprite.localScale.y, this.blockSprite.localScale.z);
		this.blockSprite.localScale = givenScale;
	}

	public void SetNumber(int number) {
		this.number = number;
	}

	public int GetNumber() {
		return this.number;
	}

	public Bounds GetSpriteBounds() {
		return this.blockSpriteRenderer.bounds;
	}
}
