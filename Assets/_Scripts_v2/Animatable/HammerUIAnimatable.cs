using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerUIAnimatable : AnimatableClass {
	public const string scriptName = "HammerUIAnimatable";

	[SerializeField] private Animator hammerUIAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;

	void Start () {
		MAX_DURATION = 0.2f;
		this.hammerUIAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (hammerUIAnimator, "hammerUIAnimator", scriptName);

		this.ResetTriggerVariables ();
		this.isPlaying = false;

	}

	// Functions to be called by external scripts
	public void Open() {
		this.triggerOpen = true;
	}

	public void Close() {
		this.triggerClose = true;
	}

	public override void ResetTriggers() {
		hammerUIAnimator.ResetTrigger ("open");
		hammerUIAnimator.ResetTrigger ("close");
	}

	void Update () {
		SwitchTriggers ();
	}
		
	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			hammerUIAnimator.SetTrigger ("open");
		}

		else if (triggerClose) {
			this.AnimationPlay ();
			hammerUIAnimator.SetTrigger ("close");
		}
		ResetTriggerVariables ();
	}
}
