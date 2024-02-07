using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the animator for the enemy's number line health.
/// </summary>
public class EnemyHealthAnimatable : MonoBehaviour, Animatable {
    /// <summary>
    /// Script Name
    /// </summary>
	public const string scriptName = "EnemyHealthAnimatable";
    /// <summary>
    /// Reference to the enemy health's animator
    /// </summary>
	[SerializeField] private Animator enemyHealthAnimator;
    /// <summary>
    /// Flag to check if animator if playing.
    /// </summary>
	[SerializeField] private bool isPlaying;



	// Everytime you add a variable, add it to ResetTriggerVariables, ResetTriggers, and SwitchTriggers
	[SerializeField] private bool triggerHide = false;
	[SerializeField] private bool triggerShow = false;

    /// <summary>
    /// Standard Unity Function. Starts up the MonoBehaviour. Called once throughout its life.
    /// </summary>
	void Start () {
		this.enemyHealthAnimator = gameObject.GetComponent<Animator> ();
		General.CheckIfNull (enemyHealthAnimator, "enemyHealthAnimator", scriptName);
		this.ResetTriggerVariables ();
		this.isPlaying = false;
	}

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
	void Update () {
		SwitchTriggers ();	
	}

	// Functions to be called by external scripts
    /// <summary>
    /// Function call to hide the enemy health.
    /// </summary>
	public void Hide() {
		this.triggerHide = true;
	}
    /// <summary>
    /// Function call to show the enemy health.
    /// </summary>
	public void Show() {
		this.triggerShow = true;
	}

    /// <summary>
    /// Resets animator triggers.
    /// </summary>
	public void ResetTriggers (){
		enemyHealthAnimator.ResetTrigger ("show");
		enemyHealthAnimator.ResetTrigger ("hide");
	}

    /// <summary>
    /// Resets the values of the trigger properties.
    /// </summary>
	public void ResetTriggerVariables (){
		triggerHide = false;
		triggerShow = false;
	}

    /// <summary>
    /// Checker if the animator is playing.
    /// </summary>
    /// <returns></returns>
	public bool IsPlaying() {
		return this.isPlaying;
	}

    /// <summary>
    /// Function call for the animator to play.
    /// </summary>
	public void AnimationPlay() {
		this.isPlaying = true;
	}

    /// <summary>
    /// Function call for the animator to stop.
    /// </summary>
	public void AnimationStop() {
		this.isPlaying = false;
	}

    /// <summary>
    /// Function call to play whatever trigger was activated.
    /// </summary>
	public void SwitchTriggers (){
		ResetTriggers ();

		if (triggerShow) {
			this.AnimationPlay ();
			enemyHealthAnimator.SetTrigger ("show");
		}

		else if (triggerHide) {
			this.AnimationPlay ();
			enemyHealthAnimator.SetTrigger ("hide");
		}


		ResetTriggerVariables ();
	}

}
