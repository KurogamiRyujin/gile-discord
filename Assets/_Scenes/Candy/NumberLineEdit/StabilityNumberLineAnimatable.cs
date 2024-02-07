using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extends the Animatable class. Handles the calls to the stability number line's animator component which performs the animations.
/// </summary>
public class StabilityNumberLineAnimatable : AnimatableClass {
	/// <summary>
	/// String constant key to trigger the associated animation.
	/// </summary>
	public const string OPEN = "open";
	/// <summary>
	/// String constant key to trigger the associated animation.
	/// </summary>
	public const string CLOSE = "close";
	/// <summary>
	/// String constant key to trigger the associated animation.
	/// </summary>
	public const string CLOSE_IDLE = "closeIdle";

	/// <summary>
	/// The name of the script.
	/// </summary>
	public const string scriptName = "StabilityNumberLineAnimatable";
	/// <summary>
	/// Reference to the stability number line's animator component.
	/// </summary>
	[SerializeField] private Animator stabilityNumberLineAnimator;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	/// <summary>
	/// Checker if there is a request to trigger "open" in the animator.
	/// </summary>
	[SerializeField] private bool triggerOpen = false;
	/// <summary>
	/// Checker if there is a request to trigger "close" in the animator.
	/// </summary>
	[SerializeField] private bool triggerClose = false;
	/// <summary>
	/// Checker if there is a request to trigger "closed idle" in the animator.
	/// </summary>
	[SerializeField] private bool triggerCloseIdle = false;

	/// <summary>
	/// Unity Function. Called once upon creation of the object.
	/// </summary>
	void Awake() {
		this.isPlaying = false;
	}

	/// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start () {
		MAX_DURATION = 0.2f;
		this.stabilityNumberLineAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (stabilityNumberLineAnimator, "stabilityNumberLineAnimator", scriptName);
	}

	/// <summary>
	/// Standard Unity Function. Called once every frame.
	/// </summary>
	void Update () {
		SwitchTriggers ();
	}

	/// <summary>
	/// Call to reset all triggger variables.
	/// </summary>
	public override void ResetTriggerVariables() {
		triggerOpen = false;
		triggerClose = false;
		triggerCloseIdle = false;
	}

	// Functions to be called by external scripts
	/// <summary>
	/// Call to trigger "open" in animator.
	/// </summary>
	public void Open() {
		this.triggerOpen = true;
	}

	/// <summary>
	/// Call to trigger "close" in the animator.
	/// </summary>
	public void Close() {
		this.triggerClose = true;
	}

	/// <summary>
	/// Call to trigger "closed idle" in the animator.
	/// </summary>
	public void CloseIdle() {
		this.triggerCloseIdle = true;
	}

	/// <summary>
	/// Function call to reset triggers.
	/// </summary>
	public override void ResetTriggers() {
		stabilityNumberLineAnimator.ResetTrigger (OPEN);
		stabilityNumberLineAnimator.ResetTrigger (CLOSE);
		stabilityNumberLineAnimator.ResetTrigger (CLOSE_IDLE);
	}

	/// <summary>
	/// Function call to activate triggers and reset them.
	/// </summary>
	public override void SwitchTriggers() {
		ResetTriggers ();

		if (triggerOpen) {
			this.AnimationPlay ();
			stabilityNumberLineAnimator.SetTrigger (OPEN);
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			stabilityNumberLineAnimator.SetTrigger (CLOSE);
		}
		else if (triggerCloseIdle) {
			this.AnimationPlay ();
			stabilityNumberLineAnimator.SetTrigger (CLOSE_IDLE);
		}
		ResetTriggerVariables ();
	}
}
