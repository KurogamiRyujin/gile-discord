using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI element overlay to darken the screen, indicating the game is paused.
/// </summary>
public class PauseOverlay : MonoBehaviour {

    /// <summary>
    /// Unity Function. Called once upon creation of the object.
    /// </summary>
    void Awake () {
		//Subscribe the functions to the Pause Controller's delegates
		GameController_v7.Instance.GetPauseController ().onPauseGame += OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame += OnContinue;
	}

    /// <summary>
	/// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
	/// </summary>
	void Start() {
		this.gameObject.SetActive (false);
	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
	void OnDestroy() {
		//Unsubscribe the functions
		GameController_v7.Instance.GetPauseController ().onPauseGame -= OnPause;
		GameController_v7.Instance.GetPauseController ().onContinueGame -= OnContinue;
	}

	/// <summary>
    /// Function upon pausing the game.
    /// </summary>
	private void OnPause() {
		Debug.Log ("OVERLAY ON PAUSE CALLED");
		this.gameObject.SetActive (true);
	}

	/// <summary>
    /// Function upon continuing the game from pause.
    /// </summary>
	private void OnContinue() {
		Debug.Log ("OVERLAY ON CONTINUE CALLED");
		this.gameObject.SetActive (false);
	}
}
