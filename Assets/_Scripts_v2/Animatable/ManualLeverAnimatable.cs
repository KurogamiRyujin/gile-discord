using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualLeverAnimatable : AnimatableClass {

	public const string OPEN = "open";
	public const string CLOSE = "close";
	public const string LOCK = "lock";

	public const string scriptName = "ManualLeverAnimatable";
	[SerializeField] private Animator manualLeverAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;
	[SerializeField] private bool triggerLock = false;


	void Awake() {
		this.isPlaying = false;
	}

	void Start () {
		MAX_DURATION = 0.2f;
		this.manualLeverAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (manualLeverAnimator, "manualLeverAnimator", scriptName);
		this.ResetTriggerVariables ();
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
		triggerLock = false;
	}

	// Functions to be called by external scripts
	public void Open() {
		this.triggerOpen = true;
	}

	public void Close() {
		this.triggerClose = true;
	}

	public void Lock() {
		this.triggerLock = true;
	}

	public override void ResetTriggers() {
		manualLeverAnimator.ResetTrigger (OPEN);
		manualLeverAnimator.ResetTrigger (CLOSE);
		manualLeverAnimator.ResetTrigger (LOCK);
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			SoundManager.Instance.Play (AudibleNames.Trampoline.OPERATE, false);
			manualLeverAnimator.SetTrigger (OPEN);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			SoundManager.Instance.Play (AudibleNames.Trampoline.OPERATE, false);
			manualLeverAnimator.SetTrigger (CLOSE);
		}
		else if (triggerLock) {
			this.AnimationPlay ();
			SoundManager.Instance.Play (AudibleNames.Trampoline.LOCK, false);
			manualLeverAnimator.SetTrigger (LOCK);
		}
		ResetTriggerVariables ();
	}
}
