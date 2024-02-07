using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleAnimatable : AnimatableClass {
	public const string scriptName = "BubbleAnimatable";

	[SerializeField] private Animator bubbleAnimatable;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;
	[SerializeField] private bool triggerTake = false;
	[SerializeField] private bool triggerSimpleHide = false;
	[SerializeField] private bool isObserving = true; // Set to false for Pickup items that are picked up

	void Awake() {
		this.isPlaying = false;
		this.isObserving = true;
	}
	void Start () {
		MAX_DURATION = 0.2f;
		this.bubbleAnimatable = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (bubbleAnimatable, "bubbleAnimatable", scriptName);

		this.ResetTriggerVariables ();
	}

	// Functions to be called by external scripts
	public void Open() {
		this.triggerOpen = true;
	}

	public void Close() {
		this.triggerClose = true;
	}
	public void SimpleHide() {
		this.triggerSimpleHide = true;
	}
	public void Sleep() {
		this.isObserving = false;
		this.HardClose ();
	}
	public void WakeUp() {
		this.isObserving = true;
	}
	public void HardClose() {
		this.AnimationPlay ();
		if(bubbleAnimatable != null)
			bubbleAnimatable.SetTrigger ("take");
	}

	public override void ResetTriggers() {
		bubbleAnimatable.ResetTrigger ("open");
		bubbleAnimatable.ResetTrigger ("close");
		bubbleAnimatable.ResetTrigger ("take");
		bubbleAnimatable.ResetTrigger ("simpleHide");
	}

	public bool IsObserving() {
		return this.isObserving;
	}

	public void Observe() {
		this.isObserving = true;
	}

	void Update () {
		SwitchTriggers ();
	}
		
	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
		triggerTake = false;
		triggerSimpleHide = false;
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (isObserving) {
			if (triggerOpen) {
				this.AnimationPlay ();
				bubbleAnimatable.SetTrigger ("open");
			} else if (triggerClose) {
				this.AnimationPlay ();
				bubbleAnimatable.SetTrigger ("close");
			}
//			else if (triggerSimpleHide) {
//				this.AnimationPlay ();
//				bubbleAnimatable.SetTrigger ("simpleHide");
//			}
		}
		if (triggerSimpleHide) {
			this.AnimationPlay ();
			bubbleAnimatable.SetTrigger ("simpleHide");
		}
		ResetTriggerVariables ();
	}

	public void RemoveComponents() {
		Destroy (this);
	}
}
