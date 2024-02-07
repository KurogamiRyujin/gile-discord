using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the dialogue boxes (both dialogue and hint types) in the scene.
/// </summary>
public class DialogueManager_v2 {
    /// <summary>
    /// Enum for the possible dialogue box types.
    /// </summary>
	public enum DialogueType {
		DIALOGUE,
		HINT
	}

    /// <summary>
    /// Reference to a dialogue type dialogue box.
    /// </summary>
	private DialogueInterface dialogueBox;
    /// <summary>
    /// Reference to a hint type dialogue box.
    /// </summary>
	private DialogueInterface hintBox;
    /// <summary>
    /// Reference to a cutscene graphic.
    /// </summary>
	private CutsceneGraphic cutsceneGraphic;

    /// <summary>
    /// Constructor
    /// </summary>
	public DialogueManager_v2() {
		this.dialogueBox = null;
		this.hintBox = null;
		this.cutsceneGraphic = null;
	}

    /// <summary>
    /// Function used by dialogue box objects in the scene to register themselves to the manager.
    /// </summary>
    /// <param name="type">Dialogue Box Type</param>
    /// <param name="box">Dialogue Box object</param>
	public void Register(DialogueType type, DialogueInterface box) {
		switch (type) {
		case DialogueType.DIALOGUE:
			this.dialogueBox = box;
			break;
		case DialogueType.HINT:
			this.hintBox = box;
			break;
		}
	}

    /// <summary>
    /// Function used by cutscene graphic objects to register themselves to the manager.
    /// </summary>
    /// <param name="cutsceneGraphic">CG object</param>
	public void Register(CutsceneGraphic cutsceneGraphic) {
		this.cutsceneGraphic = cutsceneGraphic;
	}

    /// <summary>
    /// Used by a cutscene graphic to unregister itself from the manager.
    /// </summary>
    /// <param name="cutsceneGraphic">CG object</param>
	public void UnregisterCG(CutsceneGraphic cutsceneGraphic) {
		if (this.cutsceneGraphic == cutsceneGraphic)
			this.cutsceneGraphic = null;
	}

    /// <summary>
    /// Used by a dialogue type dialogue box object to unregister itself from the manager.
    /// </summary>
    /// <param name="box"></param>
	public void UnregisterDialogueBox(DialogueInterface box) {
		if(this.dialogueBox == box)
			this.dialogueBox = null;
	}

    /// <summary>
    /// Used by a hint type dialogue box object to unregister itself from the manager.
    /// </summary>
    /// <param name="box"></param>
	public void UnregisterHintBox(DialogueInterface box) {
		if(this.hintBox == box)
			this.hintBox = null;
	}

    /// <summary>
    /// Called to trigger the hint success animation for the hint type dialogue box.
    /// </summary>
	public void HintSuccess() {
		this.hintBox.HintSuccess ();
	}

    /// <summary>
    /// Have either the dialogue type or hint type dialogue box to display a set of text.
    /// </summary>
    /// <param name="type">Dialogue box type</param>
    /// <param name="dialogue">Dialogue to be displayed</param>
    /// <param name="characterImage">Speaking Character's image, if any</param>
	public void DisplayMessage(DialogueType type, Dialogue dialogue, Image characterImage) {
		switch (type) {
		case DialogueType.DIALOGUE:
			if (!this.dialogueBox.IsPlaying ())
				this.dialogueBox.StartDialogue (dialogue, characterImage);
			break;
		case DialogueType.HINT:
			if (!this.hintBox.IsPlaying ())
				this.hintBox.StartHintDialogue (dialogue, characterImage);
			break;
		}
	}

	// Used by minor hints only
    /// <summary>
    /// Makes the hint type dialogue box display a minor hint.
    /// </summary>
    /// <param name="dialogue"></param>
    /// <returns></returns>
	public bool DisplayMinorHint(Dialogue dialogue) {
		if (!this.hintBox.IsPlaying ()) {
			this.hintBox.StartHintDialogue (dialogue, null);
			return true;
		} else {
			return false;
		}
	}

    /// <summary>
    /// Close the hint type dialogue box.
    /// </summary>
	public void CloseMinorHint() {
		this.hintBox.CloseHint ();
	}
    /// <summary>
    /// Have either the dialogue type or hint type dialogue box to display a set of text.
    /// </summary>
    /// <param name="type">Dialogue box type</param>
    /// <param name="dialogue">String array of dialogue to be displayed</param>
    /// <param name="characterImage">Speaking Character's Image, if any</param>
	public void DisplayMessage(DialogueType type, string[] dialogue, Image characterImage) {
		switch (type) {
		case DialogueType.DIALOGUE:
			if (!this.dialogueBox.IsPlaying ())
				this.dialogueBox.StartDialogue (dialogue, characterImage);
			break;
		case DialogueType.HINT:
			if (!this.hintBox.IsPlaying ())
				this.hintBox.StartHintDialogue (dialogue, characterImage);
			break;
		}
	}

    /// <summary>
    /// Shows the CG object.
    /// </summary>
	public void ShowCG() {
		if (this.cutsceneGraphic != null)
			this.cutsceneGraphic.ShowCG ();
		else
			Debug.Log ("CG is null");
	}

    /// <summary>
    /// Hides the CG object.
    /// </summary>
	public void HideCG() {
		if (this.cutsceneGraphic != null)
			this.cutsceneGraphic.HideCG ();
		else
			Debug.Log ("CG is null");
	}
    
    /// <summary>
    /// Checker if the dialogue type dialogue box is playing.
    /// </summary>
    /// <returns>If the dialogue type dialogue box is playing. Otherwise, false.</returns>
	public bool IsDialogueBoxPlaying () {
		return this.dialogueBox.IsPlaying ();
	}

    /// <summary>
    /// Checker if the hint type dialogue box is still shown.
    /// </summary>
    /// <returns>If hint type dialogue box is still shown. Otherwise, false.</returns>
	public bool IsHintBoxUp() {
		return this.hintBox.IsPlaying ();
	}
}