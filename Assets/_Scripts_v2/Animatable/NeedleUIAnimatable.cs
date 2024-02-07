using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleUIAnimatable : AnimatableClass {
	public const string scriptName = "NeedleUIAnimatable";
	[SerializeField] private Animator needleUIAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;

	void Start () {
		MAX_DURATION = 0.2f;
		this.needleUIAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (needleUIAnimator, "needleUIAnimator", scriptName);

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
		needleUIAnimator.ResetTrigger ("open");
		needleUIAnimator.ResetTrigger ("close");
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
			needleUIAnimator.SetTrigger ("open");
		}

		else if (triggerClose) {
			this.AnimationPlay ();
			needleUIAnimator.SetTrigger ("close");
		}
		ResetTriggerVariables ();
	}
}
