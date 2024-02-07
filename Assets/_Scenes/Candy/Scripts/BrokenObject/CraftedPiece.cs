using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftedPiece : BrokenPiece {

	private Color craftColor = new Color(0, 1, 1, 1);
	private Color craftOutlineColor = new Color (0.216f, 0.431f, 0.447f, 1f);

	void Awake() {
		base.Awake ();
		this.ChangeColor (this.craftColor, this.craftOutlineColor);
		this.rectTransform.localPosition = Vector3.zero;
	}


	public void AlignTo(Vector3 position) {
		this.rectTransform.position = position;
	}
	public void AlignToLocal(Vector3 position) {
		this.rectTransform.localPosition = position;
	}
}
