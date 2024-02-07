using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface used by game objects if they have animations.
/// </summary>
public interface Animatable {
    /// <summary>
    /// Function call to reset triggers.
    /// </summary>
	void ResetTriggers ();
    /// <summary>
    /// Call to reset all triggger all variables.
    /// </summary>
	void ResetTriggerVariables ();

    /// <summary>
    /// Checker if animator is playing.
    /// </summary>
    /// <returns></returns>
	bool IsPlaying ();
    /// <summary>
    /// Function call to play animation.
    /// </summary>
	void AnimationPlay ();
    /// <summary>
    /// Function call to stop animation.
    /// </summary>
	void AnimationStop ();
    /// <summary>
    /// Function call to switch triggers.
    /// </summary>
	void SwitchTriggers ();

}
