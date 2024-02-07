using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItemsAnimatable : AnimatableClass {

	public const string OPEN = "open";
	public const string CLOSE = "close";
	public const string scriptName = "TutorialItemsAnimatable";
	[SerializeField] private Animator tutorialItemsAnimatable;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;


	void Awake () {
		this.isLoop = true;
		this.isPlaying = false;
		MAX_DURATION = 0.2f;
		this.tutorialItemsAnimatable = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (tutorialItemsAnimatable, "tutorialItemsAnimatable", scriptName);
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
		tutorialItemsAnimatable.ResetTrigger (OPEN);
		tutorialItemsAnimatable.ResetTrigger (CLOSE);
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			tutorialItemsAnimatable.SetTrigger (OPEN);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			tutorialItemsAnimatable.SetTrigger (CLOSE);
		}
		ResetTriggerVariables ();
	}

}
