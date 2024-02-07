using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingleaderAnimatable : MonoBehaviour, Animatable {
	public const string scriptName = "RingleaderAnimatable";

	[SerializeField] private Animator ringleaderAnimator;

	[SerializeField] private bool isPlaying;


	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerDeath = false;
	[SerializeField] private bool triggerFlyOpen = false;
	[SerializeField] private bool triggerFlyClose = false;
	[SerializeField] private bool triggerIdle = false;
	[SerializeField] private bool triggerTeleportOut = false;
	[SerializeField] private bool triggerTeleportIn = false;
	[SerializeField] private bool triggerMeleeOpen = false;

	void Start () {
		this.ringleaderAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (ringleaderAnimator, "ringleaderAnimator", scriptName);

		this.ResetTriggerVariables ();
		this.isPlaying = false;

	}

	// Functions to be called by external scripts
	public void TeleportOut() {
		this.triggerTeleportOut = true;
	}

    public void PlayPhantomSound() {
        SoundManager.Instance.Play(AudibleNames.Phantom.DEATH, false);
    }
	public void TeleportIn() {
		this.triggerTeleportIn = true;
	}

	public void MeleeOpen() {
		this.triggerMeleeOpen = true;
	}

	public void FlyOpen() {
		this.triggerFlyOpen = true;
	}

	public void FlyClose() {
		this.triggerFlyClose = true;
	}

	public void Death() {
		this.triggerDeath = true;
	}

	public void ResetTriggers() {
		ringleaderAnimator.ResetTrigger ("death");
		ringleaderAnimator.ResetTrigger ("fly open");
		ringleaderAnimator.ResetTrigger ("fly close");
		ringleaderAnimator.ResetTrigger ("idle");
		ringleaderAnimator.ResetTrigger ("idle teleport out");
		ringleaderAnimator.ResetTrigger ("idle teleport in");
		ringleaderAnimator.ResetTrigger ("melee open");
	}
	void Update () {
		SwitchTriggers ();	
	}
		
	public void ResetTriggerVariables() {
		triggerDeath = false;
		triggerFlyClose = false;
		triggerFlyOpen = false;
		triggerIdle = false;
		triggerTeleportOut = false;
		triggerTeleportIn = false;
		triggerMeleeOpen = false;
	}

	public bool IsPlaying() {
		return this.isPlaying;
	}

	public void AnimationPlay() {
		this.isPlaying = true;
	}

	public void AnimationStop() {
		this.isPlaying = false;
	}

	public void SwitchTriggers() {
		ResetTriggers ();

		if (triggerDeath) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("death");
		}

		else if (triggerFlyOpen) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("fly open");
		}

		else if (triggerFlyClose) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("fly close");
		}

		else if (triggerIdle) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("idle");
		}

		else if (triggerTeleportIn) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("idle teleport in");
		}

		else if (triggerTeleportOut) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("idle teleport out");
		}

		else if (triggerMeleeOpen) {
			this.AnimationPlay ();
			ringleaderAnimator.SetTrigger ("melee open");
		}
		ResetTriggerVariables ();
	}
}
