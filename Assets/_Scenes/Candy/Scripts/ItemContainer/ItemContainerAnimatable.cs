using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerAnimatable : AnimatableClass {

	public const string OPEN = "open";
	public const string CLOSE = "close";
	public const string DOUBLE = "double";
	public const string SINGLE = "single";
	public const string DOUBLE_IDLE = "doubleIdle";
	public const string SINGLE_IDLE = "singleIdle";

	public const string scriptName = "ItemContainerAnimatable";
	[SerializeField] private Animator itemContainerAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerClose = false;

	[SerializeField] private bool triggerSingle = false;
	[SerializeField] private bool triggerDouble = false;

	[SerializeField] private bool triggerSingleIdle = false;
	[SerializeField] private bool triggerDoubleIdle = false;


	void Awake() {
		this.isPlaying = false;
	}

	void Start () {
		MAX_DURATION = 0.2f;
		this.itemContainerAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (itemContainerAnimator, "itemContainerAnimator", scriptName);
	}

	void Update () {
		SwitchTriggers ();
	}

	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
		triggerSingle = false;
		triggerSingleIdle = false;
		triggerDouble = false;
		triggerDoubleIdle = false;
	}

	// Functions to be called by external scripts
	public void Open() {
		this.triggerOpen = true;
	}

	public void Close() {
		this.triggerClose = true;
	}

	public void SingleIdle() {
		this.triggerSingleIdle = true;
	}
	public void DoubleIdle() {
		this.triggerDoubleIdle = true;
	}
	public void Single() {
		this.triggerSingle = true;
	}
	public void Double() {
		this.triggerDouble = true;
	}
	public override void ResetTriggers() {
		itemContainerAnimator.ResetTrigger (OPEN);
		itemContainerAnimator.ResetTrigger (CLOSE);
		itemContainerAnimator.ResetTrigger (SINGLE);
		itemContainerAnimator.ResetTrigger (DOUBLE);
		itemContainerAnimator.ResetTrigger (SINGLE_IDLE);
		itemContainerAnimator.ResetTrigger (DOUBLE_IDLE);
	}

	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			itemContainerAnimator.SetTrigger (OPEN);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			itemContainerAnimator.SetTrigger (CLOSE);
		}
		else if (triggerSingle) {
			this.AnimationPlay ();
			itemContainerAnimator.SetTrigger (SINGLE);
		}
		else if (triggerDouble) {
			this.AnimationPlay ();
			itemContainerAnimator.SetTrigger (DOUBLE);
		}
		else if (triggerSingleIdle) {
			this.AnimationPlay ();
			itemContainerAnimator.SetTrigger (SINGLE_IDLE);
		}
		else if (triggerDoubleIdle) {
			this.AnimationPlay ();
			itemContainerAnimator.SetTrigger (DOUBLE_IDLE);
		}
		ResetTriggerVariables ();
	}
}
