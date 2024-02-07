using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for interatable game objects.
/// </summary>
public abstract class Interactable : MonoBehaviour {
    /// <summary>
    /// Reference to the player avatar.
    /// </summary>
	public PlayerYuni player;

    /// <summary>
    /// Function performed when the object is interacted with.
    /// </summary>
	public abstract void Interact();

    /// <summary>
    /// Returns the player avatar the game object has detected.
    /// </summary>
    /// <returns></returns>
	public PlayerYuni GetPlayerYuni() {
		if (this.player == null) {	
			this.player = GameObject.FindObjectOfType<PlayerYuni> ();
		}
		return this.player;
	}

    /// <summary>
    /// Sets a reference to the player avatar.
    /// </summary>
    /// <param name="playerYuni">Player Avatar Reference</param>
	public void SetPlayer(PlayerYuni playerYuni) {
		this.player = playerYuni;
	}
}
