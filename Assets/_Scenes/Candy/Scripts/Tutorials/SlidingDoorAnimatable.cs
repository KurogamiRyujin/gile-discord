using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorAnimatable : AnimatableClass {

	public const string OPEN = "open";
	public const string CLOSE = "close";
	public const string scriptName = "SlidingDoorAnimatable";
	[SerializeField] private Animator slidingDoorAnimatable;

	[SerializeField] private bool isMute;
	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;


	void Awake () {
		this.isLoop = true;
		this.isPlaying = false;
		this.isMute = false;
		MAX_DURATION = 0.2f;
		this.slidingDoorAnimatable = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (slidingDoorAnimatable, "slidingDoorAnimatable", scriptName);
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
	}

	public void Close() {
		this.triggerClose = true;
	}

	public void Open() {
		this.triggerOpen = true;
	}

	public override void ResetTriggers() {
		slidingDoorAnimatable.ResetTrigger (OPEN);
		slidingDoorAnimatable.ResetTrigger (CLOSE);
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			slidingDoorAnimatable.SetTrigger (OPEN);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			slidingDoorAnimatable.SetTrigger (CLOSE);
		}
		ResetTriggerVariables ();
	}

	public void SingleMute() {
		this.isMute = true;
	}

	// Meant to be muted just once, so set mute false after every call.
	public void PlayDoorOpen() {
		if(!this.isMute) {
			SoundManager.Instance.Play (AudibleNames.Door.OPEN, false);
		}
		this.isMute = false;
	}

	public void PlayDoorClose() {
		if (!this.isMute) {
			SoundManager.Instance.Play (AudibleNames.Door.CLOSE, false);
		}
		this.isMute = false;
	}
}
