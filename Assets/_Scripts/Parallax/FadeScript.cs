using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour {
	private CanvasGroup fadeGroup;
	private float fadeInDuration = 2;
	private bool gameStarted;

	void Start () {
		// Get the only CanvasGroup in the scene
		this.fadeGroup = FindObjectOfType<CanvasGroup> ();
		// Set the fade to full opacity
		this.fadeGroup.alpha = 1;
		
	}

	void Update () {
		if (Time.timeSinceLevelLoad <= fadeInDuration) {
			// Initial fade-in
			fadeGroup.alpha = 1 - (Time.timeSinceLevelLoad / fadeInDuration);
		}
		// If initial fade-in is completed and the game has not been started yet
		else if (!gameStarted) {
			// Ensure the fade is completely gone
			fadeGroup.alpha = 0;
			gameStarted = true;
		}
		
	}
}
