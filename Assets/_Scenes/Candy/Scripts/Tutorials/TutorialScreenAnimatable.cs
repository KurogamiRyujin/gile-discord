using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenAnimatable : AnimatableClass {

	public const string PLAY = "play";
	public const string CLOSE = "close";
	public const string scriptName = "TutorialScreenAnimatable";
	[SerializeField] private Animator tutorialScreenAnimatable;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerPlay = false;
	[SerializeField] private bool triggerClose = false;


	void Awake () {
		this.isLoop = true;
		this.isPlaying = false;
		MAX_DURATION = 0.2f;
		this.tutorialScreenAnimatable = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (tutorialScreenAnimatable, "tutorialScreenAnimatable", scriptName);
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerPlay = false;
		triggerClose = false;
	}

	public void Close() {
		this.triggerClose = true;
	}

	public void Play() {
		this.triggerPlay = true;
	}

	public override void ResetTriggers() {
		tutorialScreenAnimatable.ResetTrigger (PLAY);
		tutorialScreenAnimatable.ResetTrigger (CLOSE);
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerPlay) {
			this.AnimationPlay ();
			tutorialScreenAnimatable.SetTrigger (PLAY);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			tutorialScreenAnimatable.SetTrigger (CLOSE);
		}
		ResetTriggerVariables ();
	}
	
}
