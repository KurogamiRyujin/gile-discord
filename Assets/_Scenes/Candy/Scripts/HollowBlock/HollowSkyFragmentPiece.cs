using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowSkyFragmentPiece : SkyFragment {
	[SerializeField] protected DetachedManager detachedManagerParent;
	[SerializeField] protected HintBubbleSkyPiece fractionBubbleLabel;

	[SerializeField] protected float mass;
	[SerializeField] protected float gravity;

	void Awake () {
		base.Awake ();
	}


	// Fix label rotation
	void Update() {
		FixLabelRotation ();
	}

	void FixLabelRotation() {
		if (GetFractionBubbleLabel () != null) {
			Vector3 prevRotation = GetFractionBubbleLabel ().transform.eulerAngles;
			Vector3 newRotation =  new Vector3 (
				GetFractionBubbleLabel ().transform.localRotation.x,
				GetFractionBubbleLabel ().transform.localRotation.y,
				-gameObject.transform.localRotation.z);

			if (prevRotation != newRotation) {
				GetFractionBubbleLabel ().transform.eulerAngles = newRotation;
			}
		}

	}

	public HintBubbleSkyPiece GetFractionBubbleLabel() {
		if (this.fractionBubbleLabel == null) {
			this.fractionBubbleLabel.GetComponentInChildren<HintBubbleSkyPiece> ();
		}
		return this.fractionBubbleLabel;
	}

	public override void Initialize(SkyBlock parent, float numValue, float denValue) {
		base.Initialize (parent, numValue, denValue);
		this.GetFractionBubbleLabel ().UpdateLabel ((int)this.GetNumerator(), (int)this.GetDenominator());
		this.detachedManagerParent = this.skyBlockParent.GetDetachedManager ();
	}
}
