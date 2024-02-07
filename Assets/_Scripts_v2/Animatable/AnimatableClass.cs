using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract, base animatable class implementing the Animatable Interface. It is inherited by other Animatables for its functionality.
/// </summary>
public abstract class AnimatableClass : MonoBehaviour, Animatable {

	public abstract void ResetTriggers ();
	public abstract void ResetTriggerVariables ();
	public abstract void SwitchTriggers ();


	// You have to set IsPlaying on and off through AnimationEvents
    /// <summary>
    /// Max allowable duration animation can play.
    /// </summary>
	[SerializeField] protected float MAX_DURATION = 0.5f;
    /// <summary>
    /// Flag if the animation is still playing.
    /// </summary>
	[SerializeField] protected bool isPlaying;
    /// <summary>
    /// Flag if the animation should loop.
    /// </summary>
	[SerializeField] protected bool isLoop;
    /// <summary>
    /// Time animation started in seconds.
    /// </summary>
	protected float timeStart;

    /// <summary>
    /// Standard Unity Function. Called once every frame.
    /// </summary>
	void Update () {
		if (IsPlaying ()) {
			if (!isLoop && this.timeStart >= (Time.deltaTime + MAX_DURATION)) {
				this.AnimationStop ();
				Debug.Log ("MAX DURATION");
			}
		}
	}
    
    /// <summary>
    /// Checker if animation is playing.
    /// </summary>
    /// <returns></returns>
	public bool IsPlaying() {
		return this.isPlaying;
	}

    /// <summary>
    /// Call to play animation.
    /// </summary>
	public void AnimationPlay() {
		this.isPlaying = true;
		this.timeStart = Time.deltaTime;
	}

    /// <summary>
    /// Call to stop animation.
    /// </summary>
	public void AnimationStop() {
		this.isPlaying = false;
	}
}
