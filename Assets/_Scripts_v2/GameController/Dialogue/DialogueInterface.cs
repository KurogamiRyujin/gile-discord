using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface for monobehaviours that will have dialogue functionality.
/// </summary>
public interface DialogueInterface {
	/// <summary>
	/// Start a dialogue with the given dialogue parameter
	/// </summary>
	/// <param name="dialogue">Dialogue instance.</param>
	/// <param name="characterImage">Character image; can be null.</param>
	void StartDialogue (Dialogue dialogue, Image characterImage);
    /// <summary>
	/// Start a dialogue with the given dialogue parameter
	/// </summary>
	/// <param name="dialogue">Dialogue instance.</param>
	/// <param name="characterImage">Character image; can be null.</param>
	void StartHintDialogue (Dialogue dialogue, Image characterImage);
	/// <summary>
	/// Start a dialogue with the given dialogue parameter
	/// </summary>
	/// <param name="dialogue">String array of dialogue or message.</param>
	/// <param name="characterImage">Character image; can be null.</param>
	void StartDialogue (string[] dialogue, Image characterImage);
    /// <summary>
	/// Start a dialogue with the given dialogue parameter
	/// </summary>
	/// <param name="dialogue">String array of dialogue or message.</param>
	/// <param name="characterImage">Character image; can be null.</param>
	void StartHintDialogue (string[] dialogue, Image characterImage);
    /// <summary>
    /// Checker if the dialogue is playing.
    /// </summary>
    /// <returns></returns>
	bool IsPlaying ();
    /// <summary>
    /// Called when a mission has been completed (used by a hint type dialogue).
    /// </summary>
	void HintSuccess ();
    /// <summary>
    /// Closes the hint box.
    /// </summary>
	void CloseHint ();
}
