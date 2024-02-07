using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolinePlatformAnimatable : MonoBehaviour, Animatable {

	public const string scriptName = "TrampolinePlatformAnimatable";

	[SerializeField] private Animator trampolineAnimator;

	[SerializeField] private bool isPlaying;
	[SerializeField] private ParticleEffect particleActivate;



	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerBounce = false;
	[SerializeField] private bool triggerActivate = false;
	[SerializeField] private bool triggerDeactivate = false;

	void Start () {
		NullCheck ();

		this.trampolineAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (trampolineAnimator, "trampolineAnimator", scriptName);

		this.ResetTriggerVariables ();
		this.isPlaying = false;

	}

	void NullCheck() {
		ParticleEffect[] particleEffects = gameObject.GetComponentsInChildren<ParticleEffect> ();

		if (this.particleActivate == null) {
			if (particleEffects != null) {
				this.particleActivate = particleEffects [0];
			}
		}
	}

	// Animation Event functions
	public void TriggerActivateEffect() {
		this.particleActivate.Play ();
	}


	// Functions to be called by external scripts
	public void Bounce() {
		this.ResetTriggerVariables ();
		this.triggerBounce = true;
	}

	public void Activate() {
		this.ResetTriggerVariables ();
		this.triggerActivate = true;
	}

	public void Deactivate() {
		this.ResetTriggerVariables ();
		this.triggerDeactivate = true;
	}

	public void ResetTriggers() {
		trampolineAnimator.ResetTrigger ("bounce");
		trampolineAnimator.ResetTrigger ("activate");
		trampolineAnimator.ResetTrigger ("deactivate");

	}
	void Update () {
		SwitchTriggers ();	
	}

	public void ResetTriggerVariables() {
		triggerBounce = false;
		triggerActivate = false;
		triggerDeactivate = false;
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

		if (triggerActivate) {
			this.AnimationPlay ();
			trampolineAnimator.SetTrigger ("activate");
		}
		else if (triggerBounce) {
			this.AnimationPlay ();
			trampolineAnimator.SetTrigger ("bounce");
		}
		else if (triggerDeactivate) {
//			this.AnimationPlay ();
//			trampolineAnimator.SetTrigger ("deactivate");
		}
	

		ResetTriggerVariables ();
	}
}
