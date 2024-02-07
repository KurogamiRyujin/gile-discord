using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavinePiece : MonoBehaviour {

	private DynamicRavine ravine;
	private List<SpriteRenderer> sprites;

	// Use this for initialization
	void Start () {
		ravine = GetComponentInParent<DynamicRavine> ();
		SpriteRenderer[] temp = GetComponentsInChildren<SpriteRenderer> ();
		sprites = new List<SpriteRenderer> ();
		for (int i = 0; i < temp.Length; i++) {
			if (temp [i].enabled)
				sprites.Add (temp [i]);
		}
		this.FillPiece (false);
	}

	public void FillPiece(bool flag) {
		if (flag) {
			foreach (SpriteRenderer sprite in sprites) {
				sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 1.0f);
				gameObject.layer = LayerMask.NameToLayer ("Ground");
			}
		} else {
			foreach (SpriteRenderer sprite in sprites) {
				sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
			}
		}
	}
}
