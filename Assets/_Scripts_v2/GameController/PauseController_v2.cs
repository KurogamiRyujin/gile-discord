using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles game pausing. Uses delegates to allow other classes to push functionalities on game pausing and/or continuing.
/// </summary>
public class PauseController_v2 {

    /// <summary>
    /// Flag if the game is paused.
    /// </summary>
	private bool isPaused = false;
    /// <summary>
    /// Delegate for game pausing.
    /// </summary>
	public delegate void PauseGame ();//delegate for when game is paused
    /// <summary>
    /// Instance of the PauseGame delegate.
    /// </summary>
	public PauseGame onPauseGame;//is subscribed with functions of other classes of THE SAME FORMAT AS ITS TYPE and is called on pause
    /// <summary>
    /// Delegate for continuing game from pause.
    /// </summary>
    public delegate void ContinueGame ();//same with PauseGame but for continuing
    /// <summary>
    /// Instance of the ContinueGame delegate.
    /// </summary>
    public ContinueGame onContinueGame;//same with onPauseGame but for continuing

    //FUNCTIONS ARE CALLED BEFORE THE GAME IS ACTUALLY PAUSED
    /// <summary>
    /// Pauses the game and executes any functions that should be called upon pausing the game.
    /// </summary>
    public void Pause() {
		this.isPaused = true;
		if (onPauseGame != null) {
			onPauseGame ();
		}
		Debug.Log ("Paused");
		Time.timeScale = 0.0f;
	}

    //FUNCTIONS ARE CALLED AFTER THE GAME IS ACTUALLY RESUMED
    /// <summary>
    /// Resumes game and executes any functions that should be called upon resuming the game.
    /// </summary>
    public void Continue() {
		Time.timeScale = 1.0f;

		if (onContinueGame != null) {
			onContinueGame ();
		}
		this.isPaused = false;
	}

    /// <summary>
    /// Checks if the game is paused.
    /// </summary>
    /// <returns>If the game is paused. Otherwise, false.</returns>
	public bool IsPaused() {
		return this.isPaused;
	}
}
