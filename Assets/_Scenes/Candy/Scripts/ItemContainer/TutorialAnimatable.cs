using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimatable : AnimatableClass {

	public const string GLOW = "glow";
	public const string OPEN = "open";
	public const string CLOSE = "close";
	public const string scriptName = "TutorialAnimatable";
	[SerializeField] private Animator tutorialAnimatable;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;
	[SerializeField] private bool triggerGlow = false;


	void Awake () {
		this.isLoop = true;
		this.isPlaying = false;
		MAX_DURATION = 0.2f;
		this.tutorialAnimatable = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (tutorialAnimatable, "tutorialAnimatable", scriptName);
//		this.ResetTriggerVariables ();
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
		triggerGlow = false;
	}

	// Functions to be called by external scripts
//	public void Open() {
//		this.triggerOpen = true;
//	}

	public void Close() {
		this.triggerClose = true;
	}

	public void Glow() {
		this.triggerGlow = true;
		Debug.Log ("<color=red>CALLED GLOW 6</color>");
	}

	public override void ResetTriggers() {
		tutorialAnimatable.ResetTrigger (OPEN);
		tutorialAnimatable.ResetTrigger (CLOSE);
		tutorialAnimatable.ResetTrigger (GLOW);
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			tutorialAnimatable.SetTrigger (OPEN);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			tutorialAnimatable.SetTrigger (CLOSE);
		}
		else if (triggerGlow) {
			this.AnimationPlay ();
			SoundManager.Instance.Play (AudibleNames.LCDInterface.GLOW, false);
			tutorialAnimatable.SetTrigger (GLOW);
			Debug.Log ("ENTERED GLOW");
		}
		ResetTriggerVariables ();
	}
}
