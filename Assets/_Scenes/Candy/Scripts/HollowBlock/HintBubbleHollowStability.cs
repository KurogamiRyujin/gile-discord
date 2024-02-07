using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintBubbleHollowStability : MonoBehaviour {
	public const string NUMERATOR = "numerator";
	public const string DENOMINATOR = "denominator";

	[SerializeField] private TextMeshPro textMesh;

	[SerializeField] private HintBubbleManager bubbleManager;
	[SerializeField] private bool isOpen;

	void Awake() {
		this.bubbleManager = GetComponent<HintBubbleManager> ();
//		EventBroadcaster.Instance.AddObserver (EventNames.ON_HOLLOW_STABILITY_UPDATE, UpdateHint);
		this.isOpen = false;
	}

	void OnDestroy() {
//		EventBroadcaster.Instance.RemoveObserver (EventNames.ON_HOLLOW_STABILITY_UPDATE);
	}

//	public void UpdateHint(Parameters parameters) {
//		float numerator = parameters.GetFloatExtra (NUMERATOR, 0f);
//		float denominator = parameters.GetFloatExtra (DENOMINATOR, 0f);
//		this.UpdateLabel ((int)numerator, (int)denominator);
//	}
	public void HardClose () {
		this.GetBubbleManager ().HardClose ();
	}

	public void UpdateLabel (int numerator, int denominator) {
		this.GetTextMesh().text = numerator+"\n"+denominator;
	}

	public void HideHint() {
		// Will only check once (for initial)
		if (this.isOpen) {
			this.GetBubbleManager().HideHint();
		}
	}

	public void Sleep() {
		this.GetBubbleManager ().Sleep ();
	}
	public void WakeUp() {
		this.GetBubbleManager ().WakeUp ();
	}

	public void Observe() {
		this.GetBubbleManager ().Observe ();
	}
	public void ShowHint() {
		this.isOpen = true;
		this.GetBubbleManager().ShowHint();
	}

	public HintBubbleManager GetBubbleManager() {
		if (this.bubbleManager == null) {
			this.bubbleManager = GetComponent<HintBubbleManager> ();
		}
		return this.bubbleManager;
	}

	public TextMeshPro GetTextMesh() {
		if (this.textMesh == null) {
			this.textMesh = GetComponentInChildren<TextMeshPro> ();
		}
		return this.textMesh;
	}
}
