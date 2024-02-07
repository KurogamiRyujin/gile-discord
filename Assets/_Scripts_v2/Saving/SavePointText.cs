using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for the text UI for the save option.
/// </summary>
public class SavePointText : MonoBehaviour {

    /// <summary>
    /// Reference to the UI's sprite renderer.
    /// </summary>
	SpriteRenderer sp;

    /// <summary>
    /// Standard Unity Function. Called once in the game object's lifetime to initiate the script once it has been enabled.
    /// </summary>
    void Start () {
		sp = GetComponent<SpriteRenderer> ();
		sp.enabled = false;
	}

    /// <summary>
    /// Shows the UI.
    /// </summary>
	public void Show() {
		sp.enabled = true;
	}

    /// <summary>
    /// Hides the UI.
    /// </summary>
	public void Hide() {
		sp.enabled = false;
	}
}
