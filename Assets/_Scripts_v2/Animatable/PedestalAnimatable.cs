using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalAnimatable : MonoBehaviour, Animatable {

	public const string scriptName = "PedestalAnimatable";

	[SerializeField] private Animator pedestalAnimator;
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;

	private bool isPlaying = false;

	void Start () {
		this.ResetTriggerVariables ();
		this.isPlaying = false;
	}

	void Update () {

	}

	public void Open() {
		this.triggerOpen = true;
	}

	public void Close() {
		this.triggerClose = true;
	}

	public void ResetTriggers () {
		pedestalAnimator.ResetTrigger ("open");
		pedestalAnimator.ResetTrigger ("close");
	}

	public void ResetTriggerVariables () {
		this.triggerOpen = false;
		this.triggerClose = false;
	}

	public bool IsPlaying () {
		return this.isPlaying;
	}

	public void AnimationPlay () {
		this.isPlaying = true;
	}

	public void AnimationStop () {
		this.isPlaying = false;
	}

	public void SwitchTriggers () {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			pedestalAnimator.SetTrigger ("open");
		}

		else if (triggerClose) {
			this.AnimationPlay ();
			pedestalAnimator.SetTrigger ("close");
		}
		ResetTriggerVariables ();
	}
}
