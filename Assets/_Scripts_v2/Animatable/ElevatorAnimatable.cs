using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the animator for the elevator game object.
/// </summary>
public class ElevatorAnimatable : MonoBehaviour, Animatable {
    /// <summary>
    /// Script Name
    /// </summary>
	public const string scriptName = "ElevatorAnimatable";

    /// <summary>
    /// Reference to the game object's animator
    /// </summary>
	[SerializeField] private Animator elevatorAnimatable;
    /// <summary>
    /// Flag if the animator is playing.
    /// </summary>
	[SerializeField] private bool isPlaying;

	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	// Call AnimationStop as an Animation Event in ALL animations if needed in a Coroutine
	[SerializeField] private bool triggerIdle = false;
	[SerializeField] private bool triggerIdleOpen = false;
	[SerializeField] private bool triggerClose = false;
	[SerializeField] private bool triggerOpen = false;
	[SerializeField] private bool triggerCloseBars = false;
	[SerializeField] private bool triggerOpenBars = false;
	[SerializeField] private bool triggerFadeRoof = false;
	[SerializeField] private bool triggerShowRoof = false;
	[SerializeField] private bool triggerIdleOpenNoRoof = false;


	/// <summary>
    /// Standard Unity Function. Starts up the MonoBehaviour. Called once throughout its life.
    /// </summary>
	void Start () {
		this.elevatorAnimatable = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (elevatorAnimatable, "elevatorAnimatable", scriptName);

		this.ResetTriggerVariables ();
		this.isPlaying = false;
	}

	// Functions to be called by external scripts
    /// <summary>
    /// Function call to open the elevator.
    /// </summary>
	public void Open() {
		this.triggerOpen = true;
	}

    /// <summary>
    /// Function call to close the elevator.
    /// </summary>
	public void Close() {
		this.triggerClose = true;
	}

    /// <summary>
    /// Function call to close elevator bars.
    /// </summary>
	public void CloseBars() {
		this.triggerCloseBars = true;
	}

    /// <summary>
    /// Function call to open elevator bars.
    /// </summary>
	public void OpenBars() {
		this.triggerOpenBars = true;
	}

    /// <summary>
    /// Function call to fade the roof out.
    /// </summary>
	public void FadeRoof() {
		this.triggerFadeRoof = true;
	}

    /// <summary>
    /// Function call to show the roof
    /// </summary>
	public void ShowRoof() {
		this.triggerShowRoof = true;
	}

    /// <summary>
    /// Function call to stop the elevator and open it without the roof.
    /// </summary>
	public void IdleOpenNoRoof() {
		this.triggerIdleOpenNoRoof = true;
	}

    /// <summary>
    /// Function call to stop.
    /// </summary>
	public void Idle() {
		this.triggerIdle = true;
	}

    /// <summary>
    /// Function call to stop the elevator and open the door.
    /// </summary>
	public void IdleOpen() {
		this.triggerIdleOpen = true;
	}

    /// <summary>
    /// Resets triggers.
    /// </summary>
	public void ResetTriggers() {
		elevatorAnimatable.ResetTrigger ("idle");
		elevatorAnimatable.ResetTrigger ("idleOpen");
		elevatorAnimatable.ResetTrigger ("close");
		elevatorAnimatable.ResetTrigger ("open");
		elevatorAnimatable.ResetTrigger ("closeBars");
		elevatorAnimatable.ResetTrigger ("openBars");
		elevatorAnimatable.ResetTrigger ("fadeRoof");
		elevatorAnimatable.ResetTrigger ("showRoof");
		elevatorAnimatable.ResetTrigger ("idleOpenNoRoof");
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
	void Update () {
		SwitchTriggers ();	
	}

    /// <summary>
    /// Resets Trigger properties back to their initial values.
    /// </summary>
	public void ResetTriggerVariables() {
		triggerIdle = false;
		triggerIdleOpen = false;
		triggerClose = false;
		triggerOpen = false;
		triggerCloseBars = false;
		triggerOpenBars = false;
		triggerFadeRoof = false;
		triggerShowRoof = false;
		triggerIdleOpenNoRoof = false;
	}

    /// <summary>
    /// Checker if the animator is playing.
    /// </summary>
    /// <returns></returns>
	public bool IsPlaying() {
		return this.isPlaying;
	}

    /// <summary>
    /// Function call to play animation.
    /// </summary>
	public void AnimationPlay() {
		this.isPlaying = true;
	}

    /// <summary>
    /// Function call to stop animation.
    /// </summary>
	public void AnimationStop() {
		this.isPlaying = false;
	}

    /// <summary>
    /// Function call to play whatever trigger was activated.
    /// </summary>
	public void SwitchTriggers() {
		ResetTriggers ();

		if (triggerIdle) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("idle");
		}
		else if (triggerIdleOpen) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("idleOpen");
		}
		else if (triggerClose) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("close");
		}
		else if (triggerOpen) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("open");
		}
		else if (triggerCloseBars) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("closeBars");
		}
		else if (triggerOpenBars) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("openBars");
		}
		else if (triggerFadeRoof) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("fadeRoof");
		}
		else if (triggerShowRoof) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("showRoof");
		}
		else if (triggerIdleOpenNoRoof) {
			this.AnimationPlay ();
			elevatorAnimatable.SetTrigger ("idleOpenNoRoof");
		}

		ResetTriggerVariables ();
	}
}
