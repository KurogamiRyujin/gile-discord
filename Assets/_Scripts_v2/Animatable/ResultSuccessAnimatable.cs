using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSuccessAnimatable : AnimatableClass {
	public const string scriptName = "ResultSuccessAnimatable";

	public const string SHOW = "show";
	public const string HIDE = "hide";

	[SerializeField] private Animator successUIAnimator;
	[SerializeField] private AudioSource sfxSuccess;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerShow = false;
	[SerializeField] private bool triggerHide = false;
	[SerializeField] private bool triggerCraft = false;

	void Start () {
		MAX_DURATION = 0.5f;
		this.successUIAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (successUIAnimator, "successUIAnimator", scriptName);

		this.sfxSuccess = GetComponent<AudioSource> ();

		this.ResetTriggerVariables ();
		this.isPlaying = false;

	}

	// Functions to be called by external scripts
	public void Show() {
		this.triggerShow = true;
	}

	public void Craft() {
		this.triggerCraft = true;
	}

	public void Hide() {
		this.triggerHide = true;
	}

	public override void ResetTriggers() {
		successUIAnimator.ResetTrigger (SHOW);
		successUIAnimator.ResetTrigger (HIDE);
	}

	void Update () {
		SwitchTriggers ();
	}
		
	public override void ResetTriggerVariables() {
		triggerShow = false;
		triggerHide = false;
		triggerCraft = false;
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerShow) {
			this.AnimationPlay ();
			successUIAnimator.SetTrigger (SHOW);
			SoundManager.Instance.Play (AudibleNames.Results.SUCCESS, false);
		}

		else if (triggerHide) {
			this.AnimationPlay ();
			successUIAnimator.SetTrigger (HIDE);
		}
		else if (triggerCraft) {
			this.AnimationPlay ();
			successUIAnimator.SetTrigger (SHOW);
			SoundManager.Instance.Play (AudibleNames.LCDInterface.INCREASE, false);
		}
		ResetTriggerVariables ();
	}
}
