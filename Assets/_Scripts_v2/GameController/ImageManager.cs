using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the Image displayed in the scene.
/// </summary>
public class ImageManager {

    /// <summary>
    /// Reference to the image UI element in the dialogue.
    /// </summary>
	private ImageUI imageManagerReference;

    /// <summary>
    /// Subscribe function for the image UI element.
    /// </summary>
    /// <param name="imageUI">Image UI Element</param>
	public void SubscribeImageUI(ImageUI imageUI) {
		this.imageManagerReference = imageUI;
	}

    /// <summary>
    /// Unsubscribe function for the image UI element.
    /// </summary>
    /// <param name="imageUI">Image UI Element</param>
	public void UnsubscribeImageUI(MobileUI imageUI) {
		//only the interface that subscribed can unsubscribe
		if (imageUI == this.imageManagerReference)
			imageManagerReference = null;
	}

    /// <summary>
    /// Returns the image of the character specified.
    /// </summary>
    /// <param name="name">Character Name</param>
    /// <returns>Image</returns>
	public Image GetDialogueImage(string name) {
		return this.imageManagerReference.GetImage (name);
	}
}
